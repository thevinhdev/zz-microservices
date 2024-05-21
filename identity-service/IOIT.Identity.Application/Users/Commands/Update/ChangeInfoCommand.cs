using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Commands.Update
{
    public class ChangeInfoCommand : IRequest<ChangeInfoCommand>
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string Avata { get; set; }
        public byte? Sex { get; set; }
        public string Birthday { get; set; }
        public string CardId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public class Validation : AbstractValidator<ChangeInfoCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId not empty");
            }
        }

        public class UserHandler : IRequestHandler<ChangeInfoCommand, ChangeInfoCommand>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IResidentAsyncRepository _residentRepo;
            private readonly IEmployeeAsyncRepository _empRepo;

            public UserHandler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IUserAsyncRepository userRepo,
                IResidentAsyncRepository residentRepo,
                IEmployeeAsyncRepository empRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _userRepo = userRepo;
                _residentRepo = residentRepo;
                _empRepo = empRepo;
            }
            public async Task<ChangeInfoCommand> Handle(ChangeInfoCommand request, CancellationToken cancellationToken)
            {
                //User exist = db.User.Where(d => d.UserId == id && d.Status != (int)Const.Status.DELETED).FirstOrDefault();
                User exist = await _userRepo.GetByKeyAsync(request.UserId);
                if (exist == null)
                {
                    //def.meta = new Meta(404, "Not Found");
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }

                exist.FullName = request.FullName;
                exist.Avata = request.Avata;
                exist.Email = request.Email;
                exist.Address = request.Address;
                exist.CardId = request.CardId;
                exist.UpdatedById = request.UserId;
                exist.UpdatedAt = DateTime.Now;
                //db.User.Update(exist);
                //await db.SaveChangesAsync();
                //transaction.Commit();

                //def.meta = new Meta(200, "Success");
                //def.data = exist;
                //return Ok(def);
                var entity = _mapper.Map<User>(exist);
                _userRepo.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                if (exist.Type == (int)AppEnum.TypeUser.RESIDENT_MAIN || exist.Type == (int)AppEnum.TypeUser.RESIDENT_GUEST)
                {
                    //Resident resident = db.Resident.Where(e => e.ResidentId == exist.UserMapId && e.Status != (int)AppEnum.EntityStatus.DELETED).FirstOrDefault();
                    Resident resident = await _residentRepo.GetByKeyAsync((long)exist.UserMapId);
                    if (resident != null)
                    {
                        resident.FullName = request.FullName;
                        resident.Sex = request.Sex + "";
                        try
                        {
                            if (request.Birthday != null)
                                resident.Birthday = DateTime.Parse(request.Birthday);
                        }
                        catch
                        {
                            //transaction.Rollback();
                            //def.meta = new Meta(215, "Birthday error");
                            //return Ok(def);
                        }
                        resident.CardId = request.CardId;
                        resident.Email = request.Email;
                        resident.Avata = request.Avata;
                        resident.Address = request.Address;
                        exist.UpdatedAt = DateTime.Now;
                        //db.Resident.Update(resident);
                        //var entity = _mapper.Map<R>(exist);
                        _residentRepo.Update(resident);
                        await _unitOfWork.CommitChangesAsync();
                    }
                }
                else
                {
                    //Employee employee = db.Employee.Where(e => e.EmployeeId == exist.UserMapId && e.Status != (int)Const.Status.DELETED).FirstOrDefault();
                    Employee employee = await _empRepo.GetByKeyAsync((int)exist.UserMapId);
                    if (employee != null)
                    {
                        employee.FullName = request.FullName;
                        //employee.Sex = data.Sex + "";
                        try
                        {
                            if (request.Birthday != null)
                                employee.Birthday = DateTime.Parse(request.Birthday);
                        }
                        catch
                        {
                            //transaction.Rollback();
                            //def.meta = new Meta(215, "Birthday error");
                            //return Ok(def);
                        }
                        employee.CardId = request.CardId;
                        employee.Email = request.Email;
                        employee.Avata = request.Avata;
                        employee.Address = request.Address;
                        exist.UpdatedAt = DateTime.Now;
                        //db.Employee.Update(employee);
                        _empRepo.Update(employee);
                        await _unitOfWork.CommitChangesAsync();
                    }
                }

                return request;

            }
        }
    }
}
