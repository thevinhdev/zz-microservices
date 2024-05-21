using AutoMapper;
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
    public class GetUserActiveQuery : IRequest<List<ResGetUser>>
    {
        public class GetUserActiveHandler : IRequestHandler<GetUserActiveQuery, List<ResGetUser>>
        {
            private readonly IMapper _mapper;
            private readonly IAsyncRepository<User> _userRepo;

            public GetUserActiveHandler(IMapper mapper, IAsyncRepository<User> userRepo)
            {
                _mapper = mapper;
                _userRepo = userRepo;
            }

            public async Task<List<ResGetUser>> Handle(GetUserActiveQuery request, CancellationToken cancellationToken)
            {
                var entities = await _userRepo.ListAllAsync();

                if (entities == null)
                {
                    throw new KeyNotFoundException("not data");
                }

                return _mapper.Map<List<ResGetUser>>(entities);
            }
        }
    }
}
