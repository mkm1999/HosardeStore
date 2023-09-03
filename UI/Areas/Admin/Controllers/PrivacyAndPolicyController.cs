using Application.PrivacyAndPolicy.IGetPrivacyAndPolicyService;
using Application.PrivacyAndPolicy.SetPrivacyAndPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Boss,Admin")]
    public class PrivacyAndPolicyController : Controller
    {
        private readonly IGetPolicy _getPolicy;
        private readonly ISetPrivacy _setPrivacy;

        public PrivacyAndPolicyController(ISetPrivacy setPrivacy, IGetPolicy getPolicy)
        {
            _setPrivacy = setPrivacy;
            _getPolicy = getPolicy;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var result = _getPolicy.GetPolicyMethod();
            return View(result);
        }
        [HttpPost]
        public IActionResult Index(string PrivacyText, string PolicyText)
        {
            var result = _setPrivacy.SetPolicyService(PrivacyText, PolicyText);
            ViewBag.Massage = result.message;
            return View(new GetPolicyDto { PolicyText = PolicyText , PrivacyText = PrivacyText});
        }
    }
}
