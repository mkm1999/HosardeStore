using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UI.Areas.Admin.Models;

namespace UI.ViewComponents
{
    [ViewComponent]
    public class GetUserRoleViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public GetUserRoleViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserRolesViewComponentDto Result;

            if (user == null)
            {
                Result = new UserRolesViewComponentDto
                {
                    Roles = new List<string> { "empty" },
                    UserName = "احراز نشده",
                };

                return View("GetUserRole", Result);

            }
            var roles = await _userManager.GetRolesAsync(user);
            Result = new UserRolesViewComponentDto
            {
                Roles = roles.ToList(),
                UserName = user.FirstName + user.LastName,
            };
            return View("GetUserRole",Result);
        }
    }
}
