using ek_loop_winui.Contracts.Services;
using ek_loop_winui.Core.Contracts.Services;
using ek_loop_winui.Core.Models;
using Microsoft.UI.Dispatching;

namespace ek_loop_winui.Services;

class AppStateControllerService : IAppStateControllerService
{
    private readonly IEKDeviceService ekDeviceService;
    private readonly ILibreHardwareService libreHardwareService;

    private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    private readonly PeriodicTimer updateWorkerTimer = new(TimeSpan.FromSeconds(1));
    private bool workerRunning = false;

    private readonly EKCacheData ekCacheData = new();
    private List<LibreHardwareDTO>? libreHardwareCacheData = null;

    public event EventHandler<EKCacheData>? OnEKUpdate;
    public event EventHandler<List<LibreHardwareDTO>>? OnLibreHardwareUpdate;

    public AppStateControllerService(IEKDeviceService ekDeviceService, ILibreHardwareService libreHardwareService)
    {
        this.ekDeviceService = ekDeviceService;
        this.libreHardwareService = libreHardwareService;
    }

    ~AppStateControllerService()
    {
        updateWorkerTimer.Dispose();
        workerRunning = false;
    }

    public void Initialize()
    {
        workerRunning = dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, Update);
    }

    protected async void Update()
    {
        while (await updateWorkerTimer.WaitForNextTickAsync())
        {
            ekCacheData.Fans = await ekDeviceService.GetAllFans();
            libreHardwareCacheData = libreHardwareService.GetHardwareWithTemperatureSensors();

            OnEKUpdate?.Invoke(this, ekCacheData);
            OnLibreHardwareUpdate?.Invoke(this, libreHardwareCacheData);
        }
    }

    public async Task<List<EKFan>> GetAllFans()
    {
        if (workerRunning && ekCacheData.Fans != null)
        {
            return ekCacheData.Fans;
        }

        return await ekDeviceService.GetAllFans();
    }

    public List<LibreHardwareDTO> GetHardwareWithTemperatureSensors()
    {
        if (workerRunning && libreHardwareCacheData != null)
        {
            return libreHardwareCacheData;
        }

        return libreHardwareService.GetHardwareWithTemperatureSensors();
    }
}
