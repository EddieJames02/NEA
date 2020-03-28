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
using System.Windows.Shapes;
using System.Data.OleDb;

namespace FirstTest
{
    /// <summary>
    /// Interaction logic for OwnedBooksWindow.xaml
    /// </summary>
    public partial class OwnedBooksWindow : Window
    {
        public OwnedBooksWindow()
        {
            InitializeComponent();

            UserOutput1.Text = CurrentUser.firstName + " " + CurrentUser.lastName;
            if (CurrentUser.firstName != null)
            {
                LoginButton.Visibility = Visibility.Hidden;
                LogOutButton.Visibility = Visibility.Visible;
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.Visibility = Visibility.Visible;
            LogOutButton.Visibility = Visibility.Hidden;

            CurrentUser.Logout();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).LogOutButton.Visibility = Visibility.Hidden;
                }
                
            }


        }
    }


}
