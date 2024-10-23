using Labb_3___Quiz_Configurator.Model;
using System.Collections.ObjectModel;
namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public PlayViewModel PlayViewModel { get; }
        public ConfigurationViewModel ConfigViewModel { get; }

        private QuestionPackViewModel? _activePack;
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            PlayViewModel = new PlayViewModel(this);
            ConfigViewModel = new ConfigurationViewModel(this);

            ActivePack = new QuestionPackViewModel(new QuestionPack("Default Pack"));
        }

    }
}
