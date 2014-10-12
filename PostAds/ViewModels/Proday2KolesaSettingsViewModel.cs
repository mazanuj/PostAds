namespace Motorcycle.ViewModels
{
    using Caliburn.Micro;
    using XmlWorker;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
    using System.Windows;

    [Export(typeof (Proday2KolesaSettingsViewModel))]
    public class Proday2KolesaSettingsViewModel : PropertyChangedBase
    {
        private readonly IWindowManager _windowManager;

        private SpareEquipItem _selectedItemCollection;

        public ObservableCollection<SpareEquipItem> ItemCollection { get; private set; }

        [ImportingConstructor]
        public Proday2KolesaSettingsViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            ItemCollection = new ObservableCollection<SpareEquipItem>();

            GetItemsFromXmlFile();
        }

        public void RemoveItem(SpareEquipItem item)
        {
            SpareEquipXmlWorker.RemoveItemNode(item);
            RefreshItemList();
        }

        public void AddNewItem()
        {
            ShowConfirmationItemDialog(null);
        }

        public void ChangeItem(SpareEquipItem item)
        {
            ShowConfirmationItemDialog(item);
        }

        public void CopyItemId(SpareEquipItem item)
        {
            Clipboard.SetText(item.Id);
        }

        private void RefreshItemList()
        {
            ItemCollection.Clear();
            GetItemsFromXmlFile();
        }

        private void ShowConfirmationItemDialog(SpareEquipItem currentItem)
        {
            var proday2KolesaViewModel = new AddChangeProday2KolesaItemViewModel(currentItem);
            _windowManager.ShowDialog(proday2KolesaViewModel);

            if (proday2KolesaViewModel.IsOkay)
                RefreshItemList();
        }

        private void GetItemsFromXmlFile()
        {
            foreach (var item in SpareEquipXmlWorker.GetAllItems())
                ItemCollection.Add(item);
        }
    }
}