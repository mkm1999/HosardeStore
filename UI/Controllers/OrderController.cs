using Application.AddressService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace UI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IAddressServices _addressServices;
        

        public OrderController(IAddressServices addressServices)
        {
            _addressServices = addressServices;

            
        }


        public IActionResult Index()
        {
            int UserId;
            string UserIdString = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            UserId = int.Parse(UserIdString);
            var activeAddress = _addressServices.GetCurrentAddress(UserId);
            return View(activeAddress);
        }

        
    }
}
