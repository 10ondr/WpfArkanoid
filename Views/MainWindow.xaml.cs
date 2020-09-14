using WpfArkanoid.ViewModels;
using WpfArkanoid.Views;

namespace WpfArkanoid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new GameViewModel();
        }
    }
}

