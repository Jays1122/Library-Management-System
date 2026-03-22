using Backend.Features.DTOs;
using MediatR;

namespace Backend.Features.Books.Queries
{
    public class GetBookByIdQuery : IRequest<BookDto>
    {
        public Guid Id { get; set; }
        public GetBookByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
