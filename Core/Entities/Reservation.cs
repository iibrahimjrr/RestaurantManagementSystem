using System;

namespace RestaurantManagementSystem.Core.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public int TableNumber { get; set; }
        public string Status { get; set; } = "Pending"; // e.g., Pending, Confirmed, Cancelled
        public string Notes { get; set; } = string.Empty;
    }
}
