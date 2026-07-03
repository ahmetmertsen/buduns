using MediatR;
using buduns_server.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace buduns_server.Application.Features.Auth.MailVerify
{
    public class MailVerifyCommand : IRequest<MailVerifyCommandResponse>, ICurrentUserRequest, IAllowUnverifiedEmail
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
