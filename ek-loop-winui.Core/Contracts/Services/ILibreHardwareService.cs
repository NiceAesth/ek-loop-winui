using ek_loop_winui.Core.Models;

namespace ek_loop_winui.Core.Contracts.Services;
public interface ILibreHardwareService
{
    void Initialize();
    List<LibreHardwareDTO> GetHardwareWithTemperatureSensors();
}
