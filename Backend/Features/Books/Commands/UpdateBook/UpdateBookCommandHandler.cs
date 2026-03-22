using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using MediatR;

namespace Backend.Features.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBookCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Repository<Book>().GetByIdAsync(request.Id);
            if (book == null) return false;

            // Logic to adjust Available Copies if Total Copies changed
            int copiesDiff = request.TotalCopies - book.TotalCopies;
            book.AvailableCopies += copiesDiff;

            book.Title = request.Title;
            book.Author = request.Author;
            book.ISBN = request.ISBN;
            book.TotalCopies = request.TotalCopies;

            await _unitOfWork.Repository<Book>().UpdateAsync(book);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
