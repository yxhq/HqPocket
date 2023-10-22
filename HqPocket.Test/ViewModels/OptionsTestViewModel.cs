using HqPocket.Extensions.Options;
using HqPocket.Mvvm.Input;
using Mapster;
using Microsoft.Extensions.Options;
using System;
using System.Windows.Input;

namespace HqPocket.Test.ViewModels
{
    public class OptionsTestViewModel
    {
        private readonly IOptions<AppSettings> _options;

        public ICommand SaveAppSettingCommand { get; }
        public AppSettings AppSettings { get; set; }    

        public OptionsTestViewModel(IOptions<AppSettings> options)
        {
            _options = options;
            AppSettings = options.Value.Adapt<AppSettings>();
            AppSettings.ProcessCount++;

            SaveAppSettingCommand = new RelayCommand(SaveAppSetting);
        }

        private void SaveAppSetting()
        {
            AppSettings.Adapt(_options.Value);
            _options.UpdateAndWrite();
        }
    }
}
