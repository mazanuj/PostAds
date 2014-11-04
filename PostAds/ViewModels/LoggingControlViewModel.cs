using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using Caliburn.Micro;
using Motorcycle.Controls.Log;
using NLog;
using NLog.Config;
using Action = System.Action;
using LogManager = NLog.LogManager;

namespace Motorcycle.ViewModels
{
    using System.Windows;

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
            var logTarget = new MemoryEventTarget();
            logTarget.EventReceived += EventReceived;

            config.AddTarget("memoryevent", logTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, logTarget));
            SimpleConfigurator.ConfigureForTargetLogging(logTarget, LogLevel.Info);
            LogManager.Configuration = config;
        }

        private static void EventReceived(LogEventInfo message)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(
                    () => LogCollection.Insert(0, message)));
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