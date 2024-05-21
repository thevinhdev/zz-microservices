using IOIT.Identity.Api.Filters;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces.Producer;
using IOIT.Identity.Application.Functions.Commands.Create;
using IOIT.Identity.Application.Functions.Commands.Delete;
using IOIT.Identity.Application.Functions.Commands.Update;
using IOIT.Identity.Application.Functions.Queries;
using IOIT.Identity.Application.UserRoles.Commands.Create;
using IOIT.Identity.Application.UserRoles.Commands.Delete;
using IOIT.Identity.Application.UserRoles.Commands.Update;
using IOIT.Identity.Application.UserRoles.Queries;
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
    public class UserRoleController : BaseController
    {
        private static string functionCode = "QLND";
        private readonly INotificationProducer _notificationProducer;

        public UserRoleController(INotificationProducer notificationProducer)
        {
            _notificationProducer = notificationProducer;
        }

        [HttpGet("GetByPage")]
        public async Task<IActionResult> GetByPage([FromQuery] GetUserRoleByPagingQuery command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            int roleLevel = int.Parse(identity.Claims.Where(c => c.Type == "RoleLevel").Select(c => c.Value).SingleOrDefault());
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.VIEW))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }
            //command.roleLevel = roleLevel;
            //command.roleMax = roleMax;
            //command.ProjectId = projectId;
            if (roleMax != 1)
            {
                if (command.query != null && command.query != "")
                    command.query += " AND RoleLevel > " + roleLevel + " AND ProjectId=" + projectId;
                else
                    command.query = "RoleLevel > " + roleLevel + " AND ProjectId=" + projectId;
            }
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                            resData.Results,
                            ApiConstants.MessageResource.ACCTION_SUCCESS,
                            ApiConstants.StatusCode.Success200,
                            resData.RowCount
                            ));
        }

        [HttpGet("listEmployee")]
        public async Task<IActionResult> ListEmployee([FromQuery] GetUserRoleEmpByPagingQuery command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            int roleLevel = int.Parse(identity.Claims.Where(c => c.Type == "RoleLevel").Select(c => c.Value).SingleOrDefault());
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.VIEW))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }
            command.roleLevel = roleLevel;
            command.roleMax = roleMax;
            command.ProjectId = projectId;
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
            var resData = await _mediator.Send(new GetUserRoleByIdQuery() { Id = id });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserRoleCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
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
                $"Thêm mới người dùng {resData.FullName}",
                "CREATE",
                resData.Id.ToString(),
                "User",
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
        public async Task<IActionResult> Put(long id, [FromBody] UpdateUserRoleCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.UPDATE))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (id != command.UserId)
            {
                return Res(new DefaultResponse().Success(
               ApiConstants.MessageResource.BAD_REQUEST_MESSAGE,
               ApiConstants.StatusCode.Error400
               ));
            }
            command.Id = id;
            command.UserId = userId;
            var resData = await _mediator.Send(command);

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Sửa người dùng {resData.FullName}",
                "UPDATE",
                resData.Id.ToString(),
                "User",
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
        public async Task<IActionResult> Delete(long id)
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

            var resData = await _mediator.Send(new DeleteUserRoleCommand() { Id = id, UserId = userId });

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Xóa người dùng {resData.FullName}",
                "DELETE",
                resData.Id.ToString(),
                "User",
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
        public async Task<IActionResult> Deletes([FromBody] long[] data)
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
            var resData = await _mediator.Send(new DeletesUserRoleCommand() { data = data });

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            foreach (var item in resData)
            {
                reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Xóa người dùng {item.FullName}",
                "DELETE",
                item.Id.ToString(),
                "User",
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

        //Get nhân viên phòng ban kỹ thuật theo mã quyền 
        [HttpGet("GetEmpByRoleQuery/{ProjectId}")]
        public async Task<IActionResult> GetEmpByRoleQuery(int ProjectId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            var resData = await _mediator.Send(new GetEmpByRoleQuery() { ProjectId = ProjectId, UserId = userId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        //Get all nhân viên phòng ban kỹ thuật theo mã quyền 
        [HttpGet("GetEmpByRoleAllQuery/{ProjectId}")]
        public async Task<IActionResult> GetEmpByRoleAllQuery(int ProjectId)
        {
            var resData = await _mediator.Send(new GetEmpByRoleQuery() { ProjectId = ProjectId, UserId = -1 });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        //Get user theo mã quyền
        [HttpGet("GetUserByRoleQuery/{ProjectId}/{RoleCode}")]
        public async Task<IActionResult> GetUserByRoleQuery(int ProjectId, string RoleCode,[FromQuery] int? towerId)
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            var resData = await _mediator.Send(new GetUserByRoleQuery() { ProjectId = ProjectId, RoleCode = RoleCode, TowerId = ((int)(towerId != null ? towerId : 0)) });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        //Get user theo chức năng
        [HttpGet("GetUserByFuctionQuery/{ProjectId}/{FunctionCode}")]
        public async Task<IActionResult> GetUserByFuctionQuery(int ProjectId, string FunctionCode)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            var resData = await _mediator.Send(new GetUserByFuctionQuery() { ProjectId = ProjectId, FunctionCode = FunctionCode, UserId = userId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        //Get danh sách nhân viên ban quản lý theo Site để nhận thông báo
        [HttpGet("GetUserManagementByProjectIdQuery/{ProjectId}")]
        public async Task<IActionResult> GetUserManagementByProjectIdQuery(int ProjectId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            var resData = await _mediator.Send(new GetUserManagementByProjectIdQuery() { ProjectId = ProjectId, UserId = userId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
            ));
        }

        //Get danh sách ban quản lý và nhân viên kỹ thuật nhận thông báo yêu cầu hỗ trợ
        [HttpPost("GetEmployeeReceiveNotificationRequireSupport")]
        public async Task<IActionResult> GetEmployeeReceiveNotificationRequireSupport(GetEmployeeReceiveNotificationRequireSupportQuery command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
            ));
        }

        //Get nhân viên bằng UserMapId
        [HttpGet("GetEmployeeByUserMapId/{UserMapId}")]
        public async Task<IActionResult> GetEmployeeByUserMapId(long UserMapId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            var resData = await _mediator.Send(new GetEmployeeByUserMapIdQuery() { UserMapId = UserMapId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
            ));
        }

        //Get Cư dân bằng UserMapId
        [HttpGet("GetResidentByUserMapId/{UserMapId}")]
        public async Task<IActionResult> GetResidentByUserMapId(long UserMapId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            var resData = await _mediator.Send(new GetResidentByUserMapIdQuery() { UserMapId = UserMapId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
            ));
        }

        //Get User bằng UserId
        [HttpGet("GetUserById/{UserId}")]
        public async Task<IActionResult> GetUserById(int UserId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");

            var resData = await _mediator.Send(new GetUserByIdQuery() { Id = UserId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
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
