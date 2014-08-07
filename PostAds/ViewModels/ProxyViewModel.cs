using System.ComponentModel.Composition;
using Caliburn.Micro;
using NLog;
using LogManager = NLog.LogManager;

namespace Motorcycle.ViewModels
{
    [Export(typeof (ProxyViewModel))]
    public class ProxyViewModel : PropertyChangedBase
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();

    }
}