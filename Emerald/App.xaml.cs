using CommonServiceLocator;
using Serilog;
using Serilog.Sinks.File;
using Uno.Resizetizer;
using Uno.UI.HotDesign;
using Microsoft.Extensions.DependencyInjection;
using Emerald.CoreX.Store.Modrinth;
using System.Diagnostics;
using System;
using Emerald.Helpers;

namespace Emerald; 
public partial class App : Application
{
    private Helpers.Settings.SettingsSystem SS;
    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent(); 

        // Register exception handlers

        this.UnhandledException += App_UnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
    }


    public Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }
    private void ConfigureServices(IServiceCollection services)
    {
        //Stores
        services.AddTransient(provider => new ModStore(typeof(ModStore).Log()));
        services.AddTransient(provider => new PluginStore(typeof(PluginStore).Log()));
        services.AddTransient(provider => new ResourcePackStore(typeof(ResourcePackStore).Log()));
        services.AddTransient(provider => new ResourcePackStore(typeof(ShaderStore).Log()));
        services.AddTransient(provider => new ModpackStore(typeof(ModpackStore).Log()));

        //Settings
        services.AddSingleton<Helpers.Settings.SettingsSystem>();


    }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Emerald",
            "logs",
            "app_.log");

        var builder = this.CreateBuilder(args)
            .Configure(host => host
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif

                //Enable Logging
                .UseSerilog(true, configureLogger: x=> x
                            .MinimumLevel.Debug()
                            .WriteTo.File(logPath,
                                            rollingInterval: RollingInterval.Day,
                                            retainedFileCountLimit: 7,
                                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}"))

                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
            );

        MainWindow = builder.Window;
#if DEBUG
        MainWindow.UseStudio();
#endif
        MainWindow.SetWindowIcon();

        Host = builder.Build();

        //Help me.
        ServiceLocator.SetLocatorProvider(() => new Emerald.Services.ServiceProviderLocator(Host!.Services));

        SS = ServiceLocator.Current.GetInstance<Helpers.Settings.SettingsSystem>();
        this.Log().LogInformation("New Instance was created. Logs are being saved at: {logPath}",logPath);

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (MainWindow.Content is not Frame rootFrame)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();

            //load settings,
            SS.LoadData();

            // Place the frame in the current Window
            MainWindow.Content = rootFrame;
        }

        if (rootFrame.Content == null)
        {
            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            rootFrame.Navigate(typeof(MainPage), args.Arguments);
        }
        // Ensure the current window is active
        MainWindow.Activate();

        MainWindow.Closed += MainWindow_Closed;
    }

    private void MainWindow_Closed(object sender, WindowEventArgs args)
    {
        //save settings,
        SS.SaveData();
    }

    /// <summary>
    /// Gets the current <see cref="App"/> instance in use
    /// </summary>
    public new static App Current => (App)Application.Current;

    #region UnhandledExceptions
    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Emerald");
        e.Handled = true;
        LogUnhandledException(e.Exception, "UI UnhandledException");
        ShowPlatformErrorDialog($"An unexpected error occurred. The application needs to be closed.\n see logs at {logPath} for more details");
    }

    private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
    {
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Emerald");
        LogUnhandledException((Exception)e.ExceptionObject, "AppDomain UnhandledException");
        ShowPlatformErrorDialog($"A critical error occurred. The application needs to be closed.\n see logs at {logPath} for more details");
    }

    private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Emerald");
        e.SetObserved();
        LogUnhandledException(e.Exception, "Task UnobservedException");
        ShowPlatformErrorDialog($"A unobserved error occurred. The application needs to be closed.\n see logs at {logPath} for more details");
    }

    private void LogUnhandledException(Exception exception, string source)
    {
        try
        {
            this.Log().LogCritical(exception,
                "Unhandled exception ({Source}). Platform: {Platform}",
                source,
                DirectResoucres.Platform
            );

            // Save to crash file (platform-specific path)
            var crashPath = Path.Combine(
#if WINDOWS
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
#else
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
#endif
                "Emerald",
                "crashes",
                $"crash_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            );

            Directory.CreateDirectory(Path.GetDirectoryName(crashPath));
            File.WriteAllText(crashPath, $"""
                Exception Details:
                Time: {DateTime.Now}
                Platform: {DirectResoucres.Platform}
                Type: {exception.GetType().FullName}
                Message: {exception.Message}
                Stack Trace: {exception.StackTrace}
                """);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to log unhandled exception: {ex.Message}");
        }
    }

    private async void ShowPlatformErrorDialog(string message)
    {
        try
        {
            await MessageBox.Show("AppCrash".Localize(), message, Helpers.Enums.MessageBoxButtons.Ok);
            Application.Current.Exit();

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {message}");
        }
        Process.GetCurrentProcess().Kill();
    }
    #endregion
}
