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
    public partial class BookInformationWindow : Window
    {
        //Unable to change file name to "BookInformationWindow" as this was cuasing problems with locating the file
        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
        Book DisplayedBook;

        public BookInformationWindow()
        {
            InitializeComponent();

        }

        public void LoadAll()//Loads the more detailed version of this window
        {
            LoadCompletedCheckbox();
            LoadExtraDetails();
            LoadOwnedCheckbox();
            LoadBookInfo();
            LoadRating();
        }

        public void LoadBasic()
        {
            LoadOwnedCheckbox();
            LoadBookInfo();
        }



        private void LoadExtraDetails()//Loads: Date added, current pageNo., End Date (if needed)
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

        public void LoadOwnedCheckbox() //Makes Ownedcheckbox visible, also loads if it should be checked or not
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

        private void LoadCompletedCheckbox()//Makes Completedcheckbox visible, also loads if it should be checked or not
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

        private void LoadBookInfo() //Displays all the info about the book stored in the database - each field
        {
            List<Book> bookList = Book.QueryDatabase($"SELECT * FROM TblBook WHERE BookID={Book.RetrievedID}");
            foreach (Book currentBook in bookList)
            {
                TitleDisplayBox.Text = currentBook.Title;
                AuthorDisplayBox.Text = currentBook.Author;
                ReleaseDateDisplayBox.Text = "Original Release Year: " + currentBook.ReleaseYear.ToString();
                OverallRatingDisplay.Text = "Overall Rating: " + currentBook.OverallRating.ToString();
                PublisherDisplayBox.Text = "Publisher: " + currentBook.Publisher;
                ISBNDisplayBox.Text = "ISBN: " + currentBook.ISBN;
                
                Book.Maxpages = currentBook.Pages;

                DisplayedBook = currentBook;
            }
        }

        private int previousSliderValue;
        private void LoadRating()//If the user has rated before the slider will be positioned where they rated it.
        {
            //makes rating visibile
            RatingText.Visibility = Visibility.Visible;
            RatingSlider.Visibility = Visibility.Visible;


            //Sets the slider to be correct rating
            OleDbCommand GetCurrentRating = new OleDbCommand($"SELECT UserRating FROM UserOwnedBooks WHERE BookID= {Book.RetrievedID} AND UserID= {CurrentUser.userID}", connect);
            connect.Open();
            OleDbDataReader ReadRating = GetCurrentRating.ExecuteReader();


            if (ReadRating.HasRows)
            {

                while (ReadRating.Read())
                {
                    double testvalue = Convert.ToDouble(ReadRating.GetInt32(0));
                    RatingSlider.Value = testvalue; //Triggers RatingSlider_ValueChanged event therefore incrementing NumberOfRatings each time the rating is loaded. This means overall rating is not correct. No current fix.
                    break;//solution to reader issue
                }

            }
            previousSliderValue = Convert.ToInt32(RatingSlider.Value); //saves the previous value of the slider
            connect.Close();
        }


        private void OwnedCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (OwnedCheckbox.IsChecked == true)//ifuser is marking book as owned
            {
                DateTime dateNow = DateTime.Now;

                //Creates a new record in the userOwnedBooks table.
                if (CurrentUser.username != null)
                {

                    //'" + dateNow.ToString("yyyyMMdd") + "'
                    OleDbCommand InsertOwnedBook = new OleDbCommand("insert into UserOwnedBooks ([BookID], [UserID], [StartDate]) values ('" + Book.RetrievedID + "', '" + CurrentUser.userID + "', '" + dateNow + "')", connect);
                    DateAddedBlock.Text = dateNow.ToString(); //displays current date to user as the date started
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
                    OwnedCheckbox.IsChecked = false;//Resets checkbox to unchecked
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();//displays login window
                    
                }
            }
            else//if user is marking book as unowned
            {
                MessageBoxResult boxResult = MessageBox.Show("This will remove from your owned books and permanently delete your progress\nAre you sure?", "Remove?", MessageBoxButton.YesNo);
                if (boxResult == MessageBoxResult.Yes)//Validates that the user wants to delete
                {
                    if (connect.State != ConnectionState.Open)
                    {
                        connect.Open(); //Opens data connection
                    }
                    OleDbCommand DeleteOwnedBook = new OleDbCommand($"DELETE * FROM UserOwnedBooks WHERE UserID= {CurrentUser.userID} AND BookID= {Book.RetrievedID}", connect);
                    DeleteOwnedBook.ExecuteNonQuery();//Deletes relavant record from UserOwnedBooks
                    connect.Close();
                    DateAddedBlock.Visibility = Visibility.Hidden;
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window.GetType() == typeof(OwnedBooksWindow))
                        {
                            (window as OwnedBooksWindow).LoadOwnedBooks(); //Refreshes the owned book list to not conatin deleted item
                        }
                    }
                }
                else
                {
                    OwnedCheckbox.IsChecked = true;//Resets checkbox as if it was never clicked
                }
            }
        }

        private void CompletedCheckbox_Click(object sender, RoutedEventArgs e)//allows user to mark a book as completed
        {
            int YesNoValue;
                     
            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }

            if (CompletedCheckbox.IsChecked == true)
            {
                DateTime dateNow = DateTime.Now;
                YesNoValue = -1; //-1 refers to a YES value in the database
                OleDbCommand InsertCompleted = new OleDbCommand("UPDATE UserOwnedBooks SET Completed= '" + YesNoValue + "', EndDate= '" + dateNow + "', PagesComplete= '" + Book.Maxpages + $"' WHERE UserID= {CurrentUser.userID} AND BookID= {Book.RetrievedID}", connect); //Sets database values, Completed = YES, DateEnded = current date, Page progress = total pages in book
                InsertCompleted.ExecuteNonQuery();
                connect.Close();
                PageNoDisplayValue.Text = Book.Maxpages.ToString();//updates page progress to max
            }
            else
            {
                YesNoValue = 0;
                DateTime dt = new DateTime(01, 01, 0001, 0, 0, 0); //best epresntation of a null date i could get
                OleDbCommand InsertCompleted = new OleDbCommand("UPDATE UserOwnedBooks SET Completed= '" + YesNoValue + "', PagesComplete= '" + 0 + "', EndDate= '" + dt + $"' WHERE UserID= {CurrentUser.userID} AND BookID= {Book.RetrievedID}", connect);
                
                InsertCompleted.ExecuteNonQuery();//updates database to show book as incomplete
                
                connect.Close();
                PageNoDisplayValue.Text = 0.ToString();//resets displayed progress to 0 from max
            }
            
            
            


        }

        

        private void PageNoDisplayValue_TextInput(object sender, TextCompositionEventArgs e)//Allows user to input their current page number
        {
            if (int.Parse(PageNoDisplayValue.Text) >= Book.Maxpages)
            {
                PageNoDisplayValue.Text = Book.Maxpages.ToString();//makes sure inputted number cannot be bigger than max value
            }
            
            connect.Open();
            OleDbCommand InsertPageNo = new OleDbCommand("UPDATE UserOwnedBooks SET PagesComplete=  '" + PageNoDisplayValue.Text + "'", connect);
            InsertPageNo.ExecuteNonQuery();
            connect.Close();
            
        }

        

        private void RatingSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }

            int OldNumberOfRatings = DisplayedBook.NumberOfRatings;
            if (previousSliderValue == 0)
            {
                //increments number of ratings if last rating was 0
                DisplayedBook.NumberOfRatings += 1;
                OleDbCommand UpdateNumberOfRatings = new OleDbCommand($"UPDATE TblBook SET NumberOfRatings= {DisplayedBook.NumberOfRatings} WHERE BookID= {Book.RetrievedID}", connect);
                UpdateNumberOfRatings.ExecuteNonQuery();
            }
            
           
            if (RatingSlider.Value == 0) //if user rates book as 0 their rating is now invalid, number of ratings is decremented. Stops one user being counted twice towards total
            {
                DisplayedBook.NumberOfRatings -= 1;
                OleDbCommand UpdateNumberOfRatings = new OleDbCommand($"UPDATE TblBook SET NumberOfRatings= {DisplayedBook.NumberOfRatings} WHERE BookID= {Book.RetrievedID}", connect);
                UpdateNumberOfRatings.ExecuteNonQuery();
            }
            //Updates the user rating in database
            OleDbCommand UpdateRating = new OleDbCommand($"UPDATE UserOwnedBooks SET UserRating= {RatingSlider.Value} WHERE BookID= {Book.RetrievedID} AND UserID= {CurrentUser.userID}", connect);
            UpdateRating.ExecuteNonQuery();
            previousSliderValue = Convert.ToInt32(RatingSlider.Value);//saves the value so next time slider is changed it can be checked if this value was equal to 0


            //Calculates new overall rating
            int NewOverallRating = ((DisplayedBook.OverallRating * OldNumberOfRatings) + (int)RatingSlider.Value) / DisplayedBook.NumberOfRatings;

            OleDbCommand UpdateOverallRating = new OleDbCommand($"UPDATE TblBook SET OverallRating= {NewOverallRating} WHERE BookID= {Book.RetrievedID}", connect);//Creates a average of all the ratings for that book
            UpdateOverallRating.ExecuteNonQuery();//updates table with new overall rating
            connect.Close();
        } 

        
    }
}
