using IOIT.Identity.Api.Filters;
using IOIT.Identity.Application.Residents.Commands.Update;
using IOIT.Identity.Application.Residents.Commands.Create;
using IOIT.Identity.Application.Residents.Commands.Delete;
using IOIT.Identity.Application.Residents.Queries;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using IOIT.Shared.Helpers;
using System.Linq;
using IOIT.Shared.Commons.Enum;
using IOIT.Identity.Application.Common.Interfaces.Producer;
using System.Collections.Generic;
using Newtonsoft.Json;
using static IOIT.Shared.Commons.Enum.AppEnum;
using AutoMapper;
using IOIT.Identity.Application.Residents.ViewModels;
using IOIT.Identity.Application.Categorys.Queries;
using IOIT.Utilities.Application.Dashboard;
using System;
using IOIT.Identity.Application.Apartments.Queries;
using IOIT.Identity.Application.Common.Exceptions;

namespace IOIT.Identity.Api.Controllers.ApiCms
{
    [Route("api/cms/[controller]"), AuthorizeFilter]
    [ApiController]
    public class ResidentController : BaseController
    {
        private static string functionCode = "QLCD";
        private readonly INotificationProducer _notificationProducer;
        private readonly IMapper _mapper;

        public ResidentController(
            INotificationProducer notificationProducer,
            IMapper mapper)
        {
            _notificationProducer = notificationProducer;
            _mapper = mapper;
        }

