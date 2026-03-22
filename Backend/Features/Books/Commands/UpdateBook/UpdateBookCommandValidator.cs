using FluentValidation;

namespace Backend.Features.Books.Commands.UpdateBook
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Book ID is required.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.TotalCopies).GreaterThan(0).WithMessage("Total copies must be greater than 0.");
        }
    }
}
