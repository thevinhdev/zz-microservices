using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities.Indentity;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Queries
{
    public class GetUserByIdQuery : IRequest<ResGetUser>
    {
        public string Id { get; set; }
        public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, ResGetUser>
        {
            private readonly IMapper _mapper;
            private readonly IAsyncRepository<User> _userRepo;

            public GetUserByIdHandler(IMapper mapper, IAsyncRepository<User> userRepo)
            {
                _mapper = mapper;
                _userRepo = userRepo;
            }

            public async Task<ResGetUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var entities = await _userRepo.GetByKeyAsync(new Guid(request.Id));

                if (entities == null)
                {
                    throw new BadRequestException(Resources.USER_NOT_FOUND);
                }

                return _mapper.Map<ResGetUser>(entities);
            }
        }
    }
}
