using AutoMapper;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
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

namespace IOIT.Identity.Application.Residents.Queries
{
    public class GetResidentByPagingQuery : FilterPagination, IRequest<IPagedResult<ResResidentDT>>
    {
        public int? ProjectId { get; set; }
        public int? TowerId { get; set; }
        public int? FloorId { get; set; }
        public int? ApartmentId { get; set; }
        public int? type { get; set; }
        public int? roleMax { get; set; }

        public class Handler : IRequestHandler<GetResidentByPagingQuery, IPagedResult<ResResidentDT>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;
            private readonly IApartmentMapAsyncRepository _amRepo;
            private readonly IProjectAsyncRepository _projectRepo;
            private readonly ITowerAsyncRepository _towerRepo;
            private readonly IFloorAsyncRepository _floorRepo;
            private readonly IApartmentAsyncRepository _apartRepo;
            private readonly IUserAsyncRepository _userRepo;
            private readonly ITypeAttributeItemAsyncRepository _typeRepo;
            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository,
                IApartmentMapAsyncRepository amRepo,
                IProjectAsyncRepository projectRepo,
                ITowerAsyncRepository towerRepo,
                IFloorAsyncRepository floorRepo,
                IApartmentAsyncRepository apartRepo,
                IUserAsyncRepository userRepo,
                ITypeAttributeItemAsyncRepository typeRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _amRepo = amRepo;
                _projectRepo = projectRepo;
                _towerRepo = towerRepo;
                _floorRepo = floorRepo;
                _apartRepo = apartRepo;
                _userRepo = userRepo;
                _typeRepo = typeRepo;
            }

