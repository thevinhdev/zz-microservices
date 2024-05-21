using AutoMapper;
using IOIT.Identity.Application.UserRoles.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.UserRoles.Queries
{
    public class GetUserByIdQuery : IRequest<UserReceiveNotification>
    {
        public int Id { get; set; }
        public class Handler : IRequestHandler<GetUserByIdQuery, UserReceiveNotification>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<UserReceiveNotification> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var entity = await _entityRepository.GetByKeyAsync(request.Id);

                if (entity != null)
                {
                    UserReceiveNotification user = new UserReceiveNotification();

                    user.UserId = entity.Id;
                    user.FullName = entity.FullName;
                    user.Phone = entity.Phone;
                    user.Avata = entity.Avata;

                    return user;
                }
                else return null;
            }
        }
    }
}
