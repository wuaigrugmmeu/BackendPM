using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;
using BackendPM.Domain.Events;
using BackendPM.Domain.Repositories;
using BackendPM.Domain.Services;
using BackendPM.Domain.ValueObjects;

namespace BackendPM.Infrastructure.Services
{
    /// <summary>
    /// 用户领域服务实现
    /// </summary>
    public class UserDomainService : IUserDomainService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPermissionRepository _permissionRepository;
        
        public UserDomainService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IMenuRepository menuRepository,
            IPermissionRepository permissionRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;
                
            if (!VerifyPassword(user, oldPassword))
                return false;
                
            string newPasswordHash = HashPassword(newPassword);
            user.ChangePassword(newPasswordHash);
            
            await _userRepository.UpdateAsync(user);
            
            // 触发密码变更事件
            var passwordChangedEvent = new UserPasswordChangedEvent(user);
            // TODO: 使用领域事件发布器发布事件
            
            return true;
        }

        public async Task<User> CreateUserAsync(string username, string password, string email, string phoneNumber)
        {
            // 验证用户名、邮箱和手机号的唯一性
            if (await _userRepository.UsernameExistsAsync(username))
                throw new InvalidOperationException($"用户名 {username} 已存在");
                
            if (!string.IsNullOrEmpty(email) && await _userRepository.EmailExistsAsync(email))
                throw new InvalidOperationException($"邮箱 {email} 已存在");
                
            if (!string.IsNullOrEmpty(phoneNumber) && await _userRepository.PhoneNumberExistsAsync(phoneNumber))
                throw new InvalidOperationException($"手机号 {phoneNumber} 已存在");
            
            // 创建用户
            var passwordHash = HashPassword(password);
            var user = new User(username, passwordHash, email, phoneNumber);
            
            await _userRepository.AddAsync(user);
            
            // 触发用户创建事件
            var userCreatedEvent = new UserCreatedEvent(user);
            // TODO: 使用领域事件发布器发布事件
            
            return user;
        }

        public async Task<IEnumerable<Menu>> GetUserMenusAsync(Guid userId)
        {
            return await _menuRepository.GetUserMenusAsync(userId);
        }

        public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            return await _userRepository.GetUserPermissionsAsync(userId);
        }

        public async Task<bool> HasPermissionAsync(Guid userId, string permissionCode)
        {
            // 获取用户的所有权限
            var permissions = await _userRepository.GetUserPermissionsAsync(userId);
            
            // 检查是否包含指定权限编码的权限
            foreach (var permission in permissions)
            {
                if (permission.Code.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            
            return false;
        }

        public async Task<bool> HasRoleAsync(Guid userId, string roleCode)
        {
            // 获取用户的所有角色
            var roles = await _userRepository.GetUserRolesAsync(userId);
            
            // 检查是否包含指定编码的角色
            foreach (var role in roles)
            {
                if (role.Code.Equals(roleCode, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            
            return false;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public async Task<(bool Success, User User, string FailReason)> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            
            if (user == null)
                return (false, null, "用户不存在");
            
            if (user.Status == UserStatus.Inactive)
                return (false, null, "用户已停用");
                
            if (user.IsLocked())
                return (false, null, "账户已锁定");
            
            if (!VerifyPassword(user, password))
            {
                user.RecordLoginFailure();
                await _userRepository.UpdateAsync(user);
                return (false, null, "密码错误");
            }
            
            // 登录成功，更新最后登录时间
            user.RecordLoginSuccess();
            await _userRepository.UpdateAsync(user);
            
            // 触发登录事件
            var loginEvent = new UserLoggedInEvent(user, "127.0.0.1", "Unknown");
            // TODO: 使用领域事件发布器发布事件
            
            return (true, user, null);
        }

        public async Task<bool> ResetPasswordAsync(Guid userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;
                
            string newPasswordHash = HashPassword(newPassword);
            user.ChangePassword(newPasswordHash);
            
            await _userRepository.UpdateAsync(user);
            
            // 触发密码变更事件
            var passwordChangedEvent = new UserPasswordChangedEvent(user);
            // TODO: 使用领域事件发布器发布事件
            
            return true;
        }

        public async Task<User> UpdateUserInfoAsync(User user, string email, string phoneNumber, string realName, string avatar, int gender, DateTime? birthday)
        {
            // 验证邮箱和手机号的唯一性
            if (!string.IsNullOrEmpty(email) && email != user.Email && await _userRepository.EmailExistsAsync(email))
                throw new InvalidOperationException($"邮箱 {email} 已存在");
                
            if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber != user.PhoneNumber && await _userRepository.PhoneNumberExistsAsync(phoneNumber))
                throw new InvalidOperationException($"手机号 {phoneNumber} 已存在");
            
            // 更新基本信息
            if (!string.IsNullOrEmpty(email))
                user.UpdateEmail(email);
                
            if (!string.IsNullOrEmpty(phoneNumber))
                user.UpdatePhoneNumber(phoneNumber);
            
            // 更新个人资料
            Gender genderEnum = (Gender)gender;
            user.UpdateProfile(realName, avatar, genderEnum, birthday);
            
            await _userRepository.UpdateAsync(user);
            
            // 触发用户更新事件
            var userUpdatedEvent = new UserUpdatedEvent(user);
            // TODO: 使用领域事件发布器发布事件
            
            return user;
        }

        public bool VerifyPassword(User user, string password)
        {
            var hashedPassword = HashPassword(password);
            return user.PasswordHash == hashedPassword;
        }
    }
}