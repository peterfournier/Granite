using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace GraniteCore.MVC.Controllers
{
    public abstract class UserBasedController<TUser, TCategoryName, TPrimaryKey> : BaseController<TCategoryName>
        where TUser : class
    {
        protected UserManager<TUser> UserManager { get; private set; }
        protected TUser ApplicationUser { get; private set; }

        public UserBasedController(
            IGraniteMapper mapper,
            ILogger<TCategoryName> logger,
            UserManager<TUser> userManager
            ) : base (mapper, logger)
        {
            UserManager = userManager;
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {

            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser = await UserManager.FindByIdAsync(userId); // todo enable sessions

            base.OnActionExecuting(context);
        }
    }
}
