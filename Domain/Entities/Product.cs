using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barnd { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public bool IsEnable { get; set; } = true;
        public virtual ICollection<ProductImages> Images { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<ProductProperties> Properties { get; set; }

    }
}
