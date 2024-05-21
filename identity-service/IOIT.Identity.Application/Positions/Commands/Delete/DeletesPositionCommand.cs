using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Application.Positions.Commands.Delete
{
    public class DeletesPositionCommand : IRequest<List<Position>>
    {
        public int[] data { get; set; }

        public class Validation : AbstractValidator<DeletesPositionCommand>
        {
            public Validation()
            {
                RuleFor(x => x.data).NotEmpty().WithMessage("data not empty");
            }
        }
        public class Handler : IRequestHandler<DeletesPositionCommand, List<Position>>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<Position> _entityRepository;
            private readonly IEmployeeAsyncRepository _empRepo;

            public Handler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<Position> entityRepository,
                IEmployeeAsyncRepository empRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _entityRepository = entityRepository;
                _empRepo = empRepo;
            }

            public async Task<List<Position>> Handle(DeletesPositionCommand request, CancellationToken cancellationToken)
            {
                List<Position> listData = new List<Position>();
                bool hasDelete = false;
                for (int i = 0; i < request.data.Count(); i++)
                {
                    //Position position = await db.Position.Where(e => e.PositionId == data[i] && e.Status != (int)Const.Status.DELETED).FirstOrDefaultAsync();
                    Position position = await _entityRepository.GetByKeyAsync(request.data[i]);
                    if (position == null)
                    {
                        continue;
                    }

                    // Kiểm tra nếu có nhân viên ràng buộc thì không được xóa
                    //List<Employee> employees = db.Employee.Where(a => a.PositionId == position.PositionId && a.Status != (int)Const.Status.DELETED).ToList();
                    var employees = await _empRepo.FindByPositionAsync(position.Id, cancellationToken);
                    if (employees.Count() > 0)
                    {
                        continue;
                    }

                    hasDelete = true;
                    position.UpdatedAt = DateTime.Now;
                    position.Status = AppEnum.EntityStatus.DELETED;
                    //db.Position.Update(position);
                    var entity1 = _mapper.Map<Position>(position);
                    _entityRepository.Update(entity1);
                    //create action
                    //IOITResident.Models.EF.Action action = new IOITResident.Models.EF.Action();
                    //action.ActionId = Guid.NewGuid();
                    //action.ActionName = "Xóa chức vụ " + position.Name;
                    //action.ActionType = "DELETE";
                    //action.TargetId = position.PositionId.ToString();
                    //action.TargetType = "Position";
                    //action.Logs = JsonConvert.SerializeObject(position);
                    //action.Time = 0;
                    //action.Type = (int)Const.TypeAction.ACTION;
                    //action.CreatedAt = DateTime.Now;
                    //action.UserPushId = userId;
                    //action.UserId = userId;
                    //action.Status = (int)Const.Status.NORMAL;
                    //db.Action.Add(action);
                    listData.Add(position);
                }

                if (!hasDelete)
                {
                    //def.meta = new Meta(215, "Tất các các chức vụ đều còn nhân viên ràng buộc! Không thể xóa.");
                    //return Ok(def);
                    throw new CommonException("Tất các các chức vụ đều còn nhân viên ràng buộc! Không thể xóa.", 215, ApiConstants.ErrorCode.ERROR_POSITION_DELETE_FAILED);
                }

                //var entity = _mapper.Map<Position>(data);
                //_entityRepository.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return listData;
            }

        }
    }
}
