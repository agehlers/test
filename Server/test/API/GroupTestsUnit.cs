using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using HETSAPI.Models;
using HETSAPI.Controllers;
using HETSAPI.Services.Impl;

namespace HETSAPI.Test
{
	public class GroupApiUnitTest 
    { 		
		private readonly GroupController _GroupApi;
		
		/// <summary>
        /// Setup the test
        /// </summary>        
		public GroupApiUnitTest()
		{
            
		}
		
		[Fact]
		/// <summary>
        /// Unit test for GroupsGet
        /// </summary>
		public void TestGroupsGet()
		{
			Assert.True(true);
		}		        
    }
}
