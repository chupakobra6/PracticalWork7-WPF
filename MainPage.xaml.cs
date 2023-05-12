using Laboratory_work_No._5.overlordDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Laboratory_work_No._5
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public profileTableAdapter profileTable = new profileTableAdapter();
        public MainPage()
        {
            InitializeComponent();
            GetProfileNames();
        }

        public void GetProfileNames()
        {
            ProfilesListBox.ItemsSource = profileTable.GetData().AsEnumerable().Select(row => row.Field<string>("name")).ToList();
        }

        private void CreateProfileButton_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).MainWindowFrame.Content = new CreateNewProfilePage((Application.Current.MainWindow as MainWindow).mainPage);
        }

        private void DeleteProfileToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            TitleTextBlock.Text = "Удалить профиль";
        }

        private void DeleteProfileToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            TitleTextBlock.Text = "Выбрать профиль";
        }

        private void ProfilesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(bool)DeleteProfileToggleButton.IsChecked)
            {
                if (ProfilesListBox.SelectedItem.ToString().Contains("Бог"))
                {
                    GodsWindow godsWindow = new GodsWindow(ProfilesListBox.SelectedItem.ToString());
                    godsWindow.Show();
                    (Application.Current.MainWindow as MainWindow).Close();
                }
                else
                {
                    CommonProfileWindow commonProfileWindow = new CommonProfileWindow(ProfilesListBox.SelectedItem.ToString());
                    commonProfileWindow.Show();
                    (Application.Current.MainWindow as MainWindow).Close();
                }
            }
            else
            {
                ConfirmationWindow confirmationWindow = new ConfirmationWindow();
                confirmationWindow.ConfirmationTextBlock.Text = "Вы точно хотите удалить профиль?";

                bool? result = confirmationWindow.ShowDialog();

                if (result == true)
                {
                    profileTable.DeleteQuery(ProfilesListBox.SelectedItem.ToString());
                    GetProfileNames();
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationWindow confirmationWindow = new ConfirmationWindow();
            confirmationWindow.ConfirmationTextBlock.Text = "Вы точно хотите выйти в Windows?";
            bool? result = confirmationWindow.ShowDialog();

            if (result == true)
            {
                (Application.Current.MainWindow as MainWindow).Close();
            }
        }
    }
}
