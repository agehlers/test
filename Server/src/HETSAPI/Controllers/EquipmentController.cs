using System;
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
    /// Equipment Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentService _service;

        /// <summary>
        /// Equipment Controller Constructor
        /// </summary>
        public EquipmentController(IEquipmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk equipment records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Equipment created</response>
        [HttpPost]
        [Route("/api/equipment/bulk")]
        [SwaggerOperation("EquipmentBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult EquipmentBulkPost([FromBody]Equipment[] items)
        {
            return _service.EquipmentBulkPostAsync(items);
        }

        /// <summary>
        /// Get all equipment records
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment")]
        [SwaggerOperation("EquipmentGet")]
        [SwaggerResponse(200, type: typeof(List<Equipment>))]
        public virtual IActionResult EquipmentGet()
        {
            return _service.EquipmentGetAsync();
        }

        /// <summary>
        /// Get all attachments associated with an equipment record
        /// </summary>
        /// <remarks>Returns attachments for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        [HttpGet]
        [Route("/api/equipment/{id}/attachments")]
        [SwaggerOperation("EquipmentIdAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<AttachmentViewModel>))]
        public virtual IActionResult EquipmentIdAttachmentsGet([FromRoute]int id)
        {
            return _service.EquipmentIdAttachmentsGetAsync(id);
        }

        /// <summary>
        /// Delete equipment
        /// </summary>
        /// <param name="id">id of Equipment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        [HttpPost]
        [Route("/api/equipment/{id}/delete")]
        [SwaggerOperation("EquipmentIdDeletePost")]
        public virtual IActionResult EquipmentIdDeletePost([FromRoute]int id)
        {
            return _service.EquipmentIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get all equipment attachments for an equipment record
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentAttachments for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/{id}/equipmentattachments")]
        [SwaggerOperation("EquipmentIdEquipmentattachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentAttachment>))]
        public virtual IActionResult EquipmentIdEquipmentattachmentsGet([FromRoute]int id)
        {
            return _service.EquipmentIdEquipmentattachmentsGetAsync(id);
        }

        /// <summary>
        /// Get equipment by id
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        [HttpGet]
        [Route("/api/equipment/{id}")]
        [SwaggerOperation("EquipmentIdGet")]
        [SwaggerResponse(200, type: typeof(Equipment))]
        public virtual IActionResult EquipmentIdGet([FromRoute]int id)
        {
            return _service.EquipmentIdGetAsync(id);
        }

        /// <summary>
        /// Get equipment history
        /// </summary>
        /// <remarks>Returns History for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/{id}/history")]
        [SwaggerOperation("EquipmentIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<HistoryViewModel>))]
        public virtual IActionResult EquipmentIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            return _service.EquipmentIdHistoryGetAsync(id, offset, limit);
        }

        /// <summary>
        /// Create equipment history
        /// </summary>
        /// <remarks>Add a History record to the Equipment</remarks>
        /// <param name="id">id of Equipment to add History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/equipment/{id}/history")]
        [SwaggerOperation("EquipmentIdHistoryPost")]
        public virtual IActionResult EquipmentIdHistoryPost([FromRoute]int id, [FromBody]History item)
        {
            return _service.EquipmentIdHistoryPostAsync(id, item);
        }

        /// <summary>
        /// Update equipment
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        [HttpPut]
        [Route("/api/equipment/{id}")]
        [SwaggerOperation("EquipmentIdPut")]
        [SwaggerResponse(200, type: typeof(Equipment))]
        public virtual IActionResult EquipmentIdPut([FromRoute]int id, [FromBody]Equipment item)
        {
            return _service.EquipmentIdPutAsync(id, item);
        }

        /// <summary>
        /// Get equipment view model by id
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentViewModel for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/{id}/view")]
        [SwaggerOperation("EquipmentIdViewGet")]
        [SwaggerResponse(200, type: typeof(EquipmentViewModel))]
        public virtual IActionResult EquipmentIdViewGet([FromRoute]int id)
        {
            return _service.EquipmentIdViewGetAsync(id);
        }

        /// <summary>
        /// Create equipment
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Equipment created</response>
        [HttpPost]
        [Route("/api/equipment")]
        [SwaggerOperation("EquipmentPost")]
        [SwaggerResponse(200, type: typeof(Equipment))]
        public virtual IActionResult EquipmentPost([FromBody]Equipment item)
        {
            return _service.EquipmentPostAsync(item);
        }

        /// <summary>
        /// Recalculates seniority
        /// </summary>
        /// <remarks>Used to calculate seniority for all database records.</remarks>
        /// <param name="region">Region to recalculate</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/recalcSeniority")]
        [SwaggerOperation("EquipmentRecalcSeniorityGet")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult EquipmentRecalcSeniorityGet([FromQuery]int region)
        {
            return _service.EquipmentRecalcSeniorityGetAsync(region);
        }

        /// <summary>
        /// Searches Equipment
        /// </summary>
        /// <remarks>Used for the equipment search page.</remarks>
        /// <param name="localareas">Local Areas (comma seperated list of id numbers)</param>
        /// <param name="types">Equipment Types (comma seperated list of id numbers)</param>
        /// <param name="equipmentAttachment">Searches equipmentAttachment type</param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="notverifiedsincedate">Not Verified Since Date</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/equipment/search")]
        [SwaggerOperation("EquipmentSearchGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentViewModel>))]
        public virtual IActionResult EquipmentSearchGet([FromQuery]string localareas, [FromQuery]string types, [FromQuery]string equipmentAttachment, [FromQuery]int? owner, [FromQuery]string status, [FromQuery]bool? hired, [FromQuery]DateTime? notverifiedsincedate)
        {
            return _service.EquipmentSearchGetAsync(localareas, types, equipmentAttachment, owner, status, hired, notverifiedsincedate);
        }
    }
}
