﻿using System.ComponentModel.Composition;
using Caliburn.Micro;
using NLog;

namespace Motorcycle.ViewModels
{
    [Export(typeof (SettingsViewModel))]
    public class SettingsViewModel : PropertyChangedBase
    {
        private readonly Logger log = NLog.LogManager.GetCurrentClassLogger();
        public GeneralSettingsViewModel GeneralSettings { get; private set; }
        public ChangeBaseViewModel ChangeBase { get; private set; }
        

        [ImportingConstructor]
        public SettingsViewModel(GeneralSettingsViewModel generalSettingsModel, ChangeBaseViewModel changeBaseModel)
        {
            GeneralSettings = generalSettingsModel;
            ChangeBase = changeBaseModel;
        }
    }
}