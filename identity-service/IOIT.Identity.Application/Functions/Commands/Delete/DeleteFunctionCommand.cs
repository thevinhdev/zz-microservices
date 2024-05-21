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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Functions.Commands.Delete
{
    public class DeleteFunctionCommand : IRequest<Function>
    {
        public int Id { get; set; }
        public long? UserId { get; set; }

        public class Validation : AbstractValidator<DeleteFunctionCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Id).NotEmpty().WithMessage("Id not empty").GreaterThan(0);
            }

            
        }
        public class Handler : IRequestHandler<DeleteFunctionCommand, Function>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IFunctionAsyncRepository _entityRepository;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IFunctionAsyncRepository entityRepository,
                IFunctionRoleAsyncRepository funcRoleRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _funcRoleRepo = funcRoleRepo;
            }

            public async Task<Function> Handle(DeleteFunctionCommand request, CancellationToken cancellationToken)
            {
                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new NotFoundException("The Function does not exist.", Constants.StatusCodeResApi.Error404);
                }

                if (data.Status == EntityStatus.DELETED)
                {
                    throw new NotFoundException("The Function does not exist.", Constants.StatusCodeResApi.Error404);
                }

                data.UpdatedAt = DateTime.Now;
                data.UpdatedById = request.UserId;
                data.Status = EntityStatus.DELETED;

                var entity = _mapper.Map<Function>(data);
                _entityRepository.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                //Xóa function role
                //var fr = db.FunctionRole.Where(e => e.FunctionId == function.FunctionId).ToList();
                var fr = await _funcRoleRepo.GetListFunctionRoleByIdAsync(request.Id, cancellationToken);
                fr.ForEach(f => f.Status = AppEnum.EntityStatus.DELETED);
                _funcRoleRepo.UpdateRange(fr);

                //var listChild = db.Function.Where(f => f.FunctionParentId == function.FunctionId && f.Status != (int)Const.Status.DELETED).ToList();
                var listChild = await _entityRepository.GetListFunctionByParentIdAsync(request.Id, cancellationToken);
                listChild.ForEach(c => c.FunctionParentId = 0);
                _entityRepository.UpdateRange(listChild);

                return entity;
            }
        }
    }
}
