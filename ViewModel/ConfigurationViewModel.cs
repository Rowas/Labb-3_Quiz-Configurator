using Labb_3___Quiz_Configurator.Command;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {

        private readonly MainWindowViewModel? mainWindowViewModel;

        public string TestDataConfig { get => "This is test data."; }

        public DelegateCommand IsEditingCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

        }

        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }

        private bool _isEditing = true;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = !value;
                RaisePropertyChanged();
            }
        }
    }
}
