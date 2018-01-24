using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Rental Agreement Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalAgreementController : Controller
    {
        private readonly IRentalAgreementService _service;

        /// <summary>
        /// Rental Agreement Controller Construtor
        /// </summary>
        public RentalAgreementController(IRentalAgreementService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk rental agreement records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreement created</response>
        [HttpPost]
        [Route("/api/rentalagreements/bulk")]
        [SwaggerOperation("RentalagreementsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RentalagreementsBulkPost([FromBody]RentalAgreement[] items)
        {
            return _service.RentalagreementsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all rental agreements
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreements")]
        [SwaggerOperation("RentalagreementsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalAgreement>))]
        public virtual IActionResult RentalagreementsGet()
        {
            return _service.RentalagreementsGetAsync();
        }

        /// <summary>
        /// Delete rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        [HttpPost]
        [Route("/api/rentalagreements/{id}/delete")]
        [SwaggerOperation("RentalagreementsIdDeletePost")]
        public virtual IActionResult RentalagreementsIdDeletePost([FromRoute]int id)
        {
            return _service.RentalagreementsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get rental agreement by id
        /// </summary>
        /// <param name="id">id of RentalAgreement to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        [HttpGet]
        [Route("/api/rentalagreements/{id}")]
        [SwaggerOperation("RentalagreementsIdGet")]
        [SwaggerResponse(200, type: typeof(RentalAgreement))]
        public virtual IActionResult RentalagreementsIdGet([FromRoute]int id)
        {
            return _service.RentalagreementsIdGetAsync(id);
        }

        /// <summary>
        /// Get a pdf version of a rental agreement
        /// </summary>
        /// <remarks>Returns a PDF version of the specified rental agreement</remarks>
        /// <param name="id">id of RentalAgreement to obtain the PDF for</param>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalagreements/{id}/pdf")]
        [SwaggerOperation("RentalagreementsIdPdfGet")]
        public virtual IActionResult RentalagreementsIdPdfGet([FromRoute]int id)
        {
            return _service.RentalagreementsIdPdfGetAsync(id);
        }

        /// <summary>
        /// Update rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        [HttpPut]
        [Route("/api/rentalagreements/{id}")]
        [SwaggerOperation("RentalagreementsIdPut")]
        [SwaggerResponse(200, type: typeof(RentalAgreement))]
        public virtual IActionResult RentalagreementsIdPut([FromRoute]int id, [FromBody]RentalAgreement item)
        {
            return _service.RentalagreementsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create rental agreement
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalAgreement created</response>
        [HttpPost]
        [Route("/api/rentalagreements")]
        [SwaggerOperation("RentalagreementsPost")]
        [SwaggerResponse(200, type: typeof(RentalAgreement))]
        public virtual IActionResult RentalagreementsPost([FromBody]RentalAgreement item)
        {
            return _service.RentalagreementsPostAsync(item);
        }
    }
}
