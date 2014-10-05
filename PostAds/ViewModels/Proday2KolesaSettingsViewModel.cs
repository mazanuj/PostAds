namespace Motorcycle.ViewModels
{
    using Caliburn.Micro;
    using Motorcycle.XmlWorker;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Windows;

    [Export(typeof(Proday2KolesaSettingsViewModel))]
    public class Proday2KolesaSettingsViewModel : PropertyChangedBase
    {
        private readonly IWindowManager _windowManager;

        private Proday2KolesaItem _selectedItemCollection;

        public ObservableCollection<Proday2KolesaItem> ItemCollection { get; private set; }

        [ImportingConstructor]
        public Proday2KolesaSettingsViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            ItemCollection = new ObservableCollection<Proday2KolesaItem>();

            GetItemsFromXmlFile();
        }

        public void RemoveItem(Proday2KolesaItem item)
        {
            Proday2KolesaXmlWorker.RemoveItemNode(item);

            RefreshItemList();
        }

        public void AddNewItem()
        {
            ShowConfirmationItemDialog(null);
        }

        public void ChangeItem(Proday2KolesaItem item)
        {
            ShowConfirmationItemDialog(item);
        }

        public void CopyItemId(Proday2KolesaItem item)
        {
            Clipboard.SetText(item.Id);
        }

        private void RefreshItemList()
        {
            ItemCollection.Clear();

            GetItemsFromXmlFile();
        }

        private void ShowConfirmationItemDialog(Proday2KolesaItem currentItem)
        {
            var proday2KolesaViewModel = new AddChangeProday2KolesaItemViewModel(currentItem);

            _windowManager.ShowDialog(proday2KolesaViewModel);

            if (proday2KolesaViewModel.IsOkay)
            {
                RefreshItemList();
            }
        }

        private void GetItemsFromXmlFile()
        {
            foreach (var item in Proday2KolesaXmlWorker.GetItems())
            {
                ItemCollection.Add(item);
            }
        }
    }
}
