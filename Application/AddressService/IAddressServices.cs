using Application.Interfaces.Context;
using Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AddressService
{
    public interface IAddressServices
    {
        ResultDto AddNewAddress(RequestNewAddressDto request);
    }

    public class AddressServices : IAddressServices
    {
        private readonly IDataBaseContext _context;

        public AddressServices(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto AddNewAddress(RequestNewAddressDto request)
        {
            var User = _context.Users.Find(request.UserId);
            if(User == null)
            {
                return new ResultDto()
                {
                    message = "کاربر یافت نشد",
                    isSuccess = false
                };
            }
            if(request.PostalCode == default  || request.City == default ||  request.address == default  || request.Phone == default)
            {
                return new ResultDto()
                {
                    message = "لطفا همه مقادیر را وارد کنید",
                    isSuccess = false
                };
            }
            var newAddress = new Address()
            {
                address = request.address,
                City = request.City,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Phone = request.Phone,
                PostalCode = request.PostalCode,
                State = request.State,
                User = User
            };
            _context.Addresses.Add(newAddress);
            _context.SaveChanges();
            return new ResultDto()
            {
                isSuccess = true,
                message = "آدرس ثبت شد"
            };
        }
    }

    public class RequestNewAddressDto
    {
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string address { get; set; }
        public int? UserId { get; set; }
        public string State { get; set; }
    }
}
