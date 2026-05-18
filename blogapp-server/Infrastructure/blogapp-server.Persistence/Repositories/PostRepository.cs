using blogapp_server.Application.Repositories;
using blogapp_server.Application.Repositories.Common;
using blogapp_server.Application.Features.Posts.Queries.GetDailyTopPosts;
using blogapp_server.Domain.Entities;
using blogapp_server.Persistence.Context;
using blogapp_server.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using blogapp_server.Application.Dtos;

namespace blogapp_server.Persistence.Repositories
{
    public class PostRepository : Repository<Post> , IPostRepository
    {
        private readonly BlogAppDbContext _context;

        public PostRepository(BlogAppDbContext context) : base(context) { _context = context; }

        
        public override async Task<List<Post>> GetAllAsync() =>
            await _context.Posts
                .Include(p => p.Tags)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Include(p => p.Bookmarks)
                .ToListAsync();
        public override async Task<Post?> GetByIdAsync(int id) =>
            await _context.Posts
                .Include(p => p.Tags)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Include(p => p.Bookmarks)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Post?>> GetAllByTagIdAsync(int TagId) => await _context.Posts
            .Include(p => p.Tags)
            .Where(p => p.Tags.Any(t => t.Id == TagId))
            .ToListAsync();

        public async Task<Post?> GetByIdWithTagsAsync(int id) =>
            await _context.Posts
                .Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<TopPostDto>> GetDailyTopPostsAsync(DateTime startDateUtc, DateTime endDateUtc, int limit, CancellationToken cancellationToken = default)
        {
            var safeLimit = Math.Clamp(limit, 1, 100);

            return await _context.Posts
                .AsNoTracking()
                .Where(p => p.isPublished && p.isActive && !p.isDeleted)
                .Select(p => new
                {
                    Post = p,
                    DailyLikeCount = p.Likes.Count(l => l.CreatedAt >= startDateUtc && l.CreatedAt < endDateUtc && l.isActive && !l.isDeleted),
                    DailyCommentCount = p.Comments.Count(c => c.CreatedAt >= startDateUtc && c.CreatedAt < endDateUtc && c.isActive && !c.isDeleted),
                    LikeCount = p.Likes.Count(l => l.isActive && !l.isDeleted),
                    CommentCount = p.Comments.Count(c => c.isActive && !c.isDeleted),
                    BookmarkCount = p.Bookmarks.Count(b => b.isActive && !b.isDeleted)
                })
                .Where(x => x.DailyLikeCount > 0 || x.DailyCommentCount > 0)
                .OrderByDescending(x => (x.DailyLikeCount * 0.4) + (x.DailyCommentCount * 0.6))
                .ThenByDescending(x => x.DailyCommentCount)
                .ThenByDescending(x => x.DailyLikeCount)
                .ThenByDescending(x => x.Post.CreatedAt)
                .Take(safeLimit)
                .Select(x => new TopPostDto
                {
                    PostId = x.Post.Id,
                    Title = x.Post.Title,
                    Content = x.Post.Content,
                    CoverImgUrl = x.Post.CoverImgUrl,
                    UserId = x.Post.UserId,
                    DailyLikeCount = x.DailyLikeCount,
                    DailyCommentCount = x.DailyCommentCount,
                    LikeCount = x.LikeCount,
                    CommentCount = x.CommentCount,
                    BookmarkCount = x.BookmarkCount,
                    Score = (x.DailyLikeCount * 0.4) + (x.DailyCommentCount * 0.6)
                })
                .ToListAsync(cancellationToken);
        }
    }
}
