using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace FirstTest
{
    class Book : Product   
    {
        int BookID;
        string BookTitle;
        string BookAuthor;
        string BookPublisher;
        string BookISBN;
        int BookPageTotal;
        int BookReleaseYear;
       

        static public int RetrievedID;
        static public int Maxpages;

        public Book(int bookID, string bookTitle, string bookAuthor, string bookPublisher, string bookISBN, int pageTotal, int releaseYear)
        {
            BookID = bookID;
            BookTitle = bookTitle ?? throw new ArgumentNullException(nameof(bookTitle)); //condensed if statements for checking to see if the returned value is null, return exception stopping program from crashing
            BookAuthor = bookAuthor ?? throw new ArgumentNullException(nameof(bookAuthor));
            BookPublisher = bookPublisher ?? throw new ArgumentNullException(nameof(bookPublisher));
            BookISBN = bookISBN ?? throw new ArgumentNullException(nameof(bookISBN));
            BookPageTotal = pageTotal;
            BookReleaseYear = releaseYear;
        }
        // class constructor
        
        public int ID { get => BookID; set => BookID = value; }
       
        public string Title { get => BookTitle; set => BookTitle = value; }
        public string Author { get => BookAuthor; set => BookAuthor = value; }
        public string Publisher { get => BookPublisher; set => BookPublisher = value; }
        public string ISBN { get => BookISBN; set => BookISBN = value; }
        public int Pages { get => BookPageTotal; set => BookPageTotal = value; }
        public int ReleaseYear1 { get => BookReleaseYear; set => BookReleaseYear = value; }

        override //overides existing method
        public string ToString()
        {
            return $"BookID: {ID.ToString()}, BookTitle: {Title}, Author: {Author}, Publisher: {Publisher}\n"; //String format for each book when outputted in lists
        }
        public string ToString2()
        {
            return $"BookID: {ID.ToString()}\n BookTitle: {Title}\n Author: {Author}\n Publisher: {Publisher}\n ISBN: {ISBN}\n Total Pages: {Pages}";//string format for each book when outputted in Book information window
        }

        public static int GetBookIDFromString(string bookString)
        {
            int stringOne = bookString.IndexOf("BookID: ") + 8;
            int stringTwo = bookString.IndexOf(",", stringOne);

            return int.Parse(bookString.Substring(stringOne, stringTwo - stringOne));
        }


        public static List<Book> QueryDatabase(string sql) //Returns a list of all the books returned by the inputted SQL statement
        {
            OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");
            OleDbCommand Data = new OleDbCommand(sql, connect);

            if (connect.State == ConnectionState.Closed)
            {
                connect.Open(); //Opens data connection
            }

            OleDbDataReader DataReader = Data.ExecuteReader(); //executes Command and saves it in DataReader
            List<Book> listToReturn = new List<Book>();

            if (DataReader.HasRows)
            {
                while (DataReader.Read()) //loops through each row of the returned databse
                {
                    int bookid = DataReader.GetInt32(0); //parameter refers to the column/field
                    string booktitle = DataReader.GetString(1);
                    string bookauthor = DataReader.GetString(2);
                    string bookpublisher = DataReader.GetString(3);
                    string bookISBN = DataReader.GetString(4);
                    int pageTotal = DataReader.GetInt32(5);
                    int releaseYear = DataReader.GetInt32(6);

                    Book TempBook = new Book(bookid, booktitle, bookauthor, bookpublisher, bookISBN, pageTotal, releaseYear); //Creates a temporary object that will be added into the book list (Overwritten by next loop)

                    listToReturn.Add(TempBook);
                }
            }
            connect.Close();
            return listToReturn;

        }



    }
}
