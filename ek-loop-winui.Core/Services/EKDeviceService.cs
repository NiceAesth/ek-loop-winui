using ek_loop_winui.Core.Contracts.Services;
using ek_loop_winui.Core.Models;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;

namespace ek_loop_winui.Core.Services;


public class EKDeviceService : IEKDeviceService
{
    private const int vendorId = 0x0483;
    private const int productId = 0x5750;

    private readonly UsbContext context = new();
    private static IUsbDevice device;
    private static UsbEndpointReader usbEndpointReader;
    private static UsbEndpointWriter usbEndpointWriter;
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private const int BUFFER_SIZE = 63;
    private const int TIMEOUT = 500;

    private const int FAN_COUNT = 6;
    private const int FAN_READ_RPM_OFFSET = 12;
    private const int FAN_READ_PWM_OFFSET = 21;
    private static readonly (byte, byte)[] FAN_CHANNELS =
    {
        (0xA0, 0xA0),
        (0xA0, 0xC0),
        (0xA0, 0xE0),
        (0xA1, 0x00),
        (0xA1, 0x20),
        (0xA1, 0x40), // Pump
        (0xA1, 0x60), // Unused
        (0xA1, 0x80)  // Unused
    };

    private bool workerRunning = false;
    private readonly PeriodicTimer updateWorkerTimer = new(TimeSpan.FromSeconds(3));
    EKCacheData EKCacheData;

    public void Initialize()
    {
        context.SetDebugLevel(LogLevel.Info);
        device = context.Find(new UsbDeviceFinder(vendorId, productId));

        if (device == null)
        {

            throw new Exception("Device not found");
        }

        device.Open();
        device.ClaimInterface(device.Configs[0].Interfaces[0].Number);

        usbEndpointReader = device.OpenEndpointReader(ReadEndpointID.Ep01, BUFFER_SIZE);
        usbEndpointWriter = device.OpenEndpointWriter(WriteEndpointID.Ep01);
    }

    ~EKDeviceService()
    {
        updateWorkerTimer?.Dispose();
        context?.Dispose();
    }

    protected static byte[] RPMToBytes(int rpm)
    {
        return new byte[] { (byte)((rpm >> 8) & 0xFF), (byte)(rpm & 0xFF) };
    }

    public async Task StartUpdateWorker()
    {
        if (workerRunning)
        {
            return;
        }

        workerRunning = true;
        while (await updateWorkerTimer.WaitForNextTickAsync())
        {
            EKCacheData = new EKCacheData
            {
                Fans = await getAllFans()
            };
        }
    }

    public async Task<EKFan> GetFan(int id)
    {
        var packet = new byte[BUFFER_SIZE];
        packet[0] = 0x10;
        packet[1] = 0x12;
        packet[2] = 0x08;
        packet[3] = 0xAA;
        packet[4] = 0x01;
        packet[5] = 0x03;
        packet[6] = FAN_CHANNELS[id].Item1;
        packet[7] = FAN_CHANNELS[id].Item2;
        packet[8] = 0x00;
        packet[9] = 0x20;
        packet[10] = 0x66;
        packet[11] = 0xFF;
        packet[12] = 0xFF;
        packet[13] = 0xED;

        await semaphore.WaitAsync();
        await Task.Run(() => usbEndpointWriter.Write(packet, TIMEOUT, out _));

        var buffer = new byte[BUFFER_SIZE];
        await Task.Run(() => usbEndpointReader.Read(buffer, TIMEOUT, out _));
        semaphore.Release();

        return new EKFan
        {
            ID = id,
            RPM = (buffer[FAN_READ_RPM_OFFSET] << 8) | buffer[FAN_READ_RPM_OFFSET + 1],
            PWM = buffer[FAN_READ_PWM_OFFSET]
        };
    }

    public async Task SetFan(int id, int pwm)
    {
        var packet = new byte[BUFFER_SIZE];
        packet[0] = 0x10;
        packet[1] = 0x12;
        packet[2] = 0x29;
        packet[3] = 0xAA;
        packet[4] = 0x01;
        packet[5] = 0x10;
        packet[6] = FAN_CHANNELS[id].Item1;
        packet[7] = FAN_CHANNELS[id].Item2;
        packet[9] = 0x10;
        packet[10] = 0x20;
        packet[24] = (byte)pwm;
        packet[46] = 0xED;

        await semaphore.WaitAsync();
        await Task.Run(() => usbEndpointWriter.Write(packet, TIMEOUT, out _));
        await Task.Run(() => usbEndpointReader.ReadFlush());
        semaphore.Release();
    }

    private async Task<List<EKFan>> getAllFans()
    {
        var fans = new List<EKFan>();
        for (var i = 0; i < FAN_COUNT; i++)
        {


            fans.Add(await GetFan(i));
        }
        return fans;
    }

    public async Task<List<EKFan>> GetAllFans()
    {
        if (workerRunning && EKCacheData != null)
        {
            return EKCacheData.Fans;
        }

        return await getAllFans();
    }
}

