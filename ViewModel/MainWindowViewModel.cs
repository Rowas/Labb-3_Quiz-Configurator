using Labb_3___Quiz_Configurator.Command;
using Labb_3___Quiz_Configurator.Dialogs;
using Labb_3___Quiz_Configurator.Model;
using System.Collections.ObjectModel;
namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public PlayViewModel PlayViewModel { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; }

        public DelegateCommand ModeSwitchCommand { get; }
        public DelegateCommand AddPackCommand { get; }

        private QuestionPackViewModel? _activePack;
        private bool _playMode = false;
        private bool _configMode = true;

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

            ModeSwitchCommand = new DelegateCommand(ModeSwitch);

            AddPackCommand = new DelegateCommand(NewQuestionPack);

            ActivePack = new QuestionPackViewModel(new QuestionPack("Default Pack"));
        }
        public void NewQuestionPack(object obj)
        {
            CreateNewPackDialog createNewPackDialog = new();
            createNewPackDialog.ShowDialog();
        }
        public bool PlayMode
        {
            get => _playMode;
            private set
            {
                _playMode = value;
                RaisePropertyChanged();
            }
        }
        public bool ConfigMode
        {
            get => _configMode;
            private set
            {
                _configMode = value;
                RaisePropertyChanged();
            }
        }
        public void ModeSwitch(object obj)
        {
            PlayMode = !PlayMode;
            ConfigMode = !ConfigMode;
        }
    }
}
