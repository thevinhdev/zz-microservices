using AutoMapper;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Application.UserRoles.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.UserRoles.Queries
{
    public class GetUserRoleByPagingQuery : FilterPagination, IRequest<ResUserRoleLists>
    {
        public class Handler : IRequestHandler<GetUserRoleByPagingQuery, ResUserRoleLists>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IUserRoleAsyncRepository _userRoleRepo;
            private readonly IAsyncRepository<Employee> _empRepo;
            private readonly IAsyncLongRepository<Resident> _residentRepo;
            private readonly IAsyncRepository<Position> _positionRepo;
            private readonly IProjectAsyncRepository _projectRepo;
            private readonly IApartmentMapAsyncRepository _amRepo;
            private readonly ITypeAttributeItemAsyncRepository _typeRepo;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo,
                IUserRoleAsyncRepository userRoleRepo, 
                IAsyncRepository<Employee> empRepo,
                IAsyncLongRepository<Resident> residentRepo, 
                IAsyncRepository<Position> positionRepo,
                IProjectAsyncRepository projectRepo,
                IApartmentMapAsyncRepository amRepo,
                ITypeAttributeItemAsyncRepository typeRepo,
                IFunctionRoleAsyncRepository funcRoleRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _userRoleRepo = userRoleRepo;
                _empRepo = empRepo;
                _residentRepo = residentRepo;
                _positionRepo = positionRepo;
                _projectRepo = projectRepo;
                _amRepo = amRepo;
                _typeRepo = typeRepo;
                _funcRoleRepo = funcRoleRepo;
            }

            public async Task<ResUserRoleLists> Handle(GetUserRoleByPagingQuery request, CancellationToken cancellationToken)
            {
                var spec = new UserRoleFilterWithPagingSpec(request);

                var entities = await _userRepo.PaggingAsync(spec);
                List<ResUserRole> listUser = new List<ResUserRole>();
                foreach (var item in entities.Results)
                {
                    ResUserRole user = new ResUserRole();
                    user.UserId = item.Id;
                    user.UserMapId = item.UserMapId;
                    user.UserName = item.UserName;
                    user.Type = item.Type;
                    user.Email = item.Email;
                    user.TypeThird = item.TypeThird;
                    user.UserCreateId = item.CreatedById;
                    user.UserEditId = item.UpdatedById;
                    user.UpdatedAt = item.UpdatedAt;
                    user.CreatedAt = item.CreatedAt;
                    user.DepartmentId = item.DepartmentId != null ? item.DepartmentId : 0;
                    user.PositionId = item.PositionId != null ? item.PositionId : 0;
                    user.RoleMax = item.RoleMax;
                    user.RoleLevel = item.RoleLevel;
                    user.IsRoleGroup = item.IsRoleGroup;
                    user.IsPhoneConfirm = item.IsPhoneConfirm;
                    user.IsEmailConfirm = item.IsEmailConfirm;
                    user.LanguageId = item.LanguageId;
                    user.Status = (int)item.Status;
                    if (user.Type == (byte)AppEnum.TypeUser.RESIDENT_MAIN || user.Type == (byte)AppEnum.TypeUser.RESIDENT_GUEST)
                    {
                        // var resident = await db.Resident.Where(e => e.ResidentId == user.UserMapId).FirstOrDefaultAsync();
                        var resident = await _residentRepo.GetByKeyAsync((int)user.UserMapId);
                        if (resident != null)
                        {
                            user.FullName = resident.FullName;
                            user.Email = (resident.Email != null && resident.Email != "") ? resident.Email : user.Email;
                            user.Phone = resident.Phone;
                            user.Address = resident.Address;
                            user.Avata = resident.Avata;
                            user.Birthday = resident.Birthday;
                            //var project = await db.Project.Where(e => e.ProjectId == user.ProjectId).FirstOrDefaultAsync();
                            var project = await _projectRepo.FindByProjectIdAsync(user.ProjectId, cancellationToken);
                            user.ProjectName = project != null ? project.Name : "";
                            //var apartment = await (from am in db.ApartmentMap
                            //                       join a in db.Apartment on am.ApartmentId equals a.ApartmentId
                            //                       where am.ResidentId == resident.ResidentId
                            //                       select a).FirstOrDefaultAsync();
                            var apartment = await _amRepo.GetApartmentNameAsync(resident.Id, cancellationToken);
                            user.DepartmentName = apartment != null ? apartment.Name : "";
                            //var position = await db.TypeAttributeItem.Where(e => e.TypeAttributeItemId == user.PositionId).FirstOrDefaultAsync();
                            var position = await _typeRepo.FindByTypeAttributeItemIdAsync(user.PositionId, cancellationToken);
                            user.PositionName = position != null ? position.Name : "";
                        }
                    }
                    else if (user.Type == (int)AppEnum.TypeUser.MANAGEMENT || user.Type == (int)AppEnum.TypeUser.TECHNICIANS)
                    {
                        //var employee = await db.Employee.Where(e => e.EmployeeId == user.UserMapId).FirstOrDefaultAsync();
                        var employee = await _empRepo.GetByKeyAsync((int)user.UserMapId);
                        if (employee != null)
                        {
                            user.DepartmentId = employee.DepartmentId;
                            user.ProjectId = employee.ProjectId;
                            user.PositionId = employee.PositionId;
                            user.FullName = employee.FullName;
                            user.CardId = employee.CardId;
                            user.Email = (employee.Email != null && employee.Email != "") ? employee.Email : user.Email;
                            user.Phone = employee.Phone;
                            user.Address = employee.Address;
                            user.Avata = employee.Avata;
                            user.Birthday = employee.Birthday;
                            //var project = await db.Project.Where(e => e.ProjectId == user.ProjectId).FirstOrDefaultAsync();
                            var project = await _projectRepo.FindByProjectIdAsync(user.ProjectId, cancellationToken);
                            user.ProjectName = project != null ? project.Name : "";
                            //var apartment = await db.Department.Where(e => e.DepartmentId == user.DepartmentId).FirstOrDefaultAsync();
                            //var apartment = await _de.FindByProjectIdAsync(user.ProjectId, cancellationToken);
                            //user.DepartmentName = apartment != null ? apartment.Name : "";
                            //var position = await db.Position.Where(e => e.PositionId == user.PositionId).FirstOrDefaultAsync();
                            var position = await _positionRepo.GetByKeyAsync((int)user.PositionId);
                            user.PositionName = position != null ? position.Name : "";
                        }
                    }
                    else
                    {
                        user.DepartmentId = item.DepartmentId != null ? item.DepartmentId : 0;
                        //user.ProjectId = item.ProjectId;
                        user.PositionId = item.PositionId != null ? item.PositionId : 0;
                        user.FullName = item.FullName;
                        user.CardId = item.CardId;
                        user.Email = (item.Email != null && item.Email != "") ? item.Email : user.Email;
                        user.Phone = item.Phone;
                        user.Address = item.Address;
                        user.Avata = item.Avata;
                        //user.Birthday = item.Birthday;
                        //var project = await db.Project.Where(e => e.ProjectId == user.ProjectId).FirstOrDefaultAsync();
                        //var project = await _projectRepo.FindByProjectIdAsync(user.ProjectId, cancellationToken);
                        //user.ProjectName = project != null ? project.Name : "";
                        //var apartment = await db.Department.Where(e => e.DepartmentId == user.DepartmentId).FirstOrDefaultAsync();
                        //user.DepartmentName = apartment != null ? apartment.Name : "";
                        //var position = await db.Position.Where(e => e.PositionId == user.PositionId).FirstOrDefaultAsync();
                        var position = await _positionRepo.GetByKeyAsync((int)user.PositionId);
                        user.PositionName = position != null ? position.Name : "";
                    }
                    var listRole = await _userRoleRepo.GetListRoleAsync(item.Id, cancellationToken);
                    user.listRole = listRole.Select(e => new RoleDT
                    {
                        RoleId = e.RoleId,
                        RoleName = e.RoleName
                    }).ToList();
                    //user.listRole = await db.UserRole.Where(e => e.UserId == item.UserId
                    //&& e.Status != (int)Const.Status.DELETED).Select(e => new RoleDT
                    //{
                    //    RoleId = e.RoleId,
                    //    RoleName = db.Role.Where(r => r.RoleId == e.RoleId).FirstOrDefault().Name,
                    //}).ToListAsync();
                    var listFunction = await _funcRoleRepo.GetListFunctionRoleAsync(item.Id, (int)AppEnum.TypeFunction.FUNCTION_USER, cancellationToken);
                    user.listFunction = listFunction.Select(e => new FunctionRoleDT
                    {
                        FunctionId = e.FunctionId,
                        ActiveKey = e.ActiveKey
                    }).ToList();
                    //user.listFunction = await db.FunctionRole.Where(e => e.TargetId == item.UserId
                    //    && e.Type == (int)Const.TypeFunction.FUNCTION_USER
                    //    && e.Status != (int)Const.Status.DELETED).Select(e => new FunctionRoleDT
                    //    {
                    //        FunctionId = e.FunctionId,
                    //        ActiveKey = e.ActiveKey
                    //    }).ToListAsync();

                    //user.listProject = await db.UserMapping.Where(e => e.UserId == item.UserId
                    //&& e.TargetType == (int)Const.TypeUserMap.USER_PROJECT
                    //&& e.Status != (int)Const.Status.DELETED).Select(e => new ListProject
                    //{
                    //    ProjectId = e.TargetId != null ? (int)e.TargetId : 0,
                    //    Name = db.Project.Where(r => r.ProjectId == e.TargetId).FirstOrDefault() != null ? db.Project.Where(r => r.ProjectId == e.TargetId).FirstOrDefault().Name : "",
                    //    Check = true,
                    //}).ToListAsync();

                    //user.listTower = await db.UserMapping.Where(e => e.UserId == item.UserId
                    //&& e.TargetType == (int)Const.TypeUserMap.USER_TOWER
                    //&& e.Status != (int)Const.Status.DELETED).Select(e => new ListTower
                    //{
                    //    TowerId = e.TargetId != null ? (int)e.TargetId : 0,
                    //    Name = db.Tower.Where(r => r.TowerId == e.TargetId).FirstOrDefault() != null ? db.Tower.Where(r => r.TowerId == e.TargetId).FirstOrDefault().Name : "",
                    //    Check = true,
                    //}).ToListAsync();

                    listUser.Add(user);

                }
                ResUserRoleLists resUserRoleLists = new ResUserRoleLists();
                resUserRoleLists.Results = listUser;
                resUserRoleLists.RowCount = entities.RowCount;
                return resUserRoleLists;
            }
        }
    }
}
