using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using NLog;

namespace Motorcycle.ViewModels
{
    [Export(typeof (ChangeBaseViewModel))]
    public class ChangeBaseViewModel : PropertyChangedBase
    {
        public static ObservableCollection<LogEventInfo> LogCollection { get; private set; }
    }
}