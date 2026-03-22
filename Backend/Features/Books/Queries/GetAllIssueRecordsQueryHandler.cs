using AutoMapper;
using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using Backend.Features.DTOs;
using MediatR;

namespace Backend.Features.Books.Queries
{
    public class GetAllIssueRecordsQueryHandler : IRequestHandler<GetAllIssueRecordsQuery, IReadOnlyList<IssueRecordDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllIssueRecordsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<IssueRecordDto>> Handle(GetAllIssueRecordsQuery request, CancellationToken cancellationToken)
        {
            var records = await _unitOfWork.Repository<IssueRecord>().GetAllAsync();
            return _mapper.Map<IReadOnlyList<IssueRecordDto>>(records);
        }
    }
}
