using AutoMapper;
using FluentValidation;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Residents.Queries
{
    public class GetResidentByPhoneQuery : IRequest<Resident>
    {
        public string Phone { get; set; }

        public class Handler : IRequestHandler<GetResidentByPhoneQuery, Resident>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
            }

            public async Task<Resident> Handle(GetResidentByPhoneQuery request, CancellationToken cancellationToken)
            {
                var entity = await _entityRepository.CheckPhoneResidentMainAsync(request.Phone, cancellationToken);

                return entity;
            }
        }
    }
}
