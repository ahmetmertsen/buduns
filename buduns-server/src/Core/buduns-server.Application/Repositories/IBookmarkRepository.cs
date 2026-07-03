using buduns_server.Application.Repositories.Common;
using buduns_server.Application.Dtos;
using buduns_server.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buduns_server.Application.Repositories
{
    public interface IBookmarkRepository : IRepository<Bookmark>
    {
        Task<(Bookmark Bookmark, bool Created)> CreateIfNotExistsAsync(Bookmark bookmark, CancellationToken cancellationToken);
        Task<bool> DeleteByUserAndPostAsync(int userId, int postId, CancellationToken cancellationToken);
        Task<(List<BookmarkDto> Items, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int size, CancellationToken cancellationToken);
        Task<Bookmark?> GetByUserAndPostAsync(int userId, int postId, CancellationToken cancellationToken);
    }
}
