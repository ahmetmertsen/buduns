using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Domain.Enums
{
    public enum NotificationType
    {
        NEW_FOLLOWER,
        POST_LIKED,
        POST_COMMENTED,
        REPORT_RESOLVED,
        MODERATION_WARNING,
        ACCOUNT_SUSPENDED,
        ACCOUNT_BANNED,
        POST_HIDDEN,
        POST_REMOVED,
        COMMENT_HIDDEN,
        COMMENT_REMOVED,
    }
}
