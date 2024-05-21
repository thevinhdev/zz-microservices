using AutoMapper;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IOIT.Identity.Application.Categorys.Queries
{
    public class GetMainApartmentAdminQuery : FilterPagination, IRequest<ResidentDT>
    {
        public int? ApartmentId { get; set; }

        public class Handler : IRequestHandler<GetMainApartmentAdminQuery, ResidentDT>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApartmentMapAsyncRepository _entityRepository;
            private readonly IResidentAsyncRepository _residentRepo;
            private readonly ITypeAttributeItemAsyncRepository _typeRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IApartmentMapAsyncRepository entityRepository,
                IResidentAsyncRepository residentRepo,
                ITypeAttributeItemAsyncRepository typeRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _residentRepo = residentRepo;
                _typeRepo = typeRepo;
            }

            public async Task<ResidentDT> Handle(GetMainApartmentAdminQuery request, CancellationToken cancellationToken)
            {
                var  data = (from ap in _entityRepository.All()
                        join r in _residentRepo.All() on ap.ResidentId equals r.Id
                        where ap.Status == AppEnum.EntityStatus.NORMAL
                        && ap.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN
                        && ap.ApartmentId == request.ApartmentId
                        && r.Status == AppEnum.EntityStatus.NORMAL
                        select new ResidentDT
                        {
                            ResidentId = r.Id,
                            FullName = r.FullName,
                            RelationshipId = ap.RelationshipId,
                            RelationshipName = _typeRepo.All().Where(ty => ty.TypeAttributeItemId == ap.RelationshipId).FirstOrDefault() != null ? _typeRepo.All().Where(ty => ty.TypeAttributeItemId == ap.RelationshipId).FirstOrDefault().Name : "",
                            Birthday = r.Birthday,
                            CardId = r.CardId,
                            DateId = r.DateId,
                            AddressId = r.AddressId,
                            Phone = r.Phone,
                            Email = r.Email,
                            Address = r.Address,
                            Avata = r.Avata,
                            Sex = r.Sex,
                            Note = r.Note,
                            DateRent = r.DateRent,
                            Type = r.Type,
                            CreatedAt = r.CreatedAt,
                            UpdatedAt = r.UpdatedAt,
                            UserId = r.CreatedById,
                            Status = (byte)r.Status
                        }).FirstOrDefault();



                return data;
            }
        }
    }
}
