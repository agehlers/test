using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Authorization;
using HETSAPI.Models;
using HETSAPI.Services.Impl;
using HETSCommon;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Test Controller
    /// </summary>
    /// <remarks>
    /// Provides examples of how to apply permission checks.
    /// </remarks>    
    [Route("api/test")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class TestController : Controller
    {
        private readonly ITestService _service;

        /// <summary>
        /// Test Controller Constructor
        /// </summary>
        /// <param name="service"></param>
        public TestController(ITestService service)
        {
            _service = service;
        }

        /// <summary>
        /// Echoes headers for troubleshooting purposes.
        /// </summary>
        [HttpGet]
        [Route("headers")]
        [Produces("text/html")]
        public virtual IActionResult EchoHeaders()
        {
            return _service.EchoHeaders();
        }

        /// <summary>
        /// An example of applying claims based permissions using <see cref="RequiresPermissionAttribute"/>.
        /// </summary>
        /// <remarks>
        /// The user requires the Login permission in order to call this endpoint.  The attribute is applied at the controller level.
        /// </remarks>
        [HttpGet]
        [Route("login/permission/attribute")]
        [RequiresPermission(Permission.LOGIN)]
        public virtual IActionResult GetLoginPermissionAttributeMessage()
        {
            return _service.GetLoginPermissionAttributeMessage();
        }

        /// <summary>
        /// An example of applying claims based permissions using the <see cref="ClaimsPrincipalExtensions.HasPermissions(ClaimsPrincipal, string[])"/> extension method.
        /// </summary>
        /// <remarks>
        /// The user requires the Login permission in order to call this endpoint.  The extension method is called inside the service implementation.
        /// </remarks>
        [HttpGet]
        [Route("login/permission/service")]
        public virtual IActionResult GetLoginPermissionServiceMessage()
        {
            return _service.GetLoginPermissionServiceMessage();
        }

        /// <summary>
        /// An example of a valid authentication message.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public virtual IActionResult GetAuthenticatedMessage()
        {
            return _service.GetAuthenticatedMessage();
        }

        /// <summary>
        /// An example of applying claims based permissions using <see cref="RequiresPermissionAttribute"/>.
        /// </summary>
        /// <remarks>
        /// The user requires the Admin permission in order to call this endpoint.  The attribute is applied at the controller level.
        /// </remarks>
        [HttpGet]
        [Route("admin/permission/attribute")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult GetAdminPermissionAttributeMessage()
        {
            return _service.GetAdminPermissionAttributeMessage();
        }

        /// <summary>
        /// An example of applying claims based permissions using <see cref="RequiresPermissionAttribute"/> at the service implementation level.
        /// </summary>
        /// <remarks>
        /// The attribute is applied at the service implementation level.  This currently does not work.  Refer to the documentation on <see cref="RequiresPermissionAttribute"/> for details.
        /// As a workaround use the <see cref="ClaimsPrincipalExtensions.HasPermissions(ClaimsPrincipal, string[])"/> extension method when applying permissions inside the service implementation.
        /// </remarks>
        [HttpGet]
        [Route("admin/permission/service/attribute")]
        public virtual IActionResult GetAdminPermissionServiceAttributeMessage()
        {
            return _service.GetAdminPermissionServiceAttributeMessage();
        }

        /// <summary>
        /// An example of applying claims based permissions using the <see cref="ClaimsPrincipalExtensions.HasPermissions(ClaimsPrincipal, string[])"/> extension method.
        /// </summary>
        /// <remarks>
        /// The user requires the Admin permission in order to call this endpoint.  The extension method is called inside the service implementation.
        /// </remarks>
        [HttpGet]
        [Route("admin/permission/service")]
        public virtual IActionResult GetAdminPermissionServiceMessage()
        {
            return _service.GetAdminPermissionServiceMessage();
        }

        /// <summary>
        /// An example of applying claims based access using the <see cref="ClaimsPrincipalExtensions.IsInGroup(ClaimsPrincipal, string)"/> extension method.
        /// </summary>
        /// <remarks>
        /// The user must be a member of the "Other" group in order to call this endpoint.  The extension method is called inside the service implementation.
        /// </remarks>
        [HttpGet]
        [Route("other/group/service")]
        public virtual IActionResult GetOtherGroupServiceMessage()
        {
            return _service.GetOtherGroupServiceMessage();
        }
    }

    /// <summary>
    /// Test Service Interface
    /// </summary>
    public interface ITestService
    {
        /// <summary>
        /// Echoes headers for troubleshooting purposes.
        /// </summary>
        /// <returns></returns>
        IActionResult EchoHeaders();

        /// <summary>
        /// An example of relying on the controller level to apply permissions.
        /// </summary>
        /// <returns></returns>
        IActionResult GetLoginPermissionAttributeMessage();

        /// <summary>
        /// An example of using the <see cref="ClaimsPrincipalExtensions.HasPermissions(ClaimsPrincipal, string[])"/> extension method at the service implementation level.
        /// </summary>
        /// <returns></returns>
        IActionResult GetLoginPermissionServiceMessage();

        /// <summary>
        /// An example of relying on the controller level to apply permissions.
        /// </summary>
        /// <returns></returns>
        IActionResult GetAuthenticatedMessage();

        /// <summary>
        /// An example of relying on the controller level to apply permissions.
        /// </summary>
        /// <returns></returns>
        IActionResult GetAdminPermissionAttributeMessage();

        /// <summary>
        /// An example of applying the <see cref="RequiresPermissionAttribute"/> at the service level.
        /// </summary>
        /// <returns></returns>
        IActionResult GetAdminPermissionServiceAttributeMessage();

        /// <summary>
        /// An example of using the <see cref="ClaimsPrincipalExtensions.HasPermissions(ClaimsPrincipal, string[])"/> extension method at the service implementation level.
        /// </summary>
        /// <returns></returns>
        IActionResult GetAdminPermissionServiceMessage();

        /// <summary>
        /// An example of using the <see cref="ClaimsPrincipalExtensions.IsInGroup(ClaimsPrincipal, string)"/> extension method at the service implementation level.
        /// </summary>
        /// <returns></returns>
        IActionResult GetOtherGroupServiceMessage();
    }

    /// <summary>
    /// TestService, the service implementation for <see cref="TestController"/>
    /// </summary>
    /// <remarks>
    /// Provides an example of how to split up the controller and service implementation while still being able to apply permissions 
    /// checks to the authenticated user.
    /// </remarks>
    public class TestService : ServiceBase, ITestService
    {
        /// <summary>
        /// Test Service Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="context"></param>
        public TestService(IHttpContextAccessor httpContextAccessor, DbAppContext context) : base(httpContextAccessor, context)
        {
            // Just pass things along to the base class.
        }

        /// <summary>
        /// Echoes headers for troubleshooting purposes.
        /// </summary>
        public IActionResult EchoHeaders()
        {
            return Ok(Request.Headers.ToHtml());
        }

        /// <summary>
        /// An example of relying on the controller level to apply permissions.
        /// </summary>
        public IActionResult GetAuthenticatedMessage()
        {
            return Ok("You have been authenticated and authorized.");
        }

        /// <summary>
        /// An example of relying on the controller level to apply permissions.
        /// </summary>
        public IActionResult GetLoginPermissionAttributeMessage()
        {
            return Ok("You have permission to login to the application.  Permissions (permission based authorization) were checked using the RequiresPermission attribute set on the controller method.");
        }

        /// <summary>
        /// An example of using the <see cref="ClaimsPrincipalExtensions.HasPermissions(ClaimsPrincipal, string[])"/> extension method at the service implementation level.
        /// </summary>
        public IActionResult GetLoginPermissionServiceMessage()
        {
            if (User.HasPermissions(Permission.LOGIN))
            {
                return Ok("You have permission to login to the application.  Permissions (permission based authorization) were checked inside the service implementation using the HasPermissions extension method.");
            }
            else
            {
                return new ForbidResult();
            }
        }

        /// <summary>
        /// An example of relying on the controller level to apply permissions.
        /// </summary>
        public IActionResult GetAdminPermissionAttributeMessage()
        {
            return Ok("You have Admin permission to the application.  Permissions (permission based authorization) were checked using the RequiresPermission attribute set on the controller method.");
        }

        /// <summary>
        /// An example of applying the <see cref="RequiresPermissionAttribute"/> at the service level.
        /// </summary>
        /// <remarks>
        /// This currently does not work.  Refer to the documentation on <see cref="RequiresPermissionAttribute"/> for details.
        /// As a workaround use the <see cref="ClaimsPrincipalExtensions.HasPermissions(ClaimsPrincipal, string[])"/> extension method when applying permissions inside the service implementation.
        /// </remarks>
        [RequiresPermission(Permission.ADMIN)]
        public IActionResult GetAdminPermissionServiceAttributeMessage()
        {
            return Ok("You have Admin permission to the application.  Permissions (permission based authorization) were checked using the RequiresPermission attribute set on the service method.");
        }

        /// <summary>
        /// An example of using the <see cref="ClaimsPrincipalExtensions.HasPermissions(ClaimsPrincipal, string[])"/> extension method at the service implementation level.
        /// </summary>
        public IActionResult GetAdminPermissionServiceMessage()
        {
            if (User.HasPermissions(Permission.ADMIN))
            {
                return Ok("You have Admin permission to the application.  Permissions (permission based authorization) were checked inside the service implementation using the HasPermissions extension method.");
            }
            else
            {
                return new ChallengeResult();
            }
        }

        /// <summary>
        /// An example of using the <see cref="ClaimsPrincipalExtensions.IsInGroup(ClaimsPrincipal, string)"/> extension method at the service implementation level.
        /// </summary>
        public IActionResult GetOtherGroupServiceMessage()
        {
            if (User.IsInGroup("Other"))
            {
                return Ok("You are a member of the \"Other\" group.  Group membership was checked (via claims based authorization) inside the service implementation using the IsInGroup extension method.");
            }
            else
            {
                return new ChallengeResult();
            }
        }
    }
}
