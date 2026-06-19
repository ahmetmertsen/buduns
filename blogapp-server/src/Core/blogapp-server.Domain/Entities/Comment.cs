using blogapp_server.Domain.Entities.Common;
using blogapp_server.Domain.Entities.Identity;
using blogapp_server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public CommentStatus Status { get; set; } = CommentStatus.Published;

        public int UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public ICollection<Report> Reports { get; set; } = new List<Report>();
        public ICollection<ModerationAction> ModerationActions { get; set; } = new List<ModerationAction>();
    }
}
