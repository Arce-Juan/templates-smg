using AutoMapper;
using Template.Domain.Entities;
using Template.Domain.ValueObjects;

namespace Template.Application.Mapping;

public class TemplateProfile : Profile
{
    public TemplateProfile()
    {
        CreateMap<TemplateEntity, TemplateDto>().ReverseMap();
    }
}
