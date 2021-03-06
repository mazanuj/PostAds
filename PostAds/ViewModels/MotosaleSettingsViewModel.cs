﻿namespace Motorcycle.ViewModels
{
    using System.Windows;

    using Caliburn.Micro;
    using XmlWorker;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;

    [Export(typeof (MotosaleSettingsViewModel))]
    public class MotosaleSettingsViewModel : PropertyChangedBase
    {
        private readonly IWindowManager _windowManager;

        private ManufactureItem _selectedItemCollection;

        public ObservableCollection<ManufactureItem> ItemCollection { get; private set; }

        public ObservableCollection<ManufactureValue> ValueCollection { get; private set; }

        [ImportingConstructor]
        public MotosaleSettingsViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            ItemCollection = new ObservableCollection<ManufactureItem>();

            GetItemsFromXmlFile();

            ValueCollection = new ObservableCollection<ManufactureValue>();
        }

        public ManufactureItem SelectedItemCollection
        {
            get { return _selectedItemCollection; }
            set
            {
                _selectedItemCollection = value;
                //NotifyOfPropertyChange(() => this.SelectedItemCollection);

                GetValuesForItem();
            }
        }

        #region Item's context menu methods

        public void RemoveItem(ManufactureItem item)
        {
            ManufactureXmlWorker.RemoveItemNode(item);

            ValueCollection.Clear();
            RefreshItemList();
        }

        public void AddNewItem()
        {
            ShowConfirmationItemDialog(null);
        }

        public void ChangeItem(ManufactureItem item)
        {
            ShowConfirmationItemDialog(item);
        }

        public void CopyItemId(ManufactureItem item)
        {
            Clipboard.SetText(item.Id);
        }

        #endregion

        #region Value's context menu methods

        public void RemoveValue(ManufactureValue value)
        {
            ManufactureXmlWorker.RemoveValueNodeUsingItemNode(_selectedItemCollection, value);

            GetValuesForItem();
        }

        public void AddNewValue()
        {
            if (_selectedItemCollection == null) return;

            ShowConfirmationValueDialog(_selectedItemCollection, null);
        }

        public void ChangeValue(ManufactureValue value)
        {
            ShowConfirmationValueDialog(_selectedItemCollection, value);
        }

        public void CopyValueName(ManufactureValue value)
        {
            Clipboard.SetText(value.Name);
        }
        #endregion

        #region Private methods

        private void RefreshItemList()
        {
            ItemCollection.Clear();

            GetItemsFromXmlFile();
        }

        private void ShowConfirmationItemDialog(ManufactureItem currentItem)
        {
            var confirmationViewModel = new AddChangeMotosaleItemViewModel(currentItem);

            _windowManager.ShowDialog(confirmationViewModel);

            if (confirmationViewModel.IsOkay)
            {
                RefreshItemList();
            }
        }

        private void ShowConfirmationValueDialog(ManufactureItem currentItem, ManufactureValue currentValue)
        {
            var confirmationValueViewModel = new AddChangeMotosaleValueViewModel(currentItem, currentValue);

            _windowManager.ShowDialog(confirmationValueViewModel);

            if (confirmationValueViewModel.IsOkay)
            {
                GetValuesForItem();
            }
        }

        private void GetItemsFromXmlFile()
        {
            foreach (var item in ManufactureXmlWorker.GetItemsWithTheirValues())
            {
                ItemCollection.Add(item);
            }
        }

        private void GetValuesForItem()
        {
            if (_selectedItemCollection == null) return;
            ValueCollection.Clear();

            foreach (var val in ManufactureXmlWorker.GetValuesForItem(_selectedItemCollection))
            {
                ValueCollection.Add(val);
            }
        }

        #endregion
    }
}