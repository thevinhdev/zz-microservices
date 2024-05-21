using System;
using Microsoft.AspNetCore.Mvc;
using IOIT.Identity.Application.Users.Commands.Update;
using IOIT.Identity.Application.Users.Queries;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IOIT.Identity.Application.Users.Commands.Create;

namespace IOIT.Identity.Api.Controllers.ApiApp
{
    [Route("api/app/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        //private readonly IConfiguration _configuration;
        //private IHostingEnvironment _hostingEnvironment;

        //public UserController(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //    _configuration = configuration;
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserAppQuery command)
        {
            Console.WriteLine("APP LOGIN =>>>>>>>>>>>>>>>>>> " + command.Username.ToString());
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                resData.MetaMess,
                resData.MetaCode
                ));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (command.PhoneMain == null || command.PhoneMain == "")
            {
                return Res(new DefaultResponse().Success(
                null,
                "PhoneMain Null!",
                ApiConstants.StatusCode.Valid211
                ));
            }
            if (command.PhoneMain.Length < 9)
            {
                return Res(new DefaultResponse().Success(
               null,
               "PhoneMain length < 9!",
               ApiConstants.StatusCode.Valid211
               ));
            }

            var resData = await _mediator.Send(command);

            return Res(resData);
        }

        [HttpPost("registerGuest")]
        public async Task<IActionResult> RegisterGuest([FromBody] UserRegisterGuestCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (command.PhoneMain == null || command.PhoneMain == "" || command.PhoneGuest == null || command.PhoneGuest == "")
            {
                return Res(new DefaultResponse().Success(
                null,
                "PhoneMain or PhoneGuest Null!",
                ApiConstants.StatusCode.Valid211
                ));
            }
            if (command.PhoneMain.Length < 9 || command.PhoneGuest.Length < 9)
            {
                return Res(new DefaultResponse().Success(
               null,
               "PhoneMain or PhoneGuest length < 9!",
               ApiConstants.StatusCode.Valid211
               ));
            }

            var resData = await _mediator.Send(command);

            return Res(resData);
        }

        [HttpPost("checkPhoneMain")]
        public async Task<IActionResult> CheckPhoneMain([FromBody] CheckPhoneMainCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (command.PhoneMain == null || command.PhoneMain == "" || command.PhoneGuest == null || command.PhoneGuest == "")
            {
                return Res(new DefaultResponse().Success(
                null,
                "PhoneMain or PhoneGuest Null!",
                ApiConstants.StatusCode.Valid211
                ));
            }
            if (command.PhoneMain.Length < 9 || command.PhoneGuest.Length < 9)
            {
                return Res(new DefaultResponse().Success(
               null,
               "PhoneMain or PhoneGuest length < 9!",
               ApiConstants.StatusCode.Valid211
               ));
            }

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPut("accsessRegisterCode/{id}/{code}")]
        public async Task<IActionResult> AccsessRegisterCode(int id, string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }

            var resData = await _mediator.Send(new AccsessRegisterCodeCommand() { id = id, code = code });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPut("sendChangePassCode/{phone}")]
        public async Task<IActionResult> SendChangePassCode(string phone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }

            var resData = await _mediator.Send(new SendChangePassCodeCommand() { phone = phone });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPut("activeGuest/{id}")]
        public async Task<IActionResult> ActiveGuest(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }

            var resData = await _mediator.Send(new ActiveGuestCommand() { id = id });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPut("newPassword/{id}")]
        public async Task<IActionResult> NewPassword(long id, NewPasswordCommand command)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }

            if (command.NewPassword == null && command.NewPassword == "")
            {
                return Res(new DefaultResponse().Success(
                   null,
                   "NewPassword null!",
                   ApiConstants.StatusCode.Valid211
                   ));
            }

            if (command.NewPassword.Length < 6)
            {
                //def.meta = new Meta(2112, "NewPassword length < 6");
                //return Ok(def);
                return Res(new DefaultResponse().Success(
                   null,
                   "NewPassword length < 6!",
                   2112
                   ));
            }
            command.id = id;

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }


        [HttpPut("changeLanguage"), Authorize]
        public async Task<IActionResult> ChangeLanguage(ChangeLanguageCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            string roleCode = identity.Claims.Where(c => c.Type == "RoleCode").Select(c => c.Value).SingleOrDefault();
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }

            if (command.LanguageId <= 0)
            {
                //def.meta = new Meta(211, "LanguageId not positive numbers");
                //return Ok(def);
                //return Ok(def);
                return Res(new DefaultResponse().Success(
                   null,
                   "LanguageId not positive numbers!",
                   ApiConstants.StatusCode.Valid211
                ));
            }

            command.ProjectId = projectId;
            command.Access_key = access_key;
            command.RoleCode = roleCode;

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [Authorize]
        [HttpPut("changePassword/{id}")]
        public async Task<IActionResult> ChangePassword(long id, ChangePasswordCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            if (!ModelState.IsValid || id != userId)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (command.CurrentPassword == null || command.NewPassword == null)
            {
                //def.meta = new Meta(211, "CurrentPassword or NewPassword null");
                //return Ok(def);
                return Res(new DefaultResponse().Success(
                   null,
                   "CurrentPassword or NewPassword null!",
                   ApiConstants.StatusCode.Valid211
                ));
            }
            command.id = id;

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [Authorize]
        [HttpPut("changeInfo")]
        public async Task<IActionResult> ChangeInfo(ChangeInfoCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPut("requildPass")]
        public async Task<IActionResult> RequildPass(RequildPassCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [Authorize]
        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveAccount(long id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            if (userId != id)
            {
                return Unauthorized("Not permission for resource.");
            }
            var resData = await _mediator.Send(new RemoveAccountCommand()
            {
                UserId = id
            });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

    }
}
