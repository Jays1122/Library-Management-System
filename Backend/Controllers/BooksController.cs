using Backend.Features.Books.Commands.AddBook;
using Backend.Features.Books.Commands.DeleteBook;
using Backend.Features.Books.Commands.IssueBook;
using Backend.Features.Books.Commands.ReturnBook;
using Backend.Features.Books.Commands.UpdateBook;
using Backend.Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("LMS/[controller]")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] AddBookCommand command)
        {
            var bookId = await _mediator.Send(command);
            return Ok(new { Message = "Book added successfully", BookId = bookId });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _mediator.Send(new GetAllBooksQuery());
            return Ok(books);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result) return NotFound("Book not found.");
            return Ok(new { Message = "Book updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var result = await _mediator.Send(new DeleteBookCommand(id));
            if (!result) return NotFound("Book not found.");
            return Ok(new { Message = "Book deleted successfully" });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return BadRequest("Search term is required.");
            var books = await _mediator.Send(new SearchBooksQuery(query));
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));

            if (book == null)
                return NotFound(new { Message = "Book not found." });

            return Ok(book);
        }

        // --- ISSUE AND RETURN ENDPOINTS ---

        [HttpPost("issue")]
        public async Task<IActionResult> IssueBook([FromBody] IssueBookCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok(new { Message = "Book issued successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok(new { Message = "Book returned successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // --- BONUS TASK ENDPOINT ---

        [HttpGet("stats/total-issued")]
        public async Task<IActionResult> GetTotalIssuedBooks()
        {
            var count = await _mediator.Send(new GetTotalIssuedBooksQuery());
            return Ok(new { TotalIssued = count });
        }

        [HttpGet("issue-records")]
        [Authorize(Roles = "Admin")] // Sirf Admin dekh sakta hai
        public async Task<IActionResult> GetAllIssueRecords()
        {
            var records = await _mediator.Send(new GetAllIssueRecordsQuery());
            return Ok(records);
        }
    }
}
