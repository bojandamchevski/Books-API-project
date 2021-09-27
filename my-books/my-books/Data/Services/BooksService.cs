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

        public void AddBookWithAuthors(BookVM book)
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
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId
            };

            _appDbContext.Books.Add(_book);
            _appDbContext.SaveChanges();

            foreach (var id in book.AuthorIds)
            {
                var _book_author = new Book_Author()
                {
                    BookId = _book.Id,
                    AuthorId = id
                };
                _appDbContext.Book_Authors.Add(_book_author);
                _appDbContext.SaveChanges();
            }
        }

        public List<Book> GetAllBooks() => _appDbContext.Books.ToList();
        public BookWithAuthorsVM GetBookById(int id)
        {
            var bookWithAuthors = _appDbContext.Books.Where(x => x.Id == id).Select(book => new BookWithAuthorsVM()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead : null,
                Rate = book.IsRead ? book.Rate : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select(x => x.Author.FullName).ToList()
            }).FirstOrDefault();

            return bookWithAuthors;
        }

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
