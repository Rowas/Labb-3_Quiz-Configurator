using Labb_3___Quiz_Configurator.Command;
using Labb_3___Quiz_Configurator.Dialogs;
using Labb_3___Quiz_Configurator.Model;
using System.IO;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private bool _changesMade = false;

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

            if (packOptionsDialog.ShowDialog() == true)
            {
                ActivePack.Name = packOptionsDialog.packName.Text;
                ActivePack.Difficulty = (Difficulty)packOptionsDialog.difficulty.SelectedIndex;
                ActivePack.TimeLimitInSeconds = (int)packOptionsDialog.timeSlider.Value;
            }
        }

        public async void ImportQuestionsDialog(object obj)
        {
            QuestionImportDialog questionImportDialog = new();
            questionImportDialog.category.ItemsSource = await mainWindowViewModel.jsonQuestionImport.ImportCategories();
            if (questionImportDialog.ShowDialog() == true)
            {
                await mainWindowViewModel.jsonQuestionImport.ImportQuestions((int)questionImportDialog.questionSlider.Value, questionImportDialog.difficulty.SelectedIndex.ToString(), questionImportDialog.category.SelectedItem.ToString());
                await mainWindowViewModel.jsonDataHandling.LoadQuestionPack(obj, json);
            }
        }
        public void RemovePack(object obj)
        {
            File.Delete($"{ActivePack.Name}.json");
            mainWindowViewModel.ActivePack = null;
            mainWindowViewModel.LoadPackDialog(this);
        }

        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }
        public string? json { get; private set; }
    }
}
