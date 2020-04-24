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
            if (CurrentUser.firstName != null)//slight window variatiions dependant on whether user is logged in or not
            {
                LoginButton.Visibility = Visibility.Hidden;
                LogOutButton.Visibility = Visibility.Visible;
                LoadOwnedBooks();
                
            }

        }
        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");

        public void LoadOwnedBooks()//Populates the owned books listbox
        {
            OwnedBookList.Items.Clear();
            connect.Open();
            List<int> RetrievedOwnedBookIDs = new List<int>();
            OleDbCommand GetOwnedBookIDs = new OleDbCommand($"SELECT BookID From UserOwnedBooks WHERE UserID={CurrentUser.userID}", connect);//Gets all the BookIDs that the user owns
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

                
                foreach (Book currentBook in Book.QueryDatabase($"SELECT * FROM TblBook WHERE BookID={item} ORDER BY BookID ASC"))//Uses the list of book ID's to retrieve the book data
                {
                    OwnedBookList.Items.Add(currentBook.ToString());
                }

            }
            connect.Close();
        }


        

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();//displays login window
            
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser.Logout();
        }

        private void OwnedBookList_MouseDoubleClick(object sender, MouseButtonEventArgs e)//Event upon clicking on a book
        {
            
            BookInformationWindow OwnedBookInfoWindow = new BookInformationWindow();
            OwnedBookInfoWindow.CompletedCheckbox.Visibility = Visibility.Visible;
            Book.RetrievedID = Book.GetBookIDFromString(OwnedBookList.SelectedItem.ToString());
            OwnedBookInfoWindow.LoadAll();//Loads the more detailed variation of Book information window


            OwnedBookInfoWindow.ShowDialog();
        }

        //Uses Entered ISBN to add owned book to owned book list
        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.firstName == null || CurrentUser.firstName == string.Empty)
            {
                MessageBox.Show("You need to be logged in to add a book");
            }
            else if (ISBNEntry.Text.Length != 14)
            {
                MessageBox.Show("Not correct Length");
            }
            else if (ISBNEntry.Text.Any(char.IsLetter))
            {
                MessageBox.Show("Digits only");

            }
            else
            {
                //ISBN validation
                string EnteredISBN = ISBNEntry.Text.Substring(0, 3) + ISBNEntry.Text.Substring(4, 10);
                int sum = 0;

                for (int x = 0; x < 12; x++)
                {
                    if (x % 2 == 0)
                    {
                        sum = sum + (1 * int.Parse(EnteredISBN.Substring(x, 1)));
                    }
                    else
                    {
                        sum = sum + (3 * int.Parse(EnteredISBN.Substring(x, 1)));
                    }


                }
                int modulo = sum % 10;
                int CheckDigit = 10 - modulo;
                if (CheckDigit == int.Parse(EnteredISBN.Substring(12, 1)))//If ISBN is valid
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        connect.Open(); //Opens data connection
                    }

                    OleDbCommand FindISBN = new OleDbCommand($"SELECT BookID FROM TblBook WHERE BookISBN= '" + ISBNEntry.Text + "'", connect);//Selects the bookID that has an ISBN equal to entered ISBN (if one exists)
                    OleDbDataReader DataReader = FindISBN.ExecuteReader();
                    if (DataReader.HasRows)
                    {
                        while (DataReader.Read())
                        {
                            DateTime dateNow = DateTime.Now;
                            OleDbCommand InsertOwnedBook = new OleDbCommand("insert into UserOwnedBooks ([BookID], [UserID], [StartDate]) values ('" + DataReader.GetInt32(0) + "', '" + CurrentUser.userID + "', '" + dateNow + "')", connect);
                            InsertOwnedBook.ExecuteNonQuery();
                            connect.Close();
                            LoadOwnedBooks();
                            break; //Reader error fix
                        }
                    }
                    else
                    {
                        MessageBox.Show("Book not found in database");
                        ISBNEntry.Text = string.Empty;
                    }
                    connect.Close();
                }
                else
                {
                    MessageBox.Show("Invalid ISBN");
                    ISBNEntry.Text = string.Empty;
                }
            }
            
            

        }




        private int PreviousLength=0;
        private void ISBNEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Adds Hyphon into ISBN string
            if (ISBNEntry.Text.Length == 3 && (PreviousLength == 2 || PreviousLength == 3)) 
            {
                ISBNEntry.Text = ISBNEntry.Text + "-";
                ISBNEntry.CaretIndex = 4;
            }
            
            PreviousLength = ISBNEntry.Text.Length;
        }
    }


}
