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

        //OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =Books.accdb");

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string EnteredUser = UsernameEntry.Text;
            string EnteredPassword = PasswordEntry.Text;
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

    }
}
