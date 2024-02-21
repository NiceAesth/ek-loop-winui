using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ek_loop_winui.Contracts.ViewModels;
using ek_loop_winui.Core.Contracts.Services;
using ek_loop_winui.Core.Models;

namespace ek_loop_winui.ViewModels;

public partial class MainViewModel : ObservableRecipient, INavigationAware
{
    private readonly ILibreHardwareService openHardwareService;

    public ObservableCollection<LibreHardware> Source { get; set; } = new ObservableCollection<LibreHardware>();

    public MainViewModel(ILibreHardwareService openHardwareService)
    {

        this.openHardwareService = openHardwareService;
    }
    public void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var data = openHardwareService.GetHardwareWithTemperatureSensors();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
