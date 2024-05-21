using AutoMapper;
using IOIT.Identity.Application.Common.Interfaces.KeyMapCodeOption;
using IOIT.Identity.Application.UserRoles.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.UserRoles.Queries
{
    public class GetEmpByRoleQuery : IRequest<List<ResGetUserByFunction>>
    {
        public int? ProjectId { get; set; }
        //public string RoleCodeKST { get; set; }
        //public string RoleCodeKT { get; set; }
        public int? UserId { get; set; }

        public class Handler : IRequestHandler<GetEmpByRoleQuery, List<ResGetUserByFunction>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IUserRoleAsyncRepository _userRoleRepo;
            private readonly IRoleAsyncRepository _roleRepo;
            private readonly IEmployeeAsyncRepository _employeeRepo;
            private readonly IAsyncRepository<Position> _positionRepo;
            private readonly IOptionsSnapshot<KeyMapCodeOption> _optionsCode;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo,
                IUserRoleAsyncRepository userRoleRepo,
                IRoleAsyncRepository roleRepo,
                IEmployeeAsyncRepository employeeRepo,
                IAsyncRepository<Position> positionRepo,
                IOptionsSnapshot<KeyMapCodeOption> optionsCode)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _userRoleRepo = userRoleRepo;
                _roleRepo = roleRepo;
                _employeeRepo = employeeRepo;
                _positionRepo = positionRepo;
                _optionsCode = optionsCode;
            }

            public async Task<List<ResGetUserByFunction>> Handle(GetEmpByRoleQuery request, CancellationToken cancellationToken)
            {
                var res = (from u in _userRepo.All().Where(u => (u.Type == (int)AppEnum.TypeUser.ADMINISTRATOR || u.Type == (int)AppEnum.TypeUser.MANAGEMENT || u.Type == (int)AppEnum.TypeUser.TECHNICIANS)
                           && u.ProjectId == request.ProjectId && u.Status == AppEnum.EntityStatus.NORMAL && u.Id != request.UserId)
                           join emp in _employeeRepo.All().Where(r => r.Status == AppEnum.EntityStatus.NORMAL) on u.UserMapId equals emp.Id
                           join pos in _positionRepo.All().Where(r => r.Status == AppEnum.EntityStatus.NORMAL) on emp.PositionId equals pos.Id
                           join ur in _userRoleRepo.All().Where(r => r.Status == AppEnum.EntityStatus.NORMAL) on u.Id equals ur.UserId
                           join r in _roleRepo.All().Where(f => f.Code == _optionsCode.Value.CodeRoleKST && f.Status == AppEnum.EntityStatus.NORMAL) on ur.RoleId equals r.Id
                           select new ResGetUserByFunction
                           {
                               UserId = u.Id,
                               FullName = u.FullName,
                               EmployeeId = emp.Id,
                               Code = emp.Code,
                               Phone = emp.Phone,
                               PositionId = emp.PositionId,
                               PositionName = pos.Name,
                               IsMain = emp.IsMain,
                               Note = emp.Note,
                           }).Distinct().ToList();

                return res;

            }
        }
    }
}
