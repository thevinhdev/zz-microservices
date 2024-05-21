using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces.Producer;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Identity.Domain.Enum.DomainEnum;

namespace IOIT.Identity.Application.Residents.Commands.Update
{
    public class UpdateResidentCommand : IRequest<Resident>
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
        public List<Domain.ViewModels.ApartmentMapDT> apartments { get; set; }
        public UserDT user { get; set; }
        public ResidentRequestIdentifyType? TypeCardId { get; set; }

        public class Validation : AbstractValidator<UpdateResidentCommand>
        {
            public Validation()
            {
                RuleFor(x => x.ResidentId).NotEmpty().WithMessage("ResidentId not empty").GreaterThan(0);
                RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateResidentCommand, Resident>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;
            private readonly IApartmentMapAsyncRepository _amRepo;
            private readonly IApartmentAsyncRepository _apartRepo;
            private readonly IPublishEndpoint _publishEndpoint;
            private readonly IIdentityProducer _identityProducer;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository,
                IApartmentMapAsyncRepository amRepo,
                IApartmentAsyncRepository apartRepo,
                IPublishEndpoint publishEndpoint,
                IIdentityProducer identityProducer)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _amRepo = amRepo;
                _apartRepo = apartRepo;
                _publishEndpoint = publishEndpoint;
                _identityProducer = identityProducer;
            }

