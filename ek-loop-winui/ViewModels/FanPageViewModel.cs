using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using ek_loop_winui.Contracts.Services;
using ek_loop_winui.Contracts.ViewModels;
using ek_loop_winui.Core.Models;

namespace ek_loop_winui.ViewModels;

public partial class FanPageViewModel : ObservableRecipient, INavigationAware
{
    private readonly IAppStateControllerService appDataService;

    public ObservableCollection<EKFan> Source { get; } = new ObservableCollection<EKFan>();

    public FanPageViewModel(IAppStateControllerService appDataService)
    {
        this.appDataService = appDataService;
        appDataService.OnEKUpdate += OnEKUpdate;
    }

    private void OnEKUpdate(object? sender, EKCacheData newData)
    {
        Source.Clear();

        foreach (var item in newData.Fans)
        {
            Source.Add(item);
        }
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = await appDataService.GetAllFans();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
