using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RestaurantManagementSystem.Application.Features.POS.Commands;
using Xunit;

namespace RestaurantManagementSystem.Tests
{
    public class POSControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public POSControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedOrder()
        {
            var command = new CreateOrderCommand
            {
                OrderDate = System.DateTime.UtcNow,
                TableNumber = 1,
                TotalAmount = 100.00m,
                PaymentStatus = "Paid",
                OrderStatus = "Completed",
                OrderItems = {
                    new Application.DTOs.OrderItemDto
                    {
                        ProductId = 1,
                        Quantity = 2,
                        UnitPrice = 50.00m
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/v1/POS", content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Completed", responseString);
        }

        [Fact]
        public async Task GetOrder_ReturnsOrder()
        {
            // First create an order
            var command = new CreateOrderCommand
            {
                OrderDate = System.DateTime.UtcNow,
                TableNumber = 2,
                TotalAmount = 200.00m,
                PaymentStatus = "Pending",
                OrderStatus = "Processing",
                OrderItems = {
                    new Application.DTOs.OrderItemDto
                    {
                        ProductId = 2,
                        Quantity = 1,
                        UnitPrice = 200.00m
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/POS", content);
            postResponse.EnsureSuccessStatusCode();

            var createdOrderJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdOrder = JsonConvert.DeserializeObject(createdOrderJson);
            int orderId = createdOrder.id;

            // Now get the order by id
            var getResponse = await _client.GetAsync($"/api/v1/POS/{orderId}");
            getResponse.EnsureSuccessStatusCode();

            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains("Processing", getResponseString);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsUpdatedOrder()
        {
            // First create an order
            var createCommand = new CreateOrderCommand
            {
                OrderDate = System.DateTime.UtcNow,
                TableNumber = 3,
                TotalAmount = 300.00m,
                PaymentStatus = "Pending",
                OrderStatus = "Processing",
                OrderItems = {
                    new Application.DTOs.OrderItemDto
                    {
                        ProductId = 3,
                        Quantity = 3,
                        UnitPrice = 100.00m
                    }
                }
            };

            var createContent = new StringContent(JsonConvert.SerializeObject(createCommand), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/POS", createContent);
            postResponse.EnsureSuccessStatusCode();

            var createdOrderJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdOrder = JsonConvert.DeserializeObject(createdOrderJson);
            int orderId = createdOrder.id;

            // Update the order
            var updateCommand = new Application.Features.POS.Commands.UpdateOrderCommand
            {
                Id = orderId,
                OrderDate = System.DateTime.UtcNow,
                TableNumber = 3,
                TotalAmount = 350.00m,
                PaymentStatus = "Paid",
                OrderStatus = "Completed",
                OrderItems = {
                    new Application.DTOs.OrderItemDto
                    {
                        ProductId = 3,
                        Quantity = 3,
                        UnitPrice = 100.00m
                    }
                }
            };

            var updateContent = new StringContent(JsonConvert.SerializeObject(updateCommand), Encoding.UTF8, "application/json");
            var putResponse = await _client.PutAsync($"/api/v1/POS/{orderId}", updateContent);
            putResponse.EnsureSuccessStatusCode();

            var updatedOrderJson = await putResponse.Content.ReadAsStringAsync();
            Assert.Contains("Completed", updatedOrderJson);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsNoContent()
        {
            // First create an order
            var createCommand = new CreateOrderCommand
            {
                OrderDate = System.DateTime.UtcNow,
                TableNumber = 4,
                TotalAmount = 400.00m,
                PaymentStatus = "Pending",
                OrderStatus = "Processing",
                OrderItems = {
                    new Application.DTOs.OrderItemDto
                    {
                        ProductId = 4,
                        Quantity = 4,
                        UnitPrice = 100.00m
                    }
                }
            };

            var createContent = new StringContent(JsonConvert.SerializeObject(createCommand), Encoding.UTF8, "application/json");
            var postResponse = await _client.PostAsync("/api/v1/POS", createContent);
            postResponse.EnsureSuccessStatusCode();

            var createdOrderJson = await postResponse.Content.ReadAsStringAsync();
            dynamic createdOrder = JsonConvert.DeserializeObject(createdOrderJson);
            int orderId = createdOrder.id;

            // Delete the order
            var deleteResponse = await _client.DeleteAsync($"/api/v1/POS/{orderId}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            // Verify order is deleted
            var getResponse = await _client.GetAsync($"/api/v1/POS/{orderId}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
