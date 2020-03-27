using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace FirstTest
{
    public class User
    {
        private string username;
        private string password;
        private string firstName;
        private string surname;

        private int[] favouriteIDs;

        public User(string username, string password, string firstName, string surname)
        {
            this.username = username ?? throw new ArgumentNullException(nameof(username));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
            this.firstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            this.surname = surname ?? throw new ArgumentNullException(nameof(surname));
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string Surname { get => surname; set => surname = value; }

        public int[] FavouriteIDs { get => favouriteIDs; set => favouriteIDs = value; }

        
    }
}
