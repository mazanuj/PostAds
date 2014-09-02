﻿
namespace Motorcycle.ViewModels
{
    using Caliburn.Micro;
    using Motorcycle.XmlWorker;
    using System.ComponentModel.Composition;
    using System.Windows;

    [Export(typeof(ConfirmationValueViewModel))]
    public class ConfirmationValueViewModel : Screen
    {
        private readonly bool _isInEditMode;

        private readonly ManufactureItem _currentItem;
        private readonly ManufactureValue _currentValue;

        public string Name { get; set; }
        public string Val { get; set; }

        public bool IsOkay { get; set; }

        [ImportingConstructor]
        public ConfirmationValueViewModel(ManufactureItem currentItem, ManufactureValue currentValue)
        {
            this._currentItem = currentItem;

            if (currentValue != null)
            {
                this._isInEditMode = true;

                this._currentValue = currentValue;

                this.Name = currentValue.Name;
                this.Val = currentValue.Val;
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
                this.ChangeCurrentValueNode();
            }
            else
            {
                ManufactureXmlWorker.AddNewValueNode(_currentItem, new ManufactureValue(Name, Val));
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
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Val)) return false;
            return true;
        }

        private void ChangeCurrentValueNode()
        {
            var newValue = new ManufactureValue(Name, Val);
            ManufactureXmlWorker.ChangeValueNodeUsingItemNode(_currentItem, _currentValue, newValue);
        }
    }
}
