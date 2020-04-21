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

            FillBookList(string.Empty);

            
        }

        private void FillBookList(string filter)
        {
            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }
            SearchResults.Items.Clear();
            string sql;

            if (filter.Equals(string.Empty))
            {
                sql = $"SELECT * FROM TblBook ORDER BY BookID ASC";
            }
            else
            {
                sql = $"SELECT * FROM TblBook ORDER BY " + filter + " ASC";
            }
            foreach (Book currentBook in Book.QueryDatabase(sql))
            {
                SearchResults.Items.Add(currentBook.ToString());
                BookList.Add(currentBook);
            }
            

            
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
                //FillBookList(currentFilter); unable to get this to work 
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



        //

        private void SearchResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            

            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }

            if (SearchResults.SelectedItem != null)
            {
                
                Window1 window1 = new Window1();
                window1.CompletedCheckbox.Visibility = Visibility.Hidden;

                window1.LoadBasic();
                Book.RetrievedID = Book.GetBookIDFromString(SearchResults.SelectedItem.ToString());
                window1.ShowDialog();

                    

                
            }
        }
    }
}
