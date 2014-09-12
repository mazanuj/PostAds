
namespace Motorcycle.ViewModels
{
    using Caliburn.Micro;
    using XmlWorker;
    using System.ComponentModel.Composition;
    using System.Windows;

    [Export(typeof(ConfirmationValueViewModel))]
    public class ConfirmationValueViewModel : Screen
    {
        private readonly bool _isInEditMode;

        private readonly ManufactureItem _currentItem;
        private readonly ManufactureValue _currentValue;

        public string Nam { get; set; }
        public string Val { get; set; }

        public bool IsOkay { get; set; }

        [ImportingConstructor]
        public ConfirmationValueViewModel(ManufactureItem currentItem, ManufactureValue currentValue)
        {
            _currentItem = currentItem;

            if (currentValue == null) return;
            _isInEditMode = true;

            _currentValue = currentValue;

            Nam = currentValue.Name;
            Val = currentValue.Val;
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
                ChangeCurrentValueNode();
            }
            else
            {
                ManufactureXmlWorker.AddNewValueNode(_currentItem, new ManufactureValue(Nam, Val));
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
            return !string.IsNullOrEmpty(Nam) && !string.IsNullOrEmpty(Val);
        }

        private void ChangeCurrentValueNode()
        {
            var newValue = new ManufactureValue(Nam, Val);
            ManufactureXmlWorker.ChangeValueNodeUsingItemNode(_currentItem, _currentValue, newValue);
        }
    }
}
