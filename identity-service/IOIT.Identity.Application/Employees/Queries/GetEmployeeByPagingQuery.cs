using AutoMapper;
using IOIT.Identity.Application.Employees.ViewModels;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static IOIT.Shared.Commons.Enum.AppEnum;
using EmployeeMap = IOIT.Identity.Domain.Entities.EmployeeMap;

namespace IOIT.Identity.Application.Employees.Queries
{
    public class GetEmployeeByPagingQuery : FilterPagination, IRequest<DefaultResponse>
    {
        public int ProjectId { get; set; }
        public class Handler : IRequestHandler<GetEmployeeByPagingQuery, DefaultResponse>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Employee> _entityRepository;
            private readonly ITowerAsyncRepository _towerRepo;
            private readonly IAsyncRepository<Position> _positionRepo;
            private readonly IAsyncRepository<EmployeeMap> _emRepo;
            private readonly IDepartmentAsyncRepository _departmentRepo;
            //private readonly IEmployeeMapAsyncRepository _emRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Employee> entityRepository,
                ITowerAsyncRepository towerRepo,
                IAsyncRepository<Position> positionRepo,
                IAsyncRepository<EmployeeMap> emRepo,
                IDepartmentAsyncRepository departmentRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _towerRepo = towerRepo;
                _positionRepo = positionRepo;
                _emRepo = emRepo;
                _departmentRepo = departmentRepo;
            }

            public async Task<DefaultResponse> Handle(GetEmployeeByPagingQuery request, CancellationToken cancellationToken)
            {

                DefaultResponse entities = new DefaultResponse();
                IQueryable<Employee> data = Enumerable.Empty<Employee>().AsQueryable();
                //if (roleMax != 1)
                //{
                data = _entityRepository.All().Where(c => (c.ProjectId == request.ProjectId || request.ProjectId == -1) && c.Status != EntityStatus.DELETED);
                //}
                //else
                //{
                //    data = db.Employee.Where(c => c.Status != (int)Const.Status.DELETED);
                //}

                if (request.query != null)
                {
                    request.query = HttpUtility.UrlDecode(request.query);
                }

                data = data.Where(request.query);
                entities.Metadata = data.Count();

                if (request.page_size > 0)
                {
                    if (request.order_by != null)
                    {
                        data = data.OrderBy(request.order_by).Skip((request.page - 1) * request.page_size).Take(request.page_size);
                    }
                    else
                    {
                        data = data.OrderBy("Id desc").Skip((request.page - 1) * request.page_size).Take(request.page_size);
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
                        data = data.OrderBy("Id desc");
                    }
                }

                if (request.select != null && request.select != "")
                {
                    request.select = "new(" + request.select + ")";
                    request.select = HttpUtility.UrlDecode(request.select);
                    entities.Data = data.Select(request.select).ToDynamicList();
                }
                else
                {
                    //def.data = data.ToList();
                    //    entities.Data = data.Select(e => new {
                    //        EmployeeId = e.Id,
                    //        e.Code,
                    //        e.FullName,
                    //        e.Avata,
                    //        e.ProjectId,
                    //        e.DepartmentId,
                    //        e.TypeEmployee,
                    //        DepartmentName = _departmentRepo.All().Where(d => d.DepartmentId == e.DepartmentId).FirstOrDefault() != null ? db.Department.Where(d => d.DepartmentId == e.DepartmentId).FirstOrDefault().Name : "",
                    //        e.PositionId,
                    //        PositionName = db.Position.Where(p => p.PositionId == e.PositionId).FirstOrDefault() != null ? db.Position.Where(p => p.PositionId == e.PositionId).FirstOrDefault().Name : "",
                    //        e.Birthday,
                    //        e.CardId,
                    //        e.Phone,
                    //        e.Email,
                    //        e.Address,
                    //        e.Note,
                    //        e.CreatedAt,
                    //        e.UpdatedAt,
                    //        UserId = e.UpdatedById,
                    //        e.Status,
                    //        employeeMaps = db.Tower.Where(t => t.ProjectId == e.ProjectId && t.Status != (int)Const.Status.DELETED).Select(t => new {
                    //            t.TowerId,
                    //            t.Name,
                    //            Selected = db.EmployeeMap.Where(em => em.EmployeeId == e.EmployeeId && em.TowerId == t.TowerId && em.Status != (int)Const.Status.DELETED).FirstOrDefault() != null ? true : false
                    //        }).ToList()
                    //    }).ToList();

                    //return Ok(def);
                    //var spec = new EmployeeFilterWithPagingSpec(request);

                    //var entities = await _entityRepository.PaggingAsync(spec);
                    List<ResEmployee> listData = new List<ResEmployee>();
                    foreach (var item in data.ToList())
                    {
                        ResEmployee resEmployee = new ResEmployee();
                        resEmployee.EmployeeId = item.Id;
                        resEmployee.Code = item.Code;
                        resEmployee.FullName = item.FullName;
                        resEmployee.Avata = item.Avata;
                        resEmployee.ProjectId = item.ProjectId;
                        resEmployee.DepartmentId = item.DepartmentId != null ? item.DepartmentId : 0;
                        resEmployee.TypeEmployee = item.TypeEmployee;
                        //resEmployee.DepartmentName = db.Department.Where(d => d.DepartmentId == e.DepartmentId).FirstOrDefault() != null ? db.Department.Where(d => d.DepartmentId == e.DepartmentId).FirstOrDefault().Name : "",
                        var department = await _departmentRepo.FindByDepartmentIdAsync((int)resEmployee.DepartmentId, cancellationToken);
                        resEmployee.DepartmentName = department != null ? department.Name : "";
                        resEmployee.PositionId = item.PositionId != null ? item.PositionId : 0;
                        //resEmployee.PositionName = db.Position.Where(p => p.PositionId == e.PositionId).FirstOrDefault() != null ? db.Position.Where(p => p.PositionId == e.PositionId).FirstOrDefault().Name : "",
                        var position = await _positionRepo.GetByKeyAsync((int)resEmployee.PositionId);
                        resEmployee.PositionName = position != null ? position.Name : "";
                        resEmployee.Birthday = item.Birthday;
                        resEmployee.CardId = item.CardId;
                        resEmployee.Phone = item.Phone;
                        resEmployee.Email = item.Email;
                        resEmployee.Address = item.Address;
                        resEmployee.Note = item.Note;
                        resEmployee.CreatedAt = item.CreatedAt;
                        resEmployee.UpdatedAt = item.UpdatedAt;
                        resEmployee.UserId = item.CreatedById;
                        resEmployee.Status = (int)item.Status;

                        resEmployee.employeeMaps = _towerRepo.All().Where(t => t.ProjectId == item.ProjectId && t.Status != AppEnum.EntityStatus.DELETED).Select(t => new ResEmployeeMap
                        {
                            TowerId = t.TowerId,
                            Name = t.Name,
                            Selected = _emRepo.All().Where(em => em.EmployeeId == item.Id && em.TowerId == t.TowerId && em.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault() != null ? true : false
                        }).ToList();
                        listData.Add(resEmployee);
                    }

                    entities.Data = listData;
                }
                //ResEmployeeLists resEmployeeLists = new ResEmployeeLists();
                //resEmployeeLists.Results = listEmps;
                //resEmployeeLists.RowCount = entities.RowCount;
                return entities;

            }
        }
    }
}
