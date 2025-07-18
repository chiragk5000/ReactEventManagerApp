using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReactEventManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
                                        ?? throw new InvalidOperationException("IMediator service is not Available");
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (!result.IsSucess && result.Code == 404)
            {
                return NotFound();
            }
            if (result.IsSucess && result.Value != null)
            {
                return Ok (result.Value);
            }
            return BadRequest(result.Error);
        }
    }
}
