using ek_loop_winui.Core.Models;

namespace ek_loop_winui.Core.Contracts.Services;
public interface IEKDeviceService
{
    void Initialize();
    Task<EKFan> GetFan(int id);
    Task SetFan(int id, int pwm, int rpm);
    Task<List<EKFan>> GetAllFans();
}
