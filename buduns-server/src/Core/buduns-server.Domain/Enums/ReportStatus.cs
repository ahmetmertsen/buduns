using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Domain.Enums
{
    public enum ReportStatus
    {
        Pending = 1,
        InReview,
        ResolvedNoViolation,
        ResolvedActionTaken
    }
}
