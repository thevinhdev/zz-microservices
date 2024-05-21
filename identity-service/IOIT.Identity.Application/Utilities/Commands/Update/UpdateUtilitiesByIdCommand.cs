using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Utilities.ViewModels;
using IOIT.Identity.Domain.Enum;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Application.Utilities.Commands.Create;
using IOIT.Identity.Domain.Entities;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using MassTransit;

namespace IOIT.Identity.Application.Utilities.Commands.Update
{
    public class UpdateUtilitiesByIdCommand : IRequest<ResGetUtilitiesById>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DomainEnum.TypeUtilities Type { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
        public long? UserId { get; set; }
        public List<int> ListProjectId { get; set; }

        public class Validation : AbstractValidator<UpdateUtilitiesByIdCommand>
        {
            public Validation()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithMessage("Có lỗi sảy ra vui lòng thử lại");
                RuleFor(x => x.Name).NotEmpty().WithMessage("Vui lòng nhập tên tiện ích");
                RuleFor(x => x.Code).NotEmpty().WithMessage("Vui lòng nhập mã tiện ích");
                RuleFor(x => x.Type).NotEmpty().WithMessage("Vui lòng chọn loại tiện ích");
            }
        }

        public class Handler : IRequestHandler<UpdateUtilitiesByIdCommand, ResGetUtilitiesById>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUtilitiesRepository _utilitiesRepository;
            private readonly IProjectUtilitiesRepository _projectUtilitiesRepository;

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

            public async Task<ResGetUtilitiesById> Handle(UpdateUtilitiesByIdCommand request, CancellationToken cancellationToken)
            {
                var entity = await _utilitiesRepository.GetByKeyAsync(request.Id);

                if (entity == null)
                {
                    throw new CommonException(
                        "Dữ liệu không tồn tại.", ApiConstants.StatusCode.Error400, ApiConstants.ErrorCode.ERROR_UTILITIES_NOT_EXIST);
                }

                var isExistCode =
                    await _utilitiesRepository.checkExistUtilitiesByNameOrCode(request.Name, request.Code, request.Id, cancellationToken);

                if (isExistCode != null)
                {
                    throw new CommonException("Tên hoặc mã tiện ích đã tồn tại! Vui lòng kiểm tra lại.", ApiConstants.StatusCode.Error400, ApiConstants.ErrorCode.ERROR_UTILITIES_NAME_EXISTED);
                }

                entity.Name = request.Name;
                entity.Code = request.Code;
                entity.Type = request.Type;
                entity.Order = request.Order ?? 0;
                entity.Description = request.Description;
                entity.Icon = request.Icon;
                entity.Url = request.Url;
                entity.UpdatedAt = DateTime.Now;

                _utilitiesRepository.Update(entity);

                var listProjectUtilities = await _projectUtilitiesRepository.getByUtilitiesId(entity.Id);
                var currentDate = DateTime.Now;

                if (request.ListProjectId.Count > 0)
                {
                    var listProjectUtilitiesAdd = new List<Domain.Entities.ProjectUtilities>();
                    var listProjectUtilitiesDelete = listProjectUtilities;

                    foreach (var projectId in request.ListProjectId)
                    {
                        var isExistProject = listProjectUtilities.Find(s => s.ProjectId == projectId);

                        if (isExistProject == null)
                        {
                            listProjectUtilitiesAdd.Add(new Domain.Entities.ProjectUtilities()
                            {
                                ProjectId = projectId,
                                UtilitiesId = entity.Id,
                                CreatedAt = currentDate,
                                UpdatedAt = currentDate,
                                CreatedById = request.UserId,
                                UpdatedById = request.UserId
                            });
                        }
                        else
                        {
                            listProjectUtilitiesDelete.RemoveAll(r => r.ProjectId == projectId);
                        }
                    }

                    listProjectUtilitiesDelete.ForEach(s =>
                    {
                        s.Status = AppEnum.EntityStatus.DELETED;
                        s.UpdatedAt = currentDate;
                        s.IsActive = false;
                    });

                    _projectUtilitiesRepository.AddRange(listProjectUtilitiesAdd);
                    _projectUtilitiesRepository.UpdateRange(listProjectUtilitiesDelete);
                }
                else
                {
                    listProjectUtilities.ForEach(s =>
                    {
                        s.Status = AppEnum.EntityStatus.DELETED;
                        s.UpdatedAt = currentDate;
                        s.IsActive = false;
                    });

                    _projectUtilitiesRepository.UpdateRange(listProjectUtilities);
                }

                await _unitOfWork.CommitChangesAsync();

                return _mapper.Map<ResGetUtilitiesById>(entity);
            }
        }
    }
}
