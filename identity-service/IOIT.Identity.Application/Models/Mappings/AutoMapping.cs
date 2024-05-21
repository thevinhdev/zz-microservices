using AutoMapper;
using IOIT.Identity.Domain.Interfaces;

namespace IOIT.Identity.Application.Models.Mappings
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap(typeof(IPagedResult<>), typeof(IPagedResult<>)).ReverseMap();
        }
    }
}
