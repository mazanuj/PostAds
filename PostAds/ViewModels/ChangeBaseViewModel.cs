<<<<<<< HEAD
﻿using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace Motorcycle.ViewModels
{
    using Motorcycle.XmlWorker;

    [Export(typeof(ChangeBaseViewModel))]
    public class ChangeBaseViewModel : PropertyChangedBase
    {
        public ObservableCollection<Item> ItemCollection
        {
            get;
            private set;
        }

        public ObservableCollection<Value> ValueCollection
        {
            get;
            private set;
        }

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
            get { return this.selectedItem; }
            set
            {
                this.selectedItem = value;
                //NotifyOfPropertyChange("SelectedItem");

                ValueCollection.Clear();

                foreach (var val in this.selectedItem.Values)
                {
                    ValueCollection.Add(val);
                }
            }
        }
=======
﻿using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using NLog;

namespace Motorcycle.ViewModels
{
    [Export(typeof (ChangeBaseViewModel))]
    public class ChangeBaseViewModel : PropertyChangedBase
    {
        public static ObservableCollection<LogEventInfo> LogCollection { get; private set; }
>>>>>>> origin/mazanuj
    }
}