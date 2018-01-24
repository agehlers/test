using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Owner Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class OwnerController : Controller
    {
        private readonly IOwnerService _service;

        /// <summary>
        /// Owner Controller Constructor
        /// </summary>
        public OwnerController(IOwnerService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk owner records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Owner created</response>
        [HttpPost]
        [Route("/api/owners/bulk")]
        [SwaggerOperation("OwnersBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult OwnersBulkPost([FromBody]Owner[] items)
        {
            return _service.OwnersBulkPostAsync(items);
        }

        /// <summary>
        /// Get all owners
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/owners")]
        [SwaggerOperation("OwnersGet")]
        [SwaggerResponse(200, type: typeof(List<Owner>))]
        public virtual IActionResult OwnersGet()
        {
            return _service.OwnersGetAsync();
        }

        /// <summary>
        /// Get attachments associated with an owner
        /// </summary>
        /// <remarks>Returns attachments for a particular Owner</remarks>
        /// <param name="id">id of Owner to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        [HttpGet]
        [Route("/api/owners/{id}/attachments")]
        [SwaggerOperation("OwnersIdAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<AttachmentViewModel>))]
        public virtual IActionResult OwnersIdAttachmentsGet([FromRoute]int id)
        {
            return _service.OwnersIdAttachmentsGetAsync(id);
        }

        /// <summary>
        /// Get contacts associated with an owner
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to fetch Contacts for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/owners/{id}/contacts")]
        [SwaggerOperation("OwnersIdContactsGet")]
        [SwaggerResponse(200, type: typeof(List<Contact>))]
        public virtual IActionResult OwnersIdContactsGet([FromRoute]int id)
        {
            return _service.OwnersIdContactsGetAsync(id);
        }

        /// <summary>
        /// Create owner contact
        /// </summary>
        /// <remarks>Adds Owner Contact</remarks>
        /// <param name="id">id of Owner to add a contact for</param>
        /// <param name="item">Adds to Owner Contact</param>
        /// <response code="200">OK</response>
        [HttpPost]
        [Route("/api/owners/{id}/contacts")]
        [SwaggerOperation("OwnersIdContactsPost")]
        [SwaggerResponse(200, type: typeof(Contact))]
        public virtual IActionResult OwnersIdContactsPost([FromRoute]int id, [FromBody]Contact item)
        {
            return _service.OwnersIdContactsPostAsync(id, item);
        }

        /// <summary>
        /// Update owner contacts
        /// </summary>
        /// <remarks>Replaces an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to replace Contacts for</param>
        /// <param name="item">Replacement Owner contacts.</param>
        /// <response code="200">OK</response>
        [HttpPut]
        [Route("/api/owners/{id}/contacts")]
        [SwaggerOperation("OwnersIdContactsPut")]
        [SwaggerResponse(200, type: typeof(List<Contact>))]
        public virtual IActionResult OwnersIdContactsPut([FromRoute]int id, [FromBody]Contact[] item)
        {
            return _service.OwnersIdContactsPutAsync(id, item);
        }

        /// <summary>
        /// Delete owner
        /// </summary>
        /// <param name="id">id of Owner to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        [HttpPost]
        [Route("/api/owners/{id}/delete")]
        [SwaggerOperation("OwnersIdDeletePost")]
        public virtual IActionResult OwnersIdDeletePost([FromRoute]int id)
        {
            return _service.OwnersIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get equipment associated with an owner
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to fetch Equipment for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/owners/{id}/equipment")]
        [SwaggerOperation("OwnersIdEquipmentGet")]
        [SwaggerResponse(200, type: typeof(List<Equipment>))]
        public virtual IActionResult OwnersIdEquipmentGet([FromRoute]int id)
        {
            return _service.OwnersIdEquipmentGetAsync(id);
        }

        /// <summary>
        /// Create owner equipment
        /// </summary>
        /// <remarks>Replaces an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to replace Equipment for</param>
        /// <param name="item">Replacement Owner Equipment.</param>
        /// <response code="200">OK</response>
        [HttpPut]
        [Route("/api/owners/{id}/equipment")]
        [SwaggerOperation("OwnersIdEquipmentPut")]
        [SwaggerResponse(200, type: typeof(List<Equipment>))]
        public virtual IActionResult OwnersIdEquipmentPut([FromRoute]int id, [FromBody]Equipment[] item)
        {
            return _service.OwnersIdEquipmentPutAsync(id, item);
        }

        /// <summary>
        /// Get owner by id
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        [HttpGet]
        [Route("/api/owners/{id}")]
        [SwaggerOperation("OwnersIdGet")]
        [SwaggerResponse(200, type: typeof(Owner))]
        public virtual IActionResult OwnersIdGet([FromRoute]int id)
        {
            return _service.OwnersIdGetAsync(id);
        }

        /// <summary>
        /// Get history associated with owner
        /// </summary>
        /// <remarks>Returns History for a particular Owner</remarks>
        /// <param name="id">id of Owner to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/owners/{id}/history")]
        [SwaggerOperation("OwnersIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<HistoryViewModel>))]
        public virtual IActionResult OwnersIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            return _service.OwnersIdHistoryGetAsync(id, offset, limit);
        }

        /// <summary>
        /// Create owner history
        /// </summary>
        /// <remarks>Add a History record to the Owner</remarks>
        /// <param name="id">id of Owner to add History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/owners/{id}/history")]
        [SwaggerOperation("OwnersIdHistoryPost")]
        public virtual IActionResult OwnersIdHistoryPost([FromRoute]int id, [FromBody]History item)
        {
            return _service.OwnersIdHistoryPostAsync(id, item);
        }

        /// <summary>
        /// Update owner
        /// </summary>
        /// <param name="id">id of Owner to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        [HttpPut]
        [Route("/api/owners/{id}")]
        [SwaggerOperation("OwnersIdPut")]
        [SwaggerResponse(200, type: typeof(Owner))]
        public virtual IActionResult OwnersIdPut([FromRoute]int id, [FromBody]Owner item)
        {
            return _service.OwnersIdPutAsync(id, item);
        }

        /// <summary>
        /// Create owner
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Owner created</response>
        [HttpPost]
        [Route("/api/owners")]
        [SwaggerOperation("OwnersPost")]
        [SwaggerResponse(200, type: typeof(Owner))]
        public virtual IActionResult OwnersPost([FromBody]Owner item)
        {
            return _service.OwnersPostAsync(item);
        }

        /// <summary>
        /// Searches Owners
        /// </summary>
        /// <remarks>Used for the owner search page.</remarks>
        /// <param name="localareas">Local Areas (comma seperated list of id numbers)</param>
        /// <param name="equipmenttypes">Equipment Types (comma seperated list of id numbers)</param>
        /// <param name="owner">Id for a specific Owner</param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/owners/search")]
        [SwaggerOperation("OwnersSearchGet")]
        [SwaggerResponse(200, type: typeof(List<Owner>))]
        public virtual IActionResult OwnersSearchGet([FromQuery]string localareas, [FromQuery]string equipmenttypes, [FromQuery]int? owner, [FromQuery]string status, [FromQuery]bool? hired)
        {
            return _service.OwnersSearchGetAsync(localareas, equipmenttypes, owner, status, hired);
        }
    }
}
