using Backend.Features.DTOs;
using MediatR;

namespace Backend.Features.Books.Queries
{
    public class SearchBooksQuery : IRequest<IReadOnlyList<BookDto>>
    {
        public string SearchTerm { get; set; } = string.Empty;
        public SearchBooksQuery(string searchTerm) { SearchTerm = searchTerm; }
    }

}
