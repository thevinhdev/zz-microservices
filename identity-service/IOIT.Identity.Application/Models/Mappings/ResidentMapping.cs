using System;
using System.Collections.Generic;
using System.Text;
using IOIT.Identity.Application.Employees.Commands;
using AutoMapper;
using IOIT.Shared.ViewModels.DtoQueues;
using IOIT.Identity.Application.Residents.Commands.Create;
using IOIT.Identity.Application.Residents.ViewModels;
using IOIT.Identity.Application.Residents.Commands.Update;
using static IOIT.Identity.Domain.Enum.DomainEnum;

namespace IOIT.Identity.Application.Models.Mappings
{
    public class ResidentMapping : Profile
    {
        public ResidentMapping()
        {
            CreateMap<Domain.Entities.Resident, DtoCommonResidentQueue>()
                .ForMember(dest => dest.ResidentId, otp => otp.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.CreatedById));

            CreateMap<Domain.Entities.Resident, DtoCommonResidentUpdateQueue>()
                .ForMember(dest => dest.ResidentId, otp => otp.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.UpdatedById));

            CreateMap<ReqRequestCreateNewResident, CreateResidentCommand>();

            CreateMap<DtoUtitlitiesResidentUpdateQueue, UpdateResidentCommand>()
                .ForMember(dest => dest.ResidentId, otp => otp.MapFrom(src => src.ResidentId))
                .ForMember(dest => dest.Id, otp => otp.MapFrom(src => src.ResidentId))
                .ForMember(dest => dest.Status, otp => otp.MapFrom(src => src.Status))
                .ForMember(dest => dest.user, otp => otp.MapFrom(src => new UserDT()))
                .ForMember(dest => dest.TypeCardId, otp => otp.MapFrom(src => (ResidentRequestIdentifyType)src.TypeCardId));
        }
    }
}
