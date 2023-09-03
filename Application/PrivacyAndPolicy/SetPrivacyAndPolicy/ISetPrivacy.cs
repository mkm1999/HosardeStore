using Application.PrivacyAndPolicy.IGetPrivacyAndPolicyService;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PrivacyAndPolicy.SetPrivacyAndPolicy
{
    public interface ISetPrivacy
    {
        public ResultDto SetPolicyService(string PrivacyText,string PolicyText);
    }
}
