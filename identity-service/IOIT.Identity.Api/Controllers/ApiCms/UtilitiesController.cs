using System.Linq;
using IOIT.Identity.Api.Filters;
using IOIT.Shared.Commons.Constants;
using IOIT.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using IOIT.Identity.Application.Utilities.Commands.Create;
using System.Collections.Generic;
using IOIT.Identity.Application.Utilities.Queries;
using IOIT.Identity.Application.Utilities.ViewModels;
using CommonException = IOIT.Identity.Application.Common.Exceptions.CommonException;
using IOIT.Identity.Application.Utilities.Commands.Update;
using IOIT.Identity.Application.ProjectUtilities.Queries;
using IOIT.Identity.Application.ProjectUtilities.ViewModels;
using IOIT.Identity.Application.Common.Exceptions;

namespace IOIT.Identity.Api.Controllers.ApiCms
{
    [Route("api/cms/[controller]"), AuthorizeFilter]
    [ApiController]
    public class UtilitiesController : BaseController
    {
        public UtilitiesController()
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUtilitiesCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());

            command.UserId = userId;

            if (roleMax != 1)
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

        [HttpGet("getByPage")]
        public async Task<Output<List<ResGetUtilitiesById>>> GetByPage([FromQuery] GetUtilitiesByPageQuery query)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());

            if (roleMax != 1)
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }

            var res = await _mediator.Send(query);

            return Res(res.Results, res.RowCount);
        }

        [HttpPut("{id}")]
        public async Task<Output<ResGetUtilitiesById>> Put(int id, UpdateUtilitiesByIdCommand command)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userId = int.Parse(identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault() ?? "0");
            int projectId = int.Parse(identity.Claims.Where(c => c.Type == "ProjectId").Select(c => c.Value).SingleOrDefault());
            int roleMax = int.Parse(identity.Claims.Where(c => c.Type == "RoleMax").Select(c => c.Value).SingleOrDefault());

            if (roleMax != 1)
            {
                throw new UnpermissionException(ApiConstants.MessageResource.NOPERMISION_ACTION_MESSAGE);
            }

            command.Id = id;

            var result = await _mediator.Send(command);

            return Res(result);
        }

        [HttpGet("getConfig")]
        public async Task<Output<List<ResGetConfigProjectUtilitiesQuery>>> getConfig([FromQuery] GetConfigUtilitiesByProjectIdQuery query)
        {
            var res = await _mediator.Send(query);

            return Res(res);
        }
    }
}
