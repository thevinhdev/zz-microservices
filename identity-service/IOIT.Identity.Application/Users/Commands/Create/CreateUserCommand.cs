using AutoMapper;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities.Indentity;
using IOIT.Identity.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using IOIT.Identity.Application.Common.Extensions;

namespace IOIT.Identity.Application.Users.Commands.Create
{
    public class CreateUserCommand : IRequest<ResCreateUser>
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


        public CreateUserCommand(User req)
        {
            UserName = req.UserName;
            Password = req.Password;
            Email = req.Email;
            FullName = req.FullName;
            Avatar = req.Avata;
            Code = req.Code;
            Note = req.Note;
            Address = req.Address;
            Phone = req.Phone;
        }

        public class CreateAccountCommandHandler : IRequestHandler<CreateUserCommand, ResCreateUser>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IAsyncRepository<User> _accountRepository;

            public CreateAccountCommandHandler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IAsyncRepository<User> accountRepository)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _accountRepository = accountRepository;
            }
            public async Task<ResCreateUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<User>(request);
                entity.Code = "XXXX-0101010-YYYYY";

                await _accountRepository.AddAsync(entity);
                await _unitOfWork.CommitChangesAsync();


                return _mapper.Map<ResCreateUser>(entity);
            }
        }
    }
}
