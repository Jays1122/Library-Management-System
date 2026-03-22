using MediatR;

namespace Backend.Features.Books.Commands.IssueBook
{
    public class IssueBookCommand : IRequest<bool>
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
    }
}
