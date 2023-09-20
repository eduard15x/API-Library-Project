using ITPLibrary.Api.Core.Dtos.Book;
using ITPLibrary.Api.Core.Services.BookService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ITPLibrary.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/books")]
    [Produces("application/json")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        // we inject our new book service in our controller
        public BookController(IBookService bookService)
        {
            _bookService = bookService; // injected the interface of services of books
        }

        [HttpGet("list")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<GetBookDto>>> GetAll()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(Json(books));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<GetBookDto>> GetSingle(int id)
        {
            var book = await _bookService.GetSingleBook(id);
            return Ok(Json(book));
        }

        [HttpPost]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<GetBookDto>>> AddBook(AddBookDto newBook)
        {
            var books = await _bookService.AddBook(newBook);
            return Ok(Json(books));
        }

        [HttpPut]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<GetBookDto>>> UpdateBook(UpdateBookDto updatedBook)
        {
            var response = await _bookService.UpdateBook(updatedBook);

            if (response == null)
            {
                return NotFound(Json(response));
            }
            return Ok(Json(response));
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<GetBookDto>>> DeleteBook(int id)
        {
            var response = await _bookService.DeleteBook(id);

            if (response == null)
            {
                return NotFound(Json(response));
            }
            return Ok(Json(response));
        }

        [HttpGet("promoted-books")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<PromotedBookDto>>> GetPromotedBooks()
        {
            var promotedBooks = await _bookService.GetPromotedBooks();
            return Ok(Json(promotedBooks));
        }

        [HttpGet("best-and-recently-added-books")]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<PromotedBookDto>>> GetBestAndRecentlyAddedBooks(int lastDaysNumber)
        {
            var promotedBooks = await _bookService.GetBestAndRecentlyAddedBooks(lastDaysNumber);
            return Ok(Json(promotedBooks));
        }


    }
}
