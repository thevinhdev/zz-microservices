using AutoMapper;
using IOIT.Identity.Application.Projects.Commands.Create;
using IOIT.Identity.Application.Projects.Commands.Update;
using IOIT.Identity.Domain.Entities;

namespace IOIT.Identity.Application.Models.Mappings
{
    class ProjectMapping : Profile
    {
        public ProjectMapping()
        {
            CreateMap<CreateProjectCommand, Project>();
            CreateMap<UpdateProjectCommand, Project>();
                //.ForMember(dest => dest.CreatedById, otp => otp.MapFrom(src => src.UserId))
                //.ForMember(dest => dest.UpdatedById, otp => otp.MapFrom(src => src.UserId))
                //.ForMember(dest => dest.EmailDisplayName, otp => otp.MapFrom(src => string.Empty));

            //CreateMap<UpdateProjectCommand, Project>()
            //    .ForMember(dest => dest.FullAddress, otp => otp.MapFrom(src => string.IsNullOrEmpty(src.Address) ? string.Empty : src.Address));

            //CreateMap<Project, CreateProjectCommand>();

            //CreateMap<Project, ResGetProjectById>()
            //    .ForMember(dest => dest.ProjectId, otp => otp.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.UpdatedById));

            //CreateMap<Project, ResGetProjectByPaging>()
            //    .ForMember(dest => dest.ProjectId, otp => otp.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => src.UpdatedById))
            //    .ForMember(dest => dest.DistrictName, otp => otp.MapFrom(src => string.Empty))
            //    .ForMember(dest => dest.ProvinceName, otp => otp.MapFrom(src => string.Empty))
            //    .ForMember(dest => dest.WardName, otp => otp.MapFrom(src => string.Empty));

            //CreateMap<ResGetProjectById, DtoCommonProjectCreatedQueue>();
            //CreateMap<ResGetProjectById, DtoCommonProjectUpdatedQueue>();
        }
    }
}
