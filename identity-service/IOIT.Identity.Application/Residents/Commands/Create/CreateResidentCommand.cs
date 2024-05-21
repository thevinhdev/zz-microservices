using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Identity.Domain.ViewModels;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.ViewModels.DtoQueues;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Identity.Domain.Enum.DomainEnum;
using static IOIT.Shared.Commons.Enum.AppEnum;
using ApartmentMapDT = IOIT.Identity.Domain.ViewModels.ApartmentMapDT;

namespace IOIT.Identity.Application.Residents.Commands.Create
{
    public class CreateResidentCommand : IRequest<Resident>
    {
        public long Id { get; set; }
        public long ResidentId { get; set; }
        public int? ProjectId { get; set; }
        public long? ResidentParentId { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string CardId { get; set; }
        public DateTime? DateId { get; set; }
        public string AddressId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avata { get; set; }
        public string Sex { get; set; }
        public string Note { get; set; }
        public int? CountryId { get; set; }
        public DateTime? DateRent { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public byte? Status { get; set; }
        public List<ApartmentMapDT> apartments { get; set; }
        public UserDT user { get; set; }
        public ResidentRequestIdentifyType? TypeCardId { get; set; } = ResidentRequestIdentifyType.NONE;
        public bool isCalledInternal { get; set; } = false;

        public class Validation : AbstractValidator<CreateResidentCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName not empty");
            }
        }

