﻿using AutoMapper;
using EventBus.Message.Events;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.Api.Mapper
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
