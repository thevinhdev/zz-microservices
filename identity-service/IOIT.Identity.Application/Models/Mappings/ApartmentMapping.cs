using AutoMapper;
using IOIT.Identity.Application.ApartmentMaps.Create;
using IOIT.Identity.Application.Apartments.Commands.Create;
using IOIT.Identity.Application.Apartments.Commands.Update;
using IOIT.Identity.Domain.Entities;
using IOIT.Shared.ViewModels.DtoQueues;

namespace IOIT.Identity.Application.Models.Mappings
{
    public class ApartmentMapping : Profile
    {
        public ApartmentMapping()
        {
            CreateMap<CreateApartmentCommand, Apartment>();
            CreateMap<UpdateApartmentCommand, Apartment>();
            //.ForMember(dest => dest.CreatedById, otp => otp.MapFrom(src => src.UserId))
            //.ForMember(dest => dest.UpdatedById, otp => otp.MapFrom(src => src.UserId));

            //CreateMap<Apartment, ResGetApartmentById>()
            //    .ForMember(dest => dest.ApartmentId, otp => otp.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => Convert.ToInt32(src.UpdatedById)));

            //CreateMap<Apartment, ResGetApartmentByPaging>()
            //    .ForMember(dest => dest.ApartmentId, otp => otp.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => Convert.ToInt32(src.UpdatedById)))
            //    .ForMember(dest => dest.ProjectName, otp => otp.MapFrom(src => string.Empty))
            //    .ForMember(dest => dest.TowerName, otp => otp.MapFrom(src => string.Empty))
            //    .ForMember(dest => dest.FloorName, otp => otp.MapFrom(src => string.Empty));

            CreateMap<ApartmentMap, DtoIdentityApartmentMapQueue>()
                .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.CreatedById));
            //CreateMap<ResGetApartmentById, DtoCommonApartmentUpdatedQueue>();
        }
    }
}
