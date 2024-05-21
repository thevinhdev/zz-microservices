using IOIT.Identity.Api.Filters;
using IOIT.Identity.Application.Categorys.Commands.Update;
using IOIT.Identity.Application.Categorys.Queries;
using IOIT.Identity.Application.Employees.Queries;
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
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        [HttpGet("GetTestNoSql")]
        public async Task<IActionResult> GetTestNoSql()
        {

            return Res(new DefaultResponse().Success(
                "Ko kết nối db",
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }


        [HttpGet("GetTestWithSql/{ApartmentId}")]
        public async Task<IActionResult> GetTestWithSql(int ApartmentId)
        {
            var resData = await _mediator.Send(new GetMainApartmentAdminQuery() { ApartmentId = ApartmentId });

            return Res(new DefaultResponse().Success(
                resData,
                ApiConstants.MessageResource.ACCTION_SUCCESS,
                ApiConstants.StatusCode.Success200
                ));
        }

    }


}
