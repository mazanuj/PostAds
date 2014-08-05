using System.ComponentModel.Composition;
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
    }
}