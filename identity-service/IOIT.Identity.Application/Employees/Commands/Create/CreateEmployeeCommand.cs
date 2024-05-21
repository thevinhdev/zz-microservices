using AutoMapper;
using FluentValidation;
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
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Employees.Commands.Create
{
    public class CreateEmployeeCommand : IRequest<Employee>
    {
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

        public class Validation : AbstractValidator<CreateEmployeeCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty").GreaterThan(0);
                RuleFor(x => x.FullName).NotEmpty().WithMessage("Name not empty");
            }
        }

        public class Handler : IRequestHandler<CreateEmployeeCommand, Employee>
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

            public async Task<Employee> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
            {
                //var entity = _mapper.Map<Employee>(request);

                //await _entityRepository.AddAsync(entity);
                //await _unitOfWork.CommitChangesAsync();

                //return entity;
                //Employee checkCode = db.Employee.AsNoTracking().Where(f => f.Code == data.Code && f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                var checkCode = await _entityRepository.FindByCodeAsync(request.Code, 0, cancellationToken);
                if (checkCode != null)
                {
                    //def.meta = new Meta(212, "Mã đã tồn tại!");
                    //return Ok(def);
                    throw new CommonException("Mã nhân viên đã tồn tại!", ApiConstants.StatusCode.Valid212, ApiConstants.ErrorCode.ERROR_EMPLOYEE_CODE_EXIST);
                }
                //Employee checkPhone = db.Employee.AsNoTracking().Where(f => f.Phone == data.Phone && f.Status != (int)Const.Status.DELETED).FirstOrDefault();
                var checkPhone = await _entityRepository.FindByPhoneAsync(request.Phone, 0, cancellationToken);
                if (checkPhone != null)
                {
                    //def.meta = new Meta(212, "Số điện thoại đã tồn tại!");
                    //return Ok(def);
                    throw new CommonException("Số điện thoại đã tồn tại!", ApiConstants.StatusCode.Valid212, ApiConstants.ErrorCode.ERROR_EMPLOYEE_PHONE_EXIST);
                }

                //using (var transaction = db.Database.BeginTransaction())
                //{
                _unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
                Employee employee = new Employee();
                employee.FullName = request.FullName;
                employee.Code = request.Code;
                employee.Avata = request.Avata;
                employee.ProjectId = request.ProjectId;
                employee.PositionId = request.PositionId;
                employee.DepartmentId = request.DepartmentId;
                employee.Birthday = request.Birthday;
                employee.CardId = request.CardId;
                employee.Phone = request.Phone;
                employee.Email = request.Email;
                employee.Address = request.Address;
                employee.Note = request.Note;
                employee.TypeEmployee = request.TypeEmployee;
                employee.IsMain = request.IsMain;
                employee.CreatedById = request.UserId;
                employee.UpdatedById = request.UserId;
                employee.CreatedAt = DateTime.Now;
                employee.UpdatedAt = DateTime.Now;
                employee.Status = AppEnum.EntityStatus.NORMAL;
                //await db.Employee.AddAsync(employee);
                await _entityRepository.AddAsync(employee);
                try
                {
                    await _unitOfWork.CommitChangesAsync();

                    //Gọi Producers để thêm vào các service khác
                    var message = _mapper.Map<DtoCommonEmployeeQueue>(employee);
                    await _publishEndpoint.Publish<DtoCommonEmployeeQueue>(message);

                    //await db.SaveChangesAsync();
                    request.EmployeeId = employee.Id;

                    if (request.EmployeeId > 0)
                    {
                        if (request.employeeMaps != null)
                        {
                            foreach (var item in request.employeeMaps)
                            {
                                EmployeeMap employeeMap = new EmployeeMap();
                                employeeMap.EmployeeId = employee.Id;
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
                        }
                        //await db.SaveChangesAsync();
                        _unitOfWork.CommitTransaction();
                        //transaction.Commit();
                        ////create action
                        //Models.EF.Action action = new Models.EF.Action();
                        //action.ActionId = Guid.NewGuid();
                        //action.ActionName = "Thêm mới nhân viên" + data.FullName;
                        //action.ActionType = "CREATE";
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

                        //def.meta = new Meta(200, Const.CREATE_SUCCESS_MESSAGE);
                        //def.data = data;
                        //return Ok(def);
                        return employee;
                    }
                    else
                    {
                        _unitOfWork.RollbackTransaction();
                        throw new UnknowException(ApiConstants.MessageResource.ERROR_500_MESSAGE, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_EMPLOYEE_CREATE_FAILED);
                    }
                }
                catch (DbUpdateException e)
                {
                    _unitOfWork.RollbackTransaction();
                    throw new UnknowException(e.Message, ApiConstants.StatusCode.Error500, ApiConstants.ErrorCode.ERROR_EMPLOYEE_CREATE_FAILED);
                }
                //}
            }
        }
    }
}
