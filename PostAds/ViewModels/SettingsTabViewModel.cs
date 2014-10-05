namespace Motorcycle.ViewModels
{
    using System.ComponentModel.Composition;
    using Caliburn.Micro;
    using NLog;
    using LogManager = NLog.LogManager;

    [Export(typeof (SettingsTabViewModel))]
    public class SettingsTabViewModel : PropertyChangedBase
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        public GeneralSettingsViewModel GeneralSettings { get; private set; }
        public MotosaleSettingsViewModel MotosaleSettings { get; private set; }
        public Proday2KolesaSettingsViewModel Proday2KolesaSettings { get; private set; }
        

        [ImportingConstructor]
        public SettingsTabViewModel(GeneralSettingsViewModel generalSettingsModel,
            MotosaleSettingsViewModel motosaleSettingsModel,
            Proday2KolesaSettingsViewModel proday2KolesaSettings)
        {
            GeneralSettings = generalSettingsModel;
            MotosaleSettings = motosaleSettingsModel;
            Proday2KolesaSettings = proday2KolesaSettings;
        }
    }
}