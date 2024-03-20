using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace cs_quick_ship_authentication_server.Controllers
{
    public abstract class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        private string _userId;
        protected string UserId => _userId = HttpContext.User.FindFirst("sub").Value;
    }
}
