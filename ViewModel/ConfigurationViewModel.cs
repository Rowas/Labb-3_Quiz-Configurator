using Labb_3___Quiz_Configurator.Command;
using Labb_3___Quiz_Configurator.Dialogs;
using Labb_3___Quiz_Configurator.Model;
using System.IO;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }

        private string _packDifficulty;
        private List<string> _categoryStrings;

        public string PackDifficulty
        {
            get => _packDifficulty;
            set
            {
                _packDifficulty = value;
                RaisePropertyChanged();
            }
        }

        public List<string> CategoryStrings
        {
            get => _categoryStrings;
            set
            {
                _categoryStrings = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand PackOptionsCommand { get; }
        public DelegateCommand RemovePackCommand { get; }
        public DelegateCommand ImportQuestionsCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            PackOptionsCommand = new DelegateCommand(PackOptions);
            RemovePackCommand = new DelegateCommand(RemovePack);
            ImportQuestionsCommand = new DelegateCommand(ImportQuestionsDialog);
        }

        public void PackOptions(object obj)
        {
            PackOptionsDialog packOptionsDialog = new();
            packOptionsDialog.packName.Text = ActivePack.Name;
            packOptionsDialog.difficulty.SelectedItem = ActivePack.Difficulty;
            packOptionsDialog.timeSlider.Value = ActivePack.TimeLimitInSeconds;

            packOptionsDialog.ShowDialog();

            if (packOptionsDialog.update.CommandParameter.ToString() == "True")
            {
                ActivePack.Name = packOptionsDialog.packName.Text;
                ActivePack.Difficulty = (Difficulty)packOptionsDialog.difficulty.SelectedIndex;
                ActivePack.TimeLimitInSeconds = (int)packOptionsDialog.timeSlider.Value;
            }

        }

        public async void ImportQuestionsDialog(object obj)
        {
            PackDifficulty = mainWindowViewModel.ActivePack.Difficulty.ToString();
            QuestionImportDialog questionImportDialog = new(mainWindowViewModel);
            List<(int, string)> categories = await mainWindowViewModel.jsonQuestionImport.ImportCategories();
            CategoryStrings = categories.Select(t => t.Item2).ToList();
            questionImportDialog.ShowDialog();
            if (questionImportDialog.import.CommandParameter.ToString() == "True")
            {
                var categoryID = questionImportDialog.category.SelectedIndex;
                await mainWindowViewModel.jsonQuestionImport.ImportQuestions(
                    (int)questionImportDialog.questionSlider.Value,
                    mainWindowViewModel.ActivePack.Difficulty,
                    categories[categoryID].Item1);
            }
        }
        public void RemovePack(object obj)
        {
            File.Delete($"{ActivePack.Name}.json");
            mainWindowViewModel.ActivePack = null;
            mainWindowViewModel.LoadPackDialog(this);
        }
    }
}
