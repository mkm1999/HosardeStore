using Application.CartServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace UI.ViewComponents
{
    public class GetCartViewComponent : ViewComponent
    {
        private readonly ICartServices _cartService;

        public GetCartViewComponent(ICartServices cartService)
        {
            _cartService = cartService;
        }

        public IViewComponentResult Invoke()
        {

            int? UserId = null;
            if (User.Identity.IsAuthenticated)
            {
                string UserIdString = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
                UserId = int.Parse(UserIdString);
            }
            Guid CartGuid;
            Guid.TryParse(Request.Cookies["CartId"], out CartGuid);
            var result = _cartService.GetCart(CartGuid,UserId);
            return View("GetCart",result);
        }
    }
}
