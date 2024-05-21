using AutoMapper;
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
    public class GetUserByRoleQuery : IRequest<List<ResGetUserByFunction>>
    {
        public int? ProjectId { get; set; }
        public string RoleCode { get; set; }
        public int TowerId { get; set; }

        public class Handler : IRequestHandler<GetUserByRoleQuery, List<ResGetUserByFunction>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IPositionAsyncRepository _positionRepo;
            private readonly IUserRoleAsyncRepository _userRoleRepo;
            private readonly IRoleAsyncRepository _roleRepo;
            private readonly IEmployeeAsyncRepository _employeeRepo;
            private readonly IEmployeeMapAsyncRepository _empMapRepo;
            private readonly IUserMappingAsyncRepository _userMapRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo,
                IUserRoleAsyncRepository userRoleRepo,
                IRoleAsyncRepository roleRepo,
                IEmployeeAsyncRepository employeeRepo,
                IPositionAsyncRepository positionRepo,
                IUserMappingAsyncRepository userMapRepo,
                IEmployeeMapAsyncRepository empMapRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _userRoleRepo = userRoleRepo;
                _roleRepo = roleRepo;
                _employeeRepo = employeeRepo;
                _positionRepo = positionRepo;
                _userMapRepo = userMapRepo;
                _empMapRepo = empMapRepo;
            }

            public async Task<List<ResGetUserByFunction>> Handle(GetUserByRoleQuery request, CancellationToken cancellationToken)
            {
                var res = (from u in _userRepo.All().Where(u => (u.Type == (int)AppEnum.TypeUser.ADMINISTRATOR || u.Type == (int)AppEnum.TypeUser.MANAGEMENT || u.Type == (int)AppEnum.TypeUser.TECHNICIANS) 
                            && u.Status == AppEnum.EntityStatus.NORMAL)
                           join ur in _userRoleRepo.All().Where(r => r.Status == AppEnum.EntityStatus.NORMAL) on u.Id equals ur.UserId
                           join r in _roleRepo.All().Where(f => f.Code.Trim() == request.RoleCode.Trim() && f.Status == AppEnum.EntityStatus.NORMAL) on ur.RoleId equals r.Id
                           join emp in _employeeRepo.All().Where(r => r.Status == AppEnum.EntityStatus.NORMAL && r.ProjectId == request.ProjectId) on u.UserMapId equals emp.Id
                           join empMap in _empMapRepo.All().Where(r => r.Status == AppEnum.EntityStatus.NORMAL && (request.TowerId == 0 || r.TowerId == request.TowerId)) on emp.Id equals empMap.EmployeeId
                           join pos in _positionRepo.All().Where(r => r.Status == AppEnum.EntityStatus.NORMAL) on emp.PositionId equals pos.Id
                           select new ResGetUserByFunction
                           {
                               UserId = u.Id,
                               FullName = u.FullName,
                               Phone = u.Phone,
                               PositionId = u.PositionId,
                               PositionName = pos.Name,
                               Code = emp.Code,
                               EmployeeId = emp.Id,
                           }).Distinct().ToList();

                return res;

            }
        }
    }
}
