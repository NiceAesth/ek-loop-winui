using ek_loop_winui.Core.Models;

namespace ek_loop_winui.Contracts.Services;

public interface IAppStateControllerService
{
    void Initialize();
    event EventHandler<EKCacheData> OnEKUpdate;
    event EventHandler<List<LibreHardwareDTO>> OnLibreHardwareUpdate;
    public Task<List<EKFan>> GetAllFans();
    public List<LibreHardwareDTO> GetHardwareWithTemperatureSensors();
}
