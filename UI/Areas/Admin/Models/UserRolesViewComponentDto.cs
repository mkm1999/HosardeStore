using System.Collections.Generic;

namespace UI.Areas.Admin.Models
{
    public class UserRolesViewComponentDto
    {
        public List<string> Roles { get; set; }
        public string UserName { get; set; }
    }
}
