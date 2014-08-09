
namespace Motorcycle.ViewModels
{
    ﻿using Caliburn.Micro;
    using Motorcycle.XmlWorker;
    using System.Collections.ObjectModel;
    using System.ComponentModel.Composition;

    [Export(typeof(ChangeBaseViewModel))]
    public class ChangeBaseViewModel : PropertyChangedBase
    {
        public ObservableCollection<Item> ItemCollection { get; private set; }

        public ObservableCollection<Value> ValueCollection { get; private set; }

        private readonly IWindowManager _windowManager;

        [ImportingConstructor]
        public ChangeBaseViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;

            ItemCollection = new ObservableCollection<Item>();

            foreach (var item in XmlWorker.GetManufacture())
            {
                ItemCollection.Add(item);
            }

            ValueCollection = new ObservableCollection<Value>();
        }

        private Item selectedItem;

        public Item SelectedItemCollection
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                this.selectedItem = value;
                //NotifyOfPropertyChange("SelectedItem");

                //ValueCollection.Clear();

                //foreach (var val in this.selectedItem.Values)
                //{
                //    ValueCollection.Add(val);
                //}
            }
        }

        public void RemoveItem(Item item)
        {
            ChangeBaseXmlWorker.RemoveItemNode(item);

            this.RefreshItemList();
        }

        public void AddNewItem()
        {
            var confirmationViewModel = new ConfirmationViewModel();
            _windowManager.ShowDialog(confirmationViewModel);

            if (confirmationViewModel.IsOkay)
                this.RefreshItemList();

        }

        private void RefreshItemList()
        {
            this.ItemCollection.Clear();

            foreach (var item in XmlWorker.GetManufacture())
            {
                this.ItemCollection.Add(item);
            }
        }
    }
}
