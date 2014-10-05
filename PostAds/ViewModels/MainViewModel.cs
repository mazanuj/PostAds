using System.ComponentModel.Composition;
using Caliburn.Micro;
using NLog;
using LogManager = NLog.LogManager;

namespace Motorcycle.ViewModels
{
    [Export(typeof(MainViewModel))]
    public class MainViewModel : PropertyChangedBase
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public FrontPanelViewModel FrontPanel { get; private set; }
        public SettingsTabViewModel Settings { get; private set; }

        [ImportingConstructor]
        public MainViewModel(FrontPanelViewModel frontPanelModel, SettingsTabViewModel settingsModel)
        {
            FrontPanel = frontPanelModel;
            Settings = settingsModel;
        }
    }
}