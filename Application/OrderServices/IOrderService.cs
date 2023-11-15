using Application.AddressService;
using Application.CartServices;
using Application.DeliveryCost;
using Application.Interfaces.Context;
using Common;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderServices
{
    public interface IOrderService
    {
        public Task<ResultDto<AddNewOrderResultDto>> AddNewOrder(AddNewOrderRequestDto request);
    }

    public class OrderService : IOrderService
    {
        private readonly IDataBaseContext _cotext;
        private readonly ICartServices _cartServices;
        private readonly IDeliveryCostService _deliveryCostService;
        private readonly IConfiguration _configuration;
        private readonly IAddressServices _addressServices;
        ILogger<OrderService> _logger;
        public OrderService(IDataBaseContext cotext, ICartServices cartServices, IDeliveryCostService eliveryCostService, IConfiguration configuration, ILogger<OrderService> logger, IAddressServices addressServices)
        {
            _cotext = cotext;
            _cartServices = cartServices;
            _deliveryCostService = eliveryCostService;
            _configuration = configuration;
            _logger = logger;
            _addressServices = addressServices;
        }

        public async Task<ResultDto<AddNewOrderResultDto>> AddNewOrder(AddNewOrderRequestDto request)
        {
            string OriginLat = _configuration["DeliveryCost:OriginLat"];
            string OriginLng = _configuration["DeliveryCost:OriginLng"];
            int EachMinutesCost = int.Parse(_configuration["DeliveryCost:EachMinutesCost"]);
            string NeshanApiKey = _configuration["DeliveryCost:NeshanApiKey"];
            string NeshanType = _configuration["DeliveryCost:NeshanType"];
            int MinimumDeliveryPrice = int.Parse(_configuration["DeliveryCost:MinimumDeliveryPrice"]);
            string PostCost = _configuration["DeliveryCost:PostCost"];
            string DeliveryTimeExpresstHours = _configuration["DeliveryCost:DeliveryTimeExpressHours"];
            string DeliveryTimePostDays = _configuration["DeliveryCost:DeliveryTimePostDays"];

            var User = _cotext.Users.Find(request.UserId);
            if (User == null)
            {
                return new ResultDto<AddNewOrderResultDto>
                {
                    isSuccess = false,
                    message = "کاربر یافت نشد"
                };
            }
            var cartDto = _cartServices.GetCart(null, request.UserId);


            if (!cartDto.isSuccess)
            {
                return new ResultDto<AddNewOrderResultDto>
                {
                    isSuccess = false,
                    message = cartDto.message
                };
            }

            var cart = _cotext.Carts.Find(cartDto.data.Id);


            var activeAddress = _addressServices.GetCurrentAddress(request.UserId);

            int DeliveryCost = 0;
            DateTime arriveTime;

            if(request.DeliveryType == DeliveryType.Express)
            {
                try
                {
                    DeliveryCost = await _deliveryCostService.BasedOnTime(activeAddress.Latitude, activeAddress.Longitude, OriginLat, OriginLng, EachMinutesCost, NeshanApiKey, NeshanType, MinimumDeliveryPrice);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                }
                arriveTime = DateTime.Now.AddHours(int.Parse(DeliveryTimeExpresstHours));
            }
            else if(request.DeliveryType == DeliveryType.Post)
            {
                DeliveryCost = int.Parse(PostCost);
                arriveTime = DateTime.Now.AddDays(int.Parse(DeliveryTimePostDays));

            }
            else
            {
                arriveTime = DateTime.Now.AddDays(2);
            }

            var newOrder = new Order
            {
                AddressId = activeAddress.Id,
                CreatedTime = DateTime.Now,
                DeliveryCost = DeliveryCost,
                DeliveryType = request.DeliveryType,
                IsCompeleted = false,
                User = User,
                Status = OrderStatus.Processing,
                ArriveTime = arriveTime,
            };
            _cotext.Orders.Add(newOrder);

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in cartDto.data.Items)
            {
                orderDetails.Add(new OrderDetail
                {
                    Order = newOrder,
                    Count = item.Count,
                    Price = item.ProductPrice,
                    ProductId = item.ProductId,
                    TotalPrice = item.TotalPrice,
                });
            }
            _cotext.OrderDetails.AddRange(orderDetails);
            cart.IsFinished = true;
            _cotext.SaveChanges();
            return new ResultDto<AddNewOrderResultDto>
            {
                isSuccess = true,
                message = "سفارش با موفقیت ثبت شد",
                data = new AddNewOrderResultDto
                {
                    ArriveTime = arriveTime,
                    DeliveryPrice = DeliveryCost,
                    ItemsCost = orderDetails.Sum(d => d.TotalPrice),
                    OrderId = newOrder.Id
                }
            };
        }
    }

    public class AddNewOrderRequestDto
    {
        public DeliveryType DeliveryType { get; set; }
        public int UserId { get; set; }
    }

    public class AddNewOrderResultDto
    {
        public int OrderId { get; set; }
        public int DeliveryPrice { get; set; }

        public DateTime ArriveTime { get; set; }
        public int ItemsCost { get; set; }
    }
}
