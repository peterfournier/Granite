﻿using GraniteCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraniteCore.MVC.Controllers
{
    public abstract class UserBasedController<TCategoryName, TUser, TUserPrimaryKey> : BaseController<TCategoryName>
        where TUser : class, IUser<TUserPrimaryKey>
    {
        protected UserManager<TUser> UserManager { get; private set; }

        protected IUser<TUserPrimaryKey> ApplicationUser { get; private set; }

        [Obsolete]
        protected IUserModifierService<TUserPrimaryKey> UserModifierService { get; private set; }

        protected IList<IUserModifierService<TUserPrimaryKey>> UserModifierServices { get; private set; }

        public UserBasedController(
            IGraniteMapper mapper,
            ILogger<TCategoryName> logger,
            UserManager<TUser> userManager,
            IUserModifierService<TUserPrimaryKey> userModifier
            ) : this(mapper, logger, userManager, new List<IUserModifierService<TUserPrimaryKey>>() { userModifier })
        {

        }

        public UserBasedController(
            IGraniteMapper mapper,
            ILogger<TCategoryName> logger,
            UserManager<TUser> userManager,
            IList<IUserModifierService<TUserPrimaryKey>> userModifierServices
            ) : base(mapper, logger)
        {
            UserManager = userManager;
            UserModifierServices = userModifierServices;
        }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser = await UserManager.FindByIdAsync(userId); // todo enable sessions

            if (ApplicationUser != null) setServicesUser();

            await base.OnActionExecutionAsync(context, next);
        }

        private void setServicesUser()
        {
            foreach (var userModifierService in UserModifierServices)
            {
                userModifierService?.SetUser(ApplicationUser);
            }
        }
    }
}
