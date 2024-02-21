using ek_loop_winui.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace ek_loop_winui.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class FanPage : Page
{
    public FanPageViewModel ViewModel
    {
        get;
    }

    public FanPage()
    {
        ViewModel = App.GetService<FanPageViewModel>();
        InitializeComponent();
    }
}
