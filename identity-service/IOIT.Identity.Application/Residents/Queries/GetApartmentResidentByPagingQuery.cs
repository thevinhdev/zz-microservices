using AutoMapper;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Residents.Queries
{
    public class GetApartmentResidentByPagingQuery : FilterPagination, IRequest<IPagedResult<ResResidentIdDT>>
    {
        public int? ProjectId { get; set; }
        public int? TowerId { get; set; }
        public int? FloorId { get; set; }
        public int? ApartmentId { get; set; }

        public class Handler : IRequestHandler<GetApartmentResidentByPagingQuery, IPagedResult<ResResidentIdDT>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;
            private readonly IApartmentMapAsyncRepository _amRepo;
            private readonly IProjectAsyncRepository _projectRepo;
            private readonly ITowerAsyncRepository _towerRepo;
            private readonly IFloorAsyncRepository _floorRepo;
            private readonly IApartmentAsyncRepository _apartRepo;
            private readonly IUserAsyncRepository _userRepo;
            private readonly ITypeAttributeItemAsyncRepository _typeRepo;
            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository,
                IApartmentMapAsyncRepository amRepo,
                IProjectAsyncRepository projectRepo,
                ITowerAsyncRepository towerRepo,
                IFloorAsyncRepository floorRepo,
                IApartmentAsyncRepository apartRepo,
                IUserAsyncRepository userRepo,
                ITypeAttributeItemAsyncRepository typeRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _amRepo = amRepo;
                _projectRepo = projectRepo;
                _towerRepo = towerRepo;
                _floorRepo = floorRepo;
                _apartRepo = apartRepo;
                _userRepo = userRepo;
                _typeRepo = typeRepo;
            }

            public async Task<IPagedResult<ResResidentIdDT>> Handle(GetApartmentResidentByPagingQuery request, CancellationToken cancellationToken)
            {
                FilterPagination filterPagination = new FilterPagination();
                filterPagination.page = request.page;
                filterPagination.page_size = request.page_size;
                filterPagination.query = request.query;
                filterPagination.order_by = request.order_by;
                filterPagination.search = request.search;
                var entities = await _entityRepository.GetApartmentResidentByPageAsync(filterPagination, request.ProjectId, request.TowerId,
                    request.FloorId, request.ApartmentId, cancellationToken);

                List<ResResidentIdDT> res = new List<ResResidentIdDT>();
                foreach (var item in entities.Results)
                {
                    ResResidentIdDT resResidentIdDT = new ResResidentIdDT();
                    resResidentIdDT.ResidentId = item.ResidentId;
                    resResidentIdDT.ApartmentId = item.ApartmentId;

                    if (item.ResidentId != null)
                    {
                        var us = await _userRepo.FindByResidentAsync((long)item.ResidentId, cancellationToken);
                        if (us != null)
                        {
                            resResidentIdDT.UserId = us.Id;
                        }

                        if (resResidentIdDT.UserId != null)
                            res.Add(resResidentIdDT);
                    }
                }
                Application.Models.PagedResult<ResResidentIdDT> pagedResult = new Application.Models.PagedResult<ResResidentIdDT>();
                pagedResult.Results = res;
                pagedResult.PageCount = entities.PageCount;
                pagedResult.CurrentPage = entities.CurrentPage;
                pagedResult.PageSize = entities.PageSize;
                return pagedResult;
            }
        }
    }
}
