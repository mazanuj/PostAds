using System.ComponentModel.Composition;
using System.Windows.Data;
using System.Xml;
using Caliburn.Micro;
using NLog;
using LogManager = NLog.LogManager;

namespace Motorcycle.ViewModels
{
    using System.Collections.ObjectModel;

    using XmlWorker;

    [Export(typeof(GeneralSettingsViewModel))]
    public class GeneralSettingsViewModel : PropertyChangedBase
    {
        private static XmlDataProvider xml;
        private const string dbPath = "Main.config";
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly IWindowManager _windowManager;

        public ObservableCollection<CityItem> ItemCollection { get; private set; }

        [ImportingConstructor]
        public GeneralSettingsViewModel(IWindowManager windowManager)
        {
            xml = new XmlDataProvider { Document = new XmlDocument() };
            xml.Document.Load(dbPath);

            _windowManager = windowManager;

            ItemCollection = new ObservableCollection<CityItem>();
            GetItemsFromXmlFile();
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

        #region CityXml

        public void RemoveItem(CityItem item)
        {
            CityXmlWorker.RemoveItemNode(item.CityName);

            RefreshItemList();
        }

        public void AddNewItem()
        {
            ShowConfirmationItemDialog(null);
        }

        public void ChangeItem(CityItem item)
        {
            ShowConfirmationItemDialog(item);
        }

        private void RefreshItemList()
        {
            ItemCollection.Clear();

            GetItemsFromXmlFile();
        }

        private void ShowConfirmationItemDialog(CityItem currentItem)
        {
            var addChangeCityViewModel = new AddChangeCityViewModel(currentItem);

            _windowManager.ShowDialog(addChangeCityViewModel);

            if (addChangeCityViewModel.IsOkay)
            {
                RefreshItemList();
            }
        }


        private void GetItemsFromXmlFile()
        {
            foreach (var item in CityXmlWorker.GetItems())
            {
                ItemCollection.Add(item);
            }
        }

        #endregion
    }
}