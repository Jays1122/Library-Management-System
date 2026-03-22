using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using MediatR;

namespace Backend.Features.Books.Commands.ReturnBook
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReturnBookCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            // 1. Get the issue record
            var issueRecord = await _unitOfWork.Repository<IssueRecord>().GetByIdAsync(request.IssueRecordId);
            if (issueRecord == null || issueRecord.IsReturned)
                throw new Exception("Issue record not found or book already returned.");

            // 2. Get the associated book
            var book = await _unitOfWork.Repository<Book>().GetByIdAsync(issueRecord.BookId);
            if (book == null) throw new Exception("Associated book not found.");

            // 3. Mark as returned
            issueRecord.IsReturned = true;
            issueRecord.ReturnDate = DateTime.UtcNow;
            await _unitOfWork.Repository<IssueRecord>().UpdateAsync(issueRecord);

            // 4. Increase Available Copies
            book.AvailableCopies += 1;
            await _unitOfWork.Repository<Book>().UpdateAsync(book);

            // 5. Save all changes
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
