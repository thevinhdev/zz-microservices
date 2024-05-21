using AutoMapper;
using FluentValidation;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.FunctionRoles.Commands.Delete
{
    public class DeletesEmployeeCommand : IRequest<List<Employee>>
    {
        public int[] data { get; set; }
        public long UserId { get; set; }

        public class Validation : AbstractValidator<DeletesEmployeeCommand>
        {
            public Validation()
            {
                RuleFor(x => x.data).NotEmpty().WithMessage("data not empty");
            }
        }
        public class Handler : IRequestHandler<DeletesEmployeeCommand, List<Employee>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmployeeAsyncRepository _entityRepository;
            private readonly IUserAsyncRepository _userRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IEmployeeAsyncRepository entityRepository,
                IUserAsyncRepository userRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _userRepo = userRepo;
            }

            public async Task<List<Employee>> Handle(DeletesEmployeeCommand request, CancellationToken cancellationToken)
            {
                List<Employee> listData = new List<Employee>();
                for (int i = 0; i < request.data.Count(); i++)
                {
                    //Employee employee = await db.Employee.Where(e => e.EmployeeId == requestdata[i] && e.Status != (int)Const.Status.DELETED).FirstOrDefaultAsync();
                    Employee employee = await _entityRepository.GetByKeyAsync(request.data[i]);
                    if (employee == null)
                    {
                        continue;
                    }

                    employee.UpdatedById = request.UserId;
                    employee.UpdatedAt = DateTime.Now;
                    employee.Status = AppEnum.EntityStatus.DELETED;
                    //db.Employee.Update(employee);
                    _entityRepository.Update(employee);
                    ////create action
                    //IOITResident.Models.EF.Action action = new IOITResident.Models.EF.Action();
                    //action.ActionId = Guid.NewGuid();
                    //action.ActionName = "Xóa nhân viên" + employee.FullName;
                    //action.ActionType = "DELETE";
                    //action.TargetId = employee.EmployeeId.ToString();
                    //action.TargetType = "Employee";
                    //action.Logs = JsonConvert.SerializeObject(employee);
                    //action.Time = 0;
                    //action.Type = (int)Const.TypeAction.ACTION;
                    //action.CreatedAt = DateTime.Now;
                    //action.UserPushId = userId;
                    //action.UserId = userId;
                    //action.Status = (int)Const.Status.NORMAL;
                    //db.Action.Add(action);
                    listData.Add(employee);
                }

                await _unitOfWork.CommitChangesAsync();

                return listData;
            }
        }
    }
}
