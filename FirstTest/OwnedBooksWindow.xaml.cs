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
    /// Interaction logic for OwnedBooksWindow.xaml
    /// </summary>
    public partial class OwnedBooksWindow : Window
    {
        public OwnedBooksWindow()
        {
            InitializeComponent();
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();

            OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
            connect.Open();
            OleDbCommand RetrieveExistingUsers = new OleDbCommand("SELECT * From TblUsers", connect);
            OleDbDataReader DataReader = RetrieveExistingUsers.ExecuteReader();

            if (DataReader.HasRows)
            {
                while (DataReader.Read())
                {
                    string tempUser = DataReader.GetString(1);
                    loginWindow.PreviousAccountsList.Items.Add(tempUser);
                }
            }

            loginWindow.ShowDialog();
            
        }
    }


}
