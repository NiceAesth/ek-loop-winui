using ek_loop_winui.Core.Contracts.Services;
using ek_loop_winui.Core.Models;
using LibreHardwareMonitor.Hardware;

namespace ek_loop_winui.Core.Services;
public class LibreHardwareService : ILibreHardwareService
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);
    private readonly Computer computer = new()
    {
        IsCpuEnabled = true,
        IsGpuEnabled = true
    };

    public void Initialize()
    {
        computer.Open();
    }

    ~LibreHardwareService()
    {

        computer.Close();
    }

    public List<LibreHardwareDTO> GetHardwareWithTemperatureSensors()
    {
        List<LibreHardwareDTO> data = new();
        semaphore.Wait();
        foreach (var hardware in computer.Hardware)
        {
            hardware.Update();
            data.Add(new LibreHardwareDTO
            {
                Name = hardware.Name,
                Sensors = hardware.Sensors.Where(sensor => sensor.SensorType == SensorType.Temperature).ToArray()
            });
        }
        semaphore.Release();
        return data;
    }
}
