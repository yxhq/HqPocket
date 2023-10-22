using HqPocket.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;

namespace HqPocket.Test.ViewModels
{
    public class OptionsMonitorTestViewModel : ObservableObject
    {
      
        public AppSettings AppSettings { get; }

        public OptionsMonitorTestViewModel(IOptions<AppSettings> options)
        {
            AppSettings = options.Value;
        }
    }
}
