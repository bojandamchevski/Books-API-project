using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.ActionResults;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private PublishersService _publisherService;

        public PublishersController(PublishersService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublishers(string sortBy, string searchString, int pageNumber)
        {
            try
            {
                var _result = _publisherService.GetAllPublishers(sortBy, searchString, pageNumber);
                return Ok(_result);
            }
            catch (Exception)
            {
                return BadRequest("Sorry, we could not load the publishers");
            }
        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
            try
            {
                var newPub = _publisherService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), newPub);
            }
            catch (PublisherNameException ex)
            {
                return BadRequest($"{ex.Message}, Publisher name: {ex.PublisherName}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-publisher-books-with-authors/{id}")]
        public IActionResult GetPublisherData(int id)
        {
            var response = _publisherService.GetPublisherData(id);
            return Ok(response);
        }

        [HttpDelete("delete-publisher-by-id/{id}")]
        public ActionResult DeletePublisherById(int id)
        {
            try
            {
                _publisherService.DeletePublisherById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var _response = _publisherService.GetPublisherById(id);
            if (_response != null)
            {
                return Ok(_response);
            }
            else
            {
                //return null;
                return NotFound();
            }

        }

        //[HttpGet("get-publisher-by-id/{id}")]
        //public CustomActionResult GetPublisherById(int id)
        //{
        //    var _response = _publisherService.GetPublisherById(id);
        //    if (_response != null)
        //    {
        //        //return Ok(_response);
        //        var _responseObj = new CustomActionResultVM()
        //        {
        //            Publisher = _response
        //        };

        //        return new CustomActionResult(_responseObj);
        //        //return _response;
        //    }
        //    else
        //    {
        //        //return null;
        //        //return NotFound();
        //        var _responseObj = new CustomActionResultVM()
        //        {
        //            Exception = new Exception("This is coming from the publisher controller")
        //        };

        //        return new CustomActionResult(_responseObj);
        //    }

        //}
    }
}
