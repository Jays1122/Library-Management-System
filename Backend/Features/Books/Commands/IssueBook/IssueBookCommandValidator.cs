using FluentValidation;

namespace Backend.Features.Books.Commands.IssueBook
{
    public class IssueBookCommandValidator : AbstractValidator<IssueBookCommand>
    {
        public IssueBookCommandValidator()
        {
            RuleFor(x => x.BookId).NotEmpty().WithMessage("Book ID is required.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        }
    }

}
