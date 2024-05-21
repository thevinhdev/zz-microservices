using AutoMapper;
using FluentValidation;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Users.ViewModels;
using IOIT.Identity.Domain.Entities;
using IOIT.Identity.Domain.Interfaces;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOIT.Identity.Application.Users.Commands.Update
{
    public class ChangeLanguageCommand : IRequest<String>
    {
        public int UserId { get; set; }
        public int LanguageId { get; set; }
        public string Access_key { get; set; }
        public int ProjectId { get; set; }
        public string RoleCode { get; set; }

        public class Validation : AbstractValidator<ChangeLanguageCommand>
        {
            public Validation()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("id not empty");
            }
        }

        public class UserHandler : IRequestHandler<ChangeLanguageCommand, String>
        {
            private readonly IMapper _mapper;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserAsyncRepository _userRepo;
            private readonly IConfiguration _configuration;

            public UserHandler(IMapper mapper,
                IUnitOfWork unitOfWork,
                IConfiguration configuration,
                IUserAsyncRepository userRepo)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _configuration = configuration;
                _userRepo = userRepo;
            }
            public async Task<String> Handle(ChangeLanguageCommand request, CancellationToken cancellationToken)
            {
                //User exist = db.User.Where(d => d.UserId == id && d.Status != (int)Const.Status.DELETED).FirstOrDefault();
                User exist = await _userRepo.GetByKeyAsync(request.UserId);
                if (exist == null)
                {
                    //def.meta = new Meta(404, "Not Found");
                    //return Ok(def);
                    throw new BadRequestException(Resources.USER_NOT_FOUND, ApiConstants.ErrorCode.ERROR_USER_NOT_EXIST);
                }
                if (request.LanguageId == exist.LanguageId)
                {
                    //def.meta = new Meta(212, "LanguageId same languageId old");
                    //return Ok(def);
                    throw new CommonException("Vui lòng chọn ngôn ngữ khác ngôn ngữ hiện tại", 212, ApiConstants.ErrorCode.ERROR_LAMGUAGE_ALREADY_EXIST);
                }
                //using (var transaction = db.Database.BeginTransaction())
                //{
                exist.LanguageId = request.LanguageId;
                exist.UpdatedById = request.UserId;
                exist.UpdatedAt = DateTime.Now;
                //db.User.Update(exist);
                //await db.SaveChangesAsync();
                //transaction.Commit();
                var claims = new List<Claim>
                                {
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                    new Claim(ClaimTypes.NameIdentifier, exist.Id.ToString()),
                                    new Claim(ClaimTypes.Name, exist.FullName != null ? exist.FullName : ""),
                                    new Claim("UserId", exist.Id.ToString()),
                                    new Claim("UserMapId", exist.UserMapId != null ? exist.UserMapId.ToString() : ""),
                                    new Claim("Type", exist.Type != null ? exist.Type.ToString() : ""),
                                    new Claim("Username", exist.UserName != null ? exist.UserName.ToString() : ""),
                                    new Claim("Language", request.LanguageId != null ? request.LanguageId.ToString() : ""),
                                    new Claim("AccessKey", request.Access_key ?? ""),
                                    new Claim("RoleCode", request.RoleCode ?? ""),
                                    new Claim("ProjectId", request.ProjectId.ToString() ?? "")
                                };

                string JwtKey = _configuration["AppSettings:JwtKey"];
                string JwtExpireDays = _configuration["AppSettings:JwtExpireDays"];
                string JwtIssuer = _configuration["AppSettings:JwtIssuer"];

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(Convert.ToDouble(JwtExpireDays));

                var token = new JwtSecurityToken(
                            JwtIssuer,
                            JwtIssuer,
                            claims,
                            expires: expires,
                            signingCredentials: creds
                        );

                string access_token = new JwtSecurityTokenHandler().WriteToken(token);
                //def.meta = new Meta(200, "Success");
                //def.data = exist;
                //return Ok(def);
                var entity = _mapper.Map<User>(exist);

                _userRepo.Update(entity);
                await _unitOfWork.CommitChangesAsync();
                return access_token;

            }
        }
    }
}
