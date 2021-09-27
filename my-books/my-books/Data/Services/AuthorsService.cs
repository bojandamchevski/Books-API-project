using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.Services
{
    public class AuthorsService
    {
        private AppDbContext _appDbContext;

        public AuthorsService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddAuthor(AuthorVM author)
        {
            var _author = new Author()
            {
                FullName = author.FullName
            };

            _appDbContext.Authors.Add(_author);
            _appDbContext.SaveChanges();
        }

        public AuthorWithBooksVM GetAuthorWithBooks(int authorId)
        {
            var author = _appDbContext.Authors.Where(x => x.Id == authorId).Select(x => new AuthorWithBooksVM
            {
                FullName = x.FullName,
                BookTitles = x.Book_Authors.Select(x => x.Book.Title).ToList()
            }).FirstOrDefault();

            return author;
        }
    }
}
