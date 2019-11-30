using GraniteCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace GraniteCore.MVC.Controllers
{
    public abstract class UserBasedController<TCategoryName, TUser, TUserPrimaryKey> : BaseController<TCategoryName>
        where TUser : class, IBaseApplicationUser<TUserPrimaryKey>
    {
        protected UserManager<TUser> UserManager { get; private set; }
        protected TUser ApplicationUser { get; private set; }
        protected IUserModifierService<TUser, TUserPrimaryKey> UserModifierService { get; private set; }

        public UserBasedController( 
            IGraniteMapper mapper,
            ILogger<TCategoryName> logger, 
            UserManager<TUser> userManager,
            IUserModifierService<TUser, TUserPrimaryKey> userModifier
            ) : base (mapper, logger)
        {
            UserManager = userManager;
            UserModifierService = userModifier;
        }

        public async override void OnActionExecuting(ActionExecutingContext context)
        {

            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser = await UserManager.FindByIdAsync(userId); // todo enable sessions

            if (ApplicationUser != null) UserModifierService?.SetUser(ApplicationUser);

            base.OnActionExecuting(context);
        }
    }
}
