using AutoMapper;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Departments.Queries
{
    public class GetDepartmentByConditionQuery : IRequest<Department>
    {
        public string condition { get; set; }

        public class Handler : IRequestHandler<GetDepartmentByConditionQuery, Department>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IDepartmentAsyncRepository _dataRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IDepartmentAsyncRepository dataRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _dataRepository = dataRepository;
            }

            public async Task<Department> Handle(GetDepartmentByConditionQuery request, CancellationToken cancellationToken)
            {
                var entity = await _dataRepository.GetDataByCondition(request.condition);

                if (entity == null)
                {
                    throw new BadRequestException("Phòng ban không tồn tại.", Constants.StatusCodeResApi.Error400, ApiConstants.ErrorCode.ERROR_DEPARTMENT_NOT_EXIST);
                }

                return entity;
            }
        }
    }
}
