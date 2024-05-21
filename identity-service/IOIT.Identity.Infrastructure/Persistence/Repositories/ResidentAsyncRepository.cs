using IOIT.Identity.Application.Models;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using IOIT.Shared.ViewModels.PagingQuery;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class ResidentAsyncRepository : AsyncGenericRepository<Resident, long>, IResidentAsyncRepository
    {
        private readonly AppDbContext db;
        public ResidentAsyncRepository(AppDbContext context) : base(context)
        {
            db = context;
        }

        public Task<Resident> CheckPhoneExitAsync(string phoneMain, long residentId, CancellationToken cancellationToken)
        {
            return DbSet.AsNoTracking().Where(f => f.Phone == phoneMain.Trim() && f.Id != residentId && f.Status == AppEnum.EntityStatus.NORMAL).FirstOrDefaultAsync();
        }

        public Task<Resident> CheckUserMapIdAsync(long? userMapId, CancellationToken cancellationToken)
        {
            return DbSet.AsNoTracking().Where(f => f.Id != userMapId && f.Status != AppEnum.EntityStatus.DELETED).FirstOrDefaultAsync();
        }

        public async Task<Resident> CheckPhoneResidentGuestAsync(string phoneMain, int projectId, int towerId, int floorId, int apartmentId, CancellationToken cancellationToken)
        {
            string phoneMain2 = Utils.ConvertPhone(phoneMain);
            return await(from res in DbSet
                         join ar in DbContext.Set<ApartmentMap>() on res.Id equals ar.ResidentId
                         where (res.Phone == phoneMain || res.Phone == phoneMain2)
                             && ar.ApartmentId == apartmentId
                             && ar.ProjectId == projectId
                             && ar.TowerId == towerId
                             && ar.FloorId == floorId
                             && (ar.Type == (int)AppEnum.TypeResident.RESIDENT_GUEST
                             || ar.Type == (int)AppEnum.TypeResident.RESIDENT_GUEST_MEMBER)
                             && res.Status == AppEnum.EntityStatus.NORMAL
                             && ar.Status != AppEnum.EntityStatus.DELETED
                         select res).FirstOrDefaultAsync();
        }

        public async Task<Resident> CheckPhoneResidentMainAsync(string phoneMain, CancellationToken cancellationToken)
        {
            string phoneMain2 = Utils.ConvertPhone(phoneMain);
            return await (from res in DbSet
                               join ar in DbContext.Set<ApartmentMap>() on res.Id equals ar.ResidentId
                               where (res.Phone == phoneMain || res.Phone == phoneMain2)
                               && res.Status ==AppEnum.EntityStatus.NORMAL
                               && ar.Status != AppEnum.EntityStatus.DELETED
                               select res).FirstOrDefaultAsync();
        }

        public async Task<Resident> CheckResidentExitAsync(int projectId, int towerId, int floorId, int apartmentId, CancellationToken cancellationToken)
        {
            return await (from res in DbSet
                          join ar in DbContext.Set<ApartmentMap>() on res.Id equals ar.ResidentId
                          where ar.ProjectId == projectId && ar.TowerId == towerId
                          && ar.FloorId == floorId && ar.ApartmentId == apartmentId
                          && ar.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN
                          && res.Status == AppEnum.EntityStatus.NORMAL
                          && ar.Status != AppEnum.EntityStatus.DELETED
                          select res).FirstOrDefaultAsync();
        }

        public async Task<IPagedResult<ResResidentDT>> GetByPageAsync(FilterPagination paging, int? ProjectId, int? TowerId, int? FloorId, int? ApartmentId, int? type, CancellationToken cancellationToken)
        {
            Application.Models.PagedResult<ResResidentDT> pagedResult = new Application.Models.PagedResult<ResResidentDT>();
            var data = (from r in db.Residents
                        join ap in db.ApartmentMaps on r.Id equals ap.ResidentId
                    where r.Status != AppEnum.EntityStatus.DELETED && r.Status != AppEnum.EntityStatus.NOT_OK
                    && ap.Status != AppEnum.EntityStatus.DELETED
                    && (ap.ProjectId == ProjectId || ProjectId == -1)
                    && (ap.TowerId == TowerId || TowerId == -1)
                    && (ap.FloorId == FloorId || FloorId == -1)
                    && (ap.ApartmentId == ApartmentId || ApartmentId == -1)
                    && (ap.Type == type || type == -1)
                    && (r.FullName.Contains(paging.search) || r.Phone.Contains(paging.search) || paging.search == "" || paging.search == null)
                    group r by new
                    {
                        r.Id,
                        r.FullName,
                        r.Phone,
                    } into x
                    select new ResResidentDT
                    {
                        ResidentId = x.Key.Id,
                        FullName = x.Key.FullName,
                        //Birthday = x.FirstOrDefault().Birthday,
                        //CardId = x.FirstOrDefault().CardId,
                        Phone = x.Key.Phone,
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

            data = data.Where(paging.query);
            //var test = db.Residents.Where(e=>e.Status != AppEnum.EntityStatus.DELETED).ToList();
            //pagedResult.RowCount = await db.Residents.Where(e => e.Status != AppEnum.EntityStatus.DELETED).CountAsync();
            pagedResult.RowCount = data.ToList().Count;

            if (paging.page_size > 0)
            {
                if (paging.order_by != null)
                {
                    data = data.OrderBy(paging.order_by).Skip((paging.page - 1) * paging.page_size).Take(paging.page_size);
                }
                else
                {
                    data = data.OrderBy("ResidentId desc").Skip((paging.page - 1) * paging.page_size).Take(paging.page_size);
                }
            }
            else
            {
                if (paging.order_by != null)
                {
                    data = data.OrderBy(paging.order_by);
                }
                else
                {
                    data = data.OrderBy("ResidentId desc");
                }
            }

            if (paging.select != null && paging.select != "")
            {
                paging.select = "new(" + paging.select + ")";
                paging.select = HttpUtility.UrlDecode(paging.select);
                //pagedResult.Results = await data.Select(paging.select).ToDynamicListAsync();
            }
            else
            {
                pagedResult.Results = await data.ToListAsync();
            }

            return pagedResult;
        }

        public async Task<IPagedResult<ResGetResidentByApartmentId>> GetByApartmentIdAndPageAsync(int? apartmentId, int? residentId, int pageSize, int pageIndex, CancellationToken cancellationToken)
        {
            Application.Models.PagedResult<ResGetResidentByApartmentId> pagedResult = new Application.Models.PagedResult<ResGetResidentByApartmentId>();
            var data = (from r in db.Residents
                        join ap in db.ApartmentMaps on r.Id equals ap.ResidentId
                        where r.Status == AppEnum.EntityStatus.NORMAL && r.Status != AppEnum.EntityStatus.NOT_OK
                        && (apartmentId > 0 ? ap.ApartmentId == apartmentId : r.Id > 0)
                        //&& (ap.Type == 3)
                        select new ResGetResidentByApartmentId
                        {
                            ResidentId = r.Id,
                            ResidentName = r.FullName,
                            Phone = r.Phone,
                            Birthday = r.Birthday,
                            Gender = r.Sex,
                            Address = r.Address,
                            IdentifyCode = r.CardId,
                            IdentifyType = (Domain.Enum.DomainEnum.ResidentRequestIdentifyType)r.TypeCardId,
                            IdentifyCreate = r.DateId,
                            IdentifyLoc = r.AddressId,
                            RelationshipId = (int)ap.RelationshipId,
                            RelationshipName = db.TypeAttributeItems.Where(s => s.TypeAttributeItemId == ap.RelationshipId && s.Status != AppEnum.EntityStatus.DELETED).Select(s => s.Name).FirstOrDefault() ?? string.Empty,
                            Status = (int)r.Status
                        }).AsQueryable();

            data = data.Where(r => (residentId != null && residentId > 0) ? r.ResidentId == residentId : r.ResidentId > 0);
            pagedResult.RowCount = data.ToList().Count;
            pagedResult.CurrentPage = pageIndex;
            pagedResult.PageSize = pageSize;

            data = data.OrderBy("ResidentId desc").Skip((pageIndex - 1) * pageSize).Take(pageSize);

            pagedResult.Results = await data.ToListAsync();

            return pagedResult;
        }

        //Danh sách căn hộ và cư dân thuộc căn hộ
        public async Task<IPagedResult<ResResidentIdDT>> GetApartmentResidentByPageAsync(FilterPagination paging, int? ProjectId, int? TowerId, int? FloorId, int? ApartmentId, CancellationToken cancellationToken)
        {
            Application.Models.PagedResult<ResResidentIdDT> pagedResult = new Application.Models.PagedResult<ResResidentIdDT>();
            var data = (from r in db.Residents
                        join ap in db.ApartmentMaps on r.Id equals ap.ResidentId
                        where r.Status != AppEnum.EntityStatus.DELETED
                        && ap.Status != AppEnum.EntityStatus.DELETED
                        && (ap.ProjectId == ProjectId || ProjectId == -1)
                        && (ap.TowerId == TowerId || TowerId == -1)
                        && (ap.FloorId == FloorId || FloorId == -1)
                        && (ap.ApartmentId == ApartmentId || ApartmentId == -1)
                        && (r.FullName.Contains(paging.search) || r.Phone.Contains(paging.search) || paging.search == "" || paging.search == null)
                        select new ResResidentIdDT
                        {
                            ResidentId = ap.ResidentId,
                            ApartmentId = ap.ApartmentId
                        }).Distinct().AsQueryable();

            if (paging.query != null)
            {
                paging.query = HttpUtility.UrlDecode(paging.query);
            }

            data = data.Where(paging.query);
            pagedResult.RowCount = data.ToList().Count;

            if (paging.page_size > 0)
            {
                if (paging.order_by != null)
                {
                    data = data.OrderBy(paging.order_by).Skip((paging.page - 1) * paging.page_size).Take(paging.page_size);
                }
                else
                {
                    data = data.OrderBy("ResidentId desc").Skip((paging.page - 1) * paging.page_size).Take(paging.page_size);
                }
            }
            else
            {
                if (paging.order_by != null)
                {
                    data = data.OrderBy(paging.order_by);
                }
                else
                {
                    data = data.OrderBy("ResidentId desc");
                }
            }

            if (paging.select != null && paging.select != "")
            {
                paging.select = "new(" + paging.select + ")";
                paging.select = HttpUtility.UrlDecode(paging.select);
            }
            else
            {
                pagedResult.Results = await data.ToListAsync();
            }

            return pagedResult;
        }
        public async Task<int> GetCountResidentByProjectId(int projectId, CancellationToken cancellationToken)
        {
            if (projectId > 0)
            {
                return await DbSet
                    .Join(
                    db.ApartmentMaps,
                    r => r.Id,
                    ap => ap.ResidentId,
                    (r, ap) => new { r, ap.ProjectId }
                    )
                    .Where(c => c.ProjectId == projectId &&
                    c.r.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED && c.r.Status != AppEnum.EntityStatus.NOT_OK)
                    .Select(fl => new Resident()
                    {
                        Id = fl.r.Id,
                    }).CountAsync();
            }
            else
            {
                return await DbSet
                   .Join(
                   db.ApartmentMaps,
                   r => r.Id,
                   ap => ap.ResidentId,
                   (r, ap) => new { r, ap.ProjectId }
                   )
                   .Where(c => c.r.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED && c.r.Status != AppEnum.EntityStatus.NOT_OK)
                   .Select(fl => new Resident()
                   {
                       Id = fl.r.Id,
                   }).CountAsync();
            }
        }

        public async Task<ResGetResidentByNameAndPhoneInApartment?> GetResidentByNameOrPhoneInApartment(int apartmentId, string name, string phone, CancellationToken cancellationToken)
        {
            var entities =  await (from res in DbSet
                         join ar in DbContext.Set<ApartmentMap>() on res.Id equals ar.ResidentId
                         where ar.ApartmentId == apartmentId
                         && (res.FullName.ToLower() == name.ToLower() || (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(res.Phone) && res.Phone == phone))
                         && res.Status == AppEnum.EntityStatus.NORMAL
                         && ar.Status != AppEnum.EntityStatus.DELETED
                         select new ResGetResidentByNameAndPhoneInApartment
                         { 
                            Id = res.Id,
                            FullName = res.FullName,
                            Phone = res.Phone,
                            Birthday = res.Birthday,
                            Sex = res.Sex,
                            CardId = res.CardId,
                            DateId = res.DateId,
                            AddressId = res.AddressId,
                            Address = res.Address,
                            TypeCardId = res.TypeCardId,
                            RelationshipId = ar.RelationshipId,
                            UpdatedAt = res.UpdatedAt,
                         })
                         .OrderByDescending(o => o.UpdatedAt)
                         .ToListAsync();

            if (entities.Count == 1)
            {
                return entities[0];
            }
            else if (entities.Count > 1)
            {
                ResGetResidentByNameAndPhoneInApartment? residentHasAccount = null;

                for(var i = 0; i < entities.Count; i++)
                {
                    var itemResident = entities[i];

                    var isExistUser = DbContext.Set<User>().Where(s =>
                    s.Status == AppEnum.EntityStatus.NORMAL &&
                    s.UserMapId == itemResident.Id &&
                    (s.Type == (byte)TypeUser.RESIDENT_MAIN || s.Type == (byte)TypeUser.RESIDENT_GUEST))
                    .Select(o => o.Id)
                    .FirstOrDefault();

                    if (isExistUser > 0)
                    {
                        residentHasAccount = itemResident;
                        break;
                    }
                }

                if (residentHasAccount == null)
                {
                    return entities[0];
                }

                return residentHasAccount;
            }

            return null;
        }
    }
}