            public async Task<Resident> Handle(UpdateResidentCommand request, CancellationToken cancellationToken)
            {
                //var data = await _entityRepository.GetByKeyAsync(request.Id);

                //if (data == null)
                //{
                //    throw new BadRequestException("The Resident does not exist.", Constants.StatusCodeResApi.Error404);
                //}

                //var entity = _mapper.Map<Resident>(request);
                //entity.UpdatedAt = DateTime.Now;
                //entity.UpdatedById = request.UserId;

                //_entityRepository.Update(entity);
                //await _unitOfWork.CommitChangesAsync();

                //using (var db = new IOITResidentGateContext())
                //{
                //Resident resident = await db.Resident.AsNoTracking().Where(e => e.ResidentId == id && e.Status != (int)Const.Status.DELETED).FirstOrDefaultAsync();
                var resident = await _entityRepository.GetByKeyAsync(request.Id);
                if (resident == null)
                {
                    throw new BadRequestException("Dữ liệu cư dân không tồn tại trong hệ thống.", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_RESIDENT_NOT_EXIST);
                }

                if (request.Phone != null && request.Phone != "")
                {
                    var checkPhone = await _entityRepository.CheckPhoneExitAsync(request.Phone, request.Id, cancellationToken);
                    //Resident checkPhone = db.Resident.AsNoTracking().Where(f => f.Phone == data.Phone.Trim() && f.ResidentId != id && f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                    if (checkPhone != null)
                    {
                        //def.meta = new Meta(212, "Số điện thoại đã tồn tại!");
                        //return Ok(def);
                        throw new CommonException("Số điện thoại đã tồn tại!", 212, ApiConstants.ErrorCode.ERROR_PHONE_EXISTED);
                    }
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                //resident.ResidentParentId = data.ResidentParentId;
                resident.FullName = request.FullName;
                resident.Birthday = request.Birthday;
                resident.CardId = request.CardId;
                resident.DateId = request.DateId;
                resident.AddressId = request.AddressId;
                resident.Phone = request.Phone != null ? request.Phone.Trim() : "";
                resident.Email = request.Email;
                resident.TypeCardId = request.TypeCardId;
                resident.Address = request.Address;
                resident.Avata = request.Avata;
                resident.Sex = request.Sex;
                resident.Note = request.Note;
                resident.DateRent = request.DateRent;
                resident.CountryId = request.CountryId ?? resident.CountryId;
                resident.UpdatedById = request.UserId;
                resident.UpdatedAt = DateTime.Now;
                //db.Resident.Update(resident);
                _entityRepository.Update(resident);
                await _unitOfWork.CommitChangesAsync();

                try
                {
                    //await db.SaveChangesAsync();
                    await _unitOfWork.CommitChangesAsync();

                    //Gọi Producers để cập nhật vào các service khác
                    var message = _mapper.Map<DtoCommonResidentUpdateQueue>(resident);
                    await _publishEndpoint.Publish<DtoCommonResidentUpdateQueue>(message);

                    if (request.Id > 0)
                    {
                        //List<ApartmentMap> apartmentMaps = db.ApartmentMap.Where(ap => ap.ResidentId == data.ResidentId && ap.Status != (int)Const.Status.DELETED).ToList();
                        List<ApartmentMap> apartmentMaps = await _amRepo.GetListByResidentAsync(request.Id, cancellationToken);
                        foreach (var item in request.apartments)
                        {

                            //Check xem căn hộ này đã có chủ hộ chưa nếu có rồi thì k thể thêm trong TH là chủ hộ mới
                            if (item.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN)
                            {
                                ApartmentMap apartmentCH = _amRepo.All().Where(a => a.ApartmentId == item.ApartmentId && a.ResidentId != request.Id && a.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN && a.Status != AppEnum.EntityStatus.DELETED).FirstOrDefault();
                                //var apartmentCH = await _amRepo.CheckResidentMainAsync(item.ApartmentId, cancellationToken);
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

                            ApartmentMap apartmentMap = apartmentMaps.Where(a => a.ApartmentId == item.ApartmentId
                                                                                && a.FloorId == item.FloorId
                                                                                && a.TowerId == item.TowerId
                                                                                && a.ProjectId == item.ProjectId).FirstOrDefault();

                            if (apartmentMap == null)
                            {
                                ApartmentMap Item = new ApartmentMap();
                                Item.Id = Guid.NewGuid();
                                Item.ApartmentId = item.ApartmentId;
                                Item.FloorId = item.FloorId;
                                Item.TowerId = item.TowerId;
                                Item.ProjectId = item.ProjectId;
                                Item.ResidentId = request.Id;
                                Item.Type = item.Type;
                                Item.RelationshipId = item.RelationshipId;
                                Item.DateRent = item.DateRent;
                                Item.DateStart = item.DateStart;
                                Item.DateEnd = item.DateEnd;
                                Item.CreatedAt = DateTime.Now;
                                Item.UpdatedAt = DateTime.Now;
                                Item.CreatedById = request.UserId;
                                Item.UpdatedById = request.UserId;
                                Item.Status = AppEnum.EntityStatus.NORMAL;
                                //db.ApartmentMap.Add(Item);
                                await _amRepo.AddAsync(Item);
                                await _identityProducer.IdentityApartmentMapCreate(Item);

                            }
                            else
                            {
                                apartmentMap.Type = item.Type;
                                apartmentMap.RelationshipId = item.RelationshipId;
                                apartmentMap.DateRent = item.DateRent;
                                apartmentMap.DateStart = item.DateStart;
                                apartmentMap.DateEnd = item.DateEnd;
                                apartmentMap.UpdatedAt = DateTime.Now;
                                apartmentMap.UpdatedById = request.UserId;
                                //db.ApartmentMap.Update(apartmentMap);
                                _amRepo.Update(apartmentMap);

                                apartmentMaps.Remove(apartmentMap);
                            }
                        }

                        if (apartmentMaps.Count() > 0)
                        {
                            //apartmentMaps.ForEach(e => e.Status = (int)Const.Status.DELETED);
                            foreach (var item in apartmentMaps)
                            {
                                item.Status = AppEnum.EntityStatus.DELETED;
                                await _identityProducer.IdentityApartmentMapCreate(item);
                            }
                        }
                        _amRepo.UpdateRange(apartmentMaps);

                        //check nếu đổi số điện thoại + đã tạo tk thì update sdt trong tk

                        await _unitOfWork.CommitChangesAsync();
                        _unitOfWork.CommitTransaction();

                        ////create action
                        //IOITResident.Models.EF.Action action = new IOITResident.Models.EF.Action();
                        //        action.ActionId = Guid.NewGuid();
                        //        action.ActionName = "Sửa thông tin cư dân " + data.FullName;
                        //        action.ActionType = "UPDATE";
                        //        action.TargetId = data.ResidentId.ToString();
                        //        action.TargetType = "Resident";
                        //        action.Logs = JsonConvert.SerializeObject(data);
                        //        action.Time = 0;
                        //        action.Type = (int)Const.TypeAction.ACTION;
                        //        action.CreatedAt = DateTime.Now;
                        //        action.UserPushId = userId;
                        //        action.UserId = userId;
                        //        action.Status = (int)Const.Status.NORMAL;
                        //        db.Action.Add(action);

                        //        await db.SaveChangesAsync();

                        //def.meta = new Meta(200, Const.UPDATE_SUCCESS_MESSAGE);
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
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_RESIDENT_UPDATE_FAILED);
                    }
                }
                catch (DbUpdateException e)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_RESIDENT_UPDATE_FAILED);
                    //transaction.Rollback();
                    //log.Error("DbUpdateException:" + e);
                    //if (!ResidentExists(data.ResidentId))
                    //{
                    //    def.meta = new Meta(404, Const.NOT_FOUND_UPDATE_MESSAGE);
                    //    return Ok(def);
                    //}
                    //else
                    //{
                    //    def.meta = new Meta(500, Const.ERROR_500_MESSAGE);
                    //    return Ok(def);
                    //}
                }
                //}
                //}


                //return entity;
            }
        }
    }
}
