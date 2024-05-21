using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Functions.Commands.Update
{
    public class UpdateFunctionCommand : IRequest<Function>
    {
        public int Id { get; set; }
        public int FunctionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int FunctionParentId { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public int? Location { get; set; }
        public string Icon { get; set; }
        public bool? IsParamRoute { get; set; }
        public long? UserId { get; set; }

        public class Validation : AbstractValidator<UpdateFunctionCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateFunctionCommand, Function>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IFunctionAsyncRepository _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IFunctionAsyncRepository entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<Function> Handle(UpdateFunctionCommand request, CancellationToken cancellationToken)
            {
                if (request.Code == null || request.Code == "")
                {
                    throw new CommonException("Vui lòng nhập mã chức năng!", 211, ApiConstants.ErrorCode.ERROR_FUNCTION_CODE_EMPTY);
                }
                var checkItemExist = await _entityRepository.FindByCodeAsync(request.Code, request.Id, cancellationToken);
                if (checkItemExist != null)
                {
                    //def.meta = new Meta(211, "Mã chức năng đã tồn tại!");
                    //return Ok(def);
                    throw new CommonException("Mã chức năng đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_FUNCTION_EXISTED);
                }
                var data = await _entityRepository.GetByKeyAsync(request.Id);

                if (data == null)
                {
                    throw new NotFoundException("The Function does not exist.", Constants.StatusCodeResApi.Error404);
                }

                var entity = _mapper.Map<Function>(request);
                entity.CreatedAt = data.CreatedAt;
                entity.UpdatedAt = DateTime.Now;
                entity.CreatedById = data.CreatedById;
                entity.UpdatedById = request.UserId;
                entity.Status = data.Status;

                _entityRepository.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return entity;
            }
        }
    }
}
