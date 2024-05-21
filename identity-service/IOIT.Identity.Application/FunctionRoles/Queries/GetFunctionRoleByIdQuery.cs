﻿using AutoMapper;
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

namespace IOIT.Identity.Application.FunctionRoles.Queries
{
    public class GetFunctionRoleByIdQuery : IRequest<FunctionRole>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetFunctionRoleByIdQuery, FunctionRole>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<FunctionRole> _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<FunctionRole> entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<FunctionRole> Handle(GetFunctionRoleByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _entityRepository.GetByKeyAsync(request.Id);

                if (entity == null)
                {
                    throw new BadRequestException("Chức năng theo mã quyền không tồn tại trong hệ thống", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_FUNCTIONROLE_NOT_EXIST);
                }

                return entity;
            }
        }
    }
}
