using ek_loop_winui.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace ek_loop_winui.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
