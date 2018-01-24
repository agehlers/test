using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HETSAPI.Test
{
    public class RoleApiIntegrationTest : ApiIntegrationTestBase
    {
        [Fact]
        /// <summary>
        /// Basic Integration test for Roles
        /// </summary>
        public async Task TestRolesBasic()
        {
            string initialName = "InitialName";
            string changedName = "ChangedName";

            // first test the POST.
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/roles");

            // create a new object.
            RoleViewModel role = new RoleViewModel();
            role.Name = initialName;
            string jsonString = role.ToJson();

            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            role = JsonConvert.DeserializeObject<RoleViewModel>(jsonString);

            // get the id
            var id = role.Id;

            // change the name
            role.Name = changedName;

            // now do an update.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/roles/" + id);
            request.Content = new StringContent(role.ToJson(), Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // do a get.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/roles/" + id);
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            role = JsonConvert.DeserializeObject<RoleViewModel>(jsonString);

            // verify the change went through.
            Assert.Equal(role.Name, changedName);

            // do a delete.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/roles/" + id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/roles/" + id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        /// <summary>
        /// Test Users and Roles
        /// </summary>
        public async Task TestUserRoles()
        {
            // first create a role.
            string initialName = "InitialName";
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/roles");
            RoleViewModel role = new RoleViewModel();
            role.Name = initialName;
            role.Description = "test";
            string jsonString = role.ToJson();
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            role = JsonConvert.DeserializeObject<RoleViewModel>(jsonString);

            // get the role id
            var role_id = role.Id;

            // now create a user.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/users");
            UserViewModel user = new UserViewModel();
            user.GivenName = initialName;
            jsonString = user.ToJson();
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            user = JsonConvert.DeserializeObject<UserViewModel>(jsonString);

            // get the user id
            var user_id = user.Id;

            // now add the user to the role.
            UserRoleViewModel userRole = new UserRoleViewModel();
            userRole.RoleId = role_id;
            userRole.UserId = user_id;
            userRole.EffectiveDate = DateTime.Now;

            UserRoleViewModel[] items = new UserRoleViewModel[1];
            items[0] = userRole;

            // send the request.
            request = new HttpRequestMessage(HttpMethod.Put, "/api/roles/" + role_id + "/users");
            jsonString = JsonConvert.SerializeObject(items, Formatting.Indented);
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // if we do a get we should get the same items.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/roles/" + role_id + "/users");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            User[] userRolesResponse = JsonConvert.DeserializeObject<User[]>(jsonString);

            Assert.Equal(items[0].UserId, userRolesResponse[0].Id);

            // Delete user
            request = new HttpRequestMessage(HttpMethod.Post, "/api/users/" + user_id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/users/" + user_id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // Delete role
            request = new HttpRequestMessage(HttpMethod.Post, "/api/roles/" + role_id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/roles/" + role_id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        /// <summary>
        /// Test Users and Roles
        /// </summary>
        public async Task TestRolePermissions()
        {
            // first create a role.
            string initialName = "InitialName";
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/roles");
            RoleViewModel roleViewModel = new RoleViewModel();
            roleViewModel.Name = initialName;
            string jsonString = roleViewModel.ToJson();
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();

            roleViewModel = JsonConvert.DeserializeObject<RoleViewModel>(jsonString);
            // get the role id
            var role_id = roleViewModel.Id;

            // now create a permission.
            request = new HttpRequestMessage(HttpMethod.Post, "/api/permissions");
            Permission permission = new Permission();
            permission.Name = initialName;
            permission.Code = initialName;
            jsonString = permission.ToJson();
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            permission = JsonConvert.DeserializeObject<Permission>(jsonString);

            // get the permission id
            int permission_id = permission.Id;

            // now add the permission to the role.                                  
            request = new HttpRequestMessage(HttpMethod.Post, "/api/roles/" + role_id + "/permissions");
            jsonString = JsonConvert.SerializeObject(permission, Formatting.Indented);
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // if we do a get we should get the same items.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/roles/" + role_id + "/permissions");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            PermissionViewModel[] rolePermissionsResponse = JsonConvert.DeserializeObject<PermissionViewModel[]>(jsonString);

            bool found = false;
            foreach (var item in rolePermissionsResponse)
            {
                if (permission.Code.Equals (item.Code) && permission.Name.Equals (item.Name))
                {
                    found = true;
                }
            }

            Assert.True(found);            

            // test the put.
            Permission[] items = new Permission[1];
            items[0] = permission;

            request = new HttpRequestMessage(HttpMethod.Put, "/api/roles/" + role_id + "/permissions");
            jsonString = JsonConvert.SerializeObject(items, Formatting.Indented);
            request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // parse as JSON.
            jsonString = await response.Content.ReadAsStringAsync();
            rolePermissionsResponse = JsonConvert.DeserializeObject<PermissionViewModel[]>(jsonString);

            found = false;
            foreach (var item in rolePermissionsResponse)
            {
                if (permission.Code.Equals(item.Code) && permission.Name.Equals(item.Name))
                {
                    found = true;
                }
            }

            Assert.True(found);

            // Delete permission
            request = new HttpRequestMessage(HttpMethod.Post, "/api/permissions/" + permission_id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/permissions/" + permission_id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            // Delete role
            request = new HttpRequestMessage(HttpMethod.Post, "/api/roles/" + role_id + "/delete");
            response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // should get a 404 if we try a get now.
            request = new HttpRequestMessage(HttpMethod.Get, "/api/roles/" + role_id);
            response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
