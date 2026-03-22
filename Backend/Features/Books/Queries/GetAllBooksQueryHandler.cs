using AutoMapper;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Features.DTOs;
using MediatR;

namespace Backend.Features.Books.Queries
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IReadOnlyList<BookDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; // AutoMapper inject kiya

        public GetAllBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.Repository<Book>().GetAllAsync();
            return _mapper.Map<IReadOnlyList<BookDto>>(books); // Entity ko DTO me map kiya
        }
    }
}
