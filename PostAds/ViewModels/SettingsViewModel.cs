using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Motorcycle.ViewModels
{
    [Export(typeof (SettingsViewModel))]
    public class SettingsViewModel : PropertyChangedBase
    {
        public GeneralSettingsViewModel GeneralSettings { get; private set; }
        public ChangeBaseViewModel ChangeBase { get; private set; }
        public ProxyViewModel Proxy { get; private set; }

        [ImportingConstructor]
        public SettingsViewModel(GeneralSettingsViewModel generalSettingsModel, ChangeBaseViewModel changeBaseModel,
            ProxyViewModel proxyModel)
        {
            GeneralSettings = generalSettingsModel;
            ChangeBase = changeBaseModel;
            Proxy = proxyModel;
        }
    }
}