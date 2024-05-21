using AutoMapper;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Application.Utilities.Queries;
using IOIT.Identity.Application.Utilities.ViewModels;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using IOIT.Identity.Application.ProjectUtilities.ViewModels;
using FluentValidation;
using IOIT.Identity.Application.Utilities.Commands.Create;
using IOIT.Identity.Domain.Enum;

namespace IOIT.Identity.Application.ProjectUtilities.Queries
{
    public class GetConfigUtilitiesByProjectIdQuery : FilterPagination, IRequest<List<ResGetConfigProjectUtilitiesQuery>>
    {
        public int ProjectId { get; set; }

        public class Validation : AbstractValidator<GetConfigUtilitiesByProjectIdQuery>
        {
            public Validation()
            {
                RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Có lỗi sảy ra vui lòng đăng nhập lại").GreaterThan(0).WithMessage("Có lỗi sảy ra vui lòng đăng nhập lại");
            }
        }

        public class Handler : IRequestHandler<GetConfigUtilitiesByProjectIdQuery, List<ResGetConfigProjectUtilitiesQuery>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUtilitiesRepository _utilitiesRepository;
            private readonly IProjectUtilitiesRepository _projectUtilitiesRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUtilitiesRepository utilitiesRepository,
                IProjectUtilitiesRepository projectUtilitiesRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _utilitiesRepository = utilitiesRepository;
                _projectUtilitiesRepository = projectUtilitiesRepository;
            }

            public async Task<List<ResGetConfigProjectUtilitiesQuery>> Handle(GetConfigUtilitiesByProjectIdQuery request, CancellationToken cancellationToken)
            {
                var resData = new List<ResGetConfigProjectUtilitiesQuery>();

                var listProjectUtilities = await _utilitiesRepository.getUtilitiesByProjectId(request.ProjectId, cancellationToken);

                if (listProjectUtilities.Count > 0)
                {
                    var listTypeUtilities =
                        listProjectUtilities.Where(s => s.Type == DomainEnum.TypeUtilities.Utilities).ToList();

                    var listTypeService = listProjectUtilities.Where(s => s.Type == DomainEnum.TypeUtilities.Services).ToList();

                    if (listTypeUtilities.Count > 0)
                    {
                        var itemUtilities = new ResGetConfigProjectUtilitiesQuery()
                        {
                            Type = DomainEnum.TypeUtilities.Utilities,
                            Code = "UTILITIES",
                            Title = "Tiện ích",
                            Description = "Tiện ích",
                            Logo = "logo_utilities_menu.jpg",
                            ListUtilitiesChild = _mapper.Map<List<ResGetConfigProjectUtilitiesQuery>>(listTypeUtilities)
                        };

                        resData.Add(itemUtilities);
                    }

                    if (listTypeService.Count > 0)
                    {
                        var itemServices = new ResGetConfigProjectUtilitiesQuery()
                        {
                            Type = DomainEnum.TypeUtilities.Services,
                            Code = "SERVICE",
                            Title = "Dịch vụ",
                            Description = "Dịch vụ",
                            Logo = "logo_service_menu.jpg",
                            ListUtilitiesChild = _mapper.Map<List<ResGetConfigProjectUtilitiesQuery>>(listTypeService)
                        };

                        resData.Add(itemServices);
                    }
                }

                return resData;
            }
        }
    }
}
