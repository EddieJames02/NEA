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
using System.Data.OleDb;
using System.Data;

namespace FirstTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");


        
        private void populateListbox(string sql)
        {
            lstBookList.Items.Clear();

            foreach (Book currentBook in Book.QueryDatabase(sql))
            {
                lstBookList.Items.Add(currentBook.ToString());
            }
        }

        private void ShowAll_Click(object sender, RoutedEventArgs e)//shows unfiltered list of all books in database
        {

            
            populateListbox($"SELECT * FROM TblBook ORDER BY BookID ASC");

            lstBookList.Visibility = Visibility.Visible;
            CloseList.Visibility = Visibility.Visible;

            connect.Close();



        }

        private void CloseList_Click(object sender, RoutedEventArgs e) //closes the list of books
        {
            lstBookList.Visibility = Visibility.Hidden;
            CloseList.Visibility = Visibility.Hidden;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//Allows users to order the list
        {
            SortBoxLabel.Visibility = Visibility.Hidden;
            string userSelection = cmbSort.SelectedValue.ToString().Substring(38);            

            switch (userSelection)//The case is the users chosen order 
            {
                case "Publisher":
                    populateListbox($"SELECT * FROM TblBook ORDER BY BookPublisher ASC");
                    break;
                case "Author":
                    populateListbox($"SELECT * FROM TblBook ORDER BY BookAuthor ASC");
                    break;
                case "Name A-Z":
                    populateListbox($"SELECT * FROM TblBook ORDER BY BookTitle ASC");
                    break;
                case "Name Z-A":
                    populateListbox($"SELECT * FROM TblBook ORDER BY BookTitle DESC");
                    break;
                
                    
                default:
                    break;
            }
        }

        


        

        private void lstBookList_MouseDoubleClick(object sender, MouseButtonEventArgs e)//When user clicks on a book
        {
            if (lstBookList.SelectedItem != null)
            {
                List<Book> bookList = Book.QueryDatabase($"SELECT * FROM TblBook WHERE" +
                    $" BookID={Book.GetBookIDFromString(lstBookList.SelectedItem.ToString())}");

                foreach (Book currentBook in bookList)
                {
                    BookInformationWindow window1 = new BookInformationWindow();
                    window1.CompletedCheckbox.Visibility = Visibility.Hidden;
                    Book.RetrievedID = Book.GetBookIDFromString(lstBookList.SelectedItem.ToString());
                    window1.LoadBasic();//Loads the basic version of book information window
                    
                    window1.ShowDialog();//displays window
                    
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow BookSearch = new SearchWindow();
            BookSearch.ShowDialog();//displays search window
        }

        private void BookSearch_MouseEvent(object sender, MouseEventArgs e)//makes button text font style change to italic when mouse hovers over
        {
            Button hoveredButton = sender as Button;

            if (hoveredButton.FontStyle != FontStyles.Italic)
            {
                hoveredButton.FontStyle = FontStyles.Italic;
            } else
            {
                hoveredButton.FontStyle = FontStyles.Normal;
            }

        }

        private void OwnedBooks_Click(object sender, RoutedEventArgs e)
        {
            OwnedBooksWindow OwnedBooks = new OwnedBooksWindow();
            OwnedBooks.ShowDialog();//displays owned books window
            
            
        }

        private void MainWindowUserButton_Click(object sender, RoutedEventArgs e)//Either takes user to login screen or shows their current details
        {
            if (CurrentUser.firstName == null)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show($"Current User: {CurrentUser.firstName} {CurrentUser.lastName}");

            }
            
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)//Logs user out
        {
            LogOutButton.Visibility = Visibility.Hidden;
            CurrentUser.Logout();

        }
    }
}
