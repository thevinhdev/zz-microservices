using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Queries
{
    public class InfoUserQuery : IRequest<ResGetUser>
    {
        public long id { get; set; }
        public class InfoUserHandler : IRequestHandler<InfoUserQuery, ResGetUser>
        {
            private readonly IMapper _mapper;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IDepartmentAsyncRepository _departmentRepo;
            private readonly IAsyncRepository<Position> _positionRepo;

            public InfoUserHandler(IMapper mapper, IUserAsyncRepository userRepo,
                IDepartmentAsyncRepository departmentRepo, IAsyncRepository<Position> positionRepo)
            {
                _mapper = mapper;
                _userRepo = userRepo;
                _departmentRepo = departmentRepo;
                _positionRepo = positionRepo;
            }

            public async Task<ResGetUser> Handle(InfoUserQuery request, CancellationToken cancellationToken)
            {
                var entities = await _userRepo.GetByKeyAsync(request.id);

                if (entities == null)
                {
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                ResGetUser resGetUser = new ResGetUser();
                resGetUser.UserId = entities.Id;
                resGetUser.Code = entities.Code;
                resGetUser.FullName = entities.FullName;
                resGetUser.UserName = entities.UserName;
                resGetUser.Avata = entities.Avata;
                resGetUser.Address = entities.Address;
                resGetUser.Email = entities.Email;
                resGetUser.Phone = entities.Phone;
                resGetUser.CreatedAt = entities.CreatedAt;
                resGetUser.Status = (int)entities.Status;
                resGetUser.DepartmentId = entities.DepartmentId;
                resGetUser.PositionId = entities.PositionId;
                resGetUser.RoleMax = entities.RoleMax;
                resGetUser.RoleLevel = entities.RoleLevel;
                resGetUser.IsRoleGroup = entities.IsRoleGroup;
                if (entities.DepartmentId != null)
                {
                    var department = await _departmentRepo.GetByKeyAsync((int)entities.DepartmentId);
                    if (department != null)
                    {
                        resGetUser.department = new DepartmentDT
                        {
                            DepartmentId = department.DepartmentId,
                            Name = department.Name,
                        };
                    }
                }
                if (entities.PositionId != null)
                {
                    var position = await _positionRepo.GetByKeyAsync((int)entities.PositionId);
                    if (position != null)
                    {
                        resGetUser.position = new PositionDT
                        {
                            PositionId = position.Id,
                            Name = position.Name,
                        };
                    }
                }
                return _mapper.Map<ResGetUser>(resGetUser);
            }
        }
    }
}
