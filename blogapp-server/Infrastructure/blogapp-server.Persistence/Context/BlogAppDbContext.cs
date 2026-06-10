using blogapp_server.Domain.Entities;
using blogapp_server.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Persistence.Context
{
    public class BlogAppDbContext : IdentityDbContext<User,Role, int>
    {
        public BlogAppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Utility> Utilities { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ModerationAction> ModerationActions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Endpoint> Endpoints { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(post => post.Status)
                .HasDefaultValue(blogapp_server.Domain.Enums.PostStatus.Published);

            modelBuilder.Entity<User>()
                .Property(user => user.Status)
                .HasDefaultValue(blogapp_server.Domain.Enums.UserStatus.Active);

            modelBuilder.Entity<Follower>(entity =>
            {
                entity.HasOne(f => f.FollowerUser)
                    .WithMany(u => u.Followings)
                    .HasForeignKey(f => f.FollowerId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(f => f.FollowingUser)
                    .WithMany(u => u.Followers)
                    .HasForeignKey(f => f.FollowingId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            modelBuilder.Entity<Follower>(entity =>
            {
                entity.HasIndex(x => new { x.FollowerId, x.FollowingId })
                    .IsUnique();
            });


            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(n => n.Type)
                    .HasConversion<string>();
            });


            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasCheckConstraint(
                    "CK_Reports_Target",
                    "(\"TargetType\" = 0 AND \"TargetPostId\" IS NOT NULL AND \"TargetUserId\" IS NULL) OR " +
                    "(\"TargetType\" = 1 AND \"TargetPostId\" IS NULL AND \"TargetUserId\" IS NOT NULL)");

                entity.Property(r => r.Description)
                    .HasMaxLength(1000);
                
                entity.Property(r => r.ReviewNote)
                    .HasMaxLength(1000);

                entity.HasOne(r => r.ReporterUser)
                    .WithMany()
                    .HasForeignKey(r => r.ReporterUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.TargetUser)
                    .WithMany()
                    .HasForeignKey(r => r.TargetUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.ReviewedByUser)
                    .WithMany()
                    .HasForeignKey(r => r.ReviewedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.TargetPost)
                    .WithMany()
                    .HasForeignKey(r => r.TargetPostId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(r => new
                {
                    r.ReporterUserId,
                    r.TargetType,
                    r.TargetPostId,
                    r.TargetUserId
                });

                entity.HasIndex(r => new { r.ReporterUserId, r.TargetPostId })
                    .IsUnique()
                    .HasFilter("\"TargetType\" = 0 AND \"Status\" IN (1, 2)");

                entity.HasIndex(r => new { r.ReporterUserId, r.TargetUserId })
                    .IsUnique()
                    .HasFilter("\"TargetType\" = 1 AND \"Status\" IN (1, 2)");

                entity.HasIndex(r => new
                {
                    r.TargetType,
                    r.TargetPostId,
                    r.TargetUserId,
                    r.Status
                });

                entity.HasIndex(r => r.Status);
                entity.HasIndex(r => r.CreatedAt);
            });

            modelBuilder.Entity<ModerationAction>(entity =>
            {
                entity.HasKey(action => action.Id);

                entity.HasCheckConstraint(
                    "CK_ModerationActions_Target",
                    "(\"TargetType\" = 0 AND \"TargetPostId\" IS NOT NULL) OR " +
                    "(\"TargetType\" = 1 AND \"TargetUserId\" IS NOT NULL)");

                entity.Property(action => action.Note)
                    .HasMaxLength(1000);

                entity.HasOne(action => action.Report)
                    .WithMany(report => report.ModerationActions)
                    .HasForeignKey(action => action.ReportId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(action => action.ModeratorUser)
                    .WithMany()
                    .HasForeignKey(action => action.ModeratorUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(action => action.TargetPost)
                    .WithMany()
                    .HasForeignKey(action => action.TargetPostId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(action => action.TargetUser)
                    .WithMany()
                    .HasForeignKey(action => action.TargetUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(action => action.ReportId);
                entity.HasIndex(action => action.ModeratorUserId);
                entity.HasIndex(action => new { action.TargetType, action.TargetPostId, action.TargetUserId });
                entity.HasIndex(action => action.CreatedAt);
            });
                


            base.OnModelCreating(modelBuilder);
        }

    }
}
