

namespace Motorcycle.ViewModels
{
    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using XmlWorker;

    [Export(typeof(ConfirmationViewModel))]
    public class ConfirmationViewModel : Screen
    {
        private readonly bool _isInEditMode;

        private readonly ManufactureItem _currentItem;

        public string Id { get; set; }
        public string M { get; set; }
        public string P { get; set; }
        public string U { get; set; }

        public bool IsOkay { get; set; }

        [ImportingConstructor]
        public ConfirmationViewModel(ManufactureItem currentItem)
        {
            if (currentItem == null) return;
            _isInEditMode = true;

            _currentItem = currentItem;

            Id = currentItem.Id;
            M = currentItem.M;
            P = currentItem.P;
            U = currentItem.U;
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
                ManufactureXmlWorker.AddNewItemNode(Id, M, P, U);    
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
            return !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(M) && !string.IsNullOrEmpty(P) && !string.IsNullOrEmpty(U);
        }

        private void ChangeCurrentItemNode()
        {
            var newItem = new ManufactureItem(Id, M, P, U);
            ManufactureXmlWorker.ChangeItemNode(_currentItem, newItem);
        }
    }
}
