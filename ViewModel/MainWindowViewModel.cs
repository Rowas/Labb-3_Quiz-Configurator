using Labb_3___Quiz_Configurator.Command;
using Labb_3___Quiz_Configurator.Dialogs;
using Labb_3___Quiz_Configurator.JSON;
using Labb_3___Quiz_Configurator.Model;
using Labb_3___Quiz_Configurator.Views;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private Question? activeQuestion;
        private QuestionPackViewModel? _activePack;
        private QuestionPackViewModel? _loadedPack;
        public MenuView MenuView { get; }
        public ObservableCollection<QuestionPackViewModel> _packs;
        public ObservableCollection<QuestionPackViewModel> Packs { get => _packs; set { _packs = value; RaisePropertyChanged(); } }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public JSONDataHandling? jsonDataHandling;
        public DelegateCommand ModeSwitchCommand { get; }
        public DelegateCommand AddPackCommand { get; }
        public DelegateCommand NewQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand SaveQuestionPackCommand { get; }
        public DelegateCommand LoadQuestionPackCommand { get; }
        public DelegateCommand FullscreenCommand { get; }
        public PlayViewModel PlayViewModel { get; }

        public WindowState _mainWindowState = WindowState.Normal;

        public JsonSerializerOptions options = new JsonSerializerOptions()
        {
            IncludeFields = true,
            UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip,
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            AllowTrailingCommas = true,
        };
        private bool execute = true;
        private string json;
        private bool _playMode = false;
        private bool _configMode = true;
        private int _numberOfQInPack;

        public WindowState MainWindowState
        {
            get => _mainWindowState;
            set
            {
                _mainWindowState = value;
                RaisePropertyChanged();
            }
        }

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
            jsonDataHandling = new JSONDataHandling(this);

            PlayViewModel = new PlayViewModel(this);

            ConfigurationViewModel = new ConfigurationViewModel(this);

            ModeSwitchCommand = new DelegateCommand(ModeSwitch);

            AddPackCommand = new DelegateCommand(NewQuestionPackDialog);

            NewQuestionCommand = new DelegateCommand(NewQuestion);

            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);

            SaveQuestionPackCommand = new DelegateCommand(SavePackDialog);

            LoadQuestionPackCommand = new DelegateCommand(LoadPackDialog);

            FullscreenCommand = new DelegateCommand(Fullscreen);

            ActivePack = new QuestionPackViewModel(new QuestionPack("Default Pack"));

            LoadedPack = new QuestionPackViewModel(new QuestionPack("Default Pack"));

            Packs = new ObservableCollection<QuestionPackViewModel>();

            Question? ActiveQuestion = null;

            Packs.Add(ActivePack);

            jsonDataHandling.LoadQuestionPack(this);

        }
        public void Fullscreen(object obj)
        {
            if (MainWindowState == WindowState.Maximized)
            {
                MainWindowState = WindowState.Normal;
            }
            else
            {
                MainWindowState = WindowState.Maximized;
            }
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
                PlayViewModel.GameEnded = false;
            }
            PlayViewModel.StopTimer();

        }
        public async void NewQuestionPackDialog(object obj)
        {
            CreateNewPackDialog createNewPackDialog = new();
            createNewPackDialog.ShowDialog();
            if (ActivePack != null)
            {
                await jsonDataHandling.SaveQuestionPack(ActivePack);
            }
            ActivePack = new QuestionPackViewModel(new QuestionPack(createNewPackDialog.packName.Text,
                (Difficulty)createNewPackDialog.difficulty.SelectedIndex, (int)createNewPackDialog.timeSlider.Value));
            await jsonDataHandling.SaveQuestionPack(ActivePack);
        }
        public async void SavePackDialog(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Question Pack Files (*.json)|*.json";
            if (!Packs.Contains(ActivePack))
            {
                if (saveFileDialog.ShowDialog() == true)
                {
                    await jsonDataHandling.SaveQuestionPack(this);
                }
            }
            else
            {
                await jsonDataHandling.SaveQuestionPack(this);
            }
        }
        public async void LoadPackDialog(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Question Pack Files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                json = File.ReadAllText(openFileDialog.FileName);
                await jsonDataHandling.LoadQuestionPack(obj);
            }
        }
    }
}
