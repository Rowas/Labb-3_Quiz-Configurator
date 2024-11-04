using Labb_3___Quiz_Configurator.Command;
using Labb_3___Quiz_Configurator.Dialogs;
using Labb_3___Quiz_Configurator.Model;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;

        private bool _changesMade = false;

        public DelegateCommand PackOptionsCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
            PackOptionsCommand = new DelegateCommand(SavePack);
        }

        public void SavePack(object obj)
        {
            PackOptionsDialog packOptionsDialog = new();
            packOptionsDialog.packName.Text = ActivePack.Name;
            packOptionsDialog.difficulty.SelectedItem = ActivePack.Difficulty;
            packOptionsDialog.timeSlider.Value = ActivePack.TimeLimitInSeconds;

            if (packOptionsDialog.ShowDialog() == true) ;
            {
                ActivePack.Name = packOptionsDialog.packName.Text;
                ActivePack.Difficulty = (Difficulty)packOptionsDialog.difficulty.SelectedIndex;
                ActivePack.TimeLimitInSeconds = (int)packOptionsDialog.timeSlider.Value;
            }
        }

        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel?.ActivePack; }
    }
}
