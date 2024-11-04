using Labb_3___Quiz_Configurator.Command;
using Labb_3___Quiz_Configurator.Dialogs;
using Labb_3___Quiz_Configurator.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private Question? activeQuestion;
        private QuestionPackViewModel? _activePack;
        private QuestionPackViewModel? _loadedPack;
        public ObservableCollection<QuestionPackViewModel> _packs;
        public ObservableCollection<QuestionPackViewModel> Packs { get => _packs; set { _packs = value; RaisePropertyChanged(); } }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public DelegateCommand ModeSwitchCommand { get; }
        public DelegateCommand AddPackCommand { get; }
        public DelegateCommand NewQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand SaveQuestionPackCommand { get; }
        public DelegateCommand LoadQuestionPackCommand { get; }
        public PlayViewModel PlayViewModel { get; }

        public JsonSerializerOptions options = new JsonSerializerOptions()
        {
            IncludeFields = true,
            UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            AllowTrailingCommas = true,
        };

        private bool _playMode = false;
        private bool _configMode = true;
        private int _numberOfQInPack;

        public int NumbersOfQInPack
        {
            get => _numberOfQInPack;
            set
            {
                _numberOfQInPack = value;
                RaisePropertyChanged();
            }
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
        public Question? ActiveQuestion
        {
            get => this.activeQuestion;
            set
            {
                this.activeQuestion = value;
                this.RaisePropertyChanged(nameof(ActiveQuestion));
            }
        }
        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                ConfigurationViewModel.RaisePropertyChanged();
            }
        }
        public QuestionPackViewModel? LoadedPack
        {
            get => _loadedPack;
            set
            {
                _loadedPack = value;
                RaisePropertyChanged();
                ConfigurationViewModel.RaisePropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            PlayViewModel = new PlayViewModel(this);

            ConfigurationViewModel = new ConfigurationViewModel(this);

            ModeSwitchCommand = new DelegateCommand(ModeSwitch);

            AddPackCommand = new DelegateCommand(NewQuestionPack);

            NewQuestionCommand = new DelegateCommand(NewQuestion);

            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);

            SaveQuestionPackCommand = new DelegateCommand(SaveQuestionPack);

            LoadQuestionPackCommand = new DelegateCommand(LoadQuestionPack);

            ActivePack = new QuestionPackViewModel(new QuestionPack("Default Pack"));

            LoadedPack = new QuestionPackViewModel(new QuestionPack("Default Pack"));

            Packs = new ObservableCollection<QuestionPackViewModel>();

            Question? ActiveQuestion = null;


            LoadQuestionPack(ActivePack);

        }
        public void NewQuestionPack(object obj)
        {
            CreateNewPackDialog createNewPackDialog = new();
            createNewPackDialog.ShowDialog();
            SaveQuestionPack(ActivePack);
            ActivePack = new QuestionPackViewModel(new QuestionPack(createNewPackDialog.packName.Text, (Difficulty)createNewPackDialog.difficulty.SelectedIndex, (int)createNewPackDialog.timeSlider.Value));
            SaveQuestionPack(ActivePack);
        }
        public void NewQuestion(object obj)
        {
            Question question = new Question("New Question", string.Empty, string.Empty, string.Empty, string.Empty);
            ActivePack.Questions.Add(question);
            ActiveQuestion = question;
            RaisePropertyChanged();
        }

        public void RemoveQuestion(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);
            RaisePropertyChanged();
        }
        public void ModeSwitch(object obj)
        {
            PlayMode = !PlayMode;
            ConfigMode = !ConfigMode;
            if (ConfigMode)
            {
                PlayViewModel.GameBegun = false;
                PlayViewModel.ConfirmPlay = true;
            }
            PlayViewModel.StopTimer();
            PlayViewModel.PlayPack = ActivePack;

        }
        public void SaveQuestionPack(object obj)
        {
            string json = JsonSerializer.Serialize(ActivePack, options);
            File.WriteAllText($"{ActivePack.Name}.json", json);
            if (!Packs.Contains(ActivePack))
            {
                Packs.Add(ActivePack);
            }
            Debug.WriteLine(Packs.Count);
        }
        public void LoadQuestionPack(object obj)
        {
            string json = File.ReadAllText("Default Pack.json");
            QuestionPack LoadedPack = JsonSerializer.Deserialize<QuestionPack>(json, options);
            ActivePack.Name = LoadedPack.Name;
            ActivePack.Difficulty = LoadedPack.Difficulty;
            ActivePack.TimeLimitInSeconds = LoadedPack.TimeLimitInSeconds;
            ActivePack.Questions.Clear();
            foreach (Question q in LoadedPack.Questions)
            {
                ActivePack.Questions.Add(q);
            }
        }
    }
}
