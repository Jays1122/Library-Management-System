using FluentValidation;

namespace Backend.Features.Books.Commands.ReturnBook
{
    public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
    {
        public ReturnBookCommandValidator()
        {
            RuleFor(x => x.IssueRecordId).NotEmpty().WithMessage("Issue Record ID is required.");
        }
    }
}
