﻿namespace Motorcycle.ViewModels
{
    using Caliburn.Micro;

    using Motorcycle.Config.Proxy;
    using Motorcycle.XmlWorker;
    using System.ComponentModel.Composition;
    using System.Windows;

    [Export(typeof(AddChangeProxyAddressViewModel))]
    public class AddChangeProxyAddressViewModel : Screen
    {
        private readonly bool _isInEditMode;

        private readonly ProxyAddressItem _currentItem;

        public string ProxyAddress { get; set; }
        public string Type { get; set; }

        public bool IsOkay { get; set; }

        [ImportingConstructor]
        public AddChangeProxyAddressViewModel(ProxyAddressItem currentItem)
        {
            if (currentItem == null) return;
            _isInEditMode = true;

            _currentItem = currentItem;

            this.ProxyAddress = currentItem.ProxyAddress;
            this.Type = currentItem.Type;
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
                ProxyXmlWorker.AddNewProxy(new ProxyAddressStruct
                {
                    ProxyAddresses = this.ProxyAddress,
                    Type = ProxyAddressStruct.GetProxyTypeEnumFromString(this.Type)
                });
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
            return !string.IsNullOrEmpty(this.ProxyAddress) && !string.IsNullOrEmpty(this.Type);
        }

        private void ChangeCurrentItemNode()
        {
            ProxyXmlWorker.ChangeProxyAddress(_currentItem.ProxyAddress, ProxyAddress, Type);
        }
    }
}
