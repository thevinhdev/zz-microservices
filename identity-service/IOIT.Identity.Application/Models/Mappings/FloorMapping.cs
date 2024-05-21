using AutoMapper;
using IOIT.Identity.Application.Floors.Commands.Create;
using IOIT.Identity.Application.Floors.Commands.Update;
using IOIT.Identity.Domain.Entities;
using IOIT.Shared.ViewModels.DtoQueues;
using System;

namespace IOIT.Identity.Application.Models.Mappings
{
    public class FloorMapping : Profile
    {
        public FloorMapping()
        {
            CreateMap<CreateFloorCommand, Floor>();
            CreateMap<UpdateFloorCommand, Floor>();
                //.ForMember(dest => dest.Address, otp => otp.MapFrom(src => src.Address ?? string.Empty))
                //.ForMember(dest => dest.CreatedById, otp => otp.MapFrom(src => src.UserId))
                //.ForMember(dest => dest.UpdatedById, otp => otp.MapFrom(src => src.UserId));

            //CreateMap<Floor, ResGetFloorById>()
            //    .ForMember(dest => dest.FloorId, otp => otp.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => Convert.ToInt32(src.UpdatedById)));

            //CreateMap<Floor, ResGetFloorByPaging>()
            //    .ForMember(dest => dest.FloorId, otp => otp.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => Convert.ToInt32(src.UpdatedById)))
            //    .ForMember(dest => dest.ProjectName, otp => otp.MapFrom(src => string.Empty))
            //    .ForMember(dest => dest.TowerName, otp => otp.MapFrom(src => string.Empty));

            //CreateMap<ResGetFloorById, DtoCommonFloorCreatedQueue>();
            //CreateMap<ResGetFloorById, DtoCommonFloorUpdatedQueue>();
        }
    }
}
