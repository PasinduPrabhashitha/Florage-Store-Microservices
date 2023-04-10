﻿using AutoMapper;
using Florage.Orders.Dtos;
using Florage.Orders.Models;

namespace Florage.Orders.Profiles
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        { 
            CreateMap<Order, GetOrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<OrderProduct, OrderProductDto>().ReverseMap();
            CreateMap<Product, GetProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
        }
    }
}