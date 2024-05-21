//using AutoMapper;
//using Dapper;
//using FluentValidation;
//using IOIT.Identity.Application.Common;
//using IOIT.Identity.Application.Common.Exceptions;
//using IOIT.Identity.Application.Interfaces;
//using IOIT.Identity.Domain.Entities;
//using IOIT.Identity.Domain.Interfaces;
//using IOIT.Shared.Commons.Enum;
//using IOIT.Shared.ViewModels.DatabaseOneS;
//using IOIT.Shared.ViewModels.DtoQueues;
//using MassTransit;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Linq.Dynamic.Core;
//using System.Threading;
//using System.Threading.Tasks;


//namespace IOIT.Identity.Application.OneSData.Commands
//{
//    public class SyncResidentDataFromOnesToTnsPlus : IRequest<bool>
//    {
//        public class Handler : IRequestHandler<SyncResidentDataFromOnesToTnsPlus, bool>
//        {
//            private readonly IMapper _mapper;
//            private readonly IUnitOfWork _unitOfWork;
//            private readonly IPublishEndpoint _publishEndpoint;
//            private readonly IAsyncGenericDataOneSRepository<Temp_ApartmentMap> _temp_ApartmentMap_Repository;
//            private readonly IAsyncGenericDataOneSRepository<Temp_Resident> _temp_Resident_Repository;
//            private readonly IAsyncGenericDataOneSRepository<Temp_Apartment> _temp_Apartment_Repository;
//            private readonly IProjectAsyncRepository _projectRepository;
//            private readonly ITowerAsyncRepository _towerRepository;
//            private readonly IFloorAsyncRepository _floorRepository;
//            private readonly IApartmentAsyncRepository _apartmentRepository;
//            private readonly IApartmentMapAsyncRepository _apartmentMapRepository;
//            private readonly IResidentAsyncRepository _residentRepository;
//            private readonly IDapper _dapper;

//            public Handler(IMapper mapper,
//                IUnitOfWork unitOfWork,
//                IAsyncGenericDataOneSRepository<Temp_ApartmentMap> temp_ApartmentMap_Repository,
//                IAsyncGenericDataOneSRepository<Temp_Resident> temp_Resident_Repository,
//                IAsyncGenericDataOneSRepository<Temp_Apartment> temp_Apartment_Repository,
//                IProjectAsyncRepository projectRepository,
//                ITowerAsyncRepository towerRepository,
//                IFloorAsyncRepository floorRepository,
//                IApartmentAsyncRepository apartmentRepository,
//                IApartmentMapAsyncRepository apartmentMapRepository,
//                IResidentAsyncRepository residentRepository,
//                IPublishEndpoint publishEndpoint,
//                IDapper dapper
//                )
//            {
//                _mapper = mapper;
//                _unitOfWork = unitOfWork;
//                _publishEndpoint = publishEndpoint;
//                _temp_ApartmentMap_Repository = temp_ApartmentMap_Repository;
//                _temp_Resident_Repository = temp_Resident_Repository;
//                _temp_Apartment_Repository = temp_Apartment_Repository;
//                _projectRepository = projectRepository;
//                _towerRepository = towerRepository;
//                _floorRepository = floorRepository;
//                _apartmentRepository = apartmentRepository;
//                _apartmentMapRepository = apartmentMapRepository;
//                _residentRepository = residentRepository;
//                _dapper = dapper;
//            }

//            public async Task<bool> Handle(SyncResidentDataFromOnesToTnsPlus request, CancellationToken cancellationToken)
//            {
//                //List<Project> projects = _projectRepository.All().Where(e => e.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED).ToList();
//                //List<Tower> towers = _towerRepository.All().Where(e => e.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED).ToList();
//                //List<Floor> floors = _floorRepository.All().Where(e => e.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED).ToList();

//                string query_Apartments = "Select * from Apartment where Status != 99";
//                //List<Apartment> apartments = _apartmentRepository.All().Where(e => e.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED).ToList();
//                List<Apartment> apartments = (_dapper.GetAll<Apartment>(query_Apartments, null, commandType: CommandType.Text));

