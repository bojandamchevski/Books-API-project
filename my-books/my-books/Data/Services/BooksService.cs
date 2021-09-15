using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.Services
{
    public class BooksService
    {
        private AppDbContext _appDbContext { get; set; }
        public BooksService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddBook(BookVM book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead : null,
                Rate = book.IsRead ? book.Rate : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                Author = book.Author,
                DateAdded = DateTime.Now
            };

            _appDbContext.Books.Add(_book);
            _appDbContext.SaveChanges();
        }

        public List<Book> GetAllBooks() => _appDbContext.Books.ToList();
        public Book GetBookById(int id) => _appDbContext.Books.FirstOrDefault(x => x.Id == id);

        public Book UpdateBookById(int id, BookVM book)
        {
            var _book = _appDbContext.Books.FirstOrDefault(x => x.Id == id);
            if (_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead : null;
                _book.Rate = book.IsRead ? book.Rate : null;
                _book.Genre = book.Genre;
                _book.CoverUrl = book.CoverUrl;
                _book.Author = book.Author;

                _appDbContext.SaveChanges();
            }
            return _book;
        }

        public void DeleteBookById(int id)
        {
            var _book = _appDbContext.Books.FirstOrDefault(x => x.Id == id);
            if (_book != null)
            {
                _appDbContext.Books.Remove(_book);
                _appDbContext.SaveChanges();
            }
        }
    }
}
