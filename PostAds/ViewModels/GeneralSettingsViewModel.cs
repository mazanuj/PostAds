namespace Motorcycle.ViewModels
{
    using Caliburn.Micro;
    using Config.Proxy;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;
    using System.Xml;
    using Utils;
    using XmlWorker;

    [Export(typeof (GeneralSettingsViewModel))]
    public class GeneralSettingsViewModel : PropertyChangedBase
    {
        private static XmlDataProvider xml;
        private const string DbPath = "Main.config";

        private readonly IWindowManager _windowManager;
        private int countOfProxyAddressInFile;
        private string password = "";

        public int CountOfProxyAddressInFile
        {
            get
            {
                countOfProxyAddressInFile = ProxyXmlWorker.GetProxyListFromFile().Count;
                return countOfProxyAddressInFile;
            }
            set { countOfProxyAddressInFile = value; }
        }

        public bool CanRefreshProxyListFromInternet { get; set; }

        public ObservableCollection<CityItem> ItemCollection { get; private set; }
        public ObservableCollection<ProxyAddressItem> ProxyAddressCollection { get; private set; }

        [ImportingConstructor]
        public GeneralSettingsViewModel(IWindowManager windowManager)
        {
            xml = new XmlDataProvider {Document = new XmlDocument()};
            xml.Document.Load(DbPath);

            _windowManager = windowManager;

            ItemCollection = new ObservableCollection<CityItem>();
            GetItemsFromFile();

            ProxyAddressCollection = new ObservableCollection<ProxyAddressItem>();
            GetProxyAddressItemsFromFile();

            CanRefreshProxyListFromInternet = true;
        }

        public async void RefreshProxyListFromInternet()
        {
            IsLoadingAnimationVisible = true;
            NotifyOfPropertyChange(() => IsLoadingAnimationVisible); //start animation

            //RefreshProxyListStatus = true;
            //NotifyOfPropertyChange(() => RefreshProxyListStatus);

            CanRefreshProxyListFromInternet = false;
            NotifyOfPropertyChange(() => CanRefreshProxyListFromInternet);

            Informer.RaiseOnProxyListFromInternetUpdatedEvent(false); //disable FrontPanel

            await TaskEx.Run(
                () => ProxyXmlWorker.AddNewProxyListToFile(ProxyData.GetProxyDataAllAtOnce()));

            //RefreshProxyListStatus = false;
            //NotifyOfPropertyChange(() => RefreshProxyListStatus);

            CanRefreshProxyListFromInternet = true;
            NotifyOfPropertyChange(() => CanRefreshProxyListFromInternet);

            NotifyOfPropertyChange(() => CountOfProxyAddressInFile);
            Informer.RaiseOnProxyListFromInternetUpdatedEvent(true); //enable FrontPanel

            RefreshProxyAddressItemList();

            IsLoadingAnimationVisible = false;
            NotifyOfPropertyChange(() => IsLoadingAnimationVisible); //end animation
        }


        public bool IsLoadingAnimationVisible { get; set; }

        public void ClearProxyFile()
        {
            ProxyXmlWorker.RemoveAllProxyAddressesFromFile();

            NotifyOfPropertyChange(() => CountOfProxyAddressInFile);
            RefreshProxyAddressItemList();
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
            xml.Document.Save(DbPath);
        }

        #region Password

        public string Password
        {
            get { return PasswordXmlWorker.GetPasswordValue(); }
            set { password = value; }
        }

        public void ChangePassword()
        {
            PasswordXmlWorker.ChangePasswordNode(password);
        }

        #endregion

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

        public void CopyCityName(CityItem item)
        {
            Clipboard.SetText(item.CityName);
        }

        private void RefreshItemList()
        {
            ItemCollection.Clear();

            GetItemsFromFile();
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

        private void GetItemsFromFile()
        {
            foreach (var item in CityXmlWorker.GetItems())
            {
                ItemCollection.Add(item);
            }
        }

        #endregion

        #region ProxyXml

        public void RemoveProxyAddressItem(ProxyAddressItem item)
        {
            ProxyXmlWorker.RemoveProxyAddressFromFile(item.ProxyAddress);
            RefreshProxyAddressItemList();
        }

        public void AddNewProxyAddressItem()
        {
            ShowConfirmationProxyAddressItemDialog(null);
        }

        public void ChangeProxyAddressItem(ProxyAddressItem item)
        {
            ShowConfirmationProxyAddressItemDialog(item);
        }

        public void CopyProxyAddress(ProxyAddressItem item)
        {
            Clipboard.SetText(item.ProxyAddress);
        }

        private void RefreshProxyAddressItemList()
        {
            ProxyAddressCollection.Clear();

            GetProxyAddressItemsFromFile();
        }

        private void ShowConfirmationProxyAddressItemDialog(ProxyAddressItem currentItem)
        {
            var addChangeProxyAddressViewModel = new AddChangeProxyAddressViewModel(currentItem);

            _windowManager.ShowDialog(addChangeProxyAddressViewModel);

            if (addChangeProxyAddressViewModel.IsOkay)
            {
                RefreshProxyAddressItemList();
            }
        }

        private void GetProxyAddressItemsFromFile()
        {
            foreach (var item in ProxyXmlWorker.GetProxyAddressItemsListFromFile())
            {
                ProxyAddressCollection.Add(item);
            }
        }

        #endregion
    }
}