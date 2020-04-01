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
            
            connect.Open();
            Console.WriteLine(SearchWindow.RetrievedID);
            Console.WriteLine(CurrentUser.userID);
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
                List<Book> TempBookList = new List<Book>();
                Console.WriteLine(item);
                OleDbCommand Data = new OleDbCommand($"SELECT * FROM TblBook WHERE BookID={item} ORDER BY BookID ASC", connect);

                OleDbDataReader DataReader = Data.ExecuteReader(); //executes Command and saves it in DataReader

                

                if (DataReader.HasRows)
                {
                    while (DataReader.Read()) //loops through each row of the returned databse
                    {


                        int bookid = DataReader.GetInt32(0); //parameter refers to the column/field
                        string booktitle = DataReader.GetString(1);
                        string bookauthor = DataReader.GetString(2);
                        string bookpublisher = DataReader.GetString(3);
                        string bookISBN = DataReader.GetString(4);
                        int pageTotal = DataReader.GetInt32(5);

                        Book TempBook = new Book(bookid, booktitle, bookauthor, bookpublisher, bookISBN, pageTotal); //Creates a temporary object that will be added into the book list (Overwritten by next loop)

                        OwnedBookList.Items.Add(TempBook.ToString());
                    }
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
            OleDbCommand CheckIfCompleted = new OleDbCommand($"SELECT Completed FROM UserOwnedBooks WHERE BookID= {Book.GetBookIDFromString(OwnedBookList.SelectedItem.ToString())}", connect);
            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }
            OleDbDataReader CompletedReader = CheckIfCompleted.ExecuteReader();
            if (CompletedReader.HasRows)
            {
                while (CompletedReader.Read())
                {
                    if (CompletedReader.GetBoolean(0) == true)
                    {
                        OwnedBookInfoWindow.CompletedCheckbox.IsChecked = true;
                    }
                    else
                    {
                        OwnedBookInfoWindow.CompletedCheckbox.IsChecked = false;
                    }
                }
            }
            

            OwnedBookInfoWindow.ShowDialog();
        }
    }


}
