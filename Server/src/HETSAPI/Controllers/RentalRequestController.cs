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
    /// Rental Request Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalRequestController : Controller
    {
        private readonly IRentalRequestService _service;

        /// <summary>
        /// Rental Request Controller Constructor
        /// </summary>
        public RentalRequestController(IRentalRequestService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk rental request records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalRequest created</response>
        [HttpPost]
        [Route("/api/rentalrequests/bulk")]
        [SwaggerOperation("RentalrequestsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RentalrequestsBulkPost([FromBody]RentalRequest[] items)
        {
            return _service.RentalrequestsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all rental requests
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalrequests")]
        [SwaggerOperation("RentalrequestsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalRequest>))]
        public virtual IActionResult RentalrequestsGet()
        {
            return _service.RentalrequestsGetAsync();
        }

        /// <summary>
        /// Get attachments associated with a rental request
        /// </summary>
        /// <remarks>Returns attachments for a particular RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        [HttpGet]
        [Route("/api/rentalrequests/{id}/attachments")]
        [SwaggerOperation("RentalrequestsIdAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<AttachmentViewModel>))]
        public virtual IActionResult RentalrequestsIdAttachmentsGet([FromRoute]int id)
        {
            return _service.RentalrequestsIdAttachmentsGetAsync(id);
        }

        /// <summary>
        /// Delete rental request
        /// </summary>
        /// <param name="id">id of RentalRequest to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        [HttpPost]
        [Route("/api/rentalrequests/{id}/delete")]
        [SwaggerOperation("RentalrequestsIdDeletePost")]
        public virtual IActionResult RentalrequestsIdDeletePost([FromRoute]int id)
        {
            return _service.RentalrequestsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get rental request by id
        /// </summary>
        /// <param name="id">id of RentalRequest to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        [HttpGet]
        [Route("/api/rentalrequests/{id}")]
        [SwaggerOperation("RentalrequestsIdGet")]
        [SwaggerResponse(200, type: typeof(RentalRequest))]
        public virtual IActionResult RentalrequestsIdGet([FromRoute]int id)
        {
            return _service.RentalrequestsIdGetAsync(id);
        }

        /// <summary>
        /// Get history associated with a rental request
        /// </summary>
        /// <remarks>Returns History for a particular RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalrequests/{id}/history")]
        [SwaggerOperation("RentalrequestsIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<HistoryViewModel>))]
        public virtual IActionResult RentalrequestsIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            return _service.RentalrequestsIdHistoryGetAsync(id, offset, limit);
        }

        /// <summary>
        /// Create history for a rental request
        /// </summary>
        /// <remarks>Add a History record to the RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to add History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/rentalrequests/{id}/history")]
        [SwaggerOperation("RentalrequestsIdHistoryPost")]
        public virtual IActionResult RentalrequestsIdHistoryPost([FromRoute]int id, [FromBody]History item)
        {
            return _service.RentalrequestsIdHistoryPostAsync(id, item);
        }

        /// <summary>
        /// Update rental request
        /// </summary>
        /// <param name="id">id of RentalRequest to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>
        [HttpPut]
        [Route("/api/rentalrequests/{id}")]
        [SwaggerOperation("RentalrequestsIdPut")]
        [SwaggerResponse(200, type: typeof(RentalRequest))]
        public virtual IActionResult RentalrequestsIdPut([FromRoute]int id, [FromBody]RentalRequest item)
        {
            return _service.RentalrequestsIdPutAsync(id, item);
        }

        /// <summary>
        /// Update a rental request rotation list record
        /// </summary>
        /// <remarks>Updates a rental request rotation list entry.  Side effect is the LocalAreaRotationList is also updated</remarks>
        /// <param name="id">id of RentalRequest to update</param>
        /// <param name="rentalRequestRotationListId">id of RentalRequestRotationList to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestRotationList not found</response>
        [HttpPut]
        [Route("/api/rentalrequests/{id}/rentalrequestrotationlist/{rentalRequestRotationListId}")]
        [SwaggerOperation("RentalrequestsIdRentalrequestrotationlistRentalRequestRotationListIdPut")]
        [SwaggerResponse(200, type: typeof(RentalRequestRotationList))]
        public virtual IActionResult RentalrequestsIdRentalrequestrotationlistRentalRequestRotationListIdPut([FromRoute]int id, [FromRoute]int rentalRequestRotationListId, [FromBody]RentalRequestRotationList item)
        {
            return _service.RentalrequestsIdRentalrequestrotationlistRentalRequestRotationListIdPutAsync(id, rentalRequestRotationListId, item);
        }

        /// <summary>
        /// Create rental request
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalRequest created</response>
        [HttpPost]
        [Route("/api/rentalrequests")]
        [SwaggerOperation("RentalrequestsPost")]
        [SwaggerResponse(200, type: typeof(RentalRequest))]
        public virtual IActionResult RentalrequestsPost([FromBody]RentalRequest item)
        {
            return _service.RentalrequestsPostAsync(item);
        }

        /// <summary>
        /// Searches RentalRequests
        /// </summary>
        /// <remarks>Used for the rental request search page.</remarks>
        /// <param name="localareas">Local Areas (comma seperated list of id numbers)</param>
        /// <param name="project">Searches equipmentAttachment type</param>
        /// <param name="status">Status</param>
        /// <param name="startDate">Inspection start date</param>
        /// <param name="endDate">Inspection end date</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalrequests/search")]
        [SwaggerOperation("RentalrequestsSearchGet")]
        [SwaggerResponse(200, type: typeof(List<RentalRequestSearchResultViewModel>))]
        public virtual IActionResult RentalrequestsSearchGet([FromQuery]string localareas, [FromQuery]string project, [FromQuery]string status, [FromQuery]DateTime? startDate, [FromQuery]DateTime? endDate)
        {
            return _service.RentalrequestsSearchGetAsync(localareas, project, status, startDate, endDate);
        }
    }
}
