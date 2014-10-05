
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

        private readonly Proday2KolesaItem _currentItem;

        public string Id { get; set; }
        public string S { get; set; }
        public string E { get; set; }
        
        public bool IsOkay { get; set; }

        [ImportingConstructor]
        public AddChangeProday2KolesaItemViewModel(Proday2KolesaItem currentItem)
        {
            if (currentItem == null) return;
            _isInEditMode = true;

            _currentItem = currentItem;

            Id = currentItem.Id;
            S = currentItem.S;
            E = currentItem.E;
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
                Proday2KolesaXmlWorker.AddNewItemNode(Id, S, E);
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
            return !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(S) && !string.IsNullOrEmpty(E);
        }

        private void ChangeCurrentItemNode()
        {
            var newItem = new Proday2KolesaItem(Id, S, E);
            Proday2KolesaXmlWorker.ChangeItemNode(_currentItem, newItem);
        }
    }
}
