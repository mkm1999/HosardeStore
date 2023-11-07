using Application.AddressService;
using Application.DeliveryCost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public OrderController(IAddressServices addressServices, IDeliveryCostService deliveryCostService, IConfiguration configuration)
        {
            _addressServices = addressServices;
            _deliveryCostService = deliveryCostService;
            _configuration = configuration;
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

            int deliveryCost = await _deliveryCostService.BasedOnTime(activeAddress.Latitude, activeAddress.Longitude, OriginLat, OriginLng, EachMinutesCost, NeshanApiKey, NeshanType, MinimumDeliveryPrice);

            ViewBag.IsBikeActive = (deliveryCost < 100000);
            ViewBag.BikeDeliveryCost = deliveryCost;
            return View(activeAddress);
        }

        
    }
}
