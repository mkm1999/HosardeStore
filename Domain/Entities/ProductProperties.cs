using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductProperties
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
