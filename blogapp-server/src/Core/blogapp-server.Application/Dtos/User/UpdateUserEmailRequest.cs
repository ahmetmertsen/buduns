using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Dtos.User
{
    public class UpdateUserEmailRequest
    {
        public int UserId { get; set; }
        public string OldEmailVerificationCode { get; set; }
        public string NewEmailVerificationCode { get; set; }
        public string NewEmail { get; set; }
    }
}
