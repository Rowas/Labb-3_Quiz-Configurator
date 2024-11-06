using Labb_3___Quiz_Configurator.Command;
using Labb_3___Quiz_Configurator.Dialogs;
using Labb_3___Quiz_Configurator.JSON;
using Labb_3___Quiz_Configurator.Model;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private Questions? activeQuestion;

        private QuestionPackViewModel? _activePack;

        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public JSONDataHandling? jsonDataHandling;
        public JSONQuestionImport? jsonQuestionImport;

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
        public Questions? ActiveQuestion
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

        public MainWindowViewModel()
        {
            jsonDataHandling = new JSONDataHandling(this);

            jsonQuestionImport = new JSONQuestionImport(this);

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

            Packs = new ObservableCollection<QuestionPackViewModel>();

            Questions? ActiveQuestion = null;

            Packs.Add(ActivePack);

            jsonDataHandling.LoadQuestionPack(this, json);

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
            Questions question = new Questions("New Question", string.Empty, string.Empty, string.Empty, string.Empty);
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
            PlayViewModel.TimeLimit = ActivePack.TimeLimitInSeconds;
            PlayViewModel.TotalQuestionsCount = ActivePack.Questions.Count;
            PlayViewModel.PlayPack.Name = ActivePack.Name;

        }
        public async void NewQuestionPackDialog(object obj)
        {
            CreateNewPackDialog createNewPackDialog = new();
            createNewPackDialog.ShowDialog();
            if (createNewPackDialog.create.CommandParameter.ToString() == "True")
            {
                if (ActivePack != null)
                {
                    await jsonDataHandling.SaveQuestionPack(ActivePack);
                }
                ActivePack = new QuestionPackViewModel(new QuestionPack(createNewPackDialog.packName.Text,
                    (Difficulty)createNewPackDialog.difficulty.SelectedIndex, (int)createNewPackDialog.timeSlider.Value));
                await jsonDataHandling.SaveQuestionPack(ActivePack);
            }
        }
        public async void SavePackDialog(object obj)
        {
            await jsonDataHandling.SaveQuestionPack(this);
        }
        public async void LoadPackDialog(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Question Pack Files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                json = File.ReadAllText(openFileDialog.FileName);
                await jsonDataHandling.LoadQuestionPack(obj, json);
            }
        }
    }
}
