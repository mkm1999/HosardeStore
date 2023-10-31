using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public virtual User User { get; set; }
        public int? UserId { get; set; }
        public bool IsFinished { get; set; }
        public virtual ICollection<CartItem> Items { get; set; }
        public Guid Guid { get; set; }
    }
}
