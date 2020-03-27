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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            
        }

        
        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
        //OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =Books.accdb");

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GoToLogin.Visibility = Visibility.Visible;
            LoginButton.Visibility = Visibility.Hidden;

            PassConText.Visibility = Visibility.Visible;
            FirstNameText.Visibility = Visibility.Visible;
            LastNameText.Visibility = Visibility.Visible;

            FirstEntry.Visibility = Visibility.Visible;
            LastEntry.Visibility = Visibility.Visible;
            PasswordConEntry.Visibility = Visibility.Visible;

            


            string EnteredUser = UsernameEntry.Text;
            string EnteredPassword = PasswordEntry.Text;
            string ConfirmedPassword = PasswordConEntry.Text;
            string EnteredFirst = FirstEntry.Text;
            string EnteredLast = LastEntry.Text;

            OleDbCommand InsertCmd = new OleDbCommand("insert into TblUsers ([Username], [Password], [FirstName], [LastName]) values ('" + UsernameEntry.Text + "', '" + PasswordEntry.Text + "', '" + FirstEntry.Text + "','" + LastEntry.Text + "')", connect);
            //InsertCmd.Parameters.AddWithValue("@Username", UsernameEntry.Text);
            //InsertCmd.Parameters.AddWithValue("@Password", PasswordEntry.Text);
            //InsertCmd.Parameters.AddWithValue("@FirstName", FirstEntry.Text);
            //InsertCmd.Parameters.AddWithValue("@LastName", LastEntry.Text);

            connect.Open();
            InsertCmd.ExecuteNonQuery();
            connect.Close();

            //User TempUser = new User(EnteredUser, EnteredPassword, EnteredFirst, EnteredLast);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {



            if (UsernameEntry.Text != "")
            {
                string EnteredUser = UsernameEntry.Text;

                if (connect.State != ConnectionState.Open)
                {
                    connect.Open(); //Opens data connection
                }

                OleDbCommand InsertCmd = new OleDbCommand($"SELECT * FROM TblUsers WHERE Username= '" + EnteredUser + "' ", connect);
                OleDbDataReader DataReader = InsertCmd.ExecuteReader();

                if (DataReader.HasRows)
                {
                    while (DataReader.Read()) //loops through each row of the returned databse
                    {
                        int RetrievedUserID = DataReader.GetInt32(0);
                        string RetreievedUserName = DataReader.GetString(1);
                        string RetrievedPassword = DataReader.GetString(2);
                        string RetrievedFirstName = DataReader.GetString(3);
                        string RetrievedLastName = DataReader.GetString(4);

                        User CurrentUser = new User(RetreievedUserName, RetrievedPassword, RetrievedFirstName, RetrievedLastName);

                    }
                }
                
            }
            //string EnteredPassword = PasswordEntry.Text;



            //else if (DataReader == null)
            //{
            //    MessageBox.Show("user not found\n Please sign up if you dont have existing account");
            //    UsernameEntry.Text = "";
            //    PasswordEntry.Text = "";
            //}
            connect.Close();


        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GoToLogin.Visibility = Visibility.Hidden;
            LoginButton.Visibility = Visibility.Visible;

            PassConText.Visibility = Visibility.Hidden;
            FirstNameText.Visibility = Visibility.Hidden;
            LastNameText.Visibility = Visibility.Hidden;

            FirstEntry.Visibility = Visibility.Hidden;
            LastEntry.Visibility = Visibility.Hidden;
            PasswordConEntry.Visibility = Visibility.Hidden;


        }
    }
}
