using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormAdvanced.Infrastructure.Services.AzureAD
{
    public class AzureADOptions
    {
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string SecretId { get; set; }
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string SignUpSignInPolicyId { get; set; }
    }
}
