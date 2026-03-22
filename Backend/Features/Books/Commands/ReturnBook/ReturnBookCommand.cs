using MediatR;

namespace Backend.Features.Books.Commands.ReturnBook
{
    public class ReturnBookCommand : IRequest<bool>
    {
        public Guid IssueRecordId { get; set; }
    }

}
