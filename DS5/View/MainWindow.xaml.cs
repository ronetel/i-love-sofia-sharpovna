using System.Windows;
using DS5.ViewModel.Helpers;

namespace DS5
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}

