﻿
namespace Motorcycle.ViewModels
{
    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using XmlWorker;

    [Export(typeof(AddChangeCityViewModel))]
    public class AddChangeCityViewModel : Screen
    {
        private readonly bool _isInEditMode;

        private readonly CityItem _currentItem;

        public string CityName { get; set; }
        public string M { get; set; }
        public string P { get; set; }
        public string U { get; set; }
        public string O { get; set; }

        public bool IsOkay { get; set; }

        [ImportingConstructor]
        public AddChangeCityViewModel(CityItem currentItem)
        {
            if (currentItem == null) return;
            _isInEditMode = true;

            _currentItem = currentItem;

            CityName = currentItem.CityName;
            M = currentItem.M;
            P = currentItem.P;
            U = currentItem.U;
            O = currentItem.O;
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
                CityXmlWorker.AddNewItemNode(CityName, M, P, U, O);    
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
            return !string.IsNullOrEmpty(CityName) && !string.IsNullOrEmpty(M) && !string.IsNullOrEmpty(P) && !string.IsNullOrEmpty(U) && !string.IsNullOrEmpty(O);
        }

        private void ChangeCurrentItemNode()
        {
            var newItem = new CityItem(CityName, M, P, U, O);
            CityXmlWorker.ChangeItemNode(_currentItem.CityName, newItem);
        }
    }
}
