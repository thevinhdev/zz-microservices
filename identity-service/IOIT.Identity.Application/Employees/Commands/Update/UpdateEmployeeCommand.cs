using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Employees.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
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

namespace IOIT.Identity.Application.Employees.Commands.Update
{
    public class UpdateEmployeeCommand : IRequest<Employee>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Avata { get; set; }
        public int? PositionId { get; set; }
        public int? ProjectId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? Birthday { get; set; }
        public string CardId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public byte? TypeEmployee { get; set; }
        public bool? IsMain { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UserId { get; set; }
        public byte? Status { get; set; }
        public List<ResEmployeeMapDT> employeeMaps { get; set; }

        public class Validation : AbstractValidator<UpdateEmployeeCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.FullName).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<UpdateEmployeeCommand, Employee>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmployeeAsyncRepository _entityRepository;
            private readonly IEmployeeMapAsyncRepository _emRepo;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IEmployeeAsyncRepository entityRepository,
                IEmployeeMapAsyncRepository emRepo,
                IPublishEndpoint publishEndpoint)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _emRepo = emRepo;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<Employee> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
            {
                var exist = await _entityRepository.GetByKeyAsync(request.Id);

                if (exist == null)
                {
                    throw new BadRequestException("Nhân viên không tồn tại.", Constants.StatusCodeResApi.Error404, ApiConstants.ErrorCode.ERROR_EMPLOYEE_NOT_EXIST);
                }

                //Employee checkCode = db.Employee.AsNoTracking().Where(f => f.Code == data.Code && f.EmployeeId != id && f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                var checkCode = await _entityRepository.FindByCodeAsync(request.Code, request.Id, cancellationToken);
                if (checkCode != null)
                {
                    throw new CommonException("Mã nhân viên đã tồn tại!", ApiConstants.StatusCode.Valid212, ApiConstants.ErrorCode.ERROR_EMPLOYEE_CODE_EXIST);
                }
                //Employee checkPhone = db.Employee.AsNoTracking().Where(f => f.Phone == data.Phone && f.EmployeeId != id && f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                var checkPhone = await _entityRepository.FindByCodeAsync(request.Code, request.Id, cancellationToken);
                if (checkPhone != null)
                {
                    throw new CommonException("Số điện thoại đã tồn tại!", ApiConstants.StatusCode.Valid212, ApiConstants.ErrorCode.ERROR_EMPLOYEE_PHONE_EXIST);
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                exist.FullName = request.FullName;
                exist.Code = request.Code;
                exist.Avata = request.Avata;
                exist.ProjectId = request.ProjectId;
                exist.PositionId = request.PositionId;
                exist.DepartmentId = request.DepartmentId;
                exist.Birthday = request.Birthday;
                exist.CardId = request.CardId;
                exist.Phone = request.Phone;
                exist.Email = request.Email;
                exist.Address = request.Address;
                exist.Note = request.Note;
                exist.TypeEmployee = request.TypeEmployee;
                exist.IsMain = request.IsMain;
                exist.UpdatedById = request.UserId;
                exist.UpdatedAt = DateTime.Now;
                //db.Update(exist);
                _entityRepository.Update(exist);

                try
                {
                    //await db.SaveChangesAsync();
                    await _unitOfWork.CommitChangesAsync();
                    if (exist.Id > 0)
                    {
                        //List<EmployeeMap> employeeMaps = db.EmployeeMap.Where(ap => ap.EmployeeId == data.EmployeeId && ap.Status != (int)Const.Status.DELETED).ToList();
                        List<EmployeeMap> employeeMaps = await _emRepo.GetByEmployeeIdAsync(request.Id, cancellationToken);
                        if (request.employeeMaps != null)
                        {
                            foreach (var item in request.employeeMaps)
                            {
                                EmployeeMap employeeMapExist = employeeMaps.Where(epe => epe.TowerId == item.TowerId).FirstOrDefault();
                                if (employeeMapExist == null)
                                {
                                    EmployeeMap employeeMap = new EmployeeMap();
                                    employeeMap.EmployeeId = request.Id;
                                    employeeMap.TowerId = item.TowerId;
                                    employeeMap.CreatedAt = DateTime.Now;
                                    employeeMap.UpdatedAt = DateTime.Now;
                                    employeeMap.CreatedById = request.UserId;
                                    employeeMap.UpdatedById = request.UserId;
                                    employeeMap.Status = AppEnum.EntityStatus.NORMAL;
                                    //db.EmployeeMap.Add(employeeMap);
                                    await _emRepo.AddAsync(employeeMap);
                                    await _unitOfWork.CommitChangesAsync();

                                    var messageMap = _mapper.Map<DtoCommonEmployeeMapQueue>(employeeMap);
                                    await _publishEndpoint.Publish<DtoCommonEmployeeMapQueue>(messageMap);
                                }
                                else employeeMaps.Remove(employeeMapExist);
                            }
                        }
                        employeeMaps.ForEach(x => x.Status = AppEnum.EntityStatus.DELETED);
                        foreach (var employeeMap in employeeMaps)
                        {
                            var messageMapUpdate = _mapper.Map<DtoCommonEmployeeMapUpdatedQueue>(employeeMap);
                            await _publishEndpoint.Publish<DtoCommonEmployeeMapUpdatedQueue>(messageMapUpdate);
                        }

                        _emRepo.UpdateRange(employeeMaps);
                        await _unitOfWork.CommitChangesAsync();
                        _unitOfWork.CommitTransaction();
                        //await db.SaveChangesAsync();

                        //transaction.Commit();
                        ////create action
                        //IOITResident.Models.EF.Action action = new IOITResident.Models.EF.Action();
                        //action.ActionId = Guid.NewGuid();
                        //action.ActionName = "Sửa nhân viên" + data.FullName;
                        //action.ActionType = "UPDATE";
                        //action.TargetId = data.EmployeeId.ToString();
                        //action.TargetType = "Employee";
                        //action.Logs = JsonConvert.SerializeObject(data);
                        //action.Time = 0;
                        //action.Type = (int)Const.TypeAction.ACTION;
                        //action.CreatedAt = DateTime.Now;
                        //action.UserPushId = userId;
                        //action.UserId = userId;
                        //action.Status = (int)Const.Status.NORMAL;
                        //db.Action.Add(action);

                        //await db.SaveChangesAsync();

                        //def.meta = new Meta(200, Const.UPDATE_SUCCESS_MESSAGE);
                        //def.data = data;
                        //return Ok(def);
                        return exist;
                    }
                    else
                    {
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_EMPLOYEE_UPDATE_FAILED);
                    }
                }
                catch (DbUpdateException e)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_EMPLOYEE_UPDATE_FAILED);
                }
                //}
            }
        }
    }
}
