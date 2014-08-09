

namespace Motorcycle.ViewModels
{
    using System.ComponentModel.Composition;
    using System.Windows;

    using Caliburn.Micro;

    using Motorcycle.XmlWorker;

    [Export(typeof(ConfirmationViewModel))]
    public class ConfirmationViewModel : Screen
    {
        public string Id { get; set; }
        public string M { get; set; }
        public string P { get; set; }
        public string U { get; set; }

        public bool IsOkay { get; set; }

        [ImportingConstructor]
        public ConfirmationViewModel()
        {

        }

        public void Save()
        {
            if (!this.CheckIfFieldsAreFilled())
            {
                MessageBox.Show("Not all fields are filled");
                return;
            }

            ChangeBaseXmlWorker.AddNewItemNode(Id, M, P, U);
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
    }
}
