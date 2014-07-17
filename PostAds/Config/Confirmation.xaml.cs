using System.Windows;

namespace Motorcycle.Config
{
    /// <summary>
    /// Interaction logic for Confirmation.xaml
    /// </summary>
    public partial class Confirmation
    {
        public Confirmation()
        {
            InitializeComponent();
        }

        public Confirmation(int width)
        {
            InitializeComponent();
            Width -= width;
        }

        private void DialogResult_OK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}