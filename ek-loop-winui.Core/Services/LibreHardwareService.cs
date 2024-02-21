using ek_loop_winui.Core.Contracts.Services;
using ek_loop_winui.Core.Models;
using LibreHardwareMonitor.Hardware;

namespace ek_loop_winui.Core.Services;
public class LibreHardwareService : ILibreHardwareService
{
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

    public LibreHardware[] GetHardwareWithTemperatureSensors()
    {
        return computer.Hardware
            .Where(hardware => hardware.Sensors.Any(sensor => sensor.SensorType == SensorType.Temperature))
            .Select(hardware => new LibreHardware
            {
                Name = hardware.Name,
                Sensors = hardware.Sensors.Where(sensor => sensor.SensorType == SensorType.Temperature).ToArray()
            })
            .ToArray();
    }
}
