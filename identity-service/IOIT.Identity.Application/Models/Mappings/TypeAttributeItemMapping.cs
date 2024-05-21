using AutoMapper;
using IOIT.Identity.Application.TypeAttributeItems.Commands.Create;
using IOIT.Identity.Application.TypeAttributeItems.Commands.Update;
using IOIT.Identity.Domain.Entities;
using System;

namespace IOIT.Identity.Application.Models.Mappings
{
    public class TypeAttributeItemMapping : Profile
    {
        public TypeAttributeItemMapping()
        {
            CreateMap<CreateTypeAttributeItemCommand, TypeAttributeItem>();
            CreateMap<UpdateTypeAttributeItemCommand, TypeAttributeItem>();
                //.ForMember(dest => dest.CreatedById, otp => otp.MapFrom(src => src.UserId))
                //.ForMember(dest => dest.UpdatedById, otp => otp.MapFrom(src => src.UserId));

            //CreateMap<TypeAttributeItem, ResGetTypeAttributeItemById>()
            //    .ForMember(dest => dest.TypeAttributeItemId, otp => otp.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.UserId, otp => otp.MapFrom(src => Convert.ToInt32(src.UpdatedById)));
        }
    }
}
