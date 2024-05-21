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
    public class GetEmployeeReceiveNotificationRequireSupportQuery : IRequest<List<UserReceiveNotification>>
    {
        public string ApiGateway { get; set; }
        public string Token { get; set; }
        public int TowerId { get; set; }
        public int ProjectId { get; set; }
        public int TypeRequireId { get; set; }

        public class Handler : IRequestHandler<GetEmployeeReceiveNotificationRequireSupportQuery, List<UserReceiveNotification>>
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

            public async Task<List<UserReceiveNotification>> Handle(GetEmployeeReceiveNotificationRequireSupportQuery request, CancellationToken cancellationToken)
            {
                List<Shared.ViewModels.DepartmentMap> departmentMaps = new List<Shared.ViewModels.DepartmentMap>();
                try
                {
                    HttpClient client = _factory.CreateClient();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", request.Token);
                    var resDepartmentMap = await client.GetAsync($"{request.ApiGateway}/api/cms/department/GetAllDepartmentMaps");

                    if (resDepartmentMap.IsSuccessStatusCode)
                    {
                        var contentDepartmentMap = resDepartmentMap.Content.ReadAsStringAsync().Result;
                        var json = JsonConvert.DeserializeObject<dynamic>(contentDepartmentMap);

                        if (json["data"] != null)
                        {
                            departmentMaps = json["data"].ToObject<List<Shared.ViewModels.DepartmentMap>>();
                        }
                    }
                }
                catch { }

                //List<int> ky_thuat = (from em in _employeeMapRepository.All().Where(em => em.TowerId == request.TowerId && em.Status != AppEnum.EntityStatus.DELETED)
                //           join e in _employeeRepo.All().Where(e => e.ProjectId == request.ProjectId && e.TypeEmployee == (int)AppEnum.TypeDepartment.GROUP && e.Status != AppEnum.EntityStatus.DELETED) on em.EmployeeId equals e.Id
                //           join dm in departmentMaps.Where(dm => dm.RequireTypeId == request.TypeRequireId) on e.DepartmentId equals dm.DepartmentId
                //           select e.Id).ToList();

                List<int> ky_thuat = (from dm in departmentMaps.Where(dm => dm.RequireTypeId == request.TypeRequireId)
                                      join emp in _employeeRepo.All().Where(e => e.ProjectId == request.ProjectId && e.TypeEmployee == (int)AppEnum.TypeDepartment.GROUP && e.Status != AppEnum.EntityStatus.DELETED) on dm.DepartmentId equals emp.DepartmentId
                                      join empMap in _employeeMapRepository.All().Where(em => em.TowerId == request.TowerId && em.Status != AppEnum.EntityStatus.DELETED) on emp.Id equals empMap.EmployeeId
                                      select emp.Id).Distinct().ToList();

                List<int> ban_quan_ly = _employeeRepo.All().Where(e => e.TypeEmployee == (int)AppEnum.TypeDepartment.MANAGEMENT && e.ProjectId == request.ProjectId).Select(e => e.Id).ToList();

                List<int> ds_nhan_thong_bao = ky_thuat.Concat(ban_quan_ly).ToList();

                List<UserReceiveNotification> res = (from e in ds_nhan_thong_bao
                                                     join u in _userRepo.All().Where(u => u.Status != AppEnum.EntityStatus.DELETED && (u.Type == (int)AppEnum.TypeUser.MANAGEMENT || u.Type == (int)AppEnum.TypeUser.TECHNICIANS)) on e equals u.UserMapId
                                                     select new UserReceiveNotification
                                                     {
                                                         UserId = u.Id,
                                                         FullName = u.FullName,
                                                         Phone = u.Phone,
                                                         Avata = u.Avata
                                                     }).Distinct().ToList();

                return res;

            }
        }
    }
}
