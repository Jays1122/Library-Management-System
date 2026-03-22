using MediatR;

namespace Backend.Features.Books.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
    }
}
