

namespace Motorcycle.ViewModels
{
    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using Motorcycle.XmlWorker;

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
            if (currentItem != null)
            {
                this._isInEditMode = true;

                this._currentItem = currentItem;

                this.Id = currentItem.Id;
                this.M = currentItem.M;
                this.P = currentItem.P;
                this.U = currentItem.U;
            }
        }

        public void Save()
        {
            if (!this.CheckIfFieldsAreFilled())
            {
                MessageBox.Show("Not all fields are filled");
                return;
            }

            if (_isInEditMode)
            {
                this.ChangeCurrentItemNode();
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
            this.TryClose();
        }

        private bool CheckIfFieldsAreFilled()
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(M) || string.IsNullOrEmpty(P) || string.IsNullOrEmpty(U)) return false;
            return true;
        }

        private void ChangeCurrentItemNode()
        {
            var newItem = new ManufactureItem(Id, M, P, U);
            ManufactureXmlWorker.ChangeItemNode(this._currentItem, newItem);
        }
    }
}
