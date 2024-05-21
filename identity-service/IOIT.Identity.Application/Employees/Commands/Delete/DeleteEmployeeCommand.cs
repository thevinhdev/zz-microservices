using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Employees.Commands.Delete
{
    public class DeleteEmployeeCommand : IRequest<Employee>
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public class Validation : AbstractValidator<DeleteEmployeeCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
            }

            
        }
        public class Handler : IRequestHandler<DeleteEmployeeCommand, Employee>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Employee> _entityRepository;
            private readonly IUserAsyncRepository _userRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Employee> entityRepository,
                IUserAsyncRepository userRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _userRepo = userRepo;
            }

            public async Task<Employee> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
            {
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);

                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new NotFoundException("The Employee does not exist.", Constants.StatusCodeResApi.Error404);
                }

                if (data.Status == EntityStatus.DELETED)
                {
                    throw new NotFoundException("The Employee does not exist.", Constants.StatusCodeResApi.Error404);
                }

                data.UpdatedAt = DateTime.Now;
                data.UpdatedById = request.UserId;
                data.Status = EntityStatus.DELETED;
                _entityRepository.Update(data);
                
                //check xem nhân viên có tài khoản ko, nếu có thì xóa luôn tài khoản user
                //var user = db.User.Where(e => e.UserMapId == data.EmployeeId).FirstOrDefault();
                var user = await _userRepo.FindByUserMapIdAsync(data.Id, 2, cancellationToken);
                if (user != null)
                {
                    user.UpdatedAt = DateTime.Now;
                    user.UpdatedById = request.UserId;
                    user.Status = AppEnum.EntityStatus.DELETED;
                    _userRepo.Update(user);
                    //db.User.Update(user);
                }
                await _unitOfWork.CommitChangesAsync();
                _unitOfWork.CommitTransaction();
                return data;
            }
        }
    }
}
