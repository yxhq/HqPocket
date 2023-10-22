using HqPocket.Extensions.Hosting;
using HqPocket.Extensions.Regioning;
using HqPocket.Test.ViewModels;
using HqPocket.Test.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HqPocket.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : HqApplication
    {
        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            base.ConfigureServices(context, services);

            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<OptionsTestViewModel>();
            services.AddSingleton<OptionsTestView>();

            services.AddSingleton<OptionsMonitorTestViewModel>();
            services.AddSingleton<OptionsMonitorTestView>();

            services.ConfigureWithDefaultSection<AppSettings>(context);
        }

        protected override void ConfigureRegionViews(IRegionManager regionManager)
        {          
            regionManager.GetRegion(RegionNames.MainRegionName)
                .AddView<OptionsTestView>()
                .AddView<OptionsMonitorTestView>();
        }
    }
}
