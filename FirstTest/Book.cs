using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace FirstTest
{
    class Book : ScannedItem   
    {
        int BookID;
        string BookTitle;
        string BookAuthor;
        string BookPublisher;
        string BookISBN;
        int PageTotal;
        OleDbConnection connect = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source =C:\Users\user\OneDrive - Bridgwater and Taunton College\Project Code\FirstTest\Books.accdb");

        public Book(int bookID, string bookTitle, string bookAuthor, string bookPublisher, string bookISBN, int pageTotal)
        {
            BookID = bookID;
            BookTitle = bookTitle ?? throw new ArgumentNullException(nameof(bookTitle)); //condensed if statements for checking to see if the returned value is null, return exception stopping program from crashing
            BookAuthor = bookAuthor ?? throw new ArgumentNullException(nameof(bookAuthor));
            BookPublisher = bookPublisher ?? throw new ArgumentNullException(nameof(bookPublisher));
            BookISBN = bookISBN ?? throw new ArgumentNullException(nameof(bookISBN));
            PageTotal = pageTotal;
        }
        // class constructor
        
        public int ID { get => BookID; set => BookID = value; }
       
        public string Title { get => BookTitle; set => BookTitle = value; }
        public string Author { get => BookAuthor; set => BookAuthor = value; }
        public string Publisher { get => BookPublisher; set => BookPublisher = value; }
        public string ISBN { get => BookISBN; set => BookISBN = value; }
        public int Pages { get => PageTotal; set => PageTotal = value; }

        override //overides existing method
        public string ToString()
        {
            return $"BookID: {ID.ToString()}, BookTitle: {Title}, Author: {Author}, Publisher: {Publisher}\n"; //String format for each book
        }
        public string ToString2()
        {
            return $"BookID: {ID.ToString()}\n BookTitle: {Title}\n Author: {Author}\n Publisher: {Publisher}\n ISBN: {ISBN}\n Total Pages: {Pages}";
        }

        public static int GetBookIDFromString(string bookString)
        {
            int stringOne = bookString.IndexOf("BookID: ") + 8;
            int stringTwo = bookString.IndexOf(",", stringOne);

            return int.Parse(bookString.Substring(stringOne, stringTwo - stringOne));
        }


        

    }
}
