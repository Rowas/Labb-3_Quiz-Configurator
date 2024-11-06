using Labb_3___Quiz_Configurator.Model;
using System.Collections.ObjectModel;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class QuestionPackViewModel : ViewModelBase
    {
        private readonly QuestionPack model;

        public QuestionPackViewModel(QuestionPack Model)
        {
            this.model = Model;
            this.Questions = new ObservableCollection<Questions>(model.Questions);
        }

        public string Name
        {
            get => model.Name;

            set
            {
                model.Name = value;
                RaisePropertyChanged();
            }
        }
        public Difficulty Difficulty
        {
            get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimitInSeconds
        {
            get => model.TimeLimitInSeconds;
            set
            {
                model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Questions> Questions { get; private set; }


    }
}
