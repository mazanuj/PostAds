using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Caliburn.Micro;

namespace Motorcycle.ViewModels
{
    [Export(typeof (FrontPanelViewModel))]
    public class FrontPanelViewModel : PropertyChangedBase
    {
        public LoggingControlViewModel LoggingControl { get; private set; }

        [ImportingConstructor]
        public FrontPanelViewModel(LoggingControlViewModel loggingControlModel)
        {
            LoggingControl = loggingControlModel;
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
            MotoFileLabel = true;
            NotifyOfPropertyChange(() => MotoFileLabel);
            NotifyOfPropertyChange(() => CanButtonStart);
        }

        public bool SpareFileLabel { get; set; }

        public void ButtonSpare()
        {
            SpareFileLabel = true;
            NotifyOfPropertyChange(() => SpareFileLabel);
            NotifyOfPropertyChange(() => CanButtonStart);
        }
        public bool EquipFileLabel { get; set; }

        public void ButtonEquip()
        {
            EquipFileLabel = true;
            NotifyOfPropertyChange(() => EquipFileLabel);
            NotifyOfPropertyChange(() => CanButtonStart);
        }
    }
}