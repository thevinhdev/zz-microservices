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
    public class GetUserByFuctionQuery : IRequest<List<ResGetUserByFunction>>
    {
        public int? ProjectId { get; set; }
        public string FunctionCode { get; set; }
        public int? UserId { get; set; }

        public class Handler : IRequestHandler<GetUserByFuctionQuery, List<ResGetUserByFunction>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IUserRoleAsyncRepository _userRoleRepo;
            private readonly IAsyncLongRepository<Resident> _residentRepo;
            private readonly IFunctionAsyncRepository _funcRepo;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;
            private readonly IAsyncRepository<Employee> _employeeRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo,
                IUserRoleAsyncRepository userRoleRepo,
                IAsyncLongRepository<Resident> residentRepo,
                IFunctionAsyncRepository funcRepo,
                IFunctionRoleAsyncRepository funcRoleRepo,
                IAsyncRepository<Employee> employeeRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _userRoleRepo = userRoleRepo;
                _residentRepo = residentRepo;
                _funcRepo = funcRepo;
                _funcRoleRepo = funcRoleRepo;
                _employeeRepo = employeeRepo;
            }

            public async Task<List<ResGetUserByFunction>> Handle(GetUserByFuctionQuery request, CancellationToken cancellationToken)
            {
                var res = (from u in _userRepo.All().Where(u => (u.Type == (int)AppEnum.TypeUser.ADMINISTRATOR || u.Type == (int)AppEnum.TypeUser.MANAGEMENT || u.Type == (int)AppEnum.TypeUser.TECHNICIANS) && u.Status == AppEnum.EntityStatus.NORMAL)
                           join r in _userRoleRepo.All().Where(r => r.Status == AppEnum.EntityStatus.NORMAL) on u.Id equals r.UserId
                           join fr in _funcRoleRepo.All().Where(fr => fr.Status == AppEnum.EntityStatus.NORMAL && fr.ActiveKey.Substring(0, 1) == "1" && fr.Type == (int)AppEnum.TypeFunction.FUNCTION_ROLE) on r.RoleId equals fr.TargetId
                           join f in _funcRepo.All().Where(f => f.Code == request.FunctionCode && f.Status == AppEnum.EntityStatus.NORMAL) on fr.FunctionId equals f.Id
                           join e in _employeeRepo.All().Where(e => e.Status == AppEnum.EntityStatus.NORMAL && e.ProjectId == request.ProjectId) on u.UserMapId equals e.Id
                           select new ResGetUserByFunction
                           {
                               UserId = u.Id,
                               FullName = u.FullName
                           }).Distinct().ToList();

                return res;

            }
        }
    }
}
