using Application.AddressService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace UI.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IAddressServices addressServices;

        public AddressController(IAddressServices addressServices)
        {
            this.addressServices = addressServices;
        }

        public IActionResult Index()
        {
            int UserId;
            string UserIdString = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            UserId = int.Parse(UserIdString);
            var addresses = addressServices.GetAddresses(UserId);
            return View(addresses);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add([FromBody]RequestNewAddressDto request)
        {
            int? UserId = null;
            if (User.Identity.IsAuthenticated)
            {
                string UserIdString = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
                UserId = int.Parse(UserIdString);
            }
            request.UserId = UserId;
            var result = addressServices.AddNewAddress(request);
            return Json(result);
        }
        public IActionResult Change(int AddressId)
        {
            int UserId;
            string UserIdString = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            UserId = int.Parse(UserIdString);

            addressServices.ChangeActiveAddress(UserId, AddressId);
            return RedirectToAction("Index", "Order");
        }
    }
}
