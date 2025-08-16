using AutoMapper;
using RestaurantManagementSystem.Application.DTOs;
using RestaurantManagementSystem.Core.Entities;

namespace RestaurantManagementSystem.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Core.Entities.InventoryItem, InventoryItemDto>().ReverseMap();
            CreateMap<Core.Entities.Order, OrderDto>().ReverseMap();
            CreateMap<Core.Entities.OrderItem, OrderItemDto>().ReverseMap();
        }
    }
}
