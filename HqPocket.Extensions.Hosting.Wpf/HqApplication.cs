using HqPocket.Extensions.Plugins;
using HqPocket.Extensions.Regioning;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Windows;

namespace HqPocket.Extensions.Hosting;

public abstract class HqApplication : Application
{
    private Type? _shellType;
    public IHost? HqHost { get; private set; }
    public static event EventHandler? MainWindowShown;
    protected virtual string ShellName { get; set; } = Conventions.ShellName;

    protected override async void OnStartup(StartupEventArgs e)
    {
        _shellType = GetShellType();
        ArgumentNullException.ThrowIfNull(_shellType);

        HqHost = CreateHostBuilder(e.Args).Build();
        await HqHost.StartAsync();

        var services = HqHost.Services;
        ConfigureIoc(services);
        ConfigureVvmTypeLocator();
        ConfigureRegionAdapters(services.GetRequiredService<IRegionAdapterFactory>());
        ConfigureRegionViews(services.GetRequiredService<IRegionManager>());
        InitializePlugins(services);

        BeforeMainWindowInitialized(services);
        MainWindow = services.GetRequiredService(_shellType) as Window;
        if (MainWindow is not null)
        {
            AfterMainWindowInitialized(services);
            MainWindow.Show();
            OnMainWindowShown();
        }
    }

    protected virtual IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(ConfigureAppConfiguration)
            .ConfigureServices(ConfigureServices);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        using (HqHost)
        {
            await HqHost!.StopAsync();
        }
    }

    protected virtual Type? GetShellType()
    {
        var typeInfo = ResourceAssembly.DefinedTypes.SingleOrDefault(t => t.Name == ShellName && t.IsAssignableTo(typeof(Window)));
        return typeInfo?.AsType();
    }

    protected virtual void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.AddWritableJsonFile(Conventions.WritableJsonFile);
    }

    protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddSingleton(_shellType!);

        services.AddNotifiedLocalization(options => options.ResourcesPath = Conventions.ResourceDirectory);
        services.AddEventAggregator();
        services.AddDialoging();
        services.AddRegioning();
        services.AddJsonOptionsWriter(options => options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All));
        services.AddPlugins(context, builder => builder.AddPluginsDirectory(Conventions.PluginsDirectory));
    }

    protected virtual void ConfigureIoc(IServiceProvider serviceProvider)
    {
        Ioc.SetServiceProvider(serviceProvider);
    }

    protected virtual void ConfigureVvmTypeLocator()
    {
    }

    protected virtual void ConfigureRegionAdapters(IRegionAdapterFactory regionAdapterFactory)
    {
        regionAdapterFactory.AddDefaultRegionAdapters();
    }

    protected virtual void ConfigureRegionViews(IRegionManager regionManager)
    {
    }

    protected virtual void InitializePlugins(IServiceProvider serviceProvider)
    {
        PluginsInitializer.Initialize(serviceProvider);
    }

    protected virtual void BeforeMainWindowInitialized(IServiceProvider serviceProvider)
    {

    }

    protected virtual void AfterMainWindowInitialized(IServiceProvider serviceProvider)
    {

    }

    protected virtual void OnMainWindowShown()
    {
        MainWindowShown?.Invoke(this, EventArgs.Empty);
    }
}