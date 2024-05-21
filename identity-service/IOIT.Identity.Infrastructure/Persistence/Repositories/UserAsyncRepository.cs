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
    public class UserAsyncRepository : AsyncGenericRepository<User, long>, IUserAsyncRepository
    {
        public UserAsyncRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<User> CheckUserLoginAsync(string userName, string password, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.UserName == userName && e.Password == password
                && e.Type != (int)AppEnum.TypeUser.RESIDENT_MAIN && e.Type != (int)AppEnum.TypeUser.RESIDENT_GUEST
                && e.Status != AppEnum.EntityStatus.DELETED
            , cancellationToken);
        }

        public async Task<User> CheckUserLoginAppAsync(string userName, string password, int type)
        {
            if (type == 1)
            {
                string userName2 = Utils.ConvertPhone(userName);
                return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => (e.UserName.Trim() == userName.Trim() || e.UserName.Trim() == userName2.Trim())
                    && e.Password.Trim() == password.Trim()
                    && (e.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN || e.Type == (int)AppEnum.TypeUser.RESIDENT_GUEST)
                    && e.Status != AppEnum.EntityStatus.DELETED);
                //var user2 = DbSet.Where(e => (e.UserName.Trim() == userName.Trim() || e.UserName.Trim() == userName2.Trim())
                //     && e.Password.Trim() == password.Trim()
                //     && (e.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN || e.Type != (int)AppEnum.TypeUser.RESIDENT_GUEST)
                //     && e.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                //return user;
            }
            else
            {
                return await DbSet.FirstOrDefaultAsync(e => e.UserName == userName && e.Password == password
                   && (e.Type == (int)AppEnum.TypeUser.MANAGEMENT || e.Type == (int)AppEnum.TypeUser.TECHNICIANS)
                   && e.Status != AppEnum.EntityStatus.DELETED);
            }
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, long? userId, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.Email == normalizedEmail
                        && c.Id != userId
                        && c.Status != AppEnum.EntityStatus.DELETED, cancellationToken);
        }

        public async Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.UserName == userName
                && e.Type != (int)AppEnum.TypeUser.RESIDENT_MAIN && e.Type != (int)AppEnum.TypeUser.RESIDENT_GUEST
                && e.Status != AppEnum.EntityStatus.DELETED
            , cancellationToken);
        }

        public async Task<User> FindByPhoneUsernameAsync(string userName, int type, CancellationToken cancellationToken)
        {
            
            if (type == 1)
            {
                string userName2 = Utils.ConvertPhone(userName);
                return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => (e.UserName == userName || e.UserName == userName2)
                && e.Status != AppEnum.EntityStatus.DELETED
                && (e.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN || e.Type == (int)AppEnum.TypeUser.RESIDENT_GUEST)
                , cancellationToken);
            }
            else if(type == 2)
            {
                return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.UserName == userName
               && e.Status != AppEnum.EntityStatus.DELETED
               && (e.Type == (int)AppEnum.TypeUser.MANAGEMENT || e.Type == (int)AppEnum.TypeUser.TECHNICIANS)
               , cancellationToken);
            }
            else
            {
                return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.UserName == userName
               && e.Status != AppEnum.EntityStatus.DELETED
               && e.Type == (int)AppEnum.TypeUser.ADMINISTRATOR
               , cancellationToken);
            }

        }

        public async Task<User> FindByUserMapIdAsync(long? userMapId, int type, CancellationToken cancellationToken)
        {
            if (type == 1)
            {
                return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.UserMapId == userMapId
                && e.Status != AppEnum.EntityStatus.DELETED
                && (e.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN || e.Type == (int)AppEnum.TypeUser.RESIDENT_GUEST)
                , cancellationToken);
            }
            else
            {
                return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.UserMapId == userMapId
                && e.Status != AppEnum.EntityStatus.DELETED
                && (e.Type == (int)AppEnum.TypeUser.MANAGEMENT || e.Type == (int)AppEnum.TypeUser.TECHNICIANS)
                , cancellationToken);
            }
        }

        public async Task<User> FindByRegisterCode(long id, string registerCode, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(c =>c.Id == id && c.Code == registerCode && c.Status != AppEnum.EntityStatus.DELETED, cancellationToken);
        }

        public async Task<User> FindByUserCode(string userCode, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(c => c.Code == userCode && c.Status != AppEnum.EntityStatus.DELETED, cancellationToken);
        }

        public async Task<User> FindByUsernameAsync(string userName, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.UserName == userName
                && (e.Type == (int)AppEnum.TypeUser.MANAGEMENT || e.Type == (int)AppEnum.TypeUser.TECHNICIANS)
                && e.Status != AppEnum.EntityStatus.DELETED
            , cancellationToken);
        }

        public async Task<User> FindByResidentAsync(long residentId, CancellationToken cancellationToken)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.UserMapId == residentId && e.Status != AppEnum.EntityStatus.DELETED, cancellationToken);
        }

        public async Task<User> CheckUserNameExitAsync(string userName, long? userId, CancellationToken cancellationToken)
        {
            //if (userId == -1)
            //    return await DbSet.FirstOrDefaultAsync(e => e.UserName == userName && e.Id != userId, cancellationToken);
            //else
                return await DbSet.FirstOrDefaultAsync(e => e.UserName == userName && e.Id != userId && e.Status != AppEnum.EntityStatus.DELETED, cancellationToken);
        }

        public async Task<IPagedResult<UserRoleDT>> GetByPageEmpAsync(FilterPagination paging, int? roleMax, int? roleLevel, int? projectId, CancellationToken cancellationToken)
        {
            Application.Models.PagedResult<UserRoleDT> pagedResult = new Application.Models.PagedResult<UserRoleDT>();
            IQueryable<UserRoleDT> data = from u in DbSet
                                          join e in DbContext.Set<Employee>() on u.UserMapId equals e.Id
                                          where (u.Type == (int)AppEnum.TypeUser.MANAGEMENT
                                          || u.Type == (int)AppEnum.TypeUser.TECHNICIANS)
                                          && u.Status != AppEnum.EntityStatus.DELETED
                                          select new UserRoleDT
                                          {
                                              UserId = u.Id,
                                              UserMapId = u.UserMapId,
                                              UserName = u.UserName,
                                              Type = u.Type,
                                              TypeThird = u.TypeThird,
                                              UserCreateId = u.CreatedById,
                                              UserEditId = u.UpdatedById,
                                              UpdatedAt = u.UpdatedAt,
                                              CreatedAt = u.CreatedAt,
                                              Status = (int)u.Status,
                                              ProjectId = e.ProjectId,
                                              DepartmentId = e.DepartmentId,
                                              PositionId = e.PositionId,
                                              RoleMax = u.RoleMax,
                                              RoleLevel = u.RoleLevel,
                                              IsRoleGroup = u.IsRoleGroup,
                                              IsPhoneConfirm = u.IsPhoneConfirm,
                                              IsEmailConfirm = u.IsEmailConfirm,
                                              LanguageId = u.LanguageId,
                                              FullName = e.FullName,
                                              CardId = e.CardId,
                                              Email = e.Email,
                                              Phone = e.Phone,
                                              Address = e.Address,
                                              Avata = e.Avata,
                                              Birthday = e.Birthday,
                                          };

            if (roleMax != 1)
            {
                if (paging.query != null && paging.query != "")
                    paging.query += " AND RoleLevel > " + roleLevel + " AND ProjectId=" + projectId;
                else
                    paging.query = "RoleLevel > " + roleLevel + " AND ProjectId=" + projectId;
            }
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
                    data = data.OrderBy("CreatedAt desc").Skip((paging.page - 1) * paging.page_size).Take(paging.page_size);
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
                    data = data.OrderBy("CreatedAt desc");
                }
            }

            if (paging.select != null && paging.select != "")
            {
                paging.select = "new(" + paging.select + ")";
                paging.select = HttpUtility.UrlDecode(paging.select);
                //def.data = await data.Select(paging.select).ToDynamicListAsync();
            }
            else
            {
                //var listData = data.ToList();
                //List<UserRoleDT> listUser = new List<UserRoleDT>();
                //foreach (var item in listData)
                //{
                //    //UserRoleDT user = new UserRoleDT();

                //    var project = await db.Project.Where(e => e.ProjectId == item.ProjectId).FirstOrDefaultAsync();
                //    item.ProjectName = project != null ? project.Name : "";
                //    var apartment = await db.Department.Where(e => e.DepartmentId == item.DepartmentId).FirstOrDefaultAsync();
                //    item.DepartmentName = apartment != null ? apartment.Name : "";
                //    var position = await db.Position.Where(e => e.PositionId == item.PositionId).FirstOrDefaultAsync();
                //    item.PositionName = position != null ? position.Name : "";

                //    item.listRole = await db.UserRole.Where(e => e.UserId == item.UserId
                //    && e.Status != (int)Const.Status.DELETED).Select(e => new RoleDT
                //    {
                //        RoleId = e.RoleId,
                //        RoleName = db.Role.Where(r => r.RoleId == e.RoleId).FirstOrDefault().Name,
                //    }).ToListAsync();

                //    item.listFunction = await db.FunctionRole.Where(e => e.TargetId == item.UserId
                //        && e.Type == (int)Const.TypeFunction.FUNCTION_USER
                //        && e.Status != (int)Const.Status.DELETED).Select(e => new FunctionRoleDT
                //        {
                //            FunctionId = e.FunctionId,
                //            ActiveKey = e.ActiveKey
                //        }).ToListAsync();

                //    listUser.Add(item);

                //}
                //def.data = listUser;
                pagedResult.Results = await data.ToListAsync();

                
            }
            return pagedResult;
        }


    }
}
