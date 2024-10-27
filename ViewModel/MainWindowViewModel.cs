using Labb_3___Quiz_Configurator.Model;
using System.Collections.ObjectModel;
namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public PlayViewModel PlayViewModel { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; }

        private QuestionPackViewModel? _activePack;
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
        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = !value;
                RaisePropertyChanged();
            }
        }
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                ConfigurationViewModel.RaisePropertyChanged("ActivePack");
            }
        }

        public MainWindowViewModel()
        {
            PlayViewModel = new PlayViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);

            ActivePack = new QuestionPackViewModel(new QuestionPack("Default Pack"));
        }

        public void ModeSwitch()
        {
            if (IsPlaying)
            {
            }
        }


    }
}
