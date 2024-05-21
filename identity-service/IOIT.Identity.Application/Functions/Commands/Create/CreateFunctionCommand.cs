using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Functions.Commands.Create
{
    public class CreateFunctionCommand : IRequest<Function>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int FunctionParentId { get; set; }
        public string Url { get; set; }
        public string Note { get; set; }
        public int? Location { get; set; }
        public string Icon { get; set; }
        public bool? IsParamRoute { get; set; }
        public long? UserId { get; set; }

        public class Validation : AbstractValidator<CreateFunctionCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name not empty");
                RuleFor(x => x.Code).NotEmpty().WithMessage("Code not empty");
            }
        }

        public class Handler : IRequestHandler<CreateFunctionCommand, Function>
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

            public async Task<Function> Handle(CreateFunctionCommand request, CancellationToken cancellationToken)
            {
                if (request.Code == null || request.Code == "")
                {
                    throw new CommonException("Mã không có dữ liệu!", 211, ApiConstants.ErrorCode.ERROR_FUNCTION_CODE_EMPTY);
                }
                //Function checkItemExist = db.Function.Where(f => f.Code == data.Code && f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                var checkItemExist = await _entityRepository.FindByCodeAsync(request.Code, 0, cancellationToken);
                if (checkItemExist != null)
                {
                    //def.meta = new Meta(211, "Mã chức năng đã tồn tại!");
                    //return Ok(def);
                    throw new CommonException("Mã không có dữ liệu!", 211, ApiConstants.ErrorCode.ERROR_FUNCTION_EXISTED);
                }

                var entity = _mapper.Map<Function>(request);
                entity.FunctionParentId = entity.FunctionParentId != null ? entity.FunctionParentId : 0;
                entity.CreatedAt = DateTime.Now;
                entity.UpdatedAt = DateTime.Now;
                entity.CreatedById = request.UserId;
                entity.UpdatedById = request.UserId;
                entity.Status = AppEnum.EntityStatus.NORMAL;
                await _entityRepository.AddAsync(entity);
                await _unitOfWork.CommitChangesAsync();

                return entity;
            }
        }
    }
}
