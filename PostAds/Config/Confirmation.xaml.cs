using System.Windows;
using System.Windows.Input;

namespace Motorcycle
{
    /// <summary>
    /// Interaction logic for Confirmation.xaml
    /// </summary>
    public partial class Confirmation:Window
    {
        public Confirmation()
        {
            InitializeComponent();
        }

        private void DialogResult_OK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}