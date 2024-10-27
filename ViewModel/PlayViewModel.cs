using Labb_3___Quiz_Configurator.Command;
using System.Windows.Threading;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class PlayViewModel : ViewModelBase
    {
        int x = 0;

        private readonly MainWindowViewModel? mainWindowViewModel;

        private bool _isPlaying = false;

        private DispatcherTimer timer;
        private string _testDataPlay = "This is test data.";
        private int _testPlayTimer = 60;

        public int TestPlayTimer
        {
            get => _testPlayTimer;
            private set
            {
                _testPlayTimer = value;
                RaisePropertyChanged();
            }
        }

        public string TestDataPlay
        {
            get => _testDataPlay;
            private set
            {
                _testDataPlay = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand UpdateButtonCommand { get; }
        public DelegateCommand IsPlayingCommand { get; }
        public DelegateCommand StartTimerCommand { get; }
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = !value;
                RaisePropertyChanged();
            }
        }

        public PlayViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;


            StartTimerCommand = new DelegateCommand(StartTimer);
            UpdateButtonCommand = new DelegateCommand(UpdateButton);
            IsPlayingCommand = new DelegateCommand(Playing);
        }

        private void StartTimer(object obj)
        {
            timer.Start();
        }

        private void Playing(object obj)
        {
            IsPlaying = true;
        }

        private void UpdateButton(object obj)
        {
            x++;
            TestDataPlay += x.ToString();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TestPlayTimer -= 1;
        }
    }
}
