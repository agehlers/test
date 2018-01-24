﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using HETSAPI.Models;

namespace HETSAPI.Authorization
{
    /// <summary>
    /// MVC Options Extension
    /// </summary>
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// Add Authorization Policy
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MvcOptions AddDefaultAuthorizationPolicyFilter(this MvcOptions options)
        {
            // Default authorization policy enforced via a global authorization filter
            AuthorizationPolicy requireLoginPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(User.PERMISSION_CLAIM, Permission.LOGIN)
                .Build();

            AuthorizeFilter filter = new AuthorizeFilter(requireLoginPolicy);
            options.Filters.Add(filter);
            return options;
        }
    }
}
