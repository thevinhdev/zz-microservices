using AutoMapper;
using IOIT.Identity.Application.Common.Interfaces.KeyMapCodeOption;
using IOIT.Identity.Application.UserRoles.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.UserRoles.Queries
{
    public class GetEmployeeByUserMapIdQuery : IRequest<UserReceiveNotification>
    {
        public long UserMapId { get; set; }

        public class Handler : IRequestHandler<GetEmployeeByUserMapIdQuery, UserReceiveNotification>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IUserRoleAsyncRepository _userRoleRepo;
            private readonly IRoleAsyncRepository _roleRepo;
            private readonly IEmployeeAsyncRepository _employeeRepo;
            private readonly IAsyncRepository<Position> _positionRepo;
            private readonly IOptionsSnapshot<KeyMapCodeOption> _optionsCode;
            private IHttpClientFactory _factory;
            private readonly IEmployeeMapAsyncRepository _employeeMapRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo,
                IUserRoleAsyncRepository userRoleRepo,
                IRoleAsyncRepository roleRepo,
                IEmployeeAsyncRepository employeeRepo,
                IAsyncRepository<Position> positionRepo,
                IOptionsSnapshot<KeyMapCodeOption> optionsCode,
                IHttpClientFactory factory,
                IEmployeeMapAsyncRepository employeeMapRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _userRoleRepo = userRoleRepo;
                _roleRepo = roleRepo;
                _employeeRepo = employeeRepo;
                _positionRepo = positionRepo;
                _optionsCode = optionsCode;
                _factory = factory;
                _employeeMapRepository = employeeMapRepository;
            }

            public async Task<UserReceiveNotification> Handle(GetEmployeeByUserMapIdQuery request, CancellationToken cancellationToken)
            {
                UserReceiveNotification res = (from u in _userRepo.All().Where(u => u.Status != AppEnum.EntityStatus.DELETED && u.UserMapId == request.UserMapId && (u.Type == (int)AppEnum.TypeUser.MANAGEMENT || u.Type == (int)AppEnum.TypeUser.TECHNICIANS))
                                                     select new UserReceiveNotification
                                                     {
                                                         UserId = u.Id,
                                                         FullName = u.FullName,
                                                         Phone = u.Phone,
                                                         Avata = u.Avata
                                                     }).FirstOrDefault();

                return res;
            }
        }
    }
}
