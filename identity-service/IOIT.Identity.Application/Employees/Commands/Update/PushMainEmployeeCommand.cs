using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.UserRoles.Queries;
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
    public class PushMainEmployeeCommand : IRequest<List<ResGetUserByFunction>>
    {
        public List<ResGetUserByFunction> data { get; set; }
        public long UserId { get; set; }

        public class Validation : AbstractValidator<PushMainEmployeeCommand>
        {
            public Validation()
            {
                RuleFor(x => x.data).NotEmpty().WithMessage("data not empty");
            }
        }
        public class Handler : IRequestHandler<PushMainEmployeeCommand, List<ResGetUserByFunction>>
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

            public async Task<List<ResGetUserByFunction>> Handle(PushMainEmployeeCommand request, CancellationToken cancellationToken)
            {
                //List<Employee> listData = new List<Employee>();
                foreach (var item in request.data)
                {
                    //Employee employee = await db.Employee.Where(e => e.EmployeeId == requestdata[i] && e.Status != (int)Const.Status.DELETED).FirstOrDefaultAsync();
                    Employee employee = await _entityRepository.GetByKeyAsync((int)item.EmployeeId);
                    if (employee == null)
                    {
                        continue;
                    }

                    employee.IsMain = item.IsMain;
                    employee.UpdatedById = request.UserId;
                    employee.UpdatedAt = DateTime.Now;
                    _entityRepository.Update(employee);
                    //listData.Add(employee);
                }

                await _unitOfWork.CommitChangesAsync();

                return request.data;
            }
        }
    }
}
