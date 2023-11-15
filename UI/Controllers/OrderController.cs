using Application.AddressService;
using Application.DeliveryCost;
using Application.OrderServices;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IAddressServices _addressServices;
        private readonly IDeliveryCostService _deliveryCostService;
        IConfiguration _configuration;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(IAddressServices addressServices, IDeliveryCostService deliveryCostService, IConfiguration configuration, ILogger<OrderController> logger, IOrderService orderService)
        {
            _addressServices = addressServices;
            _deliveryCostService = deliveryCostService;
            _configuration = configuration;
            _logger = logger;
            _orderService = orderService;
        }


        public async Task<IActionResult> Index()
        {
            int UserId;
            string UserIdString = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            UserId = int.Parse(UserIdString);
            var activeAddress = _addressServices.GetCurrentAddress(UserId);

            string OriginLat = _configuration["DeliveryCost:OriginLat"];
            string OriginLng = _configuration["DeliveryCost:OriginLng"];
            int EachMinutesCost = int.Parse(_configuration["DeliveryCost:EachMinutesCost"]);
            string NeshanApiKey = _configuration["DeliveryCost:NeshanApiKey"];
            string NeshanType = _configuration["DeliveryCost:NeshanType"];
            int MinimumDeliveryPrice = int.Parse(_configuration["DeliveryCost:MinimumDeliveryPrice"]);
            string PostCost = _configuration["DeliveryCost:PostCost"];

            try
            {
                int deliveryCost = await _deliveryCostService.BasedOnTime(activeAddress.Latitude, activeAddress.Longitude, OriginLat, OriginLng, EachMinutesCost, NeshanApiKey, NeshanType, MinimumDeliveryPrice);

                ViewBag.IsBikeActive = (deliveryCost < 100000);
                ViewBag.BikeDeliveryCost = deliveryCost;
            }
            catch (Exception e)
            {
                ViewBag.IsBikeActive = false;
                _logger.Log(LogLevel.Critical, e.Message);
            }

            ViewBag.PostDeliveryCost = PostCost;
            return View(activeAddress);
        }

        public async Task<IActionResult> Submit(string DeliveryType)
        {
            var deliveryType = Enum.Parse<DeliveryType>(DeliveryType);

            int UserId;
            string UserIdString = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            UserId = int.Parse(UserIdString);

            var result = await _orderService.AddNewOrder(new AddNewOrderRequestDto
            {
                DeliveryType = deliveryType,
                UserId = UserId,
            });

            if (!result.isSuccess) return BadRequest(result.message);
            return View(result);
        }

        
    }
}
