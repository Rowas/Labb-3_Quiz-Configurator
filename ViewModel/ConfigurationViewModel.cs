using Labb_3___Quiz_Configurator.Command;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private bool _isEditing = true;

        private readonly MainWindowViewModel? mainWindowViewModel;

        public string TestDataConfig { get => "This is test data."; }

        public DelegateCommand IsEditingCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            IsEditingCommand = new DelegateCommand(Editing);
        }

        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = !value;
                RaisePropertyChanged();
            }
        }

        private void Editing(object obj)
        {
            IsEditing = true;
        }
    }
}