            public async Task<IPagedResult<ResResidentDT>> Handle(GetResidentByPagingQuery request, CancellationToken cancellationToken)
            {
                Application.Models.PagedResult<ResResidentDT> pagedResult = new Application.Models.PagedResult<ResResidentDT>();
                //var spec = new ResidentFilterWithPagingSpec(request);
                //FilterPagination filterPagination = new FilterPagination();
                //filterPagination.page = request.page;
                //filterPagination.page_size = request.page_size;
                //filterPagination.query = request.query;
                //filterPagination.order_by = request.order_by;
                //filterPagination.search = request.search;
                //var entities = await _entityRepository.GetByPageAsync(filterPagination, request.ProjectId, request.TowerId,
                //    request.FloorId, request.ApartmentId, request.type, cancellationToken);
                var data = (from r in _entityRepository.All()
                            join ap in _amRepo.All() on r.Id equals ap.ResidentId
                            where r.Status != AppEnum.EntityStatus.DELETED && r.Status != AppEnum.EntityStatus.NOT_OK
                            && ap.Status != AppEnum.EntityStatus.DELETED
                            && (ap.ProjectId == request.ProjectId || request.ProjectId == -1)
                            && (ap.TowerId == request.TowerId || request.TowerId == -1)
                            && (ap.FloorId == request.FloorId || request.FloorId == -1)
                            && (ap.ApartmentId == request.ApartmentId || request.ApartmentId == -1)
                            && (ap.Type == request.type || request.type == -1)
                            && (r.FullName.Contains(request.search) || r.Phone.Contains(request.search) || request.search == "" || request.search == null)
                            //group r by new
                            //{
                            //    r.Id,
                            //    r.FullName,
                            //    r.Phone,
                            //} into x
                            group r by r.Id into x
                            select new ResResidentDT
                            {
                                ResidentId = x.Key,
                                //FullName = x.FirstOrDefault().FullName,
                                //Birthday = x.FirstOrDefault().Birthday,
                                //CardId = x.FirstOrDefault().CardId,
                                //Phone = x.FirstOrDefault().Phone,
                                //Email = x.FirstOrDefault().Email,
                                //Address = x.FirstOrDefault().Address,
                                //Avata = x.FirstOrDefault().Avata,
                                //Sex = x.FirstOrDefault().Sex,
                                //Note = x.FirstOrDefault().Note,
                                //CreatedAt = x.FirstOrDefault().CreatedAt,
                                //UpdatedAt = x.FirstOrDefault().UpdatedAt,
                                //UserId = x.FirstOrDefault().CreatedById,
                                //Status = x.FirstOrDefault().Status
                            }).AsQueryable();

                //if (paging.query != null)
                //{
                //    paging.query = HttpUtility.UrlDecode(paging.query);
                //}

                //data = data.Where(request.query);
                ////var test = db.Residents.Where(e=>e.Status != AppEnum.EntityStatus.DELETED).ToList();
                ////pagedResult.RowCount = await db.Residents.Where(e => e.Status != AppEnum.EntityStatus.DELETED).CountAsync();
                pagedResult.RowCount = data.ToList().Count;

                if (request.page_size > 0)
                {
                    if (request.order_by != null)
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
                    if (request.order_by != null)
                    {
                        data = data.OrderBy(request.order_by);
                    }
                    else
                    {
                        data = data.OrderBy("ResidentId desc");
                    }
                }

                if (request.select != null && request.select != "")
                {
                    request.select = "new(" + request.select + ")";
                    request.select = HttpUtility.UrlDecode(request.select);
                    //pagedResult.Results = await data.Select(paging.select).ToDynamicListAsync();
                }
                else
                {
                    pagedResult.Results = data.ToList();

                }

                List<ResResidentDT> res = new List<ResResidentDT>();
                foreach (var item in pagedResult.Results)
                {
                    var resident = await _entityRepository.GetByKeyAsync(item.ResidentId);
                    ResResidentDT residentDTO = new ResResidentDT();
                    residentDTO.ResidentId = item.ResidentId;
                    
                    if (resident != null)
                    {
                        residentDTO.FullName = resident.FullName;
                        residentDTO.Phone = resident.Phone;
                        residentDTO.Birthday = resident.Birthday;
                        residentDTO.CardId = resident.CardId;
                        residentDTO.Email = resident.Email;
                        residentDTO.Address = resident.Address;
                        residentDTO.Avata = resident.Avata;
                        residentDTO.Sex = resident.Sex;
                        residentDTO.Note = resident.Note;
                        residentDTO.CountryId = resident.CountryId;
                        residentDTO.CreatedAt = resident.CreatedAt;
                        residentDTO.UpdatedAt = resident.UpdatedAt;
                        residentDTO.UserId = resident.CreatedById;
                        residentDTO.Status = resident.Status;
                    }
                    residentDTO.apartments = new List<ApartmentMapDT>();
                    var apartmentMaps = await _amRepo.GetListByResidentAsync(item.ResidentId, cancellationToken);
                    foreach (var childItem in apartmentMaps)
                    {
                        try
                        {
                            ApartmentMapDT apartmentResiDentDTO = new ApartmentMapDT();
                            apartmentResiDentDTO.ApartmentMapId = childItem.Id;
                            apartmentResiDentDTO.ApartmentId = childItem.ApartmentId != null ? childItem.ApartmentId : -1;
                            apartmentResiDentDTO.FloorId = childItem.FloorId != null ? childItem.FloorId : -1;
                            apartmentResiDentDTO.TowerId = childItem.TowerId != null ? childItem.TowerId : -1;
                            apartmentResiDentDTO.ProjectId = childItem.ProjectId != null ? childItem.ProjectId : -1;
                            apartmentResiDentDTO.Type = childItem.Type;
                            apartmentResiDentDTO.RelationshipId = childItem.RelationshipId != null ? childItem.RelationshipId : -1;
                            apartmentResiDentDTO.DateRent = childItem.DateRent;
                            apartmentResiDentDTO.DateStart = childItem.DateStart;
                            apartmentResiDentDTO.DateEnd = childItem.DateEnd;
                            apartmentResiDentDTO.RelationshipName = "";

                            //Project project = DbContext.Set<Project>().Where(p => p.ProjectId == apartmentResiDentDTO.ProjectId && p.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                            var project = await _projectRepo.FindByProjectIdAsync((int)apartmentResiDentDTO.ProjectId, cancellationToken);
                            //Tower tower = DbContext.Set<Tower>().Where(p => p.TowerId == apartmentResiDentDTO.TowerId && p.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                            var tower = await _towerRepo.FindByTowerIdAsync((int)apartmentResiDentDTO.TowerId, cancellationToken);
                            //Floor floor = db.Floor.Where(p => p.FloorId == apartmentResiDentDTO.FloorId && p.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                            var floor = await _floorRepo.FindByFloorIdAsync((int)apartmentResiDentDTO.FloorId, cancellationToken);
                            //Apartment apartment = db.Apartment.Where(p => p.ApartmentId == apartmentResiDentDTO.ApartmentId && p.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                            var apartment = await _apartRepo.FindByApartmentIdAsync((int)apartmentResiDentDTO.ApartmentId, cancellationToken);
                            //TypeAttributeItem typeAttributeItem = db.TypeAttributeItem.Where(ta => ta.TypeAttributeItemId == childItem.RelationshipId && ta.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                            var typeAttributeItem = await _typeRepo.FindByTypeAttributeItemIdAsync((int)apartmentResiDentDTO.RelationshipId, cancellationToken);
                            apartmentResiDentDTO.FullName = "";
                            if (apartment != null)
                            {
                                apartmentResiDentDTO.Code = apartment.Code;
                                apartmentResiDentDTO.Name = apartment.Name;
                                apartmentResiDentDTO.FullName = apartment.Name;

                                if (typeAttributeItem != null)
                                {
                                    apartmentResiDentDTO.RelationshipName = typeAttributeItem.Name;
                                }
                                else
                                {
                                    apartmentResiDentDTO.RelationshipId = null;
                                }
                            }

                            if (floor != null)
                            {
                                apartmentResiDentDTO.FullName = apartmentResiDentDTO.FullName + "-" + floor.Name;
                            }

                            if (tower != null)
                            {
                                apartmentResiDentDTO.FullName = apartmentResiDentDTO.FullName + "-" + tower.Name;
                            }

                            if (project != null)
                            {
                                apartmentResiDentDTO.FullName = apartmentResiDentDTO.FullName + "-" + project.Name;
                            }

                            residentDTO.apartments.Add(apartmentResiDentDTO);
                        }
                        catch(Exception ex)
                        {
                            
                        }
                    }

                    var us = await _userRepo.FindByResidentAsync(item.ResidentId, cancellationToken);
                    if(us!= null)
                    {
                        residentDTO.user = new UserDT
                        {
                            UserId = us.Id,
                            UserName = us.UserName,
                            Address = us.Address,
                            Avata = us.Avata,
                            CardId = us.CardId,
                            Code = us.Code,
                            Email = us.Email,
                            FullName = us.FullName,
                            KeyRandom = us.KeyRandom,
                            Note = us.Note,
                            Password = "123123",
                            Phone = us.Phone,
                            RegEmail = us.RegEmail,
                            RegisterCode = us.RegisterCode,
                            Status = (int)us.Status
                        };
                    }
                    //residentDTO.user = DbContext.Set<User>().Where(e => e.UserMapId == item.ResidentId).Select(e => new UserDT
                    //{
                    //    UserId = e.Id,
                    //    UserName = e.UserName,
                    //    Status = (int)e.Status
                    //}).FirstOrDefault();

                    res.Add(residentDTO);
                }
                pagedResult.Results = res;
                return pagedResult;
            }
        }
    }
}
