using AutoMapper;
using IOIT.Shared.ViewModels.DtoQueues;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Application.Users.Commands.Create;
using IOIT.Identity.Application.Users.ViewModels;

namespace IOIT.Identity.Application.Models.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, DtoCommonUserQueue>()
                .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.Id));
            CreateMap<User, UserDT>()
                .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, otp => otp.MapFrom(src => (int)src.Status));
            CreateMap<User, ResUserRegisterApp>()
                .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.Id))
                .ForMember(dest => dest.Status, otp => otp.MapFrom(src => (int)src.Status));
        }
    }
}
