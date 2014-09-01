<<<<<<< HEAD

namespace Motorcycle.ViewModels
{
    ﻿using Caliburn.Micro;
    using Motorcycle.XmlWorker;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;
=======
﻿using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
﻿using NLog;

namespace Motorcycle.ViewModels
{
    using XmlWorker;
>>>>>>> mazanuj

    [Export(typeof (ChangeBaseViewModel))]
    public class ChangeBaseViewModel : PropertyChangedBase
    {
<<<<<<< HEAD
        private readonly IWindowManager _windowManager;

        private Item _selectedItemCollection;

=======
        private readonly Logger log = NLog.LogManager.GetCurrentClassLogger();
>>>>>>> mazanuj
        public ObservableCollection<Item> ItemCollection { get; private set; }

        public ObservableCollection<Value> ValueCollection { get; private set; }

        [ImportingConstructor]
        public ChangeBaseViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            ItemCollection = new ObservableCollection<Item>();

            this.GetItemsFromXmlFile();

            ValueCollection = new ObservableCollection<Value>();
        }

        public Item SelectedItemCollection
        {
<<<<<<< HEAD
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

        public void RemoveItem(Item item)
        {
            ChangeBaseXmlWorker.RemoveItemNode(item);

            this.ValueCollection.Clear();
            this.RefreshItemList();
        }

        public void AddNewItem()
        {
            this.ShowConfirmationItemDialog(null);
        }

        public void ChangeItem(Item item)
        {
            this.ShowConfirmationItemDialog(item);
        }

        #endregion

        #region Value's context menu methods

        public void RemoveValue(Value value)
        {
            ChangeBaseXmlWorker.RemoveValueNodeUsingItemNode(_selectedItemCollection, value);

            this.GetValuesForItem();
        }

        public void AddNewValue()
        {
            if(_selectedItemCollection == null) return;

            this.ShowConfirmationValueDialog(_selectedItemCollection, null);
        }

        public void ChangeValue(Value value)
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

        private void ShowConfirmationItemDialog(Item currentItem)
        {
            var confirmationViewModel = new ConfirmationViewModel(currentItem);

            this._windowManager.ShowDialog(confirmationViewModel);

            if (confirmationViewModel.IsOkay)
            {
                this.RefreshItemList();
            }
        }
=======
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                //NotifyOfPropertyChange("SelectedItem");
>>>>>>> mazanuj

        private void ShowConfirmationValueDialog(Item currentItem, Value currentValue)
        {
            var confirmationValueViewModel = new ConfirmationValueViewModel(currentItem, currentValue);

            this._windowManager.ShowDialog(confirmationValueViewModel);

            if (confirmationValueViewModel.IsOkay)
            {
                this.GetValuesForItem();
            }
        }

<<<<<<< HEAD
        private void GetItemsFromXmlFile()
        {
            foreach (var item in ChangeBaseXmlWorker.GetItemsWithTheirValues())
            {
                this.ItemCollection.Add(item);
            }
        }

        private void GetValuesForItem()
        {
            if (this._selectedItemCollection != null)
            {
                this.ValueCollection.Clear();

                foreach (var val in ChangeBaseXmlWorker.GetValuesForItem(_selectedItemCollection))
=======
                foreach (var val in selectedItem.Values)
>>>>>>> mazanuj
                {
                    this.ValueCollection.Add(val);
                }
            }
        }

        #endregion
    }
}
