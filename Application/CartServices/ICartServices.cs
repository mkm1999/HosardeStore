using Application.Interfaces.Context;
using Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CartServices
{
    public interface ICartServices
    {
        ResultDto AddToCart(int ProductId, Guid CartGuid, int? UserId, int Count = 1);
        ResultDto RemoveFromCart(int CartItemId);
        ResultDto<CartDto> GetCart (Guid CartGuid, int? UserId);
        ResultDto IncreaseCount(int CartItemId);
        ResultDto DecreaseCount(int CartItemId);
        void CartUserSpecification(Guid? guid, int userId);
    }

    public class CartServices : ICartServices
    {
        private readonly IDataBaseContext _context;

        public CartServices(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto AddToCart(int ProductId, Guid CartGuid, int? UserId,int Count = 1)
        {
            
            var product = _context.products.Find(ProductId);
            if (product == null)
            {
                return new ResultDto
                {
                    message = "محصول یافت نشد",
                    isSuccess = false,
                };
            }

            Cart cart;
            if (UserId == null)
            {
               cart = _context.Carts.Include(c => c.Items).Where(c => c.Guid == CartGuid && c.IsFinished == false).OrderByDescending(c => c.Id).FirstOrDefault();
            }
            else
            {
                cart = _context.Carts.Include(c => c.Items).Where(c => c.UserId == UserId && c.IsFinished == false).OrderByDescending(c => c.Id).FirstOrDefault();
            }
            if (cart == null)
            {
                var newCart = new Cart
                {
                    Guid = CartGuid,
                    AddedDate = DateTime.Now,
                    IsFinished = false,
                };
                if(UserId != null)
                {
                    newCart.User = _context.Users.Find(UserId);
                }
                _context.Carts.Add(newCart);
                _context.SaveChanges();
                cart = newCart;
            }
            var Item = cart?.Items?.Where(i => i.ProductId == ProductId).SingleOrDefault();
            if (Item == null)
            {
                var newItem = new CartItem
                {
                    Cart = cart,
                    Count = Count,
                    Price = product.Price,
                    Product = product,
                };
                _context.CartItems.Add(newItem);
                _context.SaveChanges();
                Item = newItem;
            }
            else
            {
                Item.Count += Count;
                _context.SaveChanges();
            }
            return new ResultDto
            {
                message = "با موفقیت به سبد خرید افزوده شد",
                isSuccess = true,
            };


        }

        public ResultDto<CartDto> GetCart(Guid CartGuid, int? UserId)
        {
            Cart cart;
            if (UserId == null)
            {
                cart = _context.Carts.Include(C => C.Items).ThenInclude(i => i.Product).ThenInclude(p => p.Images).Where(c => c.Guid == CartGuid && !c.IsFinished ).OrderByDescending(c => c.Id).FirstOrDefault();
            }
            else
            {
                cart = _context.Carts.Include(C => C.Items).ThenInclude(i => i.Product).ThenInclude(p => p.Images).Where(c => c.UserId == UserId && !c.IsFinished).OrderByDescending(c => c.Id).FirstOrDefault();
            }
            if (cart == null) return new ResultDto<CartDto>
            {
                message = "سبد خرید خالی است",
                isSuccess = false
            };

            var cartItems = cart.Items.Select(i => new CartItemDto
            {
                Count = i.Count,
                Id = i.Id,
                ImgSrc = i.Product.Images.FirstOrDefault().Url,
                ProductPrice = i.Product.Price,
                ProductTitle = i.Product.Name,
                TotalPrice = i.Product.Price * i.Count,
                ProductId = i.ProductId
            }).ToList();

            if (cartItems.Count == 0) return new ResultDto<CartDto>
            {
                message = "سبد خرید خالی است",
                isSuccess = false
            };

            return new ResultDto<CartDto>
            {
                isSuccess = true,
                data = new CartDto
                {
                    Items = cartItems,
                    TotalPrice = cartItems.Sum(i => i.TotalPrice),
                }
            };
        }

        public ResultDto RemoveFromCart(int CartItemId)
        {
            var CartItem = _context.CartItems.Find(CartItemId);
            if (CartItem == null) return new ResultDto
            {
                message = "ایتم یافت نشد",
                isSuccess = false
            };
            _context.CartItems.Remove(CartItem);
            _context.SaveChanges();
            return new ResultDto
            {
                message = "باموفقیت حذف شد",
                isSuccess = true,
            };
        }

        public ResultDto DecreaseCount(int CartItemId)
        {
            var Item = _context.CartItems.Find(CartItemId);
            if(Item == null)
            {
                return new ResultDto
                {
                    isSuccess = false,
                    message = "آیتم یافت نشد"
                };
            }
            if (Item.Count > 1)
            {
                Item.Count--;
                _context.SaveChanges();
            }
            else
            {
                RemoveFromCart(CartItemId);
            }
            return new ResultDto
            {
                isSuccess = true,
                message = "تعداد درخواستی کاهش یافت!"
            };
        }

        public ResultDto IncreaseCount(int CartItemId)
        {
            var Item = _context.CartItems.Find(CartItemId);
            if (Item == null)
            {
                return new ResultDto
                {
                    isSuccess = false,
                    message = "آیتم یافت نشد"
                };
            }
            Item.Count++;
            _context.SaveChanges();
            return new ResultDto
            {
                isSuccess = true,
                message = "تعداد درخواستی افزایش یافت!"
            };
        }

        public void CartUserSpecification(Guid? guid, int userId)
        {
            var User = _context.Users.Find(userId);
            var Cart = _context.Carts.Where(c => c.Guid == guid).OrderByDescending(c => c.Id).FirstOrDefault();
            if(Cart != null && User != null)
            {
                Cart.User = User;
                _context.SaveChanges();
            }
        }
    }

    public class CartDto
    {
        public int TotalPrice { get; set; }
        public List<CartItemDto> Items { get; set; }
    }

    public class CartItemDto
    {
        public int Id { get; set; }
        public string ProductTitle { get; set; }
        public string ImgSrc { get; set; }
        public int ProductPrice { get; set; }
        public int Count { get; set; }
        public int TotalPrice { get; set; }
        public int ProductId { get; set; }
    }
}
