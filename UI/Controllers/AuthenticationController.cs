using Application.PrivacyAndPolicy.IGetPrivacyAndPolicyService;
using Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Linq;
using System.Threading.Tasks;
using UI.Models.ViewModels;

namespace UI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IGetPolicy _getpolicy;
        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager , IGetPolicy getpolicy)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _getpolicy = getpolicy;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register(string ReturnUrl = "/")
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        public IActionResult Register([FromBody] RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(Register(registerViewModel));
            }
            if (User.Identity.IsAuthenticated)
            {
                return Json(new ResultDto<bool> { isSuccess = false, message = "شما قبلا وارد شده اید", data = true });
            }
            User user = new User()
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.lastName,
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email,
                PhoneNumber = registerViewModel.PhoneNumber
            };
            var result = _userManager.CreateAsync(user, registerViewModel.Password).Result;
            if (result.Succeeded)
            {
                string code = _userManager.GenerateChangePhoneNumberTokenAsync(user, registerViewModel.PhoneNumber).Result;
                //Send Code Via Sms
                //return RedirectToAction("VerifyPhoneNumber","authentication" , new { ReturnUrl = registerViewModel.ReturnUrl , PhoneNumber = registerViewModel.PhoneNumber , UserId = user.Id});
                return Json(new ResultDto { isSuccess = true });
            }
            else
            {
                string message = "";
                foreach (var item in result.Errors.ToList())
                {
                    message += item.Description + Environment.NewLine;
                }
                return Json(new ResultDto { isSuccess = false, message = message });

            }
        }
        [HttpGet]
        public IActionResult VerifyPhoneNumber(string ReturnUrl, string PhoneNumber, string UserName)
        {
            ViewBag.PhoneNumber = PhoneNumber;
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.UserName = UserName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPhoneNumber([FromBody] VerifyPhoneNumberViewModel verifyPhoneNumberViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (verifyPhoneNumberViewModel.UserName == null || verifyPhoneNumberViewModel.PhoneNumber == null || verifyPhoneNumberViewModel.VerifyCode == null || verifyPhoneNumberViewModel.ReturnUrl == null)
            {
                return Json(new ResultDto { isSuccess = false });
            }
            var user = _userManager.FindByNameAsync(verifyPhoneNumberViewModel.UserName).Result;
            bool VerifyResult = _userManager.VerifyChangePhoneNumberTokenAsync(user, verifyPhoneNumberViewModel.VerifyCode, verifyPhoneNumberViewModel.PhoneNumber).Result;
            if (VerifyResult)
            {
                user.PhoneNumberConfirmed = true;
                var update = _userManager.UpdateAsync(user).Result;
                await _signInManager.SignInAsync(user,true);
                return Json(new ResultDto<string> { isSuccess = true, message = "شما با موفقیت وارد سایت شدید", data = verifyPhoneNumberViewModel.ReturnUrl });

            }
            else
            {
                return Json(new ResultDto { isSuccess = false, message = "کد وارد شده صحیح نیست", });

            }
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl = "/",string ChangePassword = "")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            ViewBag.ChangePassword = ChangePassword;
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var user = _userManager.FindByNameAsync(loginViewModel.Email).Result;
            if (user == null)
            {
                return Json(new ResultDto { isSuccess = false, message = "کاربری با این مشخصات یافت نشد" });
            }
            _signInManager.SignOutAsync().Wait();
            var Result = _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.IsPersistent, true).Result;
            if (Result.Succeeded)
            {
                return Json(new ResultDto<string> { isSuccess = true, message = "شما با موفقیت وارد شدید",data = loginViewModel.ReturnUrl });
            }
            else if(Result.IsLockedOut)
            {
                return Json(new ResultDto { isSuccess = false, message = "حساب شما مسدود شده است!" });

            }

            return Json(new ResultDto { isSuccess = false , message = "رمز عبور اشتباه است!"});
        }
        public IActionResult Logout(string ReturnUrl = "/")
        {
            _signInManager.SignOutAsync();
            return Redirect(ReturnUrl);
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(string Email)
        {
            string Errors = "";
            if (string.IsNullOrEmpty(Email))
            {
                Errors += "لطفا ایمیلی که با آن ثبت نام کرده اید را وارد کنید";
                ViewBag.Errors = Errors;
                return View();
            }
            var user = _userManager.FindByEmailAsync(Email).Result;
            if (user == null)
            {
                Errors += "کاربری با این ایمیل یافت نشد";
                ViewBag.Errors = Errors;
                return View();
            }
            var token = _userManager.GenerateUserTokenAsync(user,TokenOptions.DefaultPhoneProvider,"ResetPassword").Result;
            //send token by sms
            
            return RedirectToAction("ResetPassword", new {UserId=user.Id});
        }

        [HttpGet]
        public IActionResult ResetPassword(string UserId)
        {
            ViewBag.UserId = UserId;
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(string Token,string UserId,string Password,string ConfirmPassword)
        {
            string Errors = "";
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword) || Password != ConfirmPassword)
            {
                Errors += "لطفا کلمه عبور و تکرار آنرا کنترل کنید";
                ViewBag.Errors = Errors;
                ViewBag.UserId = UserId;
                return View();
            }
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                Errors += "کاربری با این ایمیل یافت نشد";
                ViewBag.Errors = Errors;
                return View();
            }
            bool IsTokenTrue = _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPassword", Token).Result;
            if(!IsTokenTrue)
            {
                Errors += "کد وارد شده صحیح نیست";
                ViewBag.Errors = Errors;
                ViewBag.UserId = UserId;
                return View();
            }
            _userManager.RemovePasswordAsync(user).Wait();
            var result = _userManager.AddPasswordAsync(user, Password).Result;
            if (result.Succeeded)
            {
                
                return RedirectToAction("Login" , new { ChangePassword = "پسورد شما با موفقیت تغییر کرد"});
            }
            foreach (var item in result.Errors.ToList())
            {
                Errors += item.Description + Environment.NewLine;
            }
            ViewBag.UserId = UserId;
            ViewBag.Errors = Errors;
            return View();
        }

        public IActionResult ResetPhone(string UserName)
        {
            var User = _userManager.FindByEmailAsync(UserName).Result;
            if (User == null)
            {
                return BadRequest();
            }
            _userManager.DeleteAsync(User).Wait();
            return RedirectToAction("Register");
        }
        public IActionResult PrivacyAndPolicy()
        {
            var result = _getpolicy.GetPolicyMethod();
            return View(result);
        }
    }
}
