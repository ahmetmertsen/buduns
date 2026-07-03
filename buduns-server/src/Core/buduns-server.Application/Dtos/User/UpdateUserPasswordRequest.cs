using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Dtos.User
{
    public class UpdateUserPasswordRequest
    {
        public string EmailOrUsername { get; set; }
        public string VerificationCode { get; set; }
        public string newPassword { get; set; }
        public string newPasswordConfirmed { get; set; }
    }
}
