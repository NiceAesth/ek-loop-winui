using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ek_loop_winui.Contracts.Services;
using ek_loop_winui.Contracts.ViewModels;
using ek_loop_winui.Core.Models;

namespace ek_loop_winui.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    private readonly IAppStateControllerService appDataService;

    public ObservableCollection<LibreHardwareDTO> Source { get; set; } = new ObservableCollection<LibreHardwareDTO>();

    public MainViewModel(IAppStateControllerService appDataService)
    {
        this.appDataService = appDataService;
        appDataService.OnLibreHardwareUpdate += OnLibreHardwareUpdate;
    }

    private void OnLibreHardwareUpdate(object? sender, List<LibreHardwareDTO> newData)
    {
        Source.Clear();

        foreach (var item in newData)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = appDataService.GetHardwareWithTemperatureSensors();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
