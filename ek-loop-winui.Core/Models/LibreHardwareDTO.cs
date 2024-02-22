using LibreHardwareMonitor.Hardware;

namespace ek_loop_winui.Core.Models;
public class LibreHardwareDTO
{
    public string Name { get; set; } = string.Empty;
    public ISensor[] Sensors
    {
        get; set;
    }
}
