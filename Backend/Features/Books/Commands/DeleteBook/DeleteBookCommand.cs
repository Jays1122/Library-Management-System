using MediatR;

namespace Backend.Features.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteBookCommand(Guid id) { Id = id; }
    }
}
