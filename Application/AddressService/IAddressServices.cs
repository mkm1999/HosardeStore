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
        List<AddressDto> GetAddresses(int UserId);
        AddressDto GetCurrentAddress(int UserId);
        ResultDto ChangeActiveAddress(int UserId, int AddressToActiveId);
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

            ChangeActiveAddress(request.UserId.Value, newAddress.Id);
            return new ResultDto()
            {
                isSuccess = true,
                message = "آدرس ثبت شد"
            };
        }

        public ResultDto ChangeActiveAddress(int UserId, int AddressToActiveId)
        {
            var ActiveAdresses = _context.Addresses.Where(a => a.IsActive && a.UserId == UserId);
            foreach (var item in ActiveAdresses)
            {
                item.IsActive = false;
                
            }
            var AddressToActive = _context.Addresses.Find(AddressToActiveId);
            if(AddressToActive == null)
            {
                return new ResultDto
                {
                    isSuccess = false,

                };
            }
            AddressToActive.IsActive = true;
            _context.SaveChanges();
            return new ResultDto
            {
                isSuccess = true,
                message = "آدرس با موفقیت تغییر کرد"
            };
        }

        public List<AddressDto> GetAddresses(int UserId)
        {
            var addresses = _context.Addresses.Where(a => a.UserId == UserId).OrderByDescending(a => a.IsActive).Select(a => new AddressDto
            {
                address = a.address,
                City = a.City,
                State = a.State,
                PostalCode = a.PostalCode,
                Phone = a.Phone,
                Id = a.Id,
                IsActive = a.IsActive,
            }).ToList();
            return addresses;
        }

        public AddressDto GetCurrentAddress(int UserId)
        {
            var address = _context.Addresses.Where(a => a.UserId == UserId & a.IsActive).FirstOrDefault();
            if (address == null)
            {
                return new AddressDto
                {

                };
            }
            return new AddressDto
            {
                address = address.address,
                City = address.City,
                PostalCode = address.PostalCode,
                Id = address.Id,
                Phone = address.Phone,
                State = address.State,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
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

    public class AddressDto
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string address { get; set; }
        public bool IsActive { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
