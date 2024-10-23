using System.Collections.ObjectModel;
namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }


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
    }
}
