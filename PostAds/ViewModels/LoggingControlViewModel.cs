using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using Caliburn.Micro;
using Motorcycle.Controls;
using Motorcycle.Controls.Log;
using NLog;
using NLog.Config;
using Action = System.Action;
using LogManager = NLog.LogManager;

namespace Motorcycle.ViewModels
{
    [Export(typeof(LoggingControlViewModel))]
    public class LoggingControlViewModel : PropertyChangedBase
    {
        public static ObservableCollection<LogEventInfo> LogCollection { get; private set; }

        [ImportingConstructor]
        public LoggingControlViewModel()
        {
            LogCollection = new ObservableCollection<LogEventInfo>();

            //initialize default NLog rules
            ConfigurationItemFactory.Default.Targets.RegisterDefinition("MemoryEvent", typeof(MemoryEventTarget));

            var config = LogManager.Configuration;
            var _logTarget = new MemoryEventTarget();
            _logTarget.EventReceived += EventReceived;

            config.AddTarget("memoryevent", _logTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, _logTarget));
            SimpleConfigurator.ConfigureForTargetLogging(_logTarget, LogLevel.Debug);
            LogManager.Configuration = config;
        }

        private static void EventReceived(LogEventInfo message)
        {
            Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
            {
                if (LogCollection.Count >= 50) LogCollection.RemoveAt(LogCollection.Count - 1);
                LogCollection.Add(message);
            }));
        }

        public void SendLog()
        {
            MailSender.SendEmail("mazanuj@gmail.com", "postads@ya.ru", "Log", "See in attachment");
        }

        public void ClearLog()
        {
            LogCollection.Clear();
        }
    }
}