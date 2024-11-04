using System.Windows;
using System.Windows.Input;

namespace Labb_3___Quiz_Configurator.Dialogs
{
    /// <summary>
    /// Interaction logic for PackOptionsDialog.xaml
    /// </summary>
    public partial class PackOptionsDialog : Window
    {
        public PackOptionsDialog()
        {
            InitializeComponent();
        }
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        private (bool, string, string?, double) SaveChanges(object sender, ExecutedRoutedEventArgs e)
        {
            return (true, packName.Text, difficulty.SelectedValue.ToString(), timeSlider.Value);
        }

    }
}
