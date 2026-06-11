using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Dtos.Auth
{
    public class MailVerifyRequest
    {
        public int UserId { get; set; }
    }
}
