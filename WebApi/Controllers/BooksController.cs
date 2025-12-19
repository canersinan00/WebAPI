using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.EFCore;
using Services.Contract;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.BookService.GetAllBooks(false);
                return Ok(books);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")]int id) 
        {
            try
            {
                var book = _manager
                .BookService
                .GetOneBookById(id,false);

                if (book is null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest();
                }
                _manager.BookService.CreateOneBook(book);
                return StatusCode(201,book);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book
            )
        {
            try
            {
                if (book is null)
                {
                    return BadRequest();
                }
                _manager.BookService.UpdateOneBook(id,book,true);

                return NoContent();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBooks([FromRoute(Name ="id")] int id)
        {
            try
            {
                _manager.BookService.DeleteoneBook(id,false);

                return NoContent();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookpatch)
        {
            try
            {
                var entitiy = _manager
                    .BookService
                    .GetOneBookById(id, true);

                if (entitiy is null)
                {
                    return NotFound();
                }

                bookpatch.ApplyTo(entitiy);
                _manager.BookService.UpdateOneBook(id,entitiy,true);

                return NoContent();

            }
            catch (Exception ex)
            {

                throw new Exception (ex.Message);
            }
        }
    }
}
