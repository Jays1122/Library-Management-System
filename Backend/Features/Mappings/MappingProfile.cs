using AutoMapper;
using Backend.Domain.Entities;
using Backend.Features.DTOs;

namespace Backend.Features.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<IssueRecord, IssueRecordDto>().ReverseMap();
        }
    }
}
