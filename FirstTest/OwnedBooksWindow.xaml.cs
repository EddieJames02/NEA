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
using System.Data;

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
                LoadOwnedBooks();
                
            }

        }
        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");

        public void LoadOwnedBooks()
        {
            OwnedBookList.Items.Clear();
            connect.Open();
            List<int> RetrievedOwnedBookIDs = new List<int>();
            OleDbCommand GetOwnedBookIDs = new OleDbCommand($"SELECT BookID From UserOwnedBooks WHERE UserID={CurrentUser.userID}", connect);
            OleDbDataReader dataReader = GetOwnedBookIDs.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    RetrievedOwnedBookIDs.Add(dataReader.GetInt32(0));
                }
            }
            foreach (int item in RetrievedOwnedBookIDs)
            {

                
                foreach (Book currentBook in Book.QueryDatabase($"SELECT * FROM TblBook WHERE BookID={item} ORDER BY BookID ASC"))
                {
                    OwnedBookList.Items.Add(currentBook.ToString());
                }

            }
            connect.Close();
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

        private void OwnedBookList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            Window1 OwnedBookInfoWindow = new Window1();
            OwnedBookInfoWindow.CompletedCheckbox.Visibility = Visibility.Visible;
            Book.RetrievedID = Book.GetBookIDFromString(OwnedBookList.SelectedItem.ToString());
            OwnedBookInfoWindow.LoadAll();
            OwnedBookInfoWindow.ShowDialog();
        }
    }


}
