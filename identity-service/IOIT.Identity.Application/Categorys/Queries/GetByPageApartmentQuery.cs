using AutoMapper;
using IOIT.Identity.Application.Categorys.ViewModels;
using IOIT.Identity.Application.Specifications.PagingSpec;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.ViewModels.PagingQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Categorys.Queries
{
    public class GetByPageApartmentQuery : FilterPagination, IRequest<IPagedResult<ApartmentDT>>
    {
        public long? ResidentId { get; set; }

        public class Handler : IRequestHandler<GetByPageApartmentQuery, IPagedResult<ApartmentDT>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IApartmentMapAsyncRepository _entityRepository;
            private readonly IFloorAsyncRepository _floorRepository;
            private readonly ITowerAsyncRepository _towerRepository;
            private readonly IProjectAsyncRepository _projectRepository;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IApartmentMapAsyncRepository entityRepository,
                IFloorAsyncRepository floorRepository,
                ITowerAsyncRepository towerRepository,
                IProjectAsyncRepository projectRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _floorRepository = floorRepository;
                _towerRepository = towerRepository;
                _projectRepository = projectRepository;
            }

            public async Task<IPagedResult<ApartmentDT>> Handle(GetByPageApartmentQuery request, CancellationToken cancellationToken)
            {
                //var spec = new ResidentFilterWithPagingSpec(request);
                FilterPagination filterPagination = new FilterPagination();
                filterPagination.page = request.page;
                filterPagination.page_size = request.page_size;
                filterPagination.query = request.query;
                filterPagination.order_by = request.order_by;
                filterPagination.search = request.search;
                var entities = await _entityRepository.GetByPageApartmentAsync(filterPagination, request.ResidentId, cancellationToken);

                List<ApartmentDT> res = new List<ApartmentDT>();
                foreach (var item in entities.Results)
                {
                    ApartmentDT residentDTO = new ApartmentDT();
                    residentDTO.ApartmentId = item.ApartmentId;
                    residentDTO.Code = item.Code;
                    residentDTO.Name = item.Name;
                    residentDTO.FloorId = item.FloorId;
                    var floor = _floorRepository.All().Where(f => f.FloorId == item.FloorId).FirstOrDefault();
                    residentDTO.FloorName = floor != null ? floor.Name : "";
                    residentDTO.TowerId = item.TowerId;
                    var tower = _towerRepository.All().Where(f => f.TowerId == item.TowerId).FirstOrDefault();
                    residentDTO.TowerName = tower != null ? tower.Name : "";
                    residentDTO.ProjectId = item.ProjectId;
                    var project = _projectRepository.All().Where(f => f.ProjectId == item.ProjectId).FirstOrDefault();
                    residentDTO.ProjectName = project != null ? project.Name : "";
                    residentDTO.UserId = item.UserId;
                    residentDTO.CreatedAt = item.CreatedAt;
                    residentDTO.UpdatedAt = item.UpdatedAt;
                    residentDTO.Status = (int)item.Status;
                    residentDTO.TypeUser = item.TypeUser;

                    res.Add(residentDTO);
                }
                entities.Results = res;
                return entities;
            }
        }
    }
}
