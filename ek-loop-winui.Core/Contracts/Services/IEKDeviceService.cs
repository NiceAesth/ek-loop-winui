using ek_loop_winui.Core.Models;

namespace ek_loop_winui.Core.Contracts.Services;
public interface IEKDeviceService
{
    void Initialize();
    Task StartUpdateWorker();
    Task<EKFan> GetFan(int id);
    Task SetFan(int id, int pwm);
    Task<List<EKFan>> GetAllFans();
}
