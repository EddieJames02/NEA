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


            OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
            connect.Open();
            OleDbCommand RetrieveExistingUsers = new OleDbCommand("SELECT * From TblUsers", connect);
            OleDbDataReader DataReader = RetrieveExistingUsers.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    string tempUser = DataReader.GetString(1);
                    PreviousAccountsList.Items.Add(tempUser);
                }
            }


        }


        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
        //OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =Books.accdb");

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (LoginButton.Visibility == Visibility.Hidden)
            {
                
                if (connect.State != ConnectionState.Open)
                {
                    connect.Open(); //Opens data connection
                }

                //usercheck
                OleDbCommand ExistingUserCheck = new OleDbCommand($"SELECT UserID FROM TblUsers WHERE Username= '" + UsernameEntry.Text + "' ", connect);
                OleDbDataReader DataReader = ExistingUserCheck.ExecuteReader();

                //Password Strength Test
                bool StrongPassword = false;
                if (PasswordEntry.Password.Length > 6 && PasswordEntry.Password.Any(char.IsUpper))
                {
                    StrongPassword = true;
                }


                if (UsernameEntry.Text == "" || PasswordEntry.Password == "" || FirstEntry.Text == "" || LastEntry.Text == "")
                {
                    MessageBox.Show("One or more required fields are blank");
                }
                else
                {
                    if (PasswordEntry.Password != PasswordConEntry.Password)
                    {
                        MessageBox.Show("Passwords do not match");
                        PasswordEntry.Password = "";
                        PasswordConEntry.Password = "";
                    }
                    else if (StrongPassword == false)
                    {
                        MessageBox.Show("Password needs to be longer than 6 letters and contain at least one uppercase letter");
                        PasswordEntry.Password = "";
                        PasswordConEntry.Password = "";
                    }
                    else if (DataReader.HasRows == false)
                    {
                        string EnteredUser = UsernameEntry.Text;
                        string EnteredPassword = PasswordEntry.Password;
                        string EnteredFirst = FirstEntry.Text;
                        string EnteredLast = LastEntry.Text;

                        CurrentUser.username = EnteredUser;
                        CurrentUser.password = EnteredPassword;
                        CurrentUser.firstName = EnteredFirst;
                        CurrentUser.lastName = EnteredLast;

                        OleDbCommand InsertCmd = new OleDbCommand("insert into TblUsers ([Username], [Password], [FirstName], [LastName]) values ('" + UsernameEntry.Text + "', '" + PasswordEntry.Password + "', '" + FirstEntry.Text + "','" + LastEntry.Text + "')", connect);
                        InsertCmd.ExecuteNonQuery();
                        connect.Close();

                        this.Close();
                    }
                    else
                    {

                        MessageBox.Show("Username is already taken");
                        UsernameEntry.Text = "";
                        PasswordEntry.Password = "";
                        PasswordConEntry.Password = "";
                    }

                }


                
            }
            else
            {
                GoToLogin.Visibility = Visibility.Visible;
                LoginButton.Visibility = Visibility.Hidden;

                PassConText.Visibility = Visibility.Visible;
                FirstNameText.Visibility = Visibility.Visible;
                LastNameText.Visibility = Visibility.Visible;

                FirstEntry.Visibility = Visibility.Visible;
                LastEntry.Visibility = Visibility.Visible;
                PasswordConEntry.Visibility = Visibility.Visible;
            }
            
            

            

            //User TempUser = new User(EnteredUser, EnteredPassword, EnteredFirst, EnteredLast);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (UsernameEntry.Text != "" && PasswordEntry.Password != "")
            {
                string EnteredUser = UsernameEntry.Text;
                string EnteredPassword = PasswordEntry.Password;

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
                        string RetrievedPassword = DataReader.GetString(2);
                        if (RetrievedPassword == EnteredPassword)
                        {
                            int RetrievedUserID = DataReader.GetInt32(0);
                            string RetreievedUserName = DataReader.GetString(1);
                            string RetrievedFirstName = DataReader.GetString(3);
                            string RetrievedLastName = DataReader.GetString(4);

                            CurrentUser.username = RetreievedUserName;
                            CurrentUser.password = RetrievedPassword;
                            CurrentUser.firstName = RetrievedFirstName;
                            CurrentUser.lastName = RetrievedLastName;

                            //update "Users name" text box displays (application wide)
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.GetType() == typeof(OwnedBooksWindow))
                                {
                                    (window as OwnedBooksWindow).UserOutput1.Text = CurrentUser.firstName + " " + CurrentUser.lastName;
                                    (window as OwnedBooksWindow).LogOutButton.Visibility = Visibility.Visible;
                                }
                                else if (window.GetType() == typeof(MainWindow))
                                {
                                    (window as MainWindow).LogOutButton.Visibility = Visibility.Visible;
                                }

                            }

                            this.Close();

                            
                        }
                        else
                        {
                            MessageBox.Show("Password is incorrect");
                            PasswordEntry.Password = "";
                        }

                    }
                }
                else
                {
                    MessageBox.Show("User not found:\nPlease sign up if you don't have an existing account");
                    UsernameEntry.Text = "";
                    PasswordEntry.Password = "";
                }

            }
            else
            {
                MessageBox.Show("One or more required fields are empty");
            }
            
            connect.Close();


        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GoToLogin.Visibility = Visibility.Hidden;
            LoginButton.Visibility = Visibility.Visible;

            PassConText.Visibility = Visibility.Hidden;
            FirstNameText.Visibility = Visibility.Hidden;
            LastNameText.Visibility = Visibility.Hidden;

            PasswordConEntry.Password = "";
            PasswordConEntry.Visibility = Visibility.Hidden;

            FirstEntry.Text = "";
            FirstEntry.Visibility = Visibility.Hidden;

            LastEntry.Text = "";
            LastEntry.Visibility = Visibility.Hidden;
            




        }

        private void PreviousAccountsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DeleteUsersCheckBox.IsChecked == true)
            {
                MessageBoxResult Result = MessageBox.Show("Delete User?\nThis will delete all user data", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (Result == MessageBoxResult.Yes)
                {
                    OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
                    OleDbCommand DeleteUser = new OleDbCommand($"DELETE * FROM TblUsers WHERE Username='" + this.PreviousAccountsList.SelectedItem.ToString()+ "' ", connect);
                    if (connect.State != ConnectionState.Open)
                    {
                        connect.Open(); //Opens data connection
                    }
                    DeleteUser.ExecuteNonQuery();
                    connect.Close();

                    this.PreviousAccountsList.Items.Remove(this.PreviousAccountsList.SelectedItem);
                }
            }
            else
            {
                UsernameEntry.Text = this.PreviousAccountsList.SelectedItem.ToString();
            }
            
        }

        //private void PreviousAccountsList_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //   
        //}
    }
}
