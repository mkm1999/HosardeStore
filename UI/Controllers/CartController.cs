using Application.CartServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using UI.Models.ViewModels;

namespace UI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartServices _cartService;

        public CartController(ICartServices cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            int? UserId = null;
            if (User.Identity.IsAuthenticated)
            {
                string UserIdString = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
                UserId = int.Parse(UserIdString);
            }
            Guid CartGuid;
            Guid.TryParse(Request.Cookies["CartId"], out CartGuid);
            if (CartGuid == default)
            {
                Guid newGuid = Guid.NewGuid();
                Response.Cookies.Append("CartId", newGuid.ToString(),new CookieOptions { Expires = DateTime.Now.AddDays(15)});
                CartGuid = newGuid;
            }
            var result = _cartService.GetCart(CartGuid, UserId);
            return View(result);
        }
        public IActionResult AddToCart([FromBody] AddToCartVM request)
        {
            int? UserId = null;
            if (User.Identity.IsAuthenticated)
            {
                string UserIdString = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
                UserId = int.Parse(UserIdString);
            }
            Guid CartGuid;
            Guid.TryParse(Request.Cookies["CartId"], out CartGuid);
            if (CartGuid == default)
            {
                Guid newGuid = Guid.NewGuid();
                Response.Cookies.Append("CartId", newGuid.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(15) });
                CartGuid = newGuid;
            }
            var result = _cartService.AddToCart(request.ProductId, CartGuid, UserId, request.Count);
            return Json(result);
        }
        [Route("{controller}/{action}/{CartItemId}")]
        public IActionResult RemoveFromCart(int CartItemId)
        {
            _cartService.RemoveFromCart(CartItemId);
            return RedirectToAction("Index");
        }

        [Route("{controller}/{action}/{CartItemId}")]
        public IActionResult IncreaseCount(int CartItemId)
        {
            _cartService.IncreaseCount(CartItemId);
            return RedirectToAction("Index");
        }

        [Route("{controller}/{action}/{CartItemId}")]
        public IActionResult DecreaseCount(int CartItemId)
        {
            _cartService.DecreaseCount(CartItemId);
            return RedirectToAction("Index");
        }
    }
}
