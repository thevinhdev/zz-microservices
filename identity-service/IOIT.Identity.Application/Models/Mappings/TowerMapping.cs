using AutoMapper;
using IOIT.Identity.Application.Towers.Commands.Create;
using IOIT.Identity.Application.Towers.Commands.Update;
using IOIT.Identity.Domain.Entities;

namespace IOIT.Identity.Application.Models.Mappings
{
    public class TowerMapping : Profile
    {
        public TowerMapping()
        {
            CreateMap<CreateTowerCommand, Tower>();
            CreateMap<UpdateTowerCommand, Tower>();
                //.ForMember(dest => dest.CreatedById, otp => otp.MapFrom(src => src.UserId))
                //.ForMember(dest => dest.UpdatedById, otp => otp.MapFrom(src => src.UserId));

            //CreateMap<Tower, ResGetTowerById>()
            //    .ForMember(dest => dest.TowerId, otp => otp.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => Convert.ToInt32(src.UpdatedById)));

            //CreateMap<Tower, ResGetTowerByPaging>()
            //    .ForMember(dest => dest.TowerId, otp => otp.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => Convert.ToInt32(src.UpdatedById)))
            //    .ForMember(dest => dest.ProjectName, otp => otp.MapFrom(src => string.Empty));

            //CreateMap<ResGetTowerById, DtoCommonTowerCreatedQueue>();
            //CreateMap<ResGetTowerById, DtoCommonTowerUpdatedQueue>();
        }
    }
}
