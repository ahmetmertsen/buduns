using Microsoft.AspNetCore.Identity;
using buduns_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Domain.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? EmailVerificationSentAt { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public Enums.UserStatus Status { get; set; } = Enums.UserStatus.Active;
        public DateTime? SuspendedUntil { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Like { get; set; }
        public ICollection<AuthSession> AuthSessions { get; set; } = new List<AuthSession>();
        public ICollection<VerificationChallenge> VerificationChallenges { get; set; } = new List<VerificationChallenge>();

        public ICollection<Follower> Followers { get; set; }
        public ICollection<Follower> Followings { get; set; }

    }
}
