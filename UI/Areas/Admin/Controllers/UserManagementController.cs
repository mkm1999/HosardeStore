using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UI.Areas.Admin.Models;
using UI.Models.ViewModels;

namespace UI.Areas.Admin.Controllers
{
	[Area("admin")]
	[Authorize(Roles = "Admin,Boss,Operator")]
	public class UserManagementController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UserManagementController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
			_roleManager = roleManager;
        }
        public IActionResult Index(string SearchKey)
		{
			var UsersQuery = _userManager.Users.Select(user => new UserDto
			{
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Id = user.Id,
				PhoneNumber = user.PhoneNumber,
			}).AsQueryable();

			if(!string.IsNullOrEmpty(SearchKey) )
			{
                UsersQuery = UsersQuery.Where(u => u.Email.Contains(SearchKey) || u.FirstName.Contains(SearchKey) || u.LastName.Contains(SearchKey) || u.PhoneNumber.Contains(SearchKey));
			}

            List<UserDto> Users = UsersQuery.ToList();

            return View(Users);
		}


        [Authorize(Roles = "Admin,Boss")]
        public IActionResult DeleteUser(string UserId)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var user = _userManager.FindByIdAsync(UserId).Result;
			if(user == null)
			{
                return View();
            }
            var currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if ((_userManager.IsInRoleAsync(user, "BOSS").Result && !_userManager.IsInRoleAsync(currentUser, "BOSS").Result) || (_userManager.IsInRoleAsync(user, "ADMIN").Result && _userManager.IsInRoleAsync(currentUser, "OPERATOR").Result))
            {
                ViewBag.Errors = "شما دسترسی لازم برای حذف این کاربر را ندارید";
                return View("AddMember");
            }
            var Result = _userManager.DeleteAsync(user).Result;
			if(Result.Succeeded)
			{
				return RedirectToAction("Index", "UserManagement");
			}
			return View();
		}

        [Authorize(Roles = "Admin,Boss")]
        public IActionResult AddMember(AddUserDto addUserDto)
		{
			if(	
				addUserDto.Email == null
				|| addUserDto.FirstName == null
				|| addUserDto.LastName == null
				|| addUserDto.Password == null
				|| addUserDto.PhoneNumber == null
				)
			{
				ViewBag.Errors = "لطفا مقادیر را کنترل کنید";
				return View();
			}
			User newUser = new User
			{
				AccessFailedCount = 0,
				Email = addUserDto.Email,
				FirstName = addUserDto.FirstName,
				PhoneNumber = addUserDto.PhoneNumber,
				UserName = addUserDto.Email,
				LastName = addUserDto.LastName,
				PhoneNumberConfirmed = true,
			};
			var Result = _userManager.CreateAsync(newUser,addUserDto.Password).Result;
			if(Result.Succeeded)
			{
				return RedirectToAction("index","UserManagement");
			}
			string errors = "";
            foreach (var item in Result.Errors)
            {
				errors += item.Description + Environment.NewLine;
            }
			ViewBag.Errors = errors;
            return View();
		}
		public IActionResult EditUser(EditUserDto editUserDto) 
		{
            if (
                editUserDto.Email == null
                || editUserDto.FirstName == null
                || editUserDto.LastName == null
                || editUserDto.PhoneNumber == null
                )
            {
                ViewBag.Errors = "لطفا مقادیر را کنترل کنید";
                return View("AddMember");
            }
			User user = _userManager.FindByIdAsync(editUserDto.Id.ToString()).Result;
			if (user == null)
			{
                ViewBag.Errors = "کاربری با این مشخصات یافت نشد!";
                return View("AddMember");
            }

			var currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;

            if ((_userManager.IsInRoleAsync(user, "BOSS").Result && !_userManager.IsInRoleAsync(currentUser, "BOSS").Result) || (_userManager.IsInRoleAsync(user, "ADMIN").Result && _userManager.IsInRoleAsync(currentUser, "OPERATOR").Result))
            {
                ViewBag.Errors = "شما دسترسی لازم برای ویرایش این کاربر را ندارید";
                return View("AddMember");
            }

			user.Email = editUserDto.Email;
			user.FirstName = editUserDto.FirstName;
			user.LastName = editUserDto.LastName;
			user.PhoneNumber = editUserDto.PhoneNumber;
			user.UserName = editUserDto.Email;
			var result = _userManager.UpdateAsync(user).Result;
			if (result.Succeeded)
			{
                return RedirectToAction("index", "UserManagement");
            }
            string errors = "";
            foreach (var item in result.Errors)
            {
                errors += item.Description + Environment.NewLine;
            }
            ViewBag.Errors = errors;
            return View("AddMember");
        }
		public IActionResult EditUserPassword(string UserId, string newPassword)
		{
            User user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                ViewBag.Errors = "کاربری با این مشخصات یافت نشد!";
                return View("AddMember");
            }
            var currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if ((_userManager.IsInRoleAsync(user, "BOSS").Result && !_userManager.IsInRoleAsync(currentUser, "BOSS").Result) || (_userManager.IsInRoleAsync(user, "ADMIN").Result && _userManager.IsInRoleAsync(currentUser, "OPERATOR").Result))
            {
                ViewBag.Errors = "شما دسترسی لازم برای ویرایش این کاربر را ندارید";
                return View("AddMember");
            }
            _userManager.RemovePasswordAsync(user).Wait();
			var result = _userManager.AddPasswordAsync(user, newPassword).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("index", "UserManagement");
            }
            string errors = "";
            foreach (var item in result.Errors)
            {
                errors += item.Description + Environment.NewLine;
            }
            ViewBag.Errors = errors;
            return View("AddMember");
        }
		[HttpGet]
        [Authorize(Roles = "Admin,Boss")]
        public IActionResult EditRole(string UserId , string FName , string LName)
		{
			var user = _userManager.FindByIdAsync(UserId).Result;
			if (user == null) return RedirectToAction("Index", "UserManagement");
			var UerRoles = _userManager.GetRolesAsync(user).Result.ToList();
			ViewBag.UserId = user.Id;
			var Roles = _roleManager.Roles.Where(r => r.Name != "Boss").Select(r => r.Name).ToList();
			if(Roles != null)
			{
				ViewBag.Roles = new SelectList(Roles);

            }
            ViewBag.UserName = FName + " " + LName;
			return View(UerRoles);
		}
		[HttpPost]
        [Authorize(Roles = "Admin,Boss")]
        public IActionResult EditRole(string Role,string UserId,string UserName,string nul)
		{
			if(Role == "Boss")
			{
                ViewBag.Errors = "این نقش قابل افزودن نیست!";
                return View("AddMember");
            }
            if (Role == "Please select one")
            {
                ViewBag.Errors = "لطفا یک نقش را انتخاب کنید!";
                return View("AddMember");
            }
            var user = _userManager.FindByIdAsync(UserId).Result;
			if (user == null)
			{
                ViewBag.Errors = "کاربری با این مشخصات یافت نشد!";
                return View("AddMember");
            }
			var result = _userManager.AddToRoleAsync(user, Role).Result;
			if (result.Succeeded)
			{
				return RedirectToAction("EditRole", "UserManagement" , new { FName = UserName , UserId = UserId});
			}
            string errors = "";
            foreach (var item in result.Errors)
            {
                errors += item.Description + Environment.NewLine;
            }
            ViewBag.Errors = errors;
            return View("AddMember");
        }

        [Authorize(Roles = "Admin,Boss")]
        public IActionResult DeleteUserRole(string UserId , string Role , string UserName)
		{
            if (Role == "Boss")
            {
                ViewBag.Errors = "این نقش قابل حذف نیست!";
                return View("AddMember");
            }
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                ViewBag.Errors = "کاربری با این مشخصات یافت نشد!";
                return View("AddMember");
            }
            var result = _userManager.RemoveFromRoleAsync(user, Role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("EditRole", "UserManagement", new { FName = UserName, UserId = UserId});
            }
            string errors = "";
            foreach (var item in result.Errors)
            {
                errors += item.Description + Environment.NewLine;
            }
            ViewBag.Errors = errors;
            return View("AddMember");
        }
    }
}
