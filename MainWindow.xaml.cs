using Labb_3___Quiz_Configurator.ViewModel;
using System.Windows;

namespace Labb_3___Quiz_Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

        }

        private void ConfigurationView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}