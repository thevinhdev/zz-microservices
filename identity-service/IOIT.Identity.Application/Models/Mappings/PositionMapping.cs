using AutoMapper;
using IOIT.Identity.Application.Positions.Commands.Create;
using IOIT.Identity.Application.Positions.Commands.Update;
using IOIT.Identity.Domain.Entities;

namespace IOIT.Identity.Application.Models.Mappings
{
    class PositionMapping : Profile
    {
        public PositionMapping()
        {
            CreateMap<CreatePositionCommand, Position>()
                .ForMember(dest => dest.CreatedById, otp => otp.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UpdatedById, otp => otp.MapFrom(src => src.UserId));

            CreateMap<UpdatePositionCommand, Position>();
        }
    }
}
