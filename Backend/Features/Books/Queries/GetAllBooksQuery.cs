using Backend.Features.DTOs;
using MediatR;

namespace Backend.Features.Books.Queries
{
    public class GetAllBooksQuery : IRequest<IReadOnlyList<BookDto>> { }
}