        [HttpGet("GetByPage")]
        public async Task<IActionResult> GetByPage([FromQuery] GetResidentByPagingQuery command, 
            [FromQuery] int? ProjectId = -1, [FromQuery] int? TowerId = -1, 
            [FromQuery] int? FloorId = -1, [FromQuery] int? ApartmentId = -1, [FromQuery] int type = -1)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            string roleCode = identity.Claims.Where(c => c.Type == "RoleCode").Select(c => c.Value).SingleOrDefault();
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();

            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.VIEW))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }

            command.ProjectId = ProjectId;
            command.TowerId = TowerId;
            command.FloorId = FloorId;
            command.ApartmentId = ApartmentId;
            command.type = type;
            command.roleMax = roleMax;
            if (roleMax != 1 && roleCode != "HO")
            {
                command.ProjectId = projectId;
            }
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
                ));
        }

        [HttpGet("GetMemberApartmentAdmin/{ApartmentId}/{ResidentId}/{Type}")]
        public async Task<IActionResult> GetMemberApartmentAdmin([FromQuery] GetMemberApartmentAdminQuery command, int ApartmentId, int ResidentId, int? Type=1)
        {
            var identity = (ClaimsIdentity)User.Identity;
            //string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            //int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            command.ResidentId = ResidentId;
            command.ApartmentId = ApartmentId;
            command.Type = Type == null ? 1 : Type;
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
                ));
        }


        [HttpGet("GetMainApartmentAdmin/{ApartmentId}")]
        public async Task<IActionResult> GetMainApartmentAdmin(int ApartmentId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            //string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            //int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            var resData = await _mediator.Send(new GetMainApartmentAdminQuery() { ApartmentId = ApartmentId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var resData = await _mediator.Send(new GetResidentByIdQuery() { Id = id });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        public async Task<IActionResult> GetList(long id)
        {
            var resData = await _mediator.Send(new GetListResidentByIdQuery() { Id = id });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpGet, Route("checkExistPhone/{phone}")]
        public async Task<IActionResult> checkExistPhone(string phone)
        {
            if (phone.Length < 9)
            {
                return Res(new DefaultResponse().Success(
                false,
                "Phone number incorrect format",
                ApiConstants.StatusCode.Error400
                ));
            }

            var resData = await _mediator.Send(new GetResidentByPhoneQuery() { Phone = phone });

            return Res(new DefaultResponse().Success(
                resData == null ? true : false,
                resData == null ? "Phone number does not exist" : "Phone number already exist",
                resData == null ? ApiConstants.StatusCode.Success200 : ApiConstants.StatusCode.Error400
                ));
        }

        [HttpGet, Route("getResident/{apartmentId}/{page}/{size}")]
        public async Task<IActionResult> getResident(int? apartmentId, int page, int size)
        {

            var resData = await _mediator.Send(new GetResidentByApartmentIdQuery() { 
                ApartmentId = apartmentId ?? 0 ,
                PageIndex = page,
                PageSize = size
            });

            return Res(new DefaultResponse().Success(resData,
                   ApiConstants.MessageResource.ACCTION_SUCCESS,
                   ApiConstants.StatusCode.Success200
               ));
        }

        [HttpGet, Route("getCountResidentByProjectId/{projectId}")]
        public async Task<IActionResult> getCountResident(int projectId)
        {

            var resData = await _mediator.Send(new GetCountResidentByProjectIdQuery()
            {
                ProjectId = projectId
            });

            return Res(new DefaultResponse().Success(resData,
                   ApiConstants.MessageResource.ACCTION_SUCCESS,
                   ApiConstants.StatusCode.Success200
               ));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateResidentCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.CREATE))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (command.FullName == "" || command.apartments == null)
            {
                return Res(new DefaultResponse().Success(
                     null,
                ApiConstants.MessageResource.MISS_DATA_MESSAGE,
                ApiConstants.StatusCode.Valid210
                ));
            }

            if (command.apartments.Count() == 0)
            {
                return Res(new DefaultResponse().Success(
                    null,
                ApiConstants.MessageResource.MISS_DATA_MESSAGE,
                ApiConstants.StatusCode.Valid210
                ));
            }
            command.UserId = userId;
            command.Status = (byte?)EntityStatus.NORMAL;

            var resData = await _mediator.Send(command);

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Thêm mới cư dân {resData.FullName}",
                "CREATE",
                resData.Id.ToString(),
                "Resident",
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

        [HttpPost("createNewResidentRequest")]
        public async Task<IActionResult> CreateNewResidentRequest([FromBody] ReqRequestCreateNewResident req)
        {
            var command = _mapper.Map<CreateResidentCommand>(req);
            command.Status = req.Status;
            command.isCalledInternal = true;

            var resData = await _mediator.Send(command);

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Thêm mới cư dân {resData.FullName}",
                "CREATE",
                resData.Id.ToString(),
                "Resident",
                null,
                0,
                JsonConvert.SerializeObject(resData),
                IpAddress(),
                0,
                TypeAction.ACTION,
                command.UserId));
            await _notificationProducer.NotificationActionCreate(reqMessages);

            return Ok(resData);
        }

        [HttpGet("getActiveByName/{apartmentId}")]
        public async Task<IActionResult> GetActiveByName(int apartmentId, string name, string phone)
        {
            var resData = await _mediator.Send(new GetResidentByNameOrPhoneQuery() 
            { 
                ApartmentId = apartmentId,
                ResidentName = name,
                ResidentPhone = phone ?? string.Empty
            });

            return Ok(resData ?? null);
        }

        [HttpPost("countApartmentbyTypeResident")]
        public async Task<IActionResult> CountApartmentbyTypeResident([FromBody] GetCountApartmentWithTypeResidentQuery query)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            var type = identity.Claims.Where(c => c.Type == "Type").Select(c => c.Value).SingleOrDefault() ?? "0";
            var projectId = identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault() ?? "-1";

            query.UserType = int.Parse(type);
            query.ProjectTokenId = int.Parse(projectId);

            var resData = await _mediator.Send(query);

            return Res(new DefaultResponse().Success(
                     resData,
                   ApiConstants.MessageResource.ACCTION_SUCCESS,
                   ApiConstants.StatusCode.Success200
               ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] UpdateResidentCommand command)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(string.Join(",", ModelState.Keys.Select(e => e)));
            }
            if (command.FullName == "")
            {
                //def.meta = new Meta(210, Const.MISS_DATA_MESSAGE);
                //return Ok(def);
                return Res(new DefaultResponse().Success(
                     null,
                   ApiConstants.MessageResource.MISS_DATA_MESSAGE,
                   ApiConstants.StatusCode.Valid210
               ));
            }
            if (command.apartments.Count() == 0)
            {
                return Res(new DefaultResponse().Success(
                    null,
                ApiConstants.MessageResource.MISS_DATA_MESSAGE,
                ApiConstants.StatusCode.Valid210
                ));
            }

            command.Id = id;
            command.UserId = userId;
            var resData = await _mediator.Send(command);

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Sửa cư dân {resData.FullName}",
                "UPDATE",
                resData.Id.ToString(),
                "Resident",
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

            var resData = await _mediator.Send(new DeleteResidentCommand() { Id = id, UserId = userId });

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Xóa cư dân {resData.FullName}",
                "CREATE",
                resData.Id.ToString(),
                "Resident",
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
            var resData = await _mediator.Send(new DeletesResidentCommand() { data = data, UserId = userId });

            var reqMessages = new List<DtoNotificationActionCreateQueue>();
            foreach (var item in resData)
            {
                reqMessages.Add(new DtoNotificationActionCreateQueue(
                $"Xóa cư dân {item.FullName}",
                "DELETE",
                item.Id.ToString(),
                "Resident",
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

        [HttpPost("getByCountry")]
        public async Task<IActionResult> GetServiceRate(DataQuery data)
        {
            var identity = (ClaimsIdentity)User.Identity;
            //int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            //int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            //string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            var typeRole = identity.Claims.Where(c => c.Type == "Type").Select(c => c.Value).SingleOrDefault() ?? "0";
            var projectId = identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault() ?? "-1";

            int UserType = int.Parse(typeRole);
            int ProjectId = int.Parse(projectId);

            //if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.VIEW))
            //{
            //    return Res(new DefaultResponse().Success(
            //        null,
            //    ApiConstants.MessageResource.NOPERMISION_VIEW_MESSAGE,
            //    ApiConstants.StatusCode.NoPermision
            //    ));
            //}

            var resData = await _mediator.Send(new GetResidentByCountryQuery() { 
                Data = data,
                UserType = UserType,
                ProjectId = ProjectId
            });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [HttpPut("delDuplicate")]
        public async Task<IActionResult> DelDuplicate(DataQuery data)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            if (!CheckRole.CheckRoleByCode(access_key, functionCode, (int)AppEnum.Action.VIEW))
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }

            var resData = await _mediator.Send(new GetResidentByCountryQuery() { Data = data });

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
