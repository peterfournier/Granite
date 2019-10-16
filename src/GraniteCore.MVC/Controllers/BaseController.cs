using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GraniteCore.MVC.Controllers
{
    public abstract class BaseController<TCategoryName> : Controller
    {
        protected IGraniteMapper GraniteMapper;
        protected ILogger<TCategoryName> Logger;

        public BaseController(
            IGraniteMapper graniteMapper,
            ILogger<TCategoryName> logger
            )
        {
            GraniteMapper = graniteMapper;
            Logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
