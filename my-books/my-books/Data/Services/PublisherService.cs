using my_books.Data.Models;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace my_books.Data.Services
{
    public class PublishersService
    {
        private AppDbContext _appDbContext;

        public PublishersService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Publisher AddPublisher(PublisherVM publisher)
        {
            if (StringStartsWithNumber(publisher.Name)) throw new PublisherNameException("Name starts with number", publisher.Name);
            var _publisher = new Publisher()
            {
                Name = publisher.Name
            };

            _appDbContext.Publishers.Add(_publisher);
            _appDbContext.SaveChanges();

            return _publisher;
        }

        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId)
        {
            var publisherData = _appDbContext.Publishers.Where(n => n.Id == publisherId)
                .Select(n => new PublisherWithBooksAndAuthorsVM
                {
                    Name = n.Name,
                    BookAuthors = n.Books.Select(n => new BookAuthorVM
                    {
                        BookName = n.Title,
                        BookAuthors = n.Book_Authors.Select(n => n.Author.FullName).ToList()
                    }).ToList()
                }).FirstOrDefault();

            return publisherData;
        }

        public void DeletePublisherById(int id)
        {
            var _publisher = _appDbContext.Publishers.FirstOrDefault(x => x.Id == id);

            if (_publisher != null)
            {
                _appDbContext.Publishers.Remove(_publisher);
                _appDbContext.SaveChanges();
            }
            else
            {
                throw new Exception($"The publisher with id {_publisher.Id} does not exist");
            }
        }

        public Publisher GetPublisherById(int id) => _appDbContext.Publishers.FirstOrDefault(x => x.Id == id);

        private bool StringStartsWithNumber(string name) => Regex.IsMatch(name, @"^\d");

        public List<Publisher> GetAllPublishers(string sortBy, string searchString,int pageNumber)
        {
            var allPublishers = _appDbContext.Publishers.OrderBy(n => n.Name).ToList();

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        allPublishers = allPublishers.OrderByDescending(x => x.Name).ToList();
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                allPublishers = allPublishers.Where(x => x.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }



            return allPublishers;
        }
    }
}
