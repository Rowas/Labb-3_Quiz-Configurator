using Labb_3___Quiz_Configurator.Command;
using System.Windows.Threading;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class PlayViewModel : ViewModelBase
    {
        int x = 0;

        private readonly MainWindowViewModel? mainWindowViewModel;

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
        public DelegateCommand StartTimerCommand { get; }

        public PlayViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;


            StartTimerCommand = new DelegateCommand(StartTimer);
            UpdateButtonCommand = new DelegateCommand(UpdateButton);
        }

        private void StartTimer(object obj)
        {
            timer.Start();
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
