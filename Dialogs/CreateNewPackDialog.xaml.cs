using System.Windows;
using System.Windows.Input;

namespace Labb_3___Quiz_Configurator.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateNewPackDialog.xaml
    /// </summary>
    public partial class CreateNewPackDialog : Window
    {
        public CreateNewPackDialog()
        {
            InitializeComponent();
        }
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        private (bool, string, string?, double) NewPack(object sender, ExecutedRoutedEventArgs e)
        {
            return (true, packName.Text, difficulty.SelectedValue.ToString(), timeSlider.Value);
        }
    }
}
