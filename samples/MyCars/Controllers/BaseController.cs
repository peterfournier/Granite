using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyCars.Areas.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCars.Controllers
{
    // Optional GraniteCore install
    public class BaseController : Controller
    {
        protected GraniteCoreApplicationUser ApplicationUser { get; private set; }
        protected string UserId { get; private set; }

        public BaseController()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            ApplicationUser = User?.Identity as GraniteCoreApplicationUser; // todo, fix
            UserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            base.OnActionExecuting(context);
        }
    }
}
