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
using HETSAPI.Models;
using Newtonsoft.Json;
using System.Net;

namespace HETSAPI.Test
{
	public class ProjectIntegrationTest : ApiIntegrationTestBase
    { 
		[Fact]
		/// <summary>
        /// Integration test for ProjectsBulkPost
        /// </summary>
		public async Task TestProjectsBulkPost()
		{
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/projects/bulk");
            request.Content = new StringContent("[]", Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        /// <summary>
        /// Basic Integration test for Roles
        /// </summary>
        public async Task TestProjectsBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/projects");

            // create a new object.
            Project project = new Project();
            project.Name = initialName;
            string jsonString = project.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            project = JsonConvert.DeserializeObject<Project>(jsonString);

            // get the id
            var id = project.Id;

            // change the name
            project.Name = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/projects/" + id);
            request.Content = new StringContent(project.ToJson(), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/projects/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            project = JsonConvert.DeserializeObject<Project>(jsonString);

            // verify the change went through.
            Assert.Equal(project.Name, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/projects/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/projects/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        /// <summary>
        /// Test project contacts
        /// </summary>
        public async Task TestProjectContacts()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/projects");

            // create a new object.
            Owner owner = new Owner();
            owner.OrganizationName = initialName;
            string jsonString = owner.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            owner = JsonConvert.DeserializeObject<Owner>(jsonString);

            // get the id
            var id = owner.Id;

            // change the name
            owner.OrganizationName = changedName;

            // get contacts should be empty.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/projects/" + id + "/contacts");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list is empty.
            Assert.Empty(contacts);

            // add a contact.
            Contact contact = new Contact();
            contact.GivenName = initialName;

            contacts.Add(contact);

            request = new HttpRequestMessage(HttpMethod.Put, "/api/projects/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(contacts), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list has one element.
            Assert.Single(contacts);
            Assert.Equal(contacts[0].GivenName, initialName);

            // get contacts should be 1
            request = new HttpRequestMessage(HttpMethod.Get, "/api/projects/" + id + "/contacts");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list has a record.
            Assert.Single(contacts);
            Assert.Equal(contacts[0].GivenName, initialName);

            // test removing the contact.
            contacts.Clear();

            request = new HttpRequestMessage(HttpMethod.Put, "/api/projects/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(contacts), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // should be 0
            Assert.Empty(contacts);

            // test the get
            request = new HttpRequestMessage(HttpMethod.Get, "/api/projects/" + id + "/contacts");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // verify the list has no records.
            Assert.Empty(contacts);

            // test the post.
            Contact newContact = new Contact();
            newContact.OrganizationName = "asdf";

            request = new HttpRequestMessage(HttpMethod.Post, "/api/projects/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(newContact), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            newContact = JsonConvert.DeserializeObject<Contact>(jsonString);

            // should be 0
            Assert.NotEqual(0, newContact.Id);

            request = new HttpRequestMessage(HttpMethod.Put, "/api/projects/" + id + "/contacts");
            request.Content = new StringContent(JsonConvert.SerializeObject(contacts), Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            contacts = JsonConvert.DeserializeObject<List<Contact>>(jsonString);

            // should be 0
            Assert.Empty(contacts);

            // delete the owner.            
            request = new HttpRequestMessage(HttpMethod.Post, "/api/projects/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/projects/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
