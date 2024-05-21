using IOIT.Identity.Api.Filters;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces.Producer;
using IOIT.Identity.Application.FunctionRoles.Commands.Create;
using IOIT.Identity.Application.FunctionRoles.Commands.Delete;
using IOIT.Identity.Application.FunctionRoles.Commands.Update;
using IOIT.Identity.Application.FunctionRoles.Queries;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.Commons.Enum;
using IOIT.Shared.Helpers;
using IOIT.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static IOIT.Shared.Commons.Enum.AppEnum;

namespace IOIT.Identity.Api.Controllers.ApiCms
{
    [Route("api/cms/[controller]"), AuthorizeFilter]
    [ApiController]
    public class FunctionRoleController : BaseController
    {
        private static string functionCode = "QLQ";
        private readonly INotificationProducer _notificationProducer;

        public FunctionRoleController(INotificationProducer notificationProducer)
        {
            _notificationProducer = notificationProducer;
        }

        [HttpGet("GetByPage")]
        public async Task<IActionResult> GetByPage([FromQuery] GetFunctionRoleByPagingQuery command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.VIEW))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
               resData.Results,
               ApiConstants.MessageResource.ACCTION_SUCCESS,
               ApiConstants.StatusCode.Success200,
               resData.RowCount
               ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var resData = await _mediator.Send(new GetFunctionRoleByIdQuery() { Id = id });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateFunctionRoleCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.CREATE))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            command.UserId = userId;
            var resData = await _mediator.Send(command);

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Thêm mới quyền {resData.Name}",
                "CREATE",
                resData.Id.ToString(),
                "Role",
                null,
                0,
                JsonConvert.SerializeObject(resData),
                IpAddress(),
                0,
                TypeAction.ACTION,
                command.UserId));
            await _notificationProducer.NotificationActionCreate(reqMessages);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ADD_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateFunctionRoleCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.UPDATE))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }
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
            if (id != command.RoleId)
            {
                return Res(new DefaultResponse().Success(
                    null,
                  ApiConstants.MessageResource.BAD_REQUEST_MESSAGE,
                  ApiConstants.StatusCode.Error400
                  ));
            }
            command.Id = id;
            command.UserId = userId;
            var resData = await _mediator.Send(command);

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Sửa quyền {resData.Name}",
                "UPDATE",
                resData.Id.ToString(),
                "Role",
                null,
                0,
                JsonConvert.SerializeObject(resData),
                IpAddress(),
                0,
                TypeAction.ACTION,
                command.UserId));
            await _notificationProducer.NotificationActionCreate(reqMessages);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.UPDATE_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.DELETED))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }

            var resData = await _mediator.Send(new DeleteFunctionRoleCommand() { Id = id, UserId = userId });

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Xóa quyền {resData.Name}",
                "DELETE",
                resData.Id.ToString(),
                "Role",
                null,
                0,
                JsonConvert.SerializeObject(resData),
                IpAddress(),
                0,
                TypeAction.ACTION,
                userId));
            await _notificationProducer.NotificationActionCreate(reqMessages);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.DELETE_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPut("deletes")]
        public async Task<IActionResult> Deletes([FromBody] int[] data)
        {
            var identity = (ClaimsIdentity)User.Identity;
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.DELETED))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (data == null)
            {
                //def.meta = new Meta(211, Const.NOT_FOUND_DELETE_MESSAGE);
                //return Ok(def);
                return Res(new DefaultResponse().Success(
                    null,
                  ApiConstants.MessageResource.NOT_FOUND_DELETE_MESSAGE,
                  ApiConstants.StatusCode.Valid211
              ));
            }

            if (data.Count() == 0)
            {
                return Res(new DefaultResponse().Success(
                    null,
                  ApiConstants.MessageResource.NOT_FOUND_DELETE_MESSAGE,
                  ApiConstants.StatusCode.Valid211
              ));
            }
            var resData = await _mediator.Send(new DeletesFunctionRoleCommand() { data = data, UserId = userId });

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            foreach (var item in resData)
            {
                reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Xóa quyền {item.Name}",
                "DELETE",
                item.Id.ToString(),
                "Role",
                null,
                0,
                JsonConvert.SerializeObject(item),
                IpAddress(),
                0,
                TypeAction.ACTION,
                userId));
            }

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.DELETE_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        private string IpAddress()
        {
            return "192.168.1.1";
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

    }
}
