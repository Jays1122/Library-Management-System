using MediatR;

namespace Backend.Features.Books.Queries
{
    public class GetTotalIssuedBooksQuery : IRequest<int> { }
}
