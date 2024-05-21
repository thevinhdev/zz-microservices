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

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class ApartmentMapAsyncRepository : AsyncGenericRepository<ApartmentMap, Guid>, IApartmentMapAsyncRepository
    {
        public ApartmentMapAsyncRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<List<ApartmentMap>> GetListByResidentAsync(long residentId, CancellationToken cancellationToken)
        {
            return await DbSet.Where(c => c.ResidentId == residentId && c.Status != AppEnum.EntityStatus.DELETED).ToListAsync();
        }

        public async Task<ApartmentMap> CheckResidentMainAsync(int? apartmentId, CancellationToken cancellationToken)
        {
            return await DbSet.Where(a => a.ApartmentId == apartmentId
                                && a.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN && a.Status != AppEnum.EntityStatus.DELETED).FirstOrDefaultAsync();
        }

        public async Task<ApartmentMap> CheckApartmentResidentAsync(int? apartmentId, long? residentId, CancellationToken cancellationToken)
        {
            return await DbSet.Where(a => a.ApartmentId == apartmentId
                                && a.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN && a.Status != AppEnum.EntityStatus.DELETED).FirstOrDefaultAsync();
        }

        public async Task<Apartment> GetApartmentNameAsync(long residentId, CancellationToken cancellationToken)
        {
            return await (from res in DbSet
                         join ar in DbContext.Set<Apartment>() on res.ApartmentId equals ar.ApartmentId
                         where res.ResidentId == residentId
                         && res.Status == AppEnum.EntityStatus.NORMAL
                         && ar.Status != AppEnum.EntityStatus.DELETED
                         select ar).FirstOrDefaultAsync();
        }

        public async Task<IPagedResult<ApartmentDT>> GetByPageApartmentAsync(FilterPagination paging, long? ResidentId, CancellationToken cancellationToken)
        {
            Application.Models.PagedResult<ApartmentDT> pagedResult = new Application.Models.PagedResult<ApartmentDT>();
            IQueryable<ApartmentDT> data = (from ap in DbSet
                                          join a in DbContext.Set<Apartment>() on ap.ApartmentId equals a.ApartmentId
                                          where ap.Status == AppEnum.EntityStatus.NORMAL
                                          && ap.ResidentId == ResidentId
                                          && a.Status == AppEnum.EntityStatus.NORMAL
                                          select new ApartmentDT { 
                                              ApartmentId = a.ApartmentId,
                                              Code = a.Code,
                                              Name = a.Name,
                                              FloorId = a.FloorId,
                                              TowerId = a.TowerId,
                                              ProjectId = a.ProjectId,
                                              CreatedAt = a.CreatedAt,
                                              UpdatedAt = a.UpdatedAt,
                                              UserId = a.CreatedById,
                                              Status = (int)a.Status,
                                              TypeUser = ap.Type,
                                          }).AsQueryable();

            if (paging.query != null)
            {
                paging.query = HttpUtility.UrlDecode(paging.query);
            }

            data = data.Where(paging.query);
            pagedResult.RowCount = data.ToList().Count;

            if (paging.page_size > 0)
            {
                if (paging.order_by != null && paging.order_by != "")
                {
                    data = data.OrderBy(paging.order_by).Skip((paging.page - 1) * paging.page_size).Take(paging.page_size);
                }
                else
                {
                    data = data.OrderBy("ApartmentId desc").Skip((paging.page - 1) * paging.page_size).Take(paging.page_size);
                }
            }
            else
            {
                if (paging.order_by != null && paging.order_by != "")
                {
                    data = data.OrderBy(paging.order_by);
                }
                else
                {
                    data = data.OrderBy("ApartmentId desc");
                }
            }

            if (paging.select != null && paging.select != "")
            {
                paging.select = "new(" + paging.select + ")";
                paging.select = HttpUtility.UrlDecode(paging.select);
                //pagedResult.Results = await data.Select(paging.select).ToDynamicListAsync();
                pagedResult.Results = await data.ToListAsync();
            }
            else
            {
                pagedResult.Results = await data.ToListAsync();
            }

            return pagedResult;
        }

        public async Task<IPagedResult<ResidentDT>> GetByPageMemberApartmentAsync(FilterPagination paging, int? ApartmentId, CancellationToken cancellationToken)
        {
            Application.Models.PagedResult<ResidentDT> pagedResult = new Application.Models.PagedResult<ResidentDT>();
            IQueryable<ResidentDT> data = (from ap in DbSet
                                           join r in DbContext.Set<Resident>() on ap.ResidentId equals r.Id
                                           //join ta in DbContext.Set<TypeAttributeItem>() on ap.RelationshipId equals ta.TypeAttributeItemId
                                           where ap.Status != AppEnum.EntityStatus.DELETED
                                           && ap.Type == (int)AppEnum.TypeResident.RESIDENT_MEMBER
                                           && ap.ApartmentId == ApartmentId
                                           && r.Status != AppEnum.EntityStatus.DELETED
                                           select new ResidentDT
                                           {
                                               ResidentId = r.Id,
                                               FullName = r.FullName,
                                               RelationshipId = ap.RelationshipId,
                                               RelationshipName = DbContext.Set<TypeAttributeItem>().Where(ty => ty.TypeAttributeItemId == ap.RelationshipId).FirstOrDefault() != null ? DbContext.Set<TypeAttributeItem>().Where(ty => ty.TypeAttributeItemId == ap.RelationshipId).FirstOrDefault().Name : "",
                                               //RelationshipName = ta.Name,
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

            if (paging.query != null)
            {
                paging.query = HttpUtility.UrlDecode(paging.query);
            }

            data = data.Where(paging.query);
            pagedResult.RowCount = data.ToList().Count;

            if (paging.page_size > 0)
            {
                if (paging.order_by != null && paging.order_by != "")
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
                if (paging.order_by != null && paging.order_by != "")
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
                pagedResult.Results = await data.ToListAsync();
            }
            else
            {
                pagedResult.Results = await data.ToListAsync();
            }

            return pagedResult;
        }

        public async Task<IPagedResult<ResidentDT>> GetMemberApartmentAdminAsync(FilterPagination paging, int? ApartmentId, byte? type, CancellationToken cancellationToken)
        {
            Application.Models.PagedResult<ResidentDT> pagedResult = new Application.Models.PagedResult<ResidentDT>();

            IQueryable<ResidentDT> data = Enumerable.Empty<ResidentDT>().AsQueryable();

            if (type == (int)AppEnum.TypeResident.RESIDENT_MAIN)
            {
                data = (from ap in DbSet
                        join r in DbContext.Set<Resident>() on ap.ResidentId equals r.Id
                        where ap.Status == AppEnum.EntityStatus.NORMAL
                        && ap.Type == (int)AppEnum.TypeResident.RESIDENT_MEMBER
                        && ap.ApartmentId == ApartmentId
                        && r.Status == AppEnum.EntityStatus.NORMAL
                        select new ResidentDT
                        {
                            ResidentId = r.Id,
                            FullName = r.FullName,
                            RelationshipId = ap.RelationshipId,
                            RelationshipName = DbContext.Set<TypeAttributeItem>().Where(ty => ty.TypeAttributeItemId == ap.RelationshipId).FirstOrDefault() != null ? DbContext.Set<TypeAttributeItem>().Where(ty => ty.TypeAttributeItemId == ap.RelationshipId).FirstOrDefault().Name : "",
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
            else if (type == (int)AppEnum.TypeResident.RESIDENT_GUEST)
            {
                data = (from ap in DbSet
                        join r in DbContext.Set<Resident>() on ap.ResidentId equals r.Id
                        where ap.Status == AppEnum.EntityStatus.NORMAL
                        && ap.Type == (int)AppEnum.TypeResident.RESIDENT_GUEST_MEMBER
                        && ap.ApartmentId == ApartmentId
                        && r.Status == AppEnum.EntityStatus.NORMAL
                        select new ResidentDT
                        {
                            ResidentId = r.Id,
                            FullName = r.FullName,
                            RelationshipId = ap.RelationshipId,
                            RelationshipName = DbContext.Set<TypeAttributeItem>().Where(ty => ty.TypeAttributeItemId == ap.RelationshipId).FirstOrDefault() != null ? DbContext.Set<TypeAttributeItem>().Where(ty => ty.TypeAttributeItemId == ap.RelationshipId).FirstOrDefault().Name : "",
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
                            Status = (byte)r.Status,
                        }).AsQueryable();
            }

            if (paging.query != null)
            {
                paging.query = HttpUtility.UrlDecode(paging.query);
            }

            data = data.Where(paging.query);
            pagedResult.RowCount = data.ToList().Count;

            if (paging.page_size > 0)
            {
                if (paging.order_by != null && paging.order_by != "")
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
                if (paging.order_by != null && paging.order_by != "")
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
                pagedResult.Results = await data.ToListAsync();
            }
            else
            {
                pagedResult.Results = await data.ToListAsync();
            }

            return pagedResult;
        }

        public async Task<ResGetCountApartmentWithTypeResident> GetCountApartmentWithTypeResidentAsync(int? projectId, int projectTokenId, int userType, DateTime? dateStart, DateTime? dateEnd)
        {
            var res = new ResGetCountApartmentWithTypeResident();

            var lstIdApartment = DbContext.Set<Apartment>()
                .Where(s => s.Status == AppEnum.EntityStatus.NORMAL
            && (((userType == (int)AppEnum.TypeUser.MANAGEMENT || userType == (int)AppEnum.TypeUser.TECHNICIANS) && s.ProjectId == projectTokenId) || (userType != (int)AppEnum.TypeUser.TECHNICIANS && userType != (int)AppEnum.TypeUser.MANAGEMENT && 1 == 1))
            && ((projectId != null && projectId > 0 && s.ProjectId == projectId) || ((projectId == null || projectId == 0) && 1 == 1))
            && ((dateStart != null && s.CreatedAt >= dateStart) || (dateStart == null && 1 == 1))
            && ((dateEnd != null && s.CreatedAt <= dateEnd) || (dateEnd == null && 1 == 1))).Select(s => s.ApartmentId).Count();

            var lstApartmentRent = (from ap in DbContext.Set<Apartment>()
                     join apm in DbSet on ap.ApartmentId equals apm.ApartmentId
                     where ap.Status == AppEnum.EntityStatus.NORMAL && apm.Status == AppEnum.EntityStatus.NORMAL && apm.Type == (byte)AppEnum.TypeResident.RESIDENT_GUEST && apm.ApartmentId > 0
                     && (((userType == (int)AppEnum.TypeUser.MANAGEMENT || userType == (int)AppEnum.TypeUser.TECHNICIANS) && ap.ProjectId == projectTokenId) || (userType != (int)AppEnum.TypeUser.TECHNICIANS && userType != (int)AppEnum.TypeUser.MANAGEMENT && 1 == 1))
                     && ((projectId != null && projectId > 0 && ap.ProjectId == projectId) || ((projectId == null || projectId == 0) && 1 == 1))
                     && ((dateStart != null && ap.CreatedAt >= dateStart) || (dateStart == null && 1 == 1))
                     && ((dateEnd != null && ap.CreatedAt <= dateEnd) || (dateEnd == null && 1 == 1))
                     select ap.ApartmentId).Distinct().Count();


            res.CountApartment = lstIdApartment;
            res.CountApartmentRent = lstApartmentRent;
            res.CountApartmentMain = res.CountApartment - res.CountApartmentRent;

            //foreach (var item in lstIdApartment)
            //{
            //    var isApartmentRent = DbSet.Where(s => s.Status != AppEnum.EntityStatus.DELETED 
            //    && s.Type == (byte)AppEnum.TypeResident.RESIDENT_GUEST
            //    && s.ApartmentId == item)
            //    .Select(s => s.ApartmentId)
            //    .FirstOrDefaultAsync();

            //    if (isApartmentRent != null)
            //    {
            //        res.CountApartmentRent++;
            //    } else
            //    {
            //        res.CountApartmentMain++;
            //    }
            //}

            return res;
        }
    }
}
