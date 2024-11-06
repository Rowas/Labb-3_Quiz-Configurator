using System.Windows;
using System.Windows.Input;

namespace Labb_3___Quiz_Configurator.Dialogs
{
    /// <summary>
    /// Interaction logic for QuestionImportDialog.xaml
    /// </summary>
    public partial class QuestionImportDialog : Window
    {
        public QuestionImportDialog()
        {
            InitializeComponent();
        }
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            import.CommandParameter = "True";
            this.Close();
        }
    }
}
