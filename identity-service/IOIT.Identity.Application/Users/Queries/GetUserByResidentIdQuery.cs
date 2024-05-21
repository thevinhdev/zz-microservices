using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace IOIT.Identity.Application.Users.Queries
{
    public class GetUserByResidentIdQuery : IRequest<User>
    {
        public long ResidentId { get; set; }
        public class InfoUserHandler : IRequestHandler<GetUserByResidentIdQuery, User>
        {
            private readonly IMapper _mapper;
            private readonly IUserAsyncRepository _userRepo;

            public InfoUserHandler(IMapper mapper, IUserAsyncRepository userRepo)
            {
                _mapper = mapper;
                _userRepo = userRepo;
            }

            public async Task<User> Handle(GetUserByResidentIdQuery request, CancellationToken cancellationToken)
            {
                var entities = await _userRepo.FindByResidentAsync(request.ResidentId, cancellationToken);
                
                return entities;
            }
        }
    }
}
