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
        public Window1()
        {
            InitializeComponent();
            if (CurrentUser.username != null)
            {
                
                
                Console.WriteLine(CurrentUser.userID);
                OleDbCommand OwnedCheck = new OleDbCommand($"SELECT * FROM UserOwnedBooks WHERE BookID= {SearchWindow.RetrievedID} AND UserID={CurrentUser.userID}", connect);
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open(); //Opens data connection
                }
                OleDbDataReader ownedCheckReader = OwnedCheck.ExecuteReader();
                if (ownedCheckReader.HasRows == false)
                {
                    OwnedCheckbox.IsChecked = true;
                }
                connect.Close();
            }
            
        }

        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
        

        
        private void OwnedCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (OwnedCheckbox.IsChecked == true)
            {
                DateTime dateNow = DateTime.Now;

                //OleDbCommand OwnedBookExistsCheck = new OleDbCommand()
                if (CurrentUser.username != null)
                {
                    
                    //'" + dateNow.ToString("yyyyMMdd") + "'
                    OleDbCommand InsertOwnedBook = new OleDbCommand("insert into UserOwnedBooks ([BookID], [UserID], [StartDate]) values ('" + SearchWindow.RetrievedID + "', '" + CurrentUser.userID + "', '" + dateNow + "')", connect);

                    if (connect.State != ConnectionState.Open)
                    {
                        connect.Open(); //Opens data connection
                    }
                    InsertOwnedBook.ExecuteNonQuery();
                    connect.Close();
                }
                else
                {
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();
                    OwnedCheckbox.IsChecked = false;
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
                    OleDbCommand DeleteOwnedBook = new OleDbCommand("DELETE * FROM UserOwnedBooks WHERE UserID= '" + SearchWindow.RetrievedID + "' ");
                    DeleteOwnedBook.ExecuteNonQuery();
                    connect.Close();
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
            OleDbCommand InsertCompleted;
            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }

            if (CompletedCheckbox.IsChecked == true)
            {
                YesNoValue = -1;
                DateTime dateNow = DateTime.Now;
                InsertCompleted = new OleDbCommand($"INSERT INTO UserOwnedBooks ([Completed], [EndDate]) values ({YesNoValue}, {dateNow})", connect);
                InsertCompleted.ExecuteNonQuery();
                connect.Close();
            }
            else
            {
                YesNoValue = 0;
                //InsertCompleted = new OleDbCommand($"INSERT INTO UserOWnedBooks ")
                //InsertCompleted.ExecuteNonQuery();
                connect.Close();
            }
            
            
            


        }
    }
}
