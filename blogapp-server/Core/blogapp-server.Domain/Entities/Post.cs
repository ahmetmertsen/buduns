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
    public class Post : BaseEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool isPublished { get; set; }
        public PostStatus Status { get; set; } = PostStatus.Published;

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Tag> Tags { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }

    }
}
