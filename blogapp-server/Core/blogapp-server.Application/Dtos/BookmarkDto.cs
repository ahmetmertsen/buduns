using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Dtos
{
    public class BookmarkDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public DateTime SavedAt { get; set; }
        public PostDto Post { get; set; } = null!;
    }
}
