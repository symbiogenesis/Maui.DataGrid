namespace Maui.DataGrid.Sample.Platforms.Android;

using global::Android.App;
using global::Android.Content.PM;
using global::Android.OS;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

        base.OnCreate(savedInstanceState);
    }

    private static void TaskSchedulerOnUnobservedTaskException(object? obj, UnobservedTaskExceptionEventArgs args) => Console.WriteLine(args.Exception.Message);

    private static void CurrentDomainOnUnhandledException(object? obj, UnhandledExceptionEventArgs args)
    {
        if (args.ExceptionObject is Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}
