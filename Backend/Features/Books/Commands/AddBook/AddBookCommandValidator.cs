using FluentValidation;

namespace Backend.Features.Books.Commands.AddBook
{
    public class AddBookCommandValidator : AbstractValidator<AddBookCommand>
    {
        public AddBookCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required.");
            RuleFor(x => x.TotalCopies).GreaterThan(0).WithMessage("Total copies must be greater than 0.");
        }
    }
}
