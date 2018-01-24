using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Group Service
    /// </summary>
    public class GroupService : IGroupService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Group Service Constructor
        /// </summary>
        public GroupService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Get users associated with a group
        /// </summary>
        /// <remarks>Used to get users in a given Group</remarks>
        /// <param name="id">id of Group to fetch Users for</param>
        /// <response code="200">OK</response>
        public IActionResult GroupsIdUsersGetAsync(int id)
        {
            bool exists = _context.Groups.Any(a => a.Id == id);

            if (exists)
            {
                List<UserViewModel> result = new List<UserViewModel>();

                IQueryable<GroupMembership> data = _context.GroupMemberships
                    .Include("User")
                    .Include("Group")
                    .Where(x => x.Group.Id == id);

                // extract the users
                foreach (GroupMembership item in data)
                {
                    result.Add(item.User.ToViewModel());
                }

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create bulk group records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Groups created</response>
        public IActionResult GroupsBulkPostAsync(Group[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (Group item in items)
            {
                bool exists = _context.Groups.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Groups.Update(item);
                }
                else
                {
                    _context.Groups.Add(item);
                }
            }

            // save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Get all groups
        /// </summary>
        /// <remarks>Returns a collection of groups</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult GroupsGetAsync()
        {
            List<GroupViewModel> result = _context.Groups.Select(x => x.ToViewModel()).ToList();
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete group
        /// </summary>
        /// <param name="id">id of Group to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        public IActionResult GroupsIdDeletePostAsync(int id)
        {
            bool exists = _context.Groups.Any(a => a.Id == id);

            if (exists)
            {
                Group item = _context.Groups.First(a => a.Id == id);

                if (item != null)
                {
                    _context.Groups.Remove(item);
                    
                    // Save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get group by id
        /// </summary>
        /// <remarks>Returns a Group</remarks>
        /// <param name="id">id of Group to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        public IActionResult GroupsIdGetAsync(int id)
        {
            bool exists = _context.Groups.Any(a => a.Id == id);

            if (exists)
            {
                Group result = _context.Groups.First(a => a.Id == id);
                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="id">id of Group to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Group not found</response>
        public IActionResult GroupsIdPutAsync(int id, Group item)
        {
            bool exists = _context.Groups.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.Groups.Update(item);

                // Save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create group
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Group created</response>
        public IActionResult GroupsPostAsync(Group item)
        {
            bool exists = _context.Groups.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.Groups.Update(item);
            }
            else
            {
                // record not found - create
                _context.Groups.Add(item);
            }

            _context.SaveChanges();
            return new ObjectResult(new HetsResponse(item));
        }
    }
}
