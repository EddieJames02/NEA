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

namespace FirstTest
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        string EnteredUser = "6";
        string EnteredPassword = "";
        string EnteredFirst = "";
        string EnteredLast = "";

        User TempUser = new User(EnteredUser, EnteredPassword, EnteredFirst, EnteredLast);


        //public void UsernameEntry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string EnteredUser = UsernameEntry.Text; 
        //}

        //private void PasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string EnteredPassword = PasswordEntry.Text;
        //}

        //private void PasswordConEntry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string EnteredPasswordCon = PasswordConEntry.Text;
        //}

        //private void FirstEntry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string EnteredFirst = FirstEntry.Text;
        //}

        //private void LastEntry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string EnteredLast = LastEntry.Text;
        //}

    }
}
