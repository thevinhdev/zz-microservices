using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IOIT.Identity.Application.Categorys.Queries
{
    public class GetUserByResidentIdQuery : IRequest<User>
    {
        public long ResidentId { get; set; }
        public class Validation : AbstractValidator<GetUserByResidentIdQuery>
        {
            public Validation()
            {
                RuleFor(x => x.ResidentId).NotEmpty().WithMessage("ResidentId not empty").GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<GetUserByResidentIdQuery, User>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
            }

            public async Task<User> Handle(GetUserByResidentIdQuery request, CancellationToken cancellationToken)
            {
                var user = _userRepo.All().Where(e => e.UserMapId == request.ResidentId && (e.Type == (int)AppEnum.TypeUser.RESIDENT_GUEST || e.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN) && e.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();

                return user;
            }
        }
    }
}
