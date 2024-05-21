using AutoMapper;
using IOIT.Identity.Application.Departments.Commands.Create;
using IOIT.Identity.Application.Departments.Commands.Update;
using IOIT.Identity.Domain.Entities;

namespace IOIT.Identity.Application.Models.Mappings
{
    public class DepartmentMapping : Profile
    {
        public DepartmentMapping()
        {
            CreateMap<CreateDepartmentCommand, Department>();
            CreateMap<UpdateDepartmentCommand, Department>();
                //.ForMember(dest => dest.Type, otp => otp.MapFrom(src => src.Type != null ? src.Type : null))
                //.ForMember(dest => dest.CreatedById, otp => otp.MapFrom(src => src.UserId))
                //.ForMember(dest => dest.UpdatedById, otp => otp.MapFrom(src => src.UserId));

            //CreateMap<Domain.Entities.Department, ResGetDepartmentById>()
            //    .ForMember(dest => dest.DepartmentId, otp => otp.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => Convert.ToInt32(src.UpdatedById)));

            //CreateMap<DepartmentMapCustomDTO, Domain.Entities.DepartmentMap>()
            //    .ForMember(dest => dest.RequireTypeId, otp => otp.MapFrom(src => src.TypeAttributeItemId));

            //CreateMap<Domain.Entities.DepartmentMap, DtoCommonDepartmentMapQueue>()
            //    .ForMember(dest => dest.DepartmentMapId, otp => otp.MapFrom(src => src.Id));

            //CreateMap<ResGetDepartmentById, DtoCommonDepartmentCreatedQueue>();
            //CreateMap<ResGetDepartmentById, DtoCommonDepartmentUpdatedQueue>();
        }
    }
}
