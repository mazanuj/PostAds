using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Motorcycle.Controls.Log;
using NLog;
using NLog.Config;

namespace Motorcycle.Controls
{
    public partial class LoggingControl : UserControl
    {
        public static ObservableCollection<LogEventInfo> LogCollection { get; set; }

        public LoggingControl()
        {
            LogCollection = new ObservableCollection<LogEventInfo>();

            InitializeComponent();

            //initialize default NLog rules
            ConfigurationItemFactory.Default.Targets.RegisterDefinition("MemoryEvent", typeof(MemoryEventTarget));

            var config = LogManager.Configuration;
            var _logTarget = new MemoryEventTarget();
            _logTarget.EventReceived += EventReceived;

            config.AddTarget("memoryevent", _logTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, _logTarget));
            SimpleConfigurator.ConfigureForTargetLogging(_logTarget, LogLevel.Debug);
            LogManager.Configuration = config;
        }

        private void EventReceived(LogEventInfo message)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (LogCollection.Count >= 50) LogCollection.RemoveAt(LogCollection.Count - 1);
                LogCollection.Add(message);
            }));
        }

        private void SendLog_OnClick(object sender, RoutedEventArgs e)
        {
            MailSender.SendEmail("mazanuj@gmail.com", "postads@ya.ru", "ERROR", "See in attachment");
        }
    }
}