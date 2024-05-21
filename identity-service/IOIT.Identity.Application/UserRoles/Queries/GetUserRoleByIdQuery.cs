using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.UserRoles.Queries
{
    public class GetUserRoleByIdQuery : IRequest<UserRole>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetUserRoleByIdQuery, UserRole>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<UserRole> _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<UserRole> entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<UserRole> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _entityRepository.GetByKeyAsync(request.Id);

                if (entity == null)
                {
                    throw new BadRequestException("Nhóm quyền của tài khoản không tồn tại", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                return entity;
            }
        }
    }
}
