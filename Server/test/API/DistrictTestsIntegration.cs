using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using HETSAPI.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace HETSAPI.Test
{
	public class DistrictApiIntegrationTest : ApiIntegrationTestBase
    { 		
		[Fact]
		/// <summary>
        /// Integration test for DistrictsBulkPost
        /// </summary>
		public async Task TestDistrictsBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/districts/bulk")
            {
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }		        
		
		[Fact]
		/// <summary>
        /// Integration test for Districts
        /// </summary>
		public async Task TestDistricts()
		{
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // create a temporary region.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/regions");

            Region region = new Region
            {
                Name = initialName
            };

            request.Content = new StringContent(region.ToJson(), Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            string jsonString = await response.Content.ReadAsStringAsync();

            region = JsonConvert.DeserializeObject<Region>(jsonString);
            // get the id
            var region_id = region.Id;

            // test the POST.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/districts");

            // create a new object.
            District district = new District
            {
                Id = 0,
                Name = initialName,
                Region = region
            };
            jsonString = district.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            district = JsonConvert.DeserializeObject<District>(jsonString);
            // get the id
            var id = district.Id;

            // change the name
            district.Name = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/districts/" + id)
            {
                Content = new StringContent(district.ToJson(), Encoding.UTF8, "application/json")
            };
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/districts/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            district = JsonConvert.DeserializeObject<District>(jsonString);

            Assert.Equal(changedName, district.Name);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/districts/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/districts/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // delete the region.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/regions/" + region_id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/regions/" + region_id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }		               
    }
}
