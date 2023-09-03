using Application.Interfaces.Context;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PrivacyAndPolicy.SetPrivacyAndPolicy
{
    public class SetPrivacy : ISetPrivacy
    {
        private readonly IDataBaseContext _context;

        public SetPrivacy(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto SetPolicyService(string PrivacyText, string PolicyText)
        {
            var privacy = _context.privacyEntities.FirstOrDefault();
            if (privacy == null)
            {
                return new ResultDto
                {
                    isSuccess = false,
                    message = "هیچ رکوردی پیدا نشد"
                };
            }
            privacy.PrivacyText = PrivacyText;
            privacy.PolicyText = PolicyText;
            _context.SaveChanges();
            return new ResultDto
            {
                isSuccess = true,
                message = "با موفقیت تغییر کرد"
            };
        }
    }
}
