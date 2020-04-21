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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");

        public Window1()
        {
            InitializeComponent();

        }
        public void LoadAll()
        {
            LoadCompletedCheckbox();
            LoadExtraDetails();
            LoadOwnedCheckbox();
            LoadBookInfo();
        }
        public void LoadBasic()
        {
            LoadOwnedCheckbox();
            LoadBookInfo();
        }



        public void LoadExtraDetails()
        {
            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }
            OleDbCommand OwnedCheck = new OleDbCommand($"SELECT * FROM UserOwnedBooks WHERE BookID= {Book.RetrievedID} AND UserID={CurrentUser.userID}", connect);

            OleDbDataReader ownedCheckReader = OwnedCheck.ExecuteReader();
            DateAddedBlock.Visibility = Visibility.Hidden;
            if (ownedCheckReader.HasRows)
            {
                while (ownedCheckReader.Read())
                {
                    string DateAdded = ownedCheckReader.GetDateTime(3).ToString();
                    PageNoDisplayValue.Text = ownedCheckReader.GetInt32(5).ToString();
                    PageNoDisplayValue.Visibility = Visibility.Visible;
                    PageNoDisplay.Visibility = Visibility.Visible;

                    
                    if (CompletedCheckbox.IsChecked == true)
                    {
                        string DateCompleted = ownedCheckReader.GetDateTime(4).ToString();
                        DateCompletedBlock.Visibility = Visibility.Visible;
                        DateCompletedBlock.Text = DateCompleted;
                    }
                    DateAddedBlock.Visibility = Visibility.Visible;
                    DateAddedBlock.Text = DateAdded;
                }

            }
        }

        public void LoadOwnedCheckbox()
        {
            if (CurrentUser.username != null)
            {

                if (connect.State != ConnectionState.Open)
                {
                    connect.Open(); //Opens data connection
                }
                OleDbCommand OwnedCheck = new OleDbCommand($"SELECT * FROM UserOwnedBooks WHERE BookID= {Book.RetrievedID} AND UserID={CurrentUser.userID}", connect);

                OleDbDataReader ownedCheckReader = OwnedCheck.ExecuteReader();
                DateAddedBlock.Visibility = Visibility.Hidden;
                if (ownedCheckReader.HasRows)
                {
                    OwnedCheckbox.IsChecked = true;
                    while (ownedCheckReader.Read())
                    {
                        string DateAdded = ownedCheckReader.GetDateTime(3).ToString();
                        DateAddedBlock.Visibility = Visibility.Visible;
                        DateAddedBlock.Text = DateAdded;
                    }

                }
                connect.Close();
            }
        }

        public void LoadCompletedCheckbox()
        {
            OleDbCommand CheckIfCompleted = new OleDbCommand($"SELECT Completed FROM UserOwnedBooks WHERE BookID= {Book.RetrievedID} AND UserID= {CurrentUser.userID}", connect);
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
                        CompletedCheckbox.IsChecked = true;
                    }
                    else
                    {
                        CompletedCheckbox.IsChecked = false;
                    }
                }
            }
        }

        public void LoadBookInfo()
        {
            List<Book> bookList = Book.QueryDatabase($"SELECT * FROM TblBook WHERE BookID={Book.RetrievedID}");
            foreach (Book currentBook in bookList)
            {
                
                Book.Maxpages = currentBook.Pages;
            }
        }


        private void OwnedCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (OwnedCheckbox.IsChecked == true)
            {
                DateTime dateNow = DateTime.Now;

                //OleDbCommand OwnedBookExistsCheck = new OleDbCommand()
                if (CurrentUser.username != null)
                {

                    //'" + dateNow.ToString("yyyyMMdd") + "'
                    OleDbCommand InsertOwnedBook = new OleDbCommand("insert into UserOwnedBooks ([BookID], [UserID], [StartDate]) values ('" + Book.RetrievedID + "', '" + CurrentUser.userID + "', '" + dateNow + "')", connect);
                    DateAddedBlock.Text = dateNow.ToString();
                    DateAddedBlock.Visibility = Visibility.Visible;
                    if (connect.State != ConnectionState.Open)
                    {
                        connect.Open(); //Opens data connection
                    }
                    InsertOwnedBook.ExecuteNonQuery();
                    connect.Close();
                }
                else
                {
                    OwnedCheckbox.IsChecked = false;
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();
                    
                }
            }
            else
            {
                MessageBoxResult boxResult = MessageBox.Show("This will remove from your owned books and permanently delete your progress\nAre you sure?", "Remove?", MessageBoxButton.YesNo);
                if (boxResult == MessageBoxResult.Yes)
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        connect.Open(); //Opens data connection
                    }
                    OleDbCommand DeleteOwnedBook = new OleDbCommand($"DELETE * FROM UserOwnedBooks WHERE UserID= {CurrentUser.userID} AND BookID= {Book.RetrievedID}", connect);
                    DeleteOwnedBook.ExecuteNonQuery();
                    connect.Close();
                    DateAddedBlock.Visibility = Visibility.Hidden;
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(OwnedBooksWindow))
                        {
                            (window as OwnedBooksWindow).LoadOwnedBooks();
                        }
                    }
                }
                else
                {
                    OwnedCheckbox.IsChecked = true;
                }
            }
        }

        private void CompletedCheckbox_Click(object sender, RoutedEventArgs e)
        {
            int YesNoValue;
                     
            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }

            if (CompletedCheckbox.IsChecked == true)
            {
                DateTime dateNow = DateTime.Now;
                YesNoValue = -1;
                OleDbCommand InsertCompleted = new OleDbCommand("UPDATE UserOwnedBooks SET Completed= '" + YesNoValue + "', EndDate= '" + dateNow + "', PagesComplete= '" + Book.Maxpages + $"' WHERE UserID= {CurrentUser.userID} AND BookID= {Book.RetrievedID}", connect);
                InsertCompleted.ExecuteNonQuery();
                connect.Close();
                PageNoDisplayValue.Text = Book.Maxpages.ToString();
            }
            else
            {
                YesNoValue = 0;
                OleDbCommand InsertCompleted = new OleDbCommand("UPDATE UserOwnedBooks SET Completed= '" + YesNoValue + "', PagesComplete= '" + 0 + $"' WHERE UserID= {CurrentUser.userID} AND BookID= {Book.RetrievedID}", connect);
                OleDbCommand DeleteEndDate = new OleDbCommand($"DELETE EndDate FROM UserOwnedBooks WHERE UserID= {CurrentUser.userID} AND BookID= {Book.RetrievedID}", connect);
                InsertCompleted.ExecuteNonQuery();
                DeleteEndDate.ExecuteNonQuery();
                connect.Close();
                PageNoDisplayValue.Text = 0.ToString();
            }
            
            
            


        }

        

        private void PageNoDisplayValue_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (int.Parse(PageNoDisplayValue.Text) >= Book.Maxpages)
            {
                PageNoDisplayValue.Text = Book.Maxpages.ToString();
            }
            else
            {
                connect.Open();
                OleDbCommand InsertPageNo = new OleDbCommand("UPDATE UserOwnedBooks SET PagesComplete=  '" + PageNoDisplayValue.Text + "'", connect);
                InsertPageNo.ExecuteNonQuery();
                connect.Close();
            }
        }
    }
}
