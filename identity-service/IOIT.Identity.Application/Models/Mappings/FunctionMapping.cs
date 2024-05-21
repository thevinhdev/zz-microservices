using AutoMapper;
using IOIT.Identity.Application.Functions.Commands.Create;
using IOIT.Identity.Application.Functions.Commands.Update;
using IOIT.Identity.Domain.Entities;

namespace IOIT.Identity.Application.Models.Mappings
{
    class FunctionMapping : Profile
    {
        public FunctionMapping()
        {
            CreateMap<CreateFunctionCommand, Function>()
                .ForMember(dest => dest.CreatedById, otp => otp.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UpdatedById, otp => otp.MapFrom(src => src.UserId));

            CreateMap<UpdateFunctionCommand, Function>();
        }
    }
}
