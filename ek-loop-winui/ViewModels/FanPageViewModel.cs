using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using ek_loop_winui.Contracts.ViewModels;
using ek_loop_winui.Core.Contracts.Services;
using ek_loop_winui.Core.Models;

namespace ek_loop_winui.ViewModels;

public partial class FanPageViewModel : ObservableRecipient, INavigationAware
{
    private readonly IEKDeviceService ekDeviceService;

    public ObservableCollection<EKFan> Source { get; } = new ObservableCollection<EKFan>();

    public FanPageViewModel(IEKDeviceService ekDeviceService)
    {
        this.ekDeviceService = ekDeviceService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = await ekDeviceService.GetAllFans();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