//                string query_ApartmentMaps = "Select * from ApartmentMap where Status != 99";
//                //List<ApartmentMap> apartmentMaps = _apartmentMapRepository.All().Where(e => e.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED).ToList();
//                List<ApartmentMap> apartmentMaps = (_dapper.GetAll<ApartmentMap>(query_ApartmentMaps, null, commandType: CommandType.Text));

//                string query_Residents = "Select * from Resident where Status != 99";
//                //List<Resident> residents = _residentRepository.All().Where(e => e.Status != Shared.Commons.Enum.AppEnum.EntityStatus.DELETED).ToList();
//                List<Resident> residents = (_dapper.GetAll<Resident>(query_Residents, null, commandType: CommandType.Text));

//                string query_Temp_ApartmentMaps = "Select * from Temp_ApartmentMap where Status != 99";
//                //List<Temp_ApartmentMap> temp_ApartmentMaps = _temp_ApartmentMap_Repository.All();
//                List<Temp_ApartmentMap> temp_ApartmentMaps = (_dapper.GetAll<Temp_ApartmentMap>(query_Temp_ApartmentMaps, null, commandType: CommandType.Text));

//                string query_Temp_Apartments = "Select * from Temp_Apartment where Status != 99";
//                //List<Temp_Apartment> temp_Apartments = _temp_Apartment_Repository.All();
//                List<Temp_Apartment> temp_Apartments = (_dapper.GetAll<Temp_Apartment>(query_Temp_Apartments, null, commandType: CommandType.Text));


//                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);

//                //var list_Temp_Resident = _temp_Resident_Repository.All().ToList();
//                string query_list_Temp_Resident = "Select * from Temp_Resident where Status != 99 Order by ResidentId Desc";
//                List<Temp_Resident> list_Temp_Resident = (_dapper.GetAll<Temp_Resident>(query_list_Temp_Resident, null, commandType: CommandType.Text));

//                //int count = 0;
//                foreach (var Temp_Resident in list_Temp_Resident)
//                {
//                    Resident resident = residents.Where(am => am.OneSid == Temp_Resident.OneSid && am.Phone == Temp_Resident.Phone).FirstOrDefault();

//                    if (resident == null)
//                    {
//                        //resident = new Resident();
//                        //resident.OneSid = Temp_Resident.OneSid;
//                        //resident.FullName = Temp_Resident.FullName;
//                        //resident.Birthday = Temp_Resident.Birthday;
//                        //resident.Phone = Temp_Resident.Phone;
//                        //resident.Email = Temp_Resident.Email;
//                        //resident.CardId = Temp_Resident.CardId;
//                        //resident.Sex = Temp_Resident.Sex;
//                        //resident.Address = Temp_Resident.Address;
//                        //resident.UpdatedAt = DateTime.Now;
//                        //resident.CreatedById = -99;
//                        //resident.Status = AppEnum.EntityStatus.NORMAL;

//                        //_residentRepository.Add(resident);
//                        //await _unitOfWork.CommitChangesAsync();

//                        var dbArgs = new DynamicParameters();
//                        dbArgs.Add("OneSid", Temp_Resident.OneSid);
//                        dbArgs.Add("FullName", Temp_Resident.FullName);
//                        dbArgs.Add("Birthday", Temp_Resident.Birthday);
//                        dbArgs.Add("Email", Temp_Resident.Email);
//                        dbArgs.Add("Phone", Temp_Resident.Phone);
//                        dbArgs.Add("CardId", Temp_Resident.CardId);
//                        dbArgs.Add("Sex", Temp_Resident.Sex);
//                        dbArgs.Add("Address", Temp_Resident.Address);
//                        dbArgs.Add("CreatedById", -99);
//                        dbArgs.Add("CreatedAt", DateTime.Now);
//                        dbArgs.Add("UpdatedAt", DateTime.Now);
//                        dbArgs.Add("CountryId", Temp_Resident.UserId == 0 || Temp_Resident.UserId == 1 ? 1 : 2);

//                        string query_Insert_Resident = "INSERT INTO Resident (OneSid, FullName, Birthday, Email, Phone, CardId, Sex, Address, CreatedById, CreatedAt, UpdatedAt, CountryId) OUTPUT INSERTED.Id VALUES(@OneSid, @FullName, @Birthday, @Email, @Phone, @CardId, @Sex, @Address, @CreatedById, @CreatedAt, @UpdatedAt, @CountryId);";
//                        resident = _dapper.Insert<Resident>(query_Insert_Resident, dbArgs, commandType: CommandType.Text);
//                    }

