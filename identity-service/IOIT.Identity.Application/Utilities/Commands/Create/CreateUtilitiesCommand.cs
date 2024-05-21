using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.UserRoles.Commands.Create;
using IOIT.Identity.Application.UserRoles.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Utilities.ViewModels;
using IOIT.Identity.Domain.Enum;

namespace IOIT.Identity.Application.Utilities.Commands.Create
{
    public class CreateUtilitiesCommand : IRequest<ResGetUtilitiesById>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public DomainEnum.TypeUtilities Type { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
        public long? UserId { get; set; }
        public List<int> ListProjectId { get; set; }

        public class Validation : AbstractValidator<CreateUtilitiesCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("Có lỗi sảy ra vui lòng đăng nhập lại").GreaterThan(0).WithMessage("Có lỗi sảy ra vui lòng đăng nhập lại");
                RuleFor(x => x.Name).NotEmpty().WithMessage("Vui lòng nhập tên tiện ích");
                RuleFor(x => x.Code).NotEmpty().WithMessage("Vui lòng nhập mã tiện ích");
                RuleFor(x => x.Type).NotEmpty().WithMessage("Vui lòng chọn loại tiện ích");
            }
        }

        public class Handler : IRequestHandler<CreateUtilitiesCommand, ResGetUtilitiesById>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUtilitiesRepository _utilitiesRepository;
            private readonly IProjectUtilitiesRepository _projectUtilitiesRepository;
            private readonly IEmployeeAsyncRepository _empRepo;
            private readonly IResidentAsyncRepository _residentRepo;
            private readonly IRoleAsyncRepository _roleRepo;
            private readonly IFunctionRoleAsyncRepository _funcRoleRepo;
            private readonly IUserRoleAsyncRepository _userRoleRepo;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUtilitiesRepository utilitiesRepository,
                IProjectUtilitiesRepository projectUtilitiesRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _utilitiesRepository = utilitiesRepository;
                _projectUtilitiesRepository = projectUtilitiesRepository;
            }

            public async Task<ResGetUtilitiesById> Handle(CreateUtilitiesCommand request, CancellationToken cancellationToken)
            {
                var isExist =
                    await _utilitiesRepository.getUtilitiesByNameOrCode(request.Name, request.Code, cancellationToken);

                if (isExist != null)
                {
                    throw new CommonException(
                        "Tên hoặc mã tiện ích đã tồn tại! Vui lòng kiểm tra lại.", ApiConstants.StatusCode.Error400, ApiConstants.ErrorCode.ERROR_UTILITIES_NAME_EXISTED);
                }

                var entity = _mapper.Map<Domain.Entities.Utilities>(request);

                await _utilitiesRepository.AddAsync(entity);
                await _unitOfWork.CommitChangesAsync();

                if (request.ListProjectId.Count > 0)
                {
                    var entityProjectUtilities = new List<Domain.Entities.ProjectUtilities>();

                    foreach (var projectId in request.ListProjectId)
                    {
                        entityProjectUtilities.Add(new Domain.Entities.ProjectUtilities()
                        {
                            ProjectId = projectId,
                            UtilitiesId = entity.Id,
                            CreatedById = request.UserId,
                            UpdatedById = request.UserId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        });
                    }

                    _projectUtilitiesRepository.AddRange(entityProjectUtilities);
                    await _unitOfWork.CommitChangesAsync();
                }

                return _mapper.Map<ResGetUtilitiesById>(entity);
            }
        }
    }
}
