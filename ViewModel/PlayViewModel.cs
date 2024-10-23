namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class PlayViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        public string TestDataPlay { get => "This is test data."; }

        public PlayViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }
    }
}
