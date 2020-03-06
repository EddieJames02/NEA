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
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        //OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =H:\A-Levels\computer science\DataTestingWPF\FirstTest\Books.accdb");
        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
        List<Book> BookList = new List<Book>();
        
        

        string currentFilter = "Default";

        public SearchWindow()
        {
            InitializeComponent();

            BookList = FilteredBookList(string.Empty);

            foreach (Book book in BookList)
            {
                SearchResults.Items.Add(book.ToString());
            }
        }

        private List<Book> FilteredBookList(string filter)
        {
            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }

            OleDbCommand Data;

            if (filter.Equals(string.Empty))
            {
                Data = new OleDbCommand($"SELECT * FROM TblBook ORDER BY BookID ASC", connect);
            }
            else
            {
                Data = new OleDbCommand($"SELECT * FROM TblBook ORDER BY " + filter, connect);
            }

            List<Book> TempBookList = new List<Book>();

            
            OleDbDataReader DataReader = Data.ExecuteReader(); //executes Command and saves it in DataReader

            BookList.Clear();
            SearchResults.Items.Clear();

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

                    TempBookList.Add(TempBook);
                }
            }

            return TempBookList;
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchResults.Items.Clear();

            string CurrentSearch = SearchBox.Text;

            Title = $"Filter: {currentFilter} and md Search: {CurrentSearch}";

            foreach (Book currentBook in BookList)
            {
                switch (currentFilter)
                {
                    case "Title":
                        if (currentBook.Title.ToLower().Contains(CurrentSearch.ToLower()))
                        {
                            SearchResults.Items.Add(currentBook.ToString());
                        }
                    break;
                    case "Publisher":
                        if (currentBook.Publisher.ToLower().Contains(CurrentSearch.ToLower()))
                        {
                            SearchResults.Items.Add(currentBook.ToString());
                        }
                        break;
                    case "Author":
                        if (currentBook.Author.ToLower().Contains(CurrentSearch.ToLower()))
                        {
                            SearchResults.Items.Add(currentBook.ToString());
                        }
                        break;
                    case "Default":
                        if (currentBook.Author.ToLower().Contains(CurrentSearch.ToLower()) || currentBook.Publisher.ToLower().Contains(CurrentSearch.ToLower()) || currentBook.Title.ToLower().Contains(CurrentSearch.ToLower()))
                        {
                            SearchResults.Items.Add(currentBook.ToString());
                        }
                        break;
                    default:
                        break;
                }
            }
            

        }


        private void PublisherButton_Click(object sender, RoutedEventArgs e)
        {

            if (PublisherButton.Background == Brushes.Gray)
            {
                PublisherButton.Background = Brushes.White;
                currentFilter = "Default";
            }
            else
            {
                PublisherButton.Background = Brushes.Gray;
                AuthorButton.Background = Brushes.White;
                TitleButton.Background = Brushes.White;


                //foreach (Book currentBook in FilteredBookList("BookPublisher"))
                //{
                //    SearchResults.Items.Add(currentBook.ToString());
                //}

                currentFilter = "Publisher";
            }    
        }

        private void TitleButton_Click(object sender, RoutedEventArgs e)
        {
            if (TitleButton.Background == Brushes.Gray)
            {
                TitleButton.Background = Brushes.White;
                currentFilter = "Default";
            }
            else
            {
                PublisherButton.Background = Brushes.White;
                AuthorButton.Background = Brushes.White;
                TitleButton.Background = Brushes.Gray;


                //foreach (Book currentBook in FilteredBookList("BookTitle"))
                //{
                //    SearchResults.Items.Add(currentBook.ToString());
                //}

                currentFilter = "Title";
            }
        }

        private void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorButton.Background == Brushes.Gray)
            {
                AuthorButton.Background = Brushes.White;
                currentFilter = "Default";
            }
            else
            {
                PublisherButton.Background = Brushes.White;
                AuthorButton.Background = Brushes.Gray;
                TitleButton.Background = Brushes.White;


                //foreach (Book currentBook in FilteredBookList("BookAuthor"))
                //{
                //    SearchResults.Items.Add(currentBook.ToString());
                //}

                currentFilter = "Author";
            }
        }

        private List<Book> QueryDatabase(string sql)
        {
            OleDbCommand Data = new OleDbCommand(sql, connect);

            if (connect.State == ConnectionState.Closed)
            {
                connect.Open(); //Opens data connection
            }

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

        private void SearchResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }

            if (SearchResults.SelectedItem != null)
            {
                List<Book> bookList = QueryDatabase($"SELECT * FROM TblBook WHERE BookID={Book.GetBookIDFromString(SearchResults.SelectedItem.ToString())}");

                foreach (Book currentBook in bookList)
                {
                    Window1 window1 = new Window1();
                    window1.BookInfoDisplay1.AppendText(currentBook.ToString2());
                    window1.ShowDialog();

                }
            }
        }
    }
}
