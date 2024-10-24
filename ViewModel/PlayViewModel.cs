using Labb_3___Quiz_Configurator.Command;
using System.Windows.Input;
using System.Windows.Threading;

namespace Labb_3___Quiz_Configurator.ViewModel
{
    internal class PlayViewModel : ViewModelBase
    {
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

        public PlayViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            //timer.Start();

            UpdateButtonCommand = new DelegateCommand(UpdateButton);
        }

        private void UpdateButton(object obj)
        {
            TestDataPlay += " Updated";
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TestPlayTimer -= 1;
        }
    }
}
