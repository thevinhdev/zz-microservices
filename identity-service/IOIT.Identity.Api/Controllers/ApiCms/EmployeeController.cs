using IOIT.Identity.Api.Filters;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces.Producer;
using IOIT.Identity.Application.Employees.Commands.Create;
using IOIT.Identity.Application.Employees.Commands.Delete;
using IOIT.Identity.Application.Employees.Commands.Update;
using IOIT.Identity.Application.Employees.Queries;
using IOIT.Identity.Application.FunctionRoles.Commands.Delete;
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
    //[Authorize]
    [Route("api/cms/[controller]"), AuthorizeFilter]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private static string functionCode = "DSNV";
        private readonly INotificationProducer _notificationProducer;

        public EmployeeController(INotificationProducer notificationProducer)
        {
            _notificationProducer = notificationProducer;
        }

        [HttpGet("GetByPage")]
        public async Task<IActionResult> GetByPage([FromQuery] GetEmployeeByPagingQuery command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            string roleCode = identity.Claims.Where(c => c.Type == "RoleCode").Select(c => c.Value).SingleOrDefault();
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.VIEW))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_VIEW_MESSAGE);
            }
            command.ProjectId = -1;
            if (roleMax != 1 && roleCode != "HO")
            {
                command.ProjectId = projectId;
            }
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                 resData.Data,
                 (int)resData.Metadata,
                 ApiConstants.MessageResource.ACCTION_SUCCESS,
                 ApiConstants.StatusCode.Success200
                 ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            //int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            //string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            var resData = await _mediator.Send(new GetEmployeeByIdQuery() { Id = id });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEmployeeCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.CREATE))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }
            command.UserId = userId;
            var resData = await _mediator.Send(command);

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Thêm mới nhân viên {resData.FullName}",
                "CREATE",
                resData.Id.ToString(),
                "Employee",
                null,
                resData.ProjectId,
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
        public async Task<IActionResult> Put(int id, [FromBody] UpdateEmployeeCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.UPDATE))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }

            command.Id = id;
            var resData = await _mediator.Send(command);

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Sửa nhân viên {resData.FullName}",
                "UPDATE",
                resData.Id.ToString(),
                "Employee",
                null,
                resData.ProjectId,
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
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.DELETED))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }

            var resData = await _mediator.Send(new DeleteEmployeeCommand {Id = id, UserId = userId});

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Xóa nhân viên {resData.FullName}",
                "DELETE",
                resData.Id.ToString(),
                "Employee",
                null,
                resData.ProjectId,
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
            var resData = await _mediator.Send(new DeletesEmployeeCommand() { data = data });

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            foreach (var item in resData)
            {
                reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Xóa nhân viên {item.FullName}",
                "DELETE",
                item.Id.ToString(),
                "Employee",
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

        [HttpGet("GetEmployeeHandle/{TowerId}/{DepartmentId}")]
        public async Task<IActionResult> GetEmployeeHandle(int TowerId, int DepartmentId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            //int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            //int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            //string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            //if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.VIEW))
            //{
            //    return Res(new DefaultResponse().Success(
            //        null,
            //    ApiConstants.MessageResource.NOPERMISION_VIEW_MESSAGE,
            //    ApiConstants.StatusCode.NoPermision
            //    ));
            //}
            
            var resData = await _mediator.Send(new GetEmployeeHandle() { TowerId= TowerId, DepartmentId = DepartmentId, UserMapId = userMapId });

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
                ));
        }

        //Get danh sách employeemap theo Site
        [HttpGet("GetEmployeeMaps")]
        public async Task<IActionResult> GetAllDepartmentMaps([FromQuery] GetEmployeeMapsQuery query)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();


            var res = await _mediator.Send(query);
            return Res(new DefaultResponse().Success(
                res,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                res.Count()
            ));
        }

        [HttpPut("putIsMain")]
        public async Task<IActionResult> PutIsMain([FromBody] List<ResGetUserByFunction> data)
        {
            var identity = (ClaimsIdentity)User.Identity;
            long userId = long.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.VIEW))
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
            var resData = await _mediator.Send(new PushMainEmployeeCommand() { data = data });

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            foreach (var item in resData)
            {
                reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Sửa nhân viên {item.FullName}",
                "DELETE",
                item.EmployeeId.ToString(),
                "Employee",
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
