namespace RestaurantManagementSystem.Application.DTOs
{
    public class InventoryItemDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int ReorderLevel { get; set; }
    }
}