        public class Handler : IRequestHandler<CreateResidentCommand, Resident>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;
            private readonly IApartmentMapAsyncRepository _amRepo;
            private readonly IApartmentAsyncRepository _apartRepo;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository,
                IApartmentMapAsyncRepository amRepo,
                IApartmentAsyncRepository apartRepo,
                IPublishEndpoint publishEndpoint)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _amRepo = amRepo;
                _apartRepo = apartRepo;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<Resident> Handle(CreateResidentCommand request, CancellationToken cancellationToken)
            {
                Console.WriteLine("Handle CreateResidentCommand");

                //var entity = _mapper.Map<Resident>(request);
                var phone = string.Empty;
                //await _entityRepository.AddAsync(entity);
                //await _unitOfWork.CommitChangesAsync();
                if (request.Phone != null && request.Phone != "")
                {
                    //Resident checkPhone = db.Resident.AsNoTracking().Where(f => f.Phone == request.Phone.Trim() && f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                    var checkPhone = await _entityRepository.CheckPhoneExitAsync(request.Phone, 0, cancellationToken);
                    if (checkPhone != null && request.isCalledInternal == false)
                    {
                        //def.meta = new Meta(212, "Số điện thoại đã tồn tại!");
                        //return Ok(def);
                        throw new CommonException("Số điện thoại đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_PHONE_EXISTED);
                    } else if (checkPhone != null && request.isCalledInternal == true)
                    {
                        phone = string.Empty;
                    } else
                    {
                        phone = request.Phone.Trim();
                    }
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                Resident resident = new Resident();
                //resident.ResidentParentId = data.ResidentParentId;
                resident.FullName = request.FullName;
                resident.Birthday = request.Birthday;
                resident.CardId = request.CardId;
                resident.DateId = request.DateId;
                resident.AddressId = request.AddressId;
                resident.Phone = phone;
                resident.Email = request.Email;
                resident.Address = request.Address;
                resident.Avata = request.Avata;
                resident.Sex = request.Sex;
                resident.Note = request.Note;
                resident.DateRent = request.DateRent;
                resident.CountryId = request.CountryId;
                resident.CreatedById = request.UserId;
                resident.UpdatedById = request.UserId;
                resident.CreatedAt = DateTime.Now;
                resident.UpdatedAt = DateTime.Now;
                resident.Status = (AppEnum.EntityStatus)request.Status;
                resident.TypeCardId = request.TypeCardId;
                //var entity = _mapper.Map<Resident>(request);
                await _entityRepository.AddAsync(resident);
                Console.WriteLine("Handle await _entityRepository.AddAsync(resident);");
                try
                {
                    await _unitOfWork.CommitChangesAsync();
                    request.Id = resident.Id;

                    //Gọi Producers để thêm vào các service khác
                    var message = _mapper.Map<DtoCommonResidentQueue>(resident);
                    await _publishEndpoint.Publish<DtoCommonResidentQueue>(message);
                    Console.WriteLine("Handle await _publishEndpoint.Publish<DtoCommonResidentQueue>(message);");

                    if (request.Id > 0)
                    {
                        foreach (var item in request.apartments)
                        {
                            //Check xem căn hộ này đã có chủ hộ chưa nếu có rồi thì k thể thêm trong TH là chủ hộ mới
                            if (item.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN)
                            {
                                //ApartmentMap apartmentCH = db.ApartmentMap.Where(a => a.ApartmentId == item.ApartmentId
                                //&& a.Type == (int)Const.TypeResident.RESIDENT_MAIN && a.Status != (int)Const.Status.DELETED).FirstOrDefault();
                                var apartmentCH = await _amRepo.CheckResidentMainAsync(item.ApartmentId, cancellationToken);
                                if (apartmentCH != null)
                                {
                                    //Apartment apartment = db.Apartment.Where(a => a.ApartmentId == item.ApartmentId).FirstOrDefault();
                                    var apartment = await _apartRepo.FindByApartmentIdAsync(item.ApartmentId, cancellationToken);
                                    string apartmentName = apartment != null ? apartment.Name : "";
                                    //transaction.Rollback();
                                    //def.meta = new Meta(212, "Căn hộ " + apartmentName + " đã có chủ hộ!");
                                    //return Ok(def);
                                    _unitOfWork.RollbackTransaction();
                                    throw new BadRequestException("Căn hộ " + apartmentName + " đã có chủ hộ!", 212, ApiConstants.ErrorCode.ERROR_APARTMENT_ALREADY_OWNER);
                                }
                            }

                            ApartmentMap apartmentMap = new ApartmentMap();
                            apartmentMap.Id = Guid.NewGuid();
                            apartmentMap.ApartmentId = item.ApartmentId;
                            apartmentMap.FloorId = item.FloorId;
                            apartmentMap.TowerId = item.TowerId;
                            apartmentMap.ProjectId = item.ProjectId;
                            apartmentMap.ResidentId = request.Id;
                            apartmentMap.DateStart = item.DateStart;
                            apartmentMap.Type = item.Type;
                            apartmentMap.RelationshipId = item.RelationshipId;
                            apartmentMap.DateRent = item.DateRent;
                            apartmentMap.DateEnd = item.DateEnd;
                            apartmentMap.CreatedAt = DateTime.Now;
                            apartmentMap.UpdatedAt = DateTime.Now;
                            apartmentMap.CreatedById = request.UserId;
                            apartmentMap.UpdatedById = request.UserId;
                            apartmentMap.Status = AppEnum.EntityStatus.NORMAL;
                            //db.ApartmentMap.Add(apartmentMap);
                            await _amRepo.AddAsync(apartmentMap);

                            var messageA = _mapper.Map<DtoIdentityApartmentMapQueue>(apartmentMap);
                            await _publishEndpoint.Publish<DtoIdentityApartmentMapQueue>(messageA);
                        }

                        await _unitOfWork.CommitChangesAsync();
                        _unitOfWork.CommitTransaction();
                        //transaction.Commit();
                        //create action
                        //IOITResident.Models.EF.Action action = new IOITResident.Models.EF.Action();
                        //action.ActionId = Guid.NewGuid();
                        //action.ActionName = "Thêm mới cư dân " + data.FullName;
                        //action.ActionType = "CREATE";
                        //action.TargetId = data.ResidentId.ToString();
                        //action.TargetType = "Resident";
                        //action.Logs = JsonConvert.SerializeObject(data);
                        //action.Time = 0;
                        //action.Type = (int)Const.TypeAction.ACTION;
                        //action.CreatedAt = DateTime.Now;
                        //action.UserPushId = userId;
                        //action.UserId = userId;
                        //action.Status = (int)Const.Status.NORMAL;
                        //db.Action.Add(action);

                        //await db.SaveChangesAsync();

                        //def.meta = new Meta(200, Const.CREATE_SUCCESS_MESSAGE);
                        //def.data = data;
                        //return Ok(def);
                        return resident;
                    }
                    else
                    {
                        //transaction.Rollback();
                        //def.meta = new Meta(500, Const.ERROR_500_MESSAGE);
                        //return Ok(def);
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_RESIDENT_CREATE_FAILED);
                    }
                }
                catch (DbUpdateException e)
                {
                    //log.Error("DbUpdateException:" + e);
                    //transaction.Rollback();
                    //if (ResidentExists(data.ResidentId))
                    //{
                    //    def.meta = new Meta(211, Const.ERROR_EXIST_MESSAGE);
                    //    return Ok(def);
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_RESIDENT_CREATE_FAILED);
                }
                //}


            }
        }
    }
}
