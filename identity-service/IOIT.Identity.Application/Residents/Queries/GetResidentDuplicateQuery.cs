using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Residents.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Utilities.Application.Dashboard
{
    public class GetResidentDuplicateQuery : IRequest<ResidentCountry>
    {
        public DataQuery Data { get; set; }

        public class Validation : AbstractValidator<GetResidentDuplicateQuery>
        {
            public Validation()
            {
                //RuleFor(x => x.ResidentId).NotEmpty().WithMessage("ResidentId not empty").GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<GetResidentDuplicateQuery, ResidentCountry>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncLongRepository<Resident> _dataRepository;
            private readonly IApartmentMapAsyncRepository _amRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncLongRepository<Resident> dataRepository,
                IApartmentMapAsyncRepository amRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _dataRepository = dataRepository;
                _amRepo = amRepo;
            }

            public async Task<ResidentCountry> Handle(GetResidentDuplicateQuery request, CancellationToken cancellationToken)
            {
                //DateTime dateStart = request.Data.DateTimeStart != null ? (DateTime)request.Data.DateTimeStart : new DateTime(2020, 1, 1);
                //DateTime dateEnd = request.Data.DateTimeEnd != null ? (DateTime)request.Data.DateTimeEnd : DateTime.Now;
                //int projectId = request.Data.ProjectId != null ? (int)request.Data.ProjectId : -1;
                //
                var listResidents = await (from r in _dataRepository.All()
                                           join ap in _amRepo.All() on r.Id equals ap.ResidentId
                                           where r.Status != AppEnum.EntityStatus.DELETED && r.Status != AppEnum.EntityStatus.NOT_OK
                                           && ap.Status != AppEnum.EntityStatus.DELETED
                                           && r.Type != 1
                                           //&& (ap.ProjectId == projectId || projectId == -1)
                                           select new { 
                                               r.Id,
                                               r.FullName,
                                               ap.ApartmentId,
                                           }).ToListAsync();
                //
                //ResidentCountry residentCountry = new ResidentCountry();
                //residentCountry.TotalVn = listResidents.Where(e => e.CountryId != 2).Count();
                //residentCountry.TotalNn = listResidents.Where(e => e.CountryId == 2).Count();
                return null;
            }
        }
    }
}
