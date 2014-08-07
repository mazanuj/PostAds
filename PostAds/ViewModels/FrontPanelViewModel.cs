using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using NLog;

namespace Motorcycle.ViewModels
{
    [Export(typeof (FrontPanelViewModel))]
    public class FrontPanelViewModel : PropertyChangedBase
    {
        private readonly Logger log = NLog.LogManager.GetCurrentClassLogger();
        public LoggingControlViewModel LoggingControl { get; private set; }
        private readonly Microsoft.Win32.OpenFileDialog dlg;
        private string motoFile;
        private string equipmentFile;
        private string spareFile;


        [ImportingConstructor]
        public FrontPanelViewModel(LoggingControlViewModel loggingControlModel)
        {
            LoggingControl = loggingControlModel;

            // Create OpenFileDialog
            dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };
        }

        public void ButtonStart()
        {

        }

        public bool CanButtonStart
        {
            get { return MotoFileLabel || SpareFileLabel || EquipFileLabel; }
        }

        public bool MotoFileLabel { get; set; }

        public void ButtonMoto()
        {
            if (dlg.ShowDialog() == false) return;
            motoFile = dlg.FileName;

            MotoFileLabel = true;
            NotifyOfPropertyChange(() => MotoFileLabel);
            NotifyOfPropertyChange(() => CanButtonStart);
        }

        public bool SpareFileLabel { get; set; }

        public void ButtonSpare()
        {
            if (dlg.ShowDialog() == false) return;
            spareFile = dlg.FileName;

            SpareFileLabel = true;
            NotifyOfPropertyChange(() => SpareFileLabel);
            NotifyOfPropertyChange(() => CanButtonStart);
        }
        public bool EquipFileLabel { get; set; }

        public void ButtonEquip()
        {
            if (dlg.ShowDialog() == false) return;
            equipmentFile = dlg.FileName;

            EquipFileLabel = true;
            NotifyOfPropertyChange(() => EquipFileLabel);
            NotifyOfPropertyChange(() => CanButtonStart);
        }
    }
}