using AutoMapper;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Features.DTOs;
using MediatR;

namespace Backend.Features.Books.Queries
{
    public class SearchBooksQueryHandler : IRequestHandler<SearchBooksQuery, IReadOnlyList<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BookDto>> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
        {
            var allBooks = await _unitOfWork.Repository<Book>().GetAllAsync();
            var filteredBooks = allBooks.Where(b =>
                b.Title.Contains(request.SearchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(request.SearchTerm, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            return _mapper.Map<IReadOnlyList<BookDto>>(filteredBooks);
        }
    }
}
