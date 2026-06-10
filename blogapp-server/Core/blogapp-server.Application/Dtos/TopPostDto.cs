using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Dtos
{
    public class TopPostDto
    {
        public int Rank { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int DailyLikeCount { get; set; }
        public int DailyCommentCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int BookmarkCount { get; set; }
        public double Score { get; set; }
    }
}
