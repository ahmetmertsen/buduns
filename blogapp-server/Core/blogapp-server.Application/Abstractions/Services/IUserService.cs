using blogapp_server.Application.Dtos;
using blogapp_server.Application.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blogapp_server.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<RegisterUserResponseDto> RegisterAsync(RegisterUserRequestDto request, CancellationToken cancellationToken);
        Task<UpdateUserPasswordResponse> UpdatePasswordAsync(UpdateUserPasswordRequest request, CancellationToken cancellationToken);
        Task<UpdateUserMailVerifyResponse> UpdateUserMailVerify(UpdateUserMailVerifyRequest request);
        Task<UpdateUserProfileResponse> UpdateUserProfile(UpdateUserProfileRequest request);
        Task<UpdateUserEmailResponse> UpdateUserEmailAsync(UpdateUserEmailRequest request);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserById(int userId);
        Task<UserDto> GetUserByUserName(string userName);
        Task AssignRoleToUserAsync(int userId, string[] roles, CancellationToken cancellationToken);
        Task<string[]> GetRolesToUserAsync(int userId);
        Task<bool> HasRolePermissionToEndpointAsync(int userId, string code);
    }
}
