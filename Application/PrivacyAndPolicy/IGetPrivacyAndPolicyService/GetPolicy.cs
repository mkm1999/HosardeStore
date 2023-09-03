using Application.Interfaces.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PrivacyAndPolicy.IGetPrivacyAndPolicyService
{
    public class GetPolicy: IGetPolicy
    {
        private readonly IDataBaseContext _context;
        public GetPolicy(IDataBaseContext context)
        {
            _context = context;
        }

        public GetPolicyDto GetPolicyMethod()
        {
            var Privacy = _context.privacyEntities.Select(p => new GetPolicyDto
            {
                PolicyText = p.PolicyText,
                PrivacyText = p.PrivacyText,
            }).FirstOrDefault();
            return Privacy;
        }
    }
}
