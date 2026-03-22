using Backend.Features.DTOs;
using MediatR;

namespace Backend.Features.Books.Queries
{
    public class GetAllIssueRecordsQuery : IRequest<IReadOnlyList<IssueRecordDto>> { }
}
