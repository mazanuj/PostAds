
namespace Motorcycle.ViewModels
{
    using Caliburn.Micro;
    using Motorcycle.XmlWorker;
    using System.ComponentModel.Composition;
    using System.Windows;

    [Export(typeof(AddChangeProday2KolesaItemViewModel))]
    public class AddChangeProday2KolesaItemViewModel : Screen
    {
        private readonly bool _isInEditMode;

        private readonly SpareEquipItem _currentItem;

        public string Id { get; set; }
        public string Pz { get; set; }
        public string Pe { get; set; }
        
        public bool IsOkay { get; set; }

        [ImportingConstructor]
        public AddChangeProday2KolesaItemViewModel(SpareEquipItem currentItem)
        {
            if (currentItem == null) return;
            _isInEditMode = true;

            _currentItem = currentItem;

            Id = currentItem.Id;
            Pz = currentItem.Pz;
            Pe = currentItem.Pe;
        }

        public void Save()
        {
            if (!CheckIfFieldsAreFilled())
            {
                MessageBox.Show("Not all fields are filled");
                return;
            }

            if (_isInEditMode)
            {
                ChangeCurrentItemNode();
            }
            else
            {
                SpareEquipXmlWorker.AddNewItemNode(Id, Pz, Pe);
            }

            IsOkay = true;
            TryClose();
        }

        public void Cancel()
        {
            TryClose();
        }

        private bool CheckIfFieldsAreFilled()
        {
            return !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Pz) && !string.IsNullOrEmpty(Pe);
        }

        private void ChangeCurrentItemNode()
        {
            var newItem = new SpareEquipItem(Id, Pz, Pe);
            SpareEquipXmlWorker.ChangeItemNode(_currentItem, newItem);
        }
    }
}
