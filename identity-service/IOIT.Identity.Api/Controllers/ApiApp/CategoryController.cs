using IOIT.Identity.Api.Filters;
using IOIT.Identity.Application.Categorys.Commands.Update;
using IOIT.Identity.Application.Categorys.Queries;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Employees.Queries;
using IOIT.Identity.Application.Residents.Queries;
using IOIT.Identity.Application.UserRoles.Queries;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IOIT.Identity.Api.Controllers.ApiApp
{
    [Route("api/app/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {

        //Danh sách căn hộ sở hữu
        [Authorize]
        [HttpGet("GetByPageApartment/{ResidentId}")]
        public async Task<IActionResult> GetByPageApartment([FromQuery] GetByPageApartmentQuery command, long ResidentId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            if (ResidentId != userMapId)
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_VIEW_MESSAGE);
            }
            command.ResidentId = ResidentId;
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
                ));
        }

        //Danh sách thành viên trong căn hộ
        [Authorize]
        [HttpGet("GetByPageMemberApartment/{ResidentId}/{ApartmentId}")]
        public async Task<IActionResult> GetByPageMemberApartment([FromQuery] GetByPageMemberApartmentQuery command, long ResidentId, int ApartmentId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            if (ResidentId != userMapId)
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_VIEW_MESSAGE);
            }
            command.ApartmentId = ApartmentId;
            command.ResidentId = ResidentId;
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
                ));
        }

        //Danh sách khách thuê căn hộ
        [Authorize]
        [HttpGet("GetByPageGuestApartment/{ResidentId}/{ApartmentId}")]
        public async Task<IActionResult> GetByPageGuestApartment([FromQuery] GetByPageGuestApartmentQuery command, long ResidentId, int ApartmentId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            if (ResidentId != userMapId)
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_VIEW_MESSAGE);
            }
            command.ApartmentId = ApartmentId;
            command.ResidentId = ResidentId;
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
                ));
        }

        //Sửa thành viên trong căn hộ
        [Authorize]
        [HttpPut("UpdateMemberApartment/{ResidentId}/{ApartmentId}")]
        public async Task<IActionResult> UpdateMemberApartment([FromQuery] UpdateMemberCommand command, long ResidentId, int ApartmentId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            if (ResidentId != userMapId)
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_VIEW_MESSAGE);
            }
            command.userMapId = userMapId;
            command.ApartmentId = ApartmentId;
            command.ResidentId = ResidentId;
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

        [Authorize]
        [HttpGet("GetMemberApartmentAdmin/{ApartmentId}")]
        public async Task<IActionResult> GetMemberApartmentAdmin([FromQuery] GetMemberApartmentAdminQuery command, int ApartmentId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            //string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            command.ResidentId = userMapId;
            command.ApartmentId = ApartmentId;
            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
                ));
        }

        [Authorize]
        [HttpGet("GetMainApartmentAdmin/{ApartmentId}")]
        public async Task<IActionResult> GetMainApartmentAdmin(int ApartmentId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string access_key = identity.Claims.Where(c => c.Type == "AccessKey").Select(c => c.Value).SingleOrDefault();
            int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            var resData = await _mediator.Send(new GetMainApartmentAdminQuery() { ApartmentId = ApartmentId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
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

        //Danh sách nhân viên xử lý theo phòng ban và tòa nhà
        [HttpGet("GetEmployeeHandle/{TowerId}/{DepartmentId}"), AuthorizeFilter]
        public async Task<IActionResult> GetEmployeeHandle(int DepartmentId, int TowerId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            var resData = await _mediator.Send(new GetEmployeeHandle() { TowerId = TowerId, DepartmentId = DepartmentId, UserMapId = userMapId });

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
            ));
        }

        //Lấy tài khoản cư dân theo Id cư dân
        [HttpGet("GetUserByResidentId/{ResidentId}"), AuthorizeFilter]
        public async Task<IActionResult> GetUserByResidentId(long ResidentId)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userMapId = int.Parse(identity.Claims.Where(c => c.Type == "UserMapId").Select(c => c.Value).SingleOrDefault());

            var resData = await _mediator.Send(new GetUserByResidentIdQuery() { ResidentId = ResidentId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
            ));
        }

        //Danh sách nhân viên theo dự án
        [HttpGet("GetEmployeeByProject/{ProjectId}"), AuthorizeFilter]
        public async Task<IActionResult> GetEmployeeByProject(int ProjectId)
        {
            var resData = await _mediator.Send(new GetEmployeeByProject() { ProjectId = ProjectId});

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
            ));
        }

        [HttpGet("GetAllResident")]
        public async Task<IActionResult> GetAllResident([FromQuery] GetAllResidentIdByPagingQuery command,
            [FromQuery] int? ProjectId = -1, [FromQuery] int? TowerId = -1,
            [FromQuery] int? FloorId = -1, [FromQuery] int? ApartmentId = -1, [FromQuery] int type = -1)
        {
            command.ProjectId = ProjectId;
            command.TowerId = TowerId;
            command.FloorId = FloorId;
            command.ApartmentId = ApartmentId;
            command.type = type;
            command.page_size = 1000000000;

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
                ));
        }

        [HttpGet("GetApartmentResident")]
        public async Task<IActionResult> GetApartmentResident([FromQuery] GetApartmentResidentByPagingQuery command,
            [FromQuery] int? ProjectId = -1, [FromQuery] int? TowerId = -1,
            [FromQuery] int? FloorId = -1, [FromQuery] int? ApartmentId = -1)
        {
            command.ProjectId = ProjectId;
            command.TowerId = TowerId;
            command.FloorId = FloorId;
            command.ApartmentId = ApartmentId;
            command.page_size = 1000000000;

            var resData = await _mediator.Send(command);

            return Res(new DefaultResponse().Success(
                resData.Results,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200,
                resData.RowCount
                ));
        }
    }
}
