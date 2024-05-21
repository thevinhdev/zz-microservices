using IOIT.Shared.Commons.Constants;
using IOIT.Shared.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace IOIT.Identity.Api.Controllers
{
    [ApiController]
    [EnableCors("AllowCors")]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _media;

        protected IMediator _mediator => _media ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult Res(DefaultResponse res)
        {
            return Content(res?.ToJson(), "application/json");
        }

        protected Output<T> Res<T>(T data, int? metadata = 0, string msg = ApiConstants.MessageResource.ACCTION_SUCCESS, int code = ApiConstants.StatusCode.Success200)
        {
            return new Output<T>
            {
                Data = data,
                Meta = new Meta
                {
                    Error_code = code,
                    Error_message = msg
                },
                Metadata = metadata
            };
        }
    }
}
