using System;
using System.Collections.Generic;
using System.Text;
using IOIT.Identity.Application.Employees.Commands;
using AutoMapper;
using IOIT.Shared.ViewModels.DtoQueues;

namespace IOIT.Identity.Application.Models.Mappings
{
    public class EmployeeMapping : Profile
    {
        public EmployeeMapping()
        {
            CreateMap<Domain.Entities.Employee, DtoCommonEmployeeQueue>()
                .ForMember(dest => dest.EmployeeId, otp => otp.MapFrom(src => src.Id));

            CreateMap<Domain.Entities.EmployeeMap, DtoCommonEmployeeMapQueue>()
                .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.EmployeeMapId, otp => otp.MapFrom(src => src.Id));

            CreateMap<Domain.Entities.EmployeeMap, DtoCommonEmployeeMapUpdatedQueue>()
                .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.UpdatedById))
                .ForMember(dest => dest.EmployeeMapId, otp => otp.MapFrom(src => src.Id));
        }
    }
}
