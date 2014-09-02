
namespace Motorcycle.ViewModels
{
    ﻿using Caliburn.Micro;
    using Motorcycle.XmlWorker;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;

    [Export(typeof(ChangeBaseViewModel))]
    public class ChangeBaseViewModel : PropertyChangedBase
    {
        private readonly IWindowManager _windowManager;

        private ManufactureItem _selectedItemCollection;

        public ObservableCollection<ManufactureItem> ItemCollection { get; private set; }

        public ObservableCollection<ManufactureValue> ValueCollection { get; private set; }

        [ImportingConstructor]
        public ChangeBaseViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            ItemCollection = new ObservableCollection<ManufactureItem>();

            this.GetItemsFromXmlFile();

            ValueCollection = new ObservableCollection<ManufactureValue>();
        }

        public ManufactureItem SelectedItemCollection
        {
            get
            {
                return this._selectedItemCollection;
            }
            set
            {
                this._selectedItemCollection = value;
                //NotifyOfPropertyChange(() => this.SelectedItemCollection);

                this.GetValuesForItem();
            }
        }

        #region Item's context menu methods

        public void RemoveItem(ManufactureItem item)
        {
            ManufactureXmlWorker.RemoveItemNode(item);

            this.ValueCollection.Clear();
            this.RefreshItemList();
        }

        public void AddNewItem()
        {
            this.ShowConfirmationItemDialog(null);
        }

        public void ChangeItem(ManufactureItem item)
        {
            this.ShowConfirmationItemDialog(item);
        }

        #endregion

        #region Value's context menu methods

        public void RemoveValue(ManufactureValue value)
        {
            ManufactureXmlWorker.RemoveValueNodeUsingItemNode(_selectedItemCollection, value);

            this.GetValuesForItem();
        }

        public void AddNewValue()
        {
            if (_selectedItemCollection == null) return;

            this.ShowConfirmationValueDialog(_selectedItemCollection, null);
        }

        public void ChangeValue(ManufactureValue value)
        {
            this.ShowConfirmationValueDialog(_selectedItemCollection, value);
        }

        #endregion

        #region Private methods

        private void RefreshItemList()
        {
            this.ItemCollection.Clear();

            this.GetItemsFromXmlFile();
        }

        private void ShowConfirmationItemDialog(ManufactureItem currentItem)
        {
            var confirmationViewModel = new ConfirmationViewModel(currentItem);

            this._windowManager.ShowDialog(confirmationViewModel);

            if (confirmationViewModel.IsOkay)
            {
                this.RefreshItemList();
            }
        }

        private void ShowConfirmationValueDialog(ManufactureItem currentItem, ManufactureValue currentValue)
        {
            var confirmationValueViewModel = new ConfirmationValueViewModel(currentItem, currentValue);

            this._windowManager.ShowDialog(confirmationValueViewModel);

            if (confirmationValueViewModel.IsOkay)
            {
                this.GetValuesForItem();
            }
        }

        private void GetItemsFromXmlFile()
        {
            foreach (var item in ManufactureXmlWorker.GetItemsWithTheirValues())
            {
                this.ItemCollection.Add(item);
            }
        }

        private void GetValuesForItem()
        {
            if (this._selectedItemCollection != null)
            {
                this.ValueCollection.Clear();

                foreach (var val in ManufactureXmlWorker.GetValuesForItem(_selectedItemCollection))
                {
                    this.ValueCollection.Add(val);
                }
            }
        }

        #endregion
    }
}
