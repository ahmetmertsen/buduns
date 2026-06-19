using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Domain.Enums
{
    public enum ReportReason
    {
        Spam = 1,
        Harassment = 2,
        HateSpeech = 3,
        Violence = 4,
        SexualContent = 5,
        Misinformation = 6,
        Copyright = 7,
        PersonalInformation = 8,
        Impersonation = 9,
        Threat = 10,
        SelfHarm = 11,
        Scam = 12,
        Other = 99
    }
}
