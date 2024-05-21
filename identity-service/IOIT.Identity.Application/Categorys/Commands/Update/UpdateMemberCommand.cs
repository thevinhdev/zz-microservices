using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Employees.ViewModels;
using IOIT.Identity.Application.Models;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Categorys.Commands.Update
{
    public class UpdateMemberCommand : IRequest<Resident>
    {
        public int Id { get; set; }
        public long ResidentId { get; set; }
        public int? ApartmentId { get; set; }
        public long? userMapId { get; set; }
        //public int? TowerId { get; set; }
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
        public DateTime? DateRent { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UserId { get; set; }
        public byte? Status { get; set; }
        public List<ApartmentResidentDT> apartments { get; set; }
        public UserDT user { get; set; }

        public class Validation : AbstractValidator<UpdateMemberCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.FullName).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateMemberCommand, Resident>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IResidentAsyncRepository _entityRepository;
            private readonly IApartmentMapAsyncRepository _amRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IResidentAsyncRepository entityRepository,
                IApartmentMapAsyncRepository amRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _amRepo = amRepo;
            }

            public async Task<Resident> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
            {
                //using (var db = new IOITResidentGateContext())
                //{
                    //Kiểm tra chỉ có chủ hộ mới được sửa thông tin thành viên trong căn hộ đó
                    Resident resident_main = (from r in _entityRepository.All()
                                              join ap in _amRepo.All() on r.Id equals ap.ResidentId
                                              where r.Status != AppEnum.EntityStatus.DELETED
                                              && ap.Status != AppEnum.EntityStatus.DELETED
                                              && ap.ApartmentId == request.ApartmentId
                                              && ap.ResidentId == request.userMapId
                                              && ap.Type == (int)AppEnum.TypeResident.RESIDENT_MAIN
                                              select r).AsNoTracking().FirstOrDefault();

                    Resident resident_member = (from r in _entityRepository.All()
                                                join ap in _amRepo.All() on r.Id equals ap.ResidentId
                                                where r.Status != AppEnum.EntityStatus.DELETED
                                                && ap.Status != AppEnum.EntityStatus.DELETED
                                                && ap.ApartmentId == request.ApartmentId
                                                && ap.ResidentId == request.ResidentId
                                                && ap.Type == (int)AppEnum.TypeResident.RESIDENT_MEMBER
                                                select r).AsNoTracking().FirstOrDefault();

                    if (resident_main == null || resident_member == null)
                    {
                    //def.meta = new Meta(222, Const.NOPERMISION_CREATE_MESSAGE);
                    //return Ok(def);
                    throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
                }

                    Resident resident = await _entityRepository.All().AsNoTracking().Where(e => e.Id == request.ResidentId && e.Status != AppEnum.EntityStatus.DELETED).FirstOrDefaultAsync();

                    if (resident == null)
                    {
                    throw new BadRequestException("The resident does not exist.", Constants.StatusCodeResApi.Error404);
                    //def.meta = new Meta(404, Const.NOT_FOUND_UPDATE_MESSAGE);
                    //return Ok(def);
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                //resident.ResidentParentId = data.ResidentParentId;
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                resident.FullName = request.FullName;
                        resident.Birthday = request.Birthday;
                        resident.CardId = request.CardId;
                        resident.Phone = request.Phone;
                        resident.Email = request.Email;
                        resident.Address = request.Address;
                        resident.Avata = request.Avata;
                        resident.Sex = request.Sex;
                        resident.Note = request.Note;
                        resident.DateRent = request.DateRent;
                        resident.UpdatedAt = DateTime.Now;
                        //db.Resident.Update(resident);
                _entityRepository.Update(resident);
                try
                        {
                    //await db.SaveChangesAsync();
                    await _unitOfWork.CommitChangesAsync();

                    if (request.ResidentId > 0)
                            {
                        //transaction.Commit();
                        _unitOfWork.CommitTransaction();
                        //create action
                        //IOITResident.Models.EF.Action action = new IOITResident.Models.EF.Action();
                        //action.ActionId = Guid.NewGuid();
                        //action.ActionName = resident_main.FullName + "sửa thông tin thành viên căn hộ " + data.FullName;
                        //action.ActionType = "UPDATE";
                        //action.TargetId = data.ResidentId.ToString();
                        //action.TargetType = "Resident";
                        //action.Logs = JsonConvert.SerializeObject(data);
                        //action.Time = 0;
                        //action.Type = (int)Const.TypeAction.ACTION_ON_APP;
                        //action.CreatedAt = DateTime.Now;
                        //action.UserPushId = userMapId;
                        //action.UserId = userMapId;
                        //action.Status = (int)Const.Status.NORMAL;
                        //db.Action.Add(action);

                        //await db.SaveChangesAsync();

                        //def.meta = new Meta(200, Const.UPDATE_SUCCESS_MESSAGE);
                        //def.data = data;
                        //return Ok(def);
                        return resident;
                    }
                            else
                            {
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_USER_UPDATE_FAILED);
                        //transaction.Rollback();
                        //def.meta = new Meta(500, Const.ERROR_500_MESSAGE);
                        //return Ok(def);
                    }
                        }
                        catch (DbUpdateException e)
                        {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_USER_UPDATE_FAILED);
                    //transaction.Rollback();
                    //log.Error("DbUpdateException:" + e);
                    //def.meta = new Meta(500, Const.ERROR_500_MESSAGE);
                    //return Ok(def);
                }
                    //}
                //}
            }
        }
    }
}
