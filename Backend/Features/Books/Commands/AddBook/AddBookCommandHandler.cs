using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using MediatR;

namespace Backend.Features.Books.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddBookCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                TotalCopies = request.TotalCopies,
                AvailableCopies = request.TotalCopies // Start me sab available hain
            };

            await _unitOfWork.Repository<Book>().AddAsync(book);
            await _unitOfWork.SaveChangesAsync();

            return book.Id;
        }
    }
}

