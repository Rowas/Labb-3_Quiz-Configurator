using Labb_3___Quiz_Configurator.JSON;
using Labb_3___Quiz_Configurator.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace Labb_3___Quiz_Configurator.Dialogs
{
    /// <summary>
    /// Interaction logic for QuestionImportDialog.xaml
    /// </summary>
    partial class QuestionImportDialog : Window
    {
        private JSONQuestionImport? jsonQuestionImport;

        internal QuestionImportDialog(MainWindowViewModel? mainWindowViewModel)
        {
            InitializeComponent();
            jsonQuestionImport = new JSONQuestionImport(mainWindowViewModel);
            this.DataContext = mainWindowViewModel;
        }
        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            import.CommandParameter = "True";
            this.Close();
        }
    }
}
