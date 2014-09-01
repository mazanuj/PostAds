using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Motorcycle.Config;
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
        private byte[] flag = new byte[3];

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
            Advertising.Initialize(motoFile, spareFile, equipmentFile, flag);            
        }

        public bool CanButtonStart
        {
            get { return (MotoFileLabel || SpareFileLabel || EquipFileLabel) && (flag[0] + flag[1] + flag[2] != 0); }
        }

        private bool _BoxMoto;

        public bool BoxMoto
        {
            get { return _BoxMoto; }
            set
            {
                _BoxMoto = value;
                flag[0] = _BoxMoto ? flag[0] += 1 : flag[0] -= 1;
                NotifyOfPropertyChange(() => BoxMoto);
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        private bool _BoxUsed;
        public bool BoxUsed
        {
            get { return _BoxUsed; }
            set
            {
                _BoxUsed = value;
                flag[1] = _BoxUsed ? flag[1] += 1 : flag[1] -= 1;
                NotifyOfPropertyChange(() => BoxUsed);
                NotifyOfPropertyChange(() => CanButtonStart);
            }
        }

        private bool _BoxKol;
        public bool BoxKol
        {
            get { return _BoxKol; }
            set
            {
                _BoxKol = value;
                flag[2] = _BoxKol ? flag[2] += 1 : flag[2] -= 1;
                NotifyOfPropertyChange(() => BoxKol);
                NotifyOfPropertyChange(() => CanButtonStart);
            }
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