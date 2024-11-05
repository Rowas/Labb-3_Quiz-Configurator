using Labb_3___Quiz_Configurator.Command;
using Labb_3___Quiz_Configurator.Model;
using System.Windows.Threading;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class PlayViewModel : ViewModelBase
    {
        static Random rnd = new Random();
        private QuestionPackViewModel? _playPack;
        private bool _confirmPlay = true;
        private bool _gameBegun = false;
        private int _currentQuestionCount;
        private int _totalQuestionsCount;
        private int _timeLimit;
        private string _currentAskedQuestion;
        private string _correctOrNot;
        private string[] _answerList = new string[4];
        public List<string> _answers = new List<string>();
        private string CorrectAnswer;
        private bool _questionAnswered;
        private int r1;
        private bool _gameEnded = false;
        private int _correctAnswers;


        public int CorrectAnswers
        {
            get => _correctAnswers;
            set
            {
                _correctAnswers = value;
                RaisePropertyChanged();
            }
        }

        public bool QuestionAnswered
        {
            get => _questionAnswered;
            set
            {
                _questionAnswered = value;
                RaisePropertyChanged();
            }
        }

        public bool GameEnded
        {
            get => _gameEnded;
            set
            {
                _gameEnded = value;
                RaisePropertyChanged();
            }
        }
        public int CurrentQuestionCount
        {
            get => _currentQuestionCount;
            set
            {
                _currentQuestionCount = value;
                RaisePropertyChanged();
            }
        }
        public string[] AnswerList
        {
            get => _answerList;
            set
            {
                _answerList = value;
                RaisePropertyChanged();
            }
        }
        public List<string> Answers
        {
            get => _answers;
            set
            {
                _answers = value;
                RaisePropertyChanged(nameof(Answers));
            }
        }
        public int TotalQuestionsCount
        {
            get => _totalQuestionsCount;
            set
            {
                _totalQuestionsCount = value;
                RaisePropertyChanged();
            }
        }
        public string CurrentAskedQuestion
        {
            get => _currentAskedQuestion;
            set
            {
                _currentAskedQuestion = value;
                RaisePropertyChanged();
            }
        }
        public string CorrectOrNot
        {
            get => _correctOrNot;
            set
            {
                _correctOrNot = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimit
        {
            get => _timeLimit;
            set
            {
                _timeLimit = value;
                RaisePropertyChanged();
            }
        }
        public bool GameBegun
        {
            get => _gameBegun;
            set
            {
                _gameBegun = value;
                RaisePropertyChanged();
            }
        }
        public bool ConfirmPlay
        {
            get => _confirmPlay;
            set
            {
                _confirmPlay = value;
                RaisePropertyChanged();
            }
        }

        public QuestionPackViewModel? PlayPack
        {
            get => _playPack;
            set
            {
                _playPack = value;
                RaisePropertyChanged();
            }
        }

        private readonly MainWindowViewModel? mainWindowViewModel;
        private QuestionPackViewModel? _activePack;

        private DispatcherTimer timer;
        public DelegateCommand BeginGameCommand { get; }
        public DelegateCommand StartTimerCommand { get; }
        public DelegateCommand StopTimerCommand { get; }
        public DelegateCommand Response1Command { get; }
        public DelegateCommand Response2Command { get; }
        public DelegateCommand Response3Command { get; }
        public DelegateCommand Response4Command { get; }
        public DelegateCommand RestartGameCommand { get; }

        public PlayViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            BeginGameCommand = new DelegateCommand(BeginGame);

            RestartGameCommand = new DelegateCommand(RestartGame);

            Response1Command = new DelegateCommand(Response1);
            Response2Command = new DelegateCommand(Response2);
            Response3Command = new DelegateCommand(Response3);
            Response4Command = new DelegateCommand(Response4);

            PlayPack = new QuestionPackViewModel(new QuestionPack("Default Pack"));
        }
        public void StartTimer()
        {
            timer.Start();
        }
        public void StopTimer()
        {
            timer.Stop();
        }

        private void RestartGame(object obj)
        {
            ConfirmPlay = true;
            GameBegun = false;
            GameEnded = false;
            BeginGame(obj);
        }
        private void BeginGame(object obj)
        {
            mainWindowViewModel.SaveQuestionPackCommand.Execute(PlayPack.Name);
            ConfirmPlay = !ConfirmPlay;
            GameBegun = !GameBegun;

            foreach (Question question in mainWindowViewModel.ActivePack.Questions)
            {
                PlayPack.Questions.Add(question);
            }

            CurrentQuestionCount = 1;
            CorrectAnswers = 0;
            TimeLimit = mainWindowViewModel.ActivePack.TimeLimitInSeconds;
            TotalQuestionsCount = PlayPack.Questions.Count;

            PickQuestion();

            timer.Start();
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLimit -= 1;
            if (TimeLimit == 0)
            {
                QuestionAnswered = true;
                PlayPack.Questions.RemoveAt(r1);
                ResponsePicked("TimesUp");
            }
        }

        private void PickQuestion()
        {
            if (PlayPack.Questions.Count == 0)
            {
                GameBegun = false;
                GameEnded = true;
                return;
            }
            Answers.Clear();
            r1 = rnd.Next(PlayPack.Questions.Count);
            CurrentAskedQuestion = PlayPack.Questions[r1].Query;
            CorrectAnswer = PlayPack.Questions[r1].CorrectAnswer;
            Answers.Add(PlayPack.Questions[r1].CorrectAnswer);
            foreach (string IncorrectAnswer in PlayPack.Questions[r1].IncorrectAnswers)
            {
                Answers.Add(IncorrectAnswer);
            }
            Answers = Answers.OrderBy(_ => Guid.NewGuid()).ToList();
            AnswerList = Answers.ToArray();
        }

        public void Response1(object obj)
        {
            QuestionAnswered = true;
            PlayPack.Questions.RemoveAt(r1);
            ResponsePicked(AnswerList[0]);
        }
        public void Response2(object obj)
        {
            QuestionAnswered = true;
            PlayPack.Questions.RemoveAt(r1);
            ResponsePicked(AnswerList[1]);
        }
        public void Response3(object obj)
        {
            QuestionAnswered = true;
            PlayPack.Questions.RemoveAt(r1);
            ResponsePicked(AnswerList[2]);
        }
        public void Response4(object obj)
        {
            QuestionAnswered = true;
            PlayPack.Questions.RemoveAt(r1);
            ResponsePicked(AnswerList[3]);
        }
        public async Task ResponsePicked(string response)
        {
            timer.Stop();
            if (response == CorrectAnswer)
            {
                CorrectOrNot = "That is Correct! Good Job!";
                CorrectAnswers++;
            }
            else if (response == "TimesUp")
            {
                CorrectOrNot = $"Time's up. The correct answer was: {CorrectAnswer}";
            }
            else
            {
                CorrectOrNot = $"Sorry, wrong answer! The correct answer was: {CorrectAnswer}";
            }
            await Task.Delay(2000);
            QuestionAnswered = false;
            CurrentQuestionCount++;
            TimeLimit = mainWindowViewModel.ActivePack.TimeLimitInSeconds;
            PickQuestion();
            timer.Start();
        }
    }
}
