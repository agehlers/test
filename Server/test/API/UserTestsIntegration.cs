/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using HETSAPI;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HETSAPI.Test
{
	public class UserApiIntegrationTest : ApiIntegrationTestBase
    { 
		[Fact]
        /// <summary>
        /// Integration test for Users Bulk
        /// </summary>
        public async Task TestUsersBulk()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/users/bulk")
            {
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
		/// <summary>
        /// Basic Integration test for Users
        /// </summary>
		public async Task TestUsersBasic()
		{
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/users");

            // create a new object.
            UserViewModel user = new UserViewModel();
            user.GivenName = initialName;
            string jsonString = user.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);

            // get the id
            var id = user.Id;

            // change the name
            user.GivenName = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/users/" + id);
            request.Content = new StringContent(user.ToJson(), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/users/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);

            // verify the change went through.
            Assert.Equal(user.GivenName, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/users/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/users/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        /// <summary>
        /// Integration test for User Favourites.
        /// </summary>
        public async Task TestUserFavorites()
        {
            string initialName = "InitialName";

            // create a user.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/users");
            UserViewModel user = new UserViewModel();
            user.GivenName = initialName;
            string jsonString = user.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);

            // get the id
            var id = user.Id;
            
            // verify the user has a favourite.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/users/" + id + "/favourites");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            JsonConvert.DeserializeObject<UserFavouriteViewModel[]>(jsonString);

            // cleanup the user            
            response.EnsureSuccessStatusCode();
        }

        [Fact]
		/// <summary>
        /// Integration test for Users
        /// </summary>
		public async Task TestUsers()
		{
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/users");

            // create a new object.
            UserViewModel user = new UserViewModel();
            user.GivenName = initialName;
            string jsonString = user.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);
            
            // get the id
            var id = user.Id;
            
            // change the name
            user.GivenName = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/users/" + id)
            {
                Content = new StringContent(user.ToJson(), Encoding.UTF8, "application/json")
            };

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/users/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);

            // verify the change went through.
            Assert.Equal(user.GivenName, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/users/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/users/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }        
    }
}
