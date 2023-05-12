using System.Windows;

namespace Laboratory_work_No._5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainPage mainPage = new MainPage();
        public MainWindow()
        {
            InitializeComponent();
            MainWindowFrame.Content = mainPage;
        }
    }
}
