using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
﻿using NLog;

namespace Motorcycle.ViewModels
{
    using XmlWorker;

    [Export(typeof (ChangeBaseViewModel))]
    public class ChangeBaseViewModel : PropertyChangedBase
    {
        private readonly Logger log = NLog.LogManager.GetCurrentClassLogger();
        public ObservableCollection<Item> ItemCollection { get; private set; }

        public ObservableCollection<Value> ValueCollection { get; private set; }

        public ChangeBaseViewModel()
        {
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
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                //NotifyOfPropertyChange("SelectedItem");

                ValueCollection.Clear();

                foreach (var val in selectedItem.Values)
                {
                    ValueCollection.Add(val);
                }
            }
        }
    }
}