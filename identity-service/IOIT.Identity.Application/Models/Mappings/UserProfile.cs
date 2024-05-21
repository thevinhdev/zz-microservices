using AutoMapper;
//using IOIT.Identity.Application.Users.Commands.Update;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities.Indentity;

namespace IOIT.Identity.Application.Models.Mappings
{
    class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, ResGetUser>()
                .ForMember(d => d.CreatedDate, otp => otp.MapFrom(d => d.CreatedDate.DateTime))
                .ForMember(d => d.UpdatedDate, otp => otp.MapFrom(d => d.UpdatedDate.DateTime));

            CreateMap<UpdateUserByIdCommand, User>();
        }
    }
}
