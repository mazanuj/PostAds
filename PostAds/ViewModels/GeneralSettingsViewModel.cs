using System.ComponentModel.Composition;
using System.Windows.Data;
using System.Xml;
using Caliburn.Micro;
using NLog;

namespace Motorcycle.ViewModels
{
    [Export(typeof (GeneralSettingsViewModel))]
    public class GeneralSettingsViewModel : PropertyChangedBase
    {
        private static XmlDataProvider xml;
        private const string dbPath = "Main.config";
        private readonly Logger log = NLog.LogManager.GetCurrentClassLogger();

        [ImportingConstructor]
        public GeneralSettingsViewModel()
        {
            xml = new XmlDataProvider {Document = new XmlDocument()};
            xml.Document.Load(dbPath);
        }

        public string CaptchaKey
        {
            get
            {
                var selectSingleNode = xml.Document.SelectSingleNode("/configuration/captcha/key");
                return selectSingleNode == null ? string.Empty : selectSingleNode.InnerText;
            }
            set
            {
                var selectSingleNode = xml.Document.SelectSingleNode("/configuration/captcha/key");
                if (selectSingleNode != null)
                    selectSingleNode.InnerText = value;
                NotifyOfPropertyChange(() => CaptchaKey);
            }
        }

        public string CaptchaDomain
        {
            get
            {
                var selectSingleNode = xml.Document.SelectSingleNode("/configuration/captcha/domain");
                return selectSingleNode == null ? string.Empty : selectSingleNode.InnerText;
            }
            set
            {
                var selectSingleNode = xml.Document.SelectSingleNode("/configuration/captcha/domain");
                if (selectSingleNode != null)
                    selectSingleNode.InnerText = value;
                NotifyOfPropertyChange(() => CaptchaDomain);
            }
        }

        public void ChangeCaptcha()
        {
            xml.Document.Save(dbPath);
        }
    }
}