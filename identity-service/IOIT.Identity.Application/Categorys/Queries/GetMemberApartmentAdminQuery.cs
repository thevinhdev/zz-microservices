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
    public class GetMemberApartmentAdminQuery : FilterPagination, IRequest<IPagedResult<ResidentDT>>
    {
        public long? ResidentId { get; set; }
        public int? ApartmentId { get; set; }
        public int? Type { get; set; }

        public class Handler : IRequestHandler<GetMemberApartmentAdminQuery, IPagedResult<ResidentDT>>
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

            public async Task<IPagedResult<ResidentDT>> Handle(GetMemberApartmentAdminQuery request, CancellationToken cancellationToken)
            {
                Application.Models.PagedResult<ResidentDT> entities = new Application.Models.PagedResult<ResidentDT>();
                //Kiểm tra xem người dùng và căn hộ có map với nhau không
                //ApartmentMap apartmentMap = db.ApartmentMap.Where(ap => ap.ApartmentId == ApartmentId && ap.ResidentId == ResidentId && ap.Status == (int)Const.Status.NORMAL).FirstOrDefault();
                ApartmentMap apartmentMap = await _entityRepository.CheckApartmentResidentAsync(request.ApartmentId, request.ResidentId, cancellationToken);
                if (apartmentMap == null)
                {
                    throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
                }

                //var spec = new ResidentFilterWithPagingSpec(request);
                //FilterPagination filterPagination = new FilterPagination();
                //filterPagination.page = request.page;
                //filterPagination.page_size = request.page_size;
                //filterPagination.query = request.query;
                //filterPagination.order_by = request.order_by;
                //filterPagination.search = request.search;
                //var entities = await _entityRepository.GetByPageMemberApartmentAdminAsync(filterPagination, request.ApartmentId, cancellationToken);

                //List<Apartment> res = new List<Apartment>();
                //foreach (var item in entities.Results)
                //{
                //    Apartment residentDTO = new ResResidentDT();
                //    residentDTO.ResidentId = item.ResidentId;
                //    residentDTO.FullName = item.FullName;
                //    residentDTO.Phone = item.Phone;

                //    res.Add(residentDTO);
                //}
                //entities.Results = res;
                IQueryable<ResidentDT> data = Enumerable.Empty<ResidentDT>().AsQueryable();

                if (request.Type == 1)
                {
                    if (apartmentMap.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN)
                    {
                        data = (from ap in _entityRepository.All()
                                join r in _residentRepo.All() on ap.ResidentId equals r.Id
                                where ap.Status == AppEnum.EntityStatus.NORMAL
                                && ap.Type == (int)AppEnum.TypeResident.RESIDENT_MEMBER
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
                                }).AsQueryable();
                    }
                    else if (apartmentMap.Type == (int)AppEnum.TypeResident.RESIDENT_GUEST)
                    {
                        data = (from ap in _entityRepository.All()
                                join r in _residentRepo.All() on ap.ResidentId equals r.Id
                                where ap.Status == AppEnum.EntityStatus.NORMAL
                                && ap.Type == (int)AppEnum.TypeResident.RESIDENT_GUEST_MEMBER
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
                                }).AsQueryable();
                    }
                }
                else
                {
                    data = (from ap in _entityRepository.All()
                            join r in _residentRepo.All() on ap.ResidentId equals r.Id
                            where ap.Status == AppEnum.EntityStatus.NORMAL
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
                            }).AsQueryable();
                }

                if (!string.IsNullOrEmpty(request.query))
                {
                    request.query = HttpUtility.UrlDecode(request.query);
                    data = data.Where(request.query);
                }
                
                //def.metadata = data.Count();
                entities.PageCount = data.ToList().Count;
                if (request.page_size > 0)
                {
                    if (!string.IsNullOrEmpty(request.order_by))
                    {
                        data = data.OrderBy(request.order_by).Skip((request.page - 1) * request.page_size).Take(request.page_size);
                    }
                    else
                    {
                        data = data.OrderBy("ResidentId desc").Skip((request.page - 1) * request.page_size).Take(request.page_size);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.order_by))
                    {
                        data = data.OrderBy(request.order_by);
                    }
                    else
                    {
                        data = data.OrderBy("ResidentId desc");
                    }
                }

                if (!string.IsNullOrEmpty(request.select))
                {
                    request.select = "new(" + request.select + ")";
                    request.select = HttpUtility.UrlDecode(request.select);
                    entities.Results = data.ToList();
                }
                else
                    entities.Results = data.ToList();


                return entities;
            }
        }
    }
}
