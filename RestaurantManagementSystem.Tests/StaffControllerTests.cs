using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RestaurantManagementSystem.Application.Features.Staff.Commands;
using Xunit;

namespace RestaurantManagementSystem.Tests
{
    public class StaffControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StaffControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateStaff_ReturnsCreatedStaff()
        {
            var command = new CreateStaffCommand
            {
                FullName = "Ibrahim Elsyaed",
                Position = "Manager",
                DateOfBirth = System.DateTime.UtcNow.AddYears(-30),
                Email = "ibrahim@gmail.com",
                PhoneNumber = "01006609375",
                HireDate = System.DateTime.UtcNow,
                Salary = 60000m,
                Shift = "Evening",
                IsActive = true
            };

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/v1/Staff", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Ibrahim Elsyaed", responseString);
        }

        [Fact]
        public async Task GetStaff_ReturnsStaff()
        {
            // First create a staff
            var command = new CreateStaffCommand
            {
                FullName = "ahmed elzingy",
                Position = "Waiter",
                DateOfBirth = System.DateTime.UtcNow.AddYears(-25),
                Email = "ahmed@example.com",
                PhoneNumber = "1231231234",
                HireDate = System.DateTime.UtcNow,
                Salary = 30000m,
                Shift = "Morning",
                IsActive = true
            };

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/Staff", content);
            postResponse.EnsureSuccessStatusCode();

            var createdStaffJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdStaff = JsonConvert.DeserializeObject(createdStaffJson);
            int staffId = createdStaff.id;

            // Now get the staff by id
            var getResponse = await _client.GetAsync($"/api/v1/Staff/{staffId}");
            getResponse.EnsureSuccessStatusCode();

            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains("Waiter", getResponseString);
        }

        [Fact]
        public async Task UpdateStaff_ReturnsUpdatedStaff()
        {
            // First create a staff
            var createCommand = new CreateStaffCommand
            {
                FullName = "ali elzingy",
                Position = "Chef",
                DateOfBirth = System.DateTime.UtcNow.AddYears(-28),
                Email = "ali@example.com",
                PhoneNumber = "5555555555",
                HireDate = System.DateTime.UtcNow,
                Salary = 45000m,
                Shift = "Night",
                IsActive = true
            };

            var createContent = new StringContent(JsonConvert.SerializeObject(createCommand), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/Staff", createContent);
            postResponse.EnsureSuccessStatusCode();

            var createdStaffJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdStaff = JsonConvert.DeserializeObject(createdStaffJson);
            int staffId = createdStaff.id;

            // Update the staff
            var updateCommand = new Application.Features.Staff.Commands.UpdateStaffCommand
            {
                Id = staffId,
                FullName = "mohamed elzingy",
                Position = "Head Chef",
                DateOfBirth = createCommand.DateOfBirth,
                Email = "mohamed@example.com",
                PhoneNumber = "5555555555",
                HireDate = createCommand.HireDate,
                Salary = 50000m,
                Shift = "Night",
                IsActive = true
            };

            var updateContent = new StringContent(JsonConvert.SerializeObject(updateCommand), Encoding.UTF8, "application/json");
            var putResponse = await _client.PutAsync($"/api/v1/Staff/{staffId}", updateContent);
            putResponse.EnsureSuccessStatusCode();

            var updatedStaffJson = await putResponse.Content.ReadAsStringAsync();
            Assert.Contains("Head Chef", updatedStaffJson);
        }

        [Fact]
        public async Task DeleteStaff_ReturnsNoContent()
        {
            // First create a staff
            var createCommand = new CreateStaffCommand
            {
                FullName = "kareem elzingy",
                Position = "Cleaner",
                DateOfBirth = System.DateTime.UtcNow.AddYears(-40),
                Email = "kareem@example.com",
                PhoneNumber = "4444444444",
                HireDate = System.DateTime.UtcNow,
                Salary = 25000m,
                Shift = "Morning",
                IsActive = true
            };

            var createContent = new StringContent(JsonConvert.SerializeObject(createCommand), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/Staff", createContent);
            postResponse.EnsureSuccessStatusCode();

            var createdStaffJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdStaff = JsonConvert.DeserializeObject(createdStaffJson);
            int staffId = createdStaff.id;

            // Delete the staff
            var deleteResponse = await _client.DeleteAsync($"/api/v1/Staff/{staffId}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Verify staff is deleted
            var getResponse = await _client.GetAsync($"/api/v1/Staff/{staffId}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
