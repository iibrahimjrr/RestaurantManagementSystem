using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RestaurantManagementSystem.Application.Features.Products.Commands;
using Xunit;

namespace RestaurantManagementSystem.Tests
{
    public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedProduct()
        {
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                StockQuantity = 10
            };

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/v1/Products", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Test Product", responseString);
        }

        [Fact]
        public async Task GetProduct_ReturnsProduct()
        {
            // First create a product
            var command = new CreateProductCommand
            {
                Name = "GetTest Product",
                Description = "GetTest Description",
                Price = 19.99m,
                StockQuantity = 5
            };

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/Products", content);
            postResponse.EnsureSuccessStatusCode();

            var createdProductJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdProduct = Newtonsoft.Json.JsonConvert.DeserializeObject(createdProductJson);
            int productId = createdProduct.id;

            // Now get the product by id
            var getResponse = await _client.GetAsync($"/api/v1/Products/{productId}");
            getResponse.EnsureSuccessStatusCode();

            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains("GetTest Product", getResponseString);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsUpdatedProduct()
        {
            // First create a product
            var createCommand = new CreateProductCommand
            {
                Name = "UpdateTest Product",
                Description = "UpdateTest Description",
                Price = 29.99m,
                StockQuantity = 15
            };

            var createContent = new StringContent(JsonConvert.SerializeObject(createCommand), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/Products", createContent);
            postResponse.EnsureSuccessStatusCode();

            var createdProductJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdProduct = Newtonsoft.Json.JsonConvert.DeserializeObject(createdProductJson);
            int productId = createdProduct.id;

            // Update the product
            var updateCommand = new Application.Features.Products.Commands.UpdateProductCommand
            {
                Id = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 39.99m,
                StockQuantity = 20
            };

            var updateContent = new StringContent(JsonConvert.SerializeObject(updateCommand), Encoding.UTF8, "application/json");
            var putResponse = await _client.PutAsync($"/api/v1/Products/{productId}", updateContent);
            putResponse.EnsureSuccessStatusCode();

            var updatedProductJson = await putResponse.Content.ReadAsStringAsync();
            Assert.Contains("Updated Product", updatedProductJson);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            // First create a product
            var createCommand = new CreateProductCommand
            {
                Name = "DeleteTest Product",
                Description = "DeleteTest Description",
                Price = 49.99m,
                StockQuantity = 25
            };

            var createContent = new StringContent(JsonConvert.SerializeObject(createCommand), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/Products", createContent);
            postResponse.EnsureSuccessStatusCode();

            var createdProductJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdProduct = Newtonsoft.Json.JsonConvert.DeserializeObject(createdProductJson);
            int productId = createdProduct.id;

            // Delete the product
            var deleteResponse = await _client.DeleteAsync($"/api/v1/Products/{productId}");
            Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Verify product is deleted
            var getResponse = await _client.GetAsync($"/api/v1/Products/{productId}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
