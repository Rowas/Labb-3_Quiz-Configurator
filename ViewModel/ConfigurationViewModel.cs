namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public string TestDataConfig { get => "This is test data."; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }
    }
}
