using Backend.Domain.Entities;
using Backend.Domain.Interfaces;
using MediatR;

namespace Backend.Features.Books.Queries
{
    public class GetTotalIssuedBooksQueryHandler : IRequestHandler<GetTotalIssuedBooksQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalIssuedBooksQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetTotalIssuedBooksQuery request, CancellationToken cancellationToken)
        {
            var allRecords = await _unitOfWork.Repository<IssueRecord>().GetAllAsync();

            // Unka count jinki IsReturned == false hai
            return allRecords.Count(r => !r.IsReturned);
        }
    }
}
