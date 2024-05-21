using System;
using System.Collections.Generic;
using AutoMapper;
using IOIT.Identity.Application.ProjectUtilities.ViewModels;
using IOIT.Identity.Application.Utilities.Commands.Create;
using IOIT.Identity.Application.Utilities.ViewModels;
using IOIT.Shared.Commons.Enum;

namespace IOIT.Identity.Application.Models.Mappings
{
    public class UtilitiesMapping : Profile
    {
        public UtilitiesMapping()
        {
            CreateMap<CreateUtilitiesCommand, Domain.Entities.Utilities>()
                .ForMember(dest => dest.CreatedById, otp => otp.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UpdatedById, otp => otp.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UpdatedAt, otp => otp.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedAt, otp => otp.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, otp => otp.MapFrom(src => AppEnum.EntityStatus.NORMAL));

            CreateMap<Domain.Entities.Utilities, ResGetUtilitiesById>();
            CreateMap<Domain.Entities.Utilities, ResGetConfigProjectUtilitiesQuery>()
                .ForMember(dest => dest.Title, otp => otp.MapFrom(src => src.Name))
                .ForMember(dest => dest.Logo, otp => otp.MapFrom(src => src.Icon))
                .ForMember(dest => dest.Url, otp => otp.MapFrom(src => src.Url))
                .ForMember(dest => dest.ListUtilitiesChild, otp => new List<ResGetConfigProjectUtilitiesQuery>());
        }
    }
}
