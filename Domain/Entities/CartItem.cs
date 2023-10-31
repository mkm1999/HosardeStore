using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public virtual Cart Cart { get; set; }
        public int CartId { get; set; }
    }
}
