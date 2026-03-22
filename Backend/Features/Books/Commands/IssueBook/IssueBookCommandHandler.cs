using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using MediatR;

namespace Backend.Features.Books.Commands.IssueBook
{
    public class IssueBookCommandHandler : IRequestHandler<IssueBookCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public IssueBookCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(IssueBookCommand request, CancellationToken cancellationToken)
        {
            // 1. Check if book exists
            var book = await _unitOfWork.Repository<Book>().GetByIdAsync(request.BookId);
            if (book == null) throw new Exception("Book not found.");

            // 2. Bonus Task: Check Available Copies
            if (book.AvailableCopies <= 0) throw new Exception("No copies available for this book.");

            // 3. Create Issue Record
            var issueRecord = new IssueRecord
            {
                BookId = request.BookId,
                UserId = request.UserId,
                IssueDate = DateTime.UtcNow,
                IsReturned = false
            };

            await _unitOfWork.Repository<IssueRecord>().AddAsync(issueRecord);

            // 4. Update Available Copies (Bonus Task 2)
            book.AvailableCopies -= 1;
            await _unitOfWork.Repository<Book>().UpdateAsync(book);

            // 5. Save everything in one transaction
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
