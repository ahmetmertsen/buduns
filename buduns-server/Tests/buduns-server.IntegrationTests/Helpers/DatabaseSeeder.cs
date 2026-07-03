using buduns_server.Application.Common.Consts;
using buduns_server.Domain.Entities;
using buduns_server.Domain.Entities.Identity;
using buduns_server.Domain.Enums;
using buduns_server.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace buduns_server.IntegrationTests.Helpers;

public static class DatabaseSeeder
{
    public const string DefaultPassword = "Integration123!";

    public static async Task SeedSystemRolesAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<Role>>();
        foreach (var roleName in new[] { RoleConstants.Admin, RoleConstants.Moderator, RoleConstants.User })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new Role { Name = roleName });
                EnsureSucceeded(result, $"Role could not be created: {roleName}");
            }
        }
    }

    public static async Task<User> CreateUserAsync(IServiceProvider services, string userName, string fullName, params string[] roles)
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var user = new User
        {
            UserName = userName,
            Email = $"{userName}@integration.test",
            FullName = fullName,
            EmailConfirmed = true,
            Status = UserStatus.Active
        };
        EnsureSucceeded(await userManager.CreateAsync(user, DefaultPassword), $"User could not be created: {userName}");
        if (roles.Length > 0)
        {
            EnsureSucceeded(await userManager.AddToRolesAsync(user, roles), $"Roles could not be assigned to user: {userName}");
        }

        return user;
    }

    public static async Task<Post> CreatePostAsync(IServiceProvider services, int userId, string content = "Integration test post")
    {
        var context = services.GetRequiredService<BudunsDbContext>();
        var post = new Post { UserId = userId, Content = content, isPublished = true, Status = PostStatus.Published, isActive = true, isDeleted = false, CreatedAt = DateTime.UtcNow };
        context.Posts.Add(post);
        await context.SaveChangesAsync();
        return post;
    }

    private static void EnsureSucceeded(IdentityResult result, string message)
    {
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"{message}. {string.Join(", ", result.Errors.Select(error => error.Description))}");
        }
    }
}
