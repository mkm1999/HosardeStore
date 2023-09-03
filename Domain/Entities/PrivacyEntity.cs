using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PrivacyEntity
    {
        public int Id { get; set; }
        public string PrivacyText { get; set; }
        public string PolicyText { get; set; }
    }
}
