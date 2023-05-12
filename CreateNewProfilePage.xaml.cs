using Laboratory_work_No._5.overlordDataSetTableAdapters;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для CreateNewProfilePage.xaml
    /// </summary>
    public partial class CreateNewProfilePage : Page
    {
        private MainPage mainPage;

        public CreateNewProfilePage(MainPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string profileName = NewProfileNameTextBox.Text;
            if (!string.IsNullOrEmpty(profileName))
            {
                mainPage.profileTable.InsertQuery(profileName);
                mainPage.GetProfileNames();

                NavigationService.GoBack();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
