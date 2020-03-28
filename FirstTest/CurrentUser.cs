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
        public static string username;
        public static string password;
        public static string firstName;
        public static string lastName;

        //static int[] favouriteIDs;

        static public void Logout()
        {
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
                    //(window as MainWindow).UserOutput1.Text = CurrentUser.firstName + " " + CurrentUser.lastName;
                }
                

            }
        }

        

        
    }
}