//                    var list_Apartment_Map = temp_ApartmentMaps.Where(am => am.ResidentId == Temp_Resident.ResidentId).ToList();
//                    var dataApartmentMap = apartmentMaps.Where(am => am.ResidentId == resident.Id).ToList();

//                    foreach (var Apartment_Map in list_Apartment_Map)
//                    {
//                        ApartmentMap apartmentMap = dataApartmentMap.Where(dam => dam.ApartmentId == Apartment_Map.ApartmentId).FirstOrDefault();
//                        if (apartmentMap == null)
//                        {
//                            Temp_Apartment temp_Apartment = temp_Apartments.Where(a => a.ApartmentId == Apartment_Map.ApartmentId).FirstOrDefault();
//                            Apartment apartment = apartments.Where(a => a.Name == temp_Apartment.Name).FirstOrDefault();
//                            if (apartment != null)
//                            {
//                                //apartmentMap = new ApartmentMap();
//                                //apartmentMap.ApartmentId = apartment.ApartmentId;
//                                //apartmentMap.FloorId = apartment.FloorId;
//                                //apartmentMap.TowerId = apartment.TowerId;
//                                //apartmentMap.ProjectId = apartment.ProjectId;
//                                //apartmentMap.ResidentId = resident.Id;
//                                //apartmentMap.Type = Apartment_Map.Type;
//                                //apartmentMap.DateStart = Apartment_Map.DateStart;
//                                //apartmentMap.DateRent = Apartment_Map.DateRent;
//                                //apartmentMap.DateEnd = Apartment_Map.DateEnd;
//                                //apartmentMap.CreatedAt = DateTime.Now;
//                                //apartmentMap.CreatedById = -99;
//                                //apartmentMap.Status = AppEnum.EntityStatus.NORMAL;

//                                //_apartmentMapRepository.Add(apartmentMap);

//                                var dbArgs_ApartmentMap = new DynamicParameters();
//                                dbArgs_ApartmentMap.Add("Id", Guid.NewGuid());
//                                dbArgs_ApartmentMap.Add("ApartmentId", apartment.ApartmentId);
//                                dbArgs_ApartmentMap.Add("FloorId", apartment.FloorId);
//                                dbArgs_ApartmentMap.Add("TowerId", apartment.TowerId);
//                                dbArgs_ApartmentMap.Add("ProjectId", apartment.ProjectId);
//                                dbArgs_ApartmentMap.Add("ResidentId", resident.Id);
//                                dbArgs_ApartmentMap.Add("Type", Apartment_Map.Type);
//                                dbArgs_ApartmentMap.Add("DateStart", Apartment_Map.DateStart);
//                                dbArgs_ApartmentMap.Add("DateRent", Apartment_Map.DateRent);
//                                dbArgs_ApartmentMap.Add("DateEnd", Apartment_Map.DateEnd);
//                                dbArgs_ApartmentMap.Add("CreatedById", -99);
//                                dbArgs_ApartmentMap.Add("CreatedAt", DateTime.Now);
//                                dbArgs_ApartmentMap.Add("UpdatedAt", DateTime.Now);

//                                string query_Insert_ApartmentMap = "INSERT INTO ApartmentMap (Id, ApartmentId, FloorId, TowerId, ProjectId, ResidentId, Type, DateStart, DateRent, DateEnd, CreatedById, CreatedAt, UpdatedAt) VALUES(@Id, @ApartmentId, @FloorId, @TowerId, @ProjectId, @ResidentId, @Type, @DateStart, @DateRent, @DateEnd, @CreatedById, @CreatedAt, @UpdatedAt);";
//                                var res_ApartmentMap = _dapper.Insert<ApartmentMap>(query_Insert_ApartmentMap, dbArgs_ApartmentMap, commandType: CommandType.Text);
//                            }
//                        }
//                    }

//                    //count++;
//                    //if(count == 2000) break;
//                }

//                _unitOfWork.CommitTransaction();

//                return true;
//            }
//        }
//    }
//}
