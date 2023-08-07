namespace Maui.DataGrid.Sample;

using Maui.DataGrid.Sample.ViewModels;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
        _gcCollectButton1.Clicked += OnGcCollect;
    }

    private void OnGcCollect(object sender, EventArgs e)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
