﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    public class TimeRecordService : ITimeRecordService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public TimeRecordService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(TimeRecord item)
        {
            if (item != null)
            {

                if (item.RentalAgreement != null)
                {
                    item.RentalAgreement = _context.RentalAgreements.FirstOrDefault(a => a.Id == item.RentalAgreement.Id);
                }

                if (item.RentalAgreementRate != null)
                {
                    item.RentalAgreementRate = _context.RentalAgreementRates.FirstOrDefault(a => a.Id == item.RentalAgreement.Id);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult TimerecordsBulkPostAsync(TimeRecord[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (TimeRecord item in items)
            {
                AdjustRecord(item);
                bool exists = _context.TimeRecords.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.TimeRecords.Update(item);
                }
                else
                {
                    _context.TimeRecords.Add(item);
                }
            }
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult TimerecordsGetAsync()
        {
            var result = _context.RentalAgreementConditions
                .Include(x => x.RentalAgreement)
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult TimerecordsIdDeletePostAsync(int id)
        {
            var exists = _context.TimeRecords.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.TimeRecords.First(a => a.Id == id);
                if (item != null)
                {
                    _context.TimeRecords.Remove(item);
                    // Save the changes
                    _context.SaveChanges();
                }
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult TimerecordsIdGetAsync(int id)
        {
            var exists = _context.TimeRecords.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.TimeRecords                    
                    .Include(x => x.RentalAgreement)
                    .Include(x => x.RentalAgreementRate)                    
                    .First(a => a.Id == id);
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult TimerecordsIdPutAsync(int id, TimeRecord item)
        {
            AdjustRecord(item);
            var exists = _context.TimeRecords.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.TimeRecords.Update(item);
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult TimerecordsPostAsync(TimeRecord item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                var exists = _context.TimeRecords.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.TimeRecords.Update(item);
                }
                else
                {
                    // record not found
                    _context.TimeRecords.Add(item);
                }
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                return new StatusCodeResult(400);
            }
        }
    }
}
