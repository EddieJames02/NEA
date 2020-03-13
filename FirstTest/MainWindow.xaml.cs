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

        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =H:\A-Levels\computer science\DataTestingWPF\FirstTest\Books.accdb");
        //OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
        List<Book> BookList = new List<Book>();
        OleDbDataAdapter UpdateBooks = new OleDbDataAdapter();


        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand Data = new OleDbCommand($"SELECT * FROM TblBook ORDER BY BookID ASC", connect);
            if (connect.State == ConnectionState.Closed)
            {
                connect.Open();
                //Opens data connection
            }

            OleDbDataReader DataReader = Data.ExecuteReader(); //executes Command and saves it in DataReader

            BookList.Clear();
            lstBookList.Items.Clear();

            if (DataReader.HasRows)
            {
                while (DataReader.Read()) //loops through each row of the returned databse
                {


                    int bookid = DataReader.GetInt32(0); //parameter refers to the column/field
                    string booktitle = DataReader.GetString(1);
                    string bookauthor = DataReader.GetString(2);
                    string bookpublisher = DataReader.GetString(3);
                    string bookISBN = DataReader.GetString(4);

                    Book TempBook = new Book(bookid, booktitle, bookauthor, bookpublisher, bookISBN); //Creates a temporary object that will be added into the book list (Overwritten by next loop)

                    BookList.Add(TempBook);




                }
            }

            foreach (Book currentBook in BookList)
            {
                lstBookList.Items.Add(currentBook.ToString()); //Appends the textbox with each book (
            }

            lstBookList.Visibility = Visibility.Visible;
            CloseList.Visibility = Visibility.Visible;

            connect.Close();



        }

        private void CloseList_Click(object sender, RoutedEventArgs e)
        {
            lstBookList.Visibility = Visibility.Hidden;
            CloseList.Visibility = Visibility.Hidden;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string userSelection = cmbSort.SelectedValue.ToString().Substring(38);            

            switch (userSelection)
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

        private void populateListbox(string sql)
        {
            lstBookList.Items.Clear();

            foreach (Book currentBook in QueryDatabase(sql))
            {
                lstBookList.Items.Add(currentBook.ToString());
            }
        }

        private List<Book> QueryDatabase(string sql)
        {
            OleDbCommand Data = new OleDbCommand(sql, connect);
            connect.Open();
            OleDbDataReader DataReader = Data.ExecuteReader(); //executes Command and saves it in DataReader
            List<Book> listToReturn = new List<Book>();

            if (DataReader.HasRows)
            {
                while (DataReader.Read()) //loops through each row of the returned databse
                {
                    int bookid = DataReader.GetInt32(0); //parameter refers to the column/field
                    string booktitle = DataReader.GetString(1);
                    string bookauthor = DataReader.GetString(2);
                    string bookpublisher = DataReader.GetString(3);
                    string bookISBN = DataReader.GetString(4);

                    Book TempBook = new Book(bookid, booktitle, bookauthor, bookpublisher, bookISBN); //Creates a temporary object that will be added into the book list (Overwritten by next loop)

                    listToReturn.Add(TempBook);
                } 
            }
            connect.Close();
            return listToReturn;
            
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void lstBookList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstBookList.SelectedItem != null)
            {
                List<Book> bookList = QueryDatabase($"SELECT * FROM TblBook WHERE" +
                    $" BookID={Book.GetBookIDFromString(lstBookList.SelectedItem.ToString())}");

                foreach (Book currentBook in bookList)
                {
                    Window1 window1 = new Window1();
                    window1.BookInfoDisplay1.AppendText(currentBook.ToString2());
                    window1.ShowDialog();
                    
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow BookSearch = new SearchWindow();
            BookSearch.ShowDialog();
        }

        private void BookSearch_MouseEvent(object sender, MouseEventArgs e)
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
            OwnedBooks.ShowDialog();
            
        }
    }
}
