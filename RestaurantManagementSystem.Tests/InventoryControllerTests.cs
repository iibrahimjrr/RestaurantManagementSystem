using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RestaurantManagementSystem.Application.Features.Inventory.Commands;
using Xunit;

namespace RestaurantManagementSystem.Tests
{
    public class InventoryControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public InventoryControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateInventoryItem_ReturnsCreatedItem()
        {
            var command = new CreateInventoryItemCommand
            {
                ItemName = "Test Item",
                Description = "Test Description",
                Quantity = 10,
                Supplier = "Test Supplier",
                UnitPrice = 5.99m,
                ReorderLevel = 2
            };

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/v1/Inventory", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Test Item", responseString);
        }

        [Fact]
        public async Task GetInventoryItem_ReturnsItem()
        {
            // First create an inventory item
            var command = new CreateInventoryItemCommand
            {
                ItemName = "GetTest Item",
                Description = "GetTest Description",
                Quantity = 15,
                Supplier = "GetTest Supplier",
                UnitPrice = 9.99m,
                ReorderLevel = 3
            };

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/Inventory", content);
            postResponse.EnsureSuccessStatusCode();

            var createdItemJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdItem = JsonConvert.DeserializeObject(createdItemJson);
            int itemId = createdItem.id;

            // Now get the inventory item by id
            var getResponse = await _client.GetAsync($"/api/v1/Inventory/{itemId}");
            getResponse.EnsureSuccessStatusCode();

            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains("GetTest Item", getResponseString);
        }

        [Fact]
        public async Task UpdateInventoryItem_ReturnsUpdatedItem()
        {
            // First create an inventory item
            var createCommand = new CreateInventoryItemCommand
            {
                ItemName = "UpdateTest Item",
                Description = "UpdateTest Description",
                Quantity = 20,
                Supplier = "UpdateTest Supplier",
                UnitPrice = 15.99m,
                ReorderLevel = 4
            };

            var createContent = new StringContent(JsonConvert.SerializeObject(createCommand), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/Inventory", createContent);
            postResponse.EnsureSuccessStatusCode();

            var createdItemJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdItem = JsonConvert.DeserializeObject(createdItemJson);
            int itemId = createdItem.id;

            // Update the inventory item
            var updateCommand = new Application.Features.Inventory.Commands.UpdateInventoryItemCommand
            {
                Id = itemId,
                ItemName = "Updated Item",
                Description = "Updated Description",
                Quantity = 25,
                Supplier = "Updated Supplier",
                UnitPrice = 19.99m,
                ReorderLevel = 5
            };

            var updateContent = new StringContent(JsonConvert.SerializeObject(updateCommand), Encoding.UTF8, "application/json");
            var putResponse = await _client.PutAsync($"/api/v1/Inventory/{itemId}", updateContent);
            putResponse.EnsureSuccessStatusCode();

            var updatedItemJson = await putResponse.Content.ReadAsStringAsync();
            Assert.Contains("Updated Item", updatedItemJson);
        }

        [Fact]
        public async Task DeleteInventoryItem_ReturnsNoContent()
        {
            // First create an inventory item
            var createCommand = new CreateInventoryItemCommand
            {
                ItemName = "DeleteTest Item",
                Description = "DeleteTest Description",
                Quantity = 30,
                Supplier = "DeleteTest Supplier",
                UnitPrice = 25.99m,
                ReorderLevel = 6
            };

            var createContent = new StringContent(JsonConvert.SerializeObject(createCommand), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/Inventory", createContent);
            postResponse.EnsureSuccessStatusCode();

            var createdItemJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdItem = JsonConvert.DeserializeObject(createdItemJson);
            int itemId = createdItem.id;

            // Delete the inventory item
            var deleteResponse = await _client.DeleteAsync($"/api/v1/Inventory/{itemId}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Verify item is deleted
            var getResponse = await _client.GetAsync($"/api/v1/Inventory/{itemId}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
