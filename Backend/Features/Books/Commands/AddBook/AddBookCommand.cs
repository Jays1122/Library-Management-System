using MediatR;

namespace Backend.Features.Books.Commands.AddBook
{
    public class AddBookCommand : IRequest<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
    }
}
