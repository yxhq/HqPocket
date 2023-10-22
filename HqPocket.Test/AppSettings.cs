using HqPocket.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;

namespace HqPocket.Test
{
    public class AppSettings : ObservableObject, IOptions<AppSettings>
    {
        private string? _appName;
        public string? AppName
        {
            get => _appName;
            set => SetValue(ref _appName, value);
        }

        private int _processCount;
        public int ProcessCount
        {
            get => _processCount;
            set => SetValue(ref _processCount, value);
        }


        AppSettings IOptions<AppSettings>.Value => this;
    }
}
