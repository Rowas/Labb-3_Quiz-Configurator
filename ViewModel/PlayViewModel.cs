﻿using Labb_3___Quiz_Configurator.Command;
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
        private bool _isCorrectAnswer;
        private bool _questionAnswered;
        private int r1;
        private bool _gameEnded = false;

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

        public bool IsCorrectAnswer
        {
            get => _isCorrectAnswer;
            set
            {
                _isCorrectAnswer = value;
                RaisePropertyChanged(nameof(IsCorrectAnswer));
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
                RaisePropertyChanged("ActivePack");
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

        public PlayViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            BeginGameCommand = new DelegateCommand(BeginGame);

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
        private void BeginGame(object obj)
        {
            ConfirmPlay = !ConfirmPlay;
            GameBegun = !GameBegun;
            TimeLimit = PlayPack.TimeLimitInSeconds;
            CurrentQuestionCount = 1;
            TotalQuestionsCount = PlayPack.Questions.Count;
            PickQuestion();


            timer.Start();
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLimit -= 1;
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
        public async void ResponsePicked(string response)
        {
            timer.Stop();
            if (response == CorrectAnswer)
            {
                IsCorrectAnswer = true;
                CorrectOrNot = "That is Correct! Good Job!";
            }
            else
            {
                IsCorrectAnswer = false;
                CorrectOrNot = $"Sorry, wrong answer! The correct answer was: {CorrectAnswer}";
            }
            await Task.Delay(2000);
            QuestionAnswered = false;
            TimeLimit = PlayPack.TimeLimitInSeconds;
            PickQuestion();
        }
    }
}
