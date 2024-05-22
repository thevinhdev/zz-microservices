using IOIT.Identity.Application.Users.Commands.Update;
using IOIT.Identity.Application.Users.Queries;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using IOIT.Identity.Application.LogSystem.Queries;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Castle.Core.Resource;

namespace IOIT.Identity.Api.Controllers.ApiCms
{
    //[Authorize]
    [Route("api/cms/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private static string functionCode = "QLND";
        private static string functionCodeBQL = "NDBQL";
        private static string functionCodeResident = "NDCD";
        private IConfiguration _configuration { get; }

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest input)
        {
            //var resData = await _mediator.Send(command);
            var resData = await _mediator.Send(new LoginUserCmsQuery(input));
            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [Authorize]
        [HttpPut("changePass/{id}")]
        public async Task<IActionResult> ChangePass(long id, [FromBody] UserChangePassCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (id != userId)
            {
                //def.meta = new Meta(400, Const.BAD_REQUEST_MESSAGE);
                //return Ok(def);
                return Res(new DefaultResponse().Success(
                null,
                ApiConstants.MessageResource.BAD_REQUEST_MESSAGE,
                ApiConstants.StatusCode.Error400
                ));
            }
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                "Đổi mật khẩu thành công!",
                ApiConstants.StatusCode.Success200
                ));
        }

        [Authorize]
        [HttpPut("adminChangePass/{id}")]
        public async Task<IActionResult> AdminChangePass(int id, [FromBody] AdminChangePassCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            if (!CheckRole.CheckRoleByCode(access_key, command.IsTypeBql ? functionCodeBQL : functionCodeResident, (int)AppEnum.Action.UPDATE))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [Authorize]
        [HttpPut("lockUser/{id}/{k}")]
        public async Task<IActionResult> LockUser(long id, int k)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            var resData = await _mediator.Send(new LockUserCommand() { id = id, k = k });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.UPDATE_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [Authorize]
        [HttpGet("infoUser/{id}")]
        public async Task<IActionResult> InfoUser(long id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (id != userId)
            {
                //def.meta = new Meta(400, Const.BAD_REQUEST_MESSAGE);
                //return Ok(def);
                return Res(new DefaultResponse().Success(
                null,
                ApiConstants.MessageResource.BAD_REQUEST_MESSAGE,
                ApiConstants.StatusCode.Error400
                ));
            }
            var resData = await _mediator.Send(new InfoUserQuery() { id = id});

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.UPDATE_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [Authorize]
        [HttpGet("getUserByResidentId/{residentId}")]
        public async Task<IActionResult> GetUserByResidentId(long residentId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            
            var resData = await _mediator.Send(new GetUserByResidentIdQuery() { ResidentId = residentId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.UPDATE_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [Authorize]
        [HttpPut("changeInfoUser")]
        public async Task<IActionResult> ChangeInfoUser([FromBody] ChangeInfoUserCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (command.UserId != userId)
            {
                return Res(new DefaultResponse().Success(
                null,
                ApiConstants.MessageResource.BAD_REQUEST_MESSAGE,
                ApiConstants.StatusCode.Error400
                ));
            }
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.UPDATE_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpGet("GetLogSystem")]
        public async Task<IActionResult> GetLogSystem([FromQuery] GetLogSystemPagingQuery command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            if (!CheckRole.CheckRoleByCode(access_key, "LOG_ACTIVITY", (int)AppEnum.Action.VIEW))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }

            command.ApiGateway = _configuration["AppSettings:domain"];
            command.Token = Request.Headers[HeaderNames.Authorization];

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData.data,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.total
            ));
        }
    }
}
