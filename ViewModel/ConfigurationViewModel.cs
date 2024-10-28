using Labb_3___Quiz_Configurator.Command;
using Labb_3___Quiz_Configurator.Dialogs;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public string TestDataConfig { get => "This is test data."; }

        public DelegateCommand PackOptionsCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            PackOptionsCommand = new DelegateCommand(EditPack);
        }

        public void EditPack(object obj)
        {
            PackOptionsDialog packOptionsDialog = new();
            packOptionsDialog.ShowDialog();
        }

        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }

    }
}
