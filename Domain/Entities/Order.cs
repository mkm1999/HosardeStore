using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsCompeleted { get; set; }
        public OrderStatus Status { get; set; }
        public int DeliveryCost { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public virtual Address Address { get; set; }
        public int  AddressId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<OrderDetail> Details { get; set; }
        public DateTime ArriveTime { get; set; }

    }
    public enum OrderStatus
    {
        Processing = 1,
        LeaveStore = 2,
        Delivering = 3,
        Delivered = 4,
    }

    public enum DeliveryType
    {
        Express = 1,
        Post = 2,
        Tipax = 3,
    }
}
