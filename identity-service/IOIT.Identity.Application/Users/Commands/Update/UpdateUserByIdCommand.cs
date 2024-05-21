using AutoMapper;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities.Indentity;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Commands.Update
{
    public class UpdateUserByIdCommand : IRequest<Boolean>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }

        public class UpdateUserByIdHandler : IRequestHandler<UpdateUserByIdCommand, Boolean>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<User> _accountRepository;

            public UpdateUserByIdHandler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<User> accountRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _accountRepository = accountRepository;
            }
            public async Task<bool> Handle(UpdateUserByIdCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<User>(request);

                _accountRepository.Update(entity);
                await _unitOfWork.CommitChangesAsync();

                return true;
            }
        }
    }
}
