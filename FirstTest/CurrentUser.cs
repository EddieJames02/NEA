using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;

namespace FirstTest
{
    static public class CurrentUser
    {
        public static int userID;
        public static string username;
        public static string password;
        public static string firstName;
        public static string lastName;

        public static OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");


        static public void Logout()
        {
            userID = -1;
            username = null;
            password = null;
            firstName = null;
            lastName = null;

            foreach (Window window in Application.Current.Windows)
            {

                if (window.GetType() == typeof(OwnedBooksWindow))
                {
                    (window as OwnedBooksWindow).UserOutput1.Text = CurrentUser.firstName + " " + CurrentUser.lastName;
                }
                else if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).LogOutButton.Visibility = Visibility.Hidden;
                }
                

            }
        }

        static public void LoadOwnedList()
        {
            
            
        }

        static public void AssignUserID()
        {
            if (connect.State != ConnectionState.Open)
            {
                connect.Open(); //Opens data connection
            }

            OleDbCommand GetID = new OleDbCommand("SELECT UserID FROM TblUsers WHERE Username='" + CurrentUser.username + "'", connect);
            OleDbDataReader IDReader = GetID.ExecuteReader();
            CurrentUser.userID = IDReader.GetInt32(0);

        }

        

        
    }
}
