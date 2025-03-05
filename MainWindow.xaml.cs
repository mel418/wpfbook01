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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfbook01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Entity Framework DbContext
            var dbcontext = new BooksEntities();



            // query 1: titles and authors, sorted by title only
            var sortedTitlesAndAuthors =
                from book in dbcontext.Titles
                from author in book.Authors
                orderby book.Title1
                select new { book.Title1, author.FirstName, author.LastName };

            outputTextBox.AppendText("Titles and Authors:");
            foreach (var element in sortedTitlesAndAuthors)
            {
                outputTextBox.AppendText($"\r\n\t{element.Title1,-10}: " +
                   $"{element.FirstName,-10} {element.LastName,-10}");
            }

            // query 2: titles and authors, sorted by title, then authors by last/first name
            var titlesAndSortedAuthors =
                from book in dbcontext.Titles
                orderby book.Title1
                select new
                {
                    book.Title1,
                    Authors = from author in book.Authors
                              orderby author.LastName, author.FirstName
                              select new { author.FirstName, author.LastName }
                };
            outputTextBox.AppendText("\r\n\nAuthors and titles with authors sorted for each title:\r\n");
            foreach (var title in titlesAndSortedAuthors)
            {
                foreach (var author in title.Authors)
                {
                    outputTextBox.AppendText($"\r\n\t{title.Title1,-10}: " +
                        $"{author.FirstName,-10} {author.LastName,-10}");
                }
            }

            // query 3: authors grouped by title, sorted by title; for a given title sort the author names alphabetically by last name first then first name
            var titlesGroupedByAuthors =
                from book in dbcontext.Titles
                orderby book.Title1
                select new
                {
                    Title = book.Title1,
                    Authors = from author in book.Authors
                              orderby author.LastName, author.FirstName
                              select new
                              {
                                  author.FirstName,
                                  author.LastName
                              }
                };
            outputTextBox.AppendText("\r\n\nTitles grouped by author:\r\n\r\n");
            foreach (var book in titlesGroupedByAuthors)
            {
                outputTextBox.AppendText($"\r\n{book.Title}:\r\n");
                foreach (var author in book.Authors)
                {
                    outputTextBox.AppendText($"\t{author.FirstName, -10} {author.LastName,-10}\r\n");
                }
            }
        }
    }
}
