using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Services
{
    /// <summary>
    /// 用户领域服务接口
    /// </summary>
    public interface IUserDomainService
    {
        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <param name="password">明文密码</param>
        /// <returns>验证是否成功</returns>
        bool VerifyPassword(User user, string password);
        
        /// <summary>
        /// 创建密码哈希
        /// </summary>
        /// <param name="password">明文密码</param>
        /// <returns>密码哈希</returns>
        string HashPassword(string password);
        
        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">明文密码</param>
        /// <param name="email">邮箱</param>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>创建的用户实体</returns>
        Task<User> CreateUserAsync(string username, string password, string email, string phoneNumber);
        
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录是否成功，以及用户实体</returns>
        Task<(bool Success, User User, string FailReason)> LoginAsync(string username, string password);
        
        /// <summary>
        /// 检查用户是否拥有指定权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionCode">权限编码</param>
        /// <returns>是否拥有权限</returns>
        Task<bool> HasPermissionAsync(Guid userId, string permissionCode);
        
        /// <summary>
        /// 检查用户是否拥有指定角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleCode">角色编码</param>
        /// <returns>是否拥有角色</returns>
        Task<bool> HasRoleAsync(Guid userId, string roleCode);
        
        /// <summary>
        /// 获取用户的所有可访问菜单
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>菜单列表</returns>
        Task<IEnumerable<Menu>> GetUserMenusAsync(Guid userId);
        
        /// <summary>
        /// 获取用户的所有权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId);
        
        /// <summary>
        /// 更新用户基本信息
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <param name="email">新邮箱</param>
        /// <param name="phoneNumber">新手机号</param>
        /// <param name="realName">真实姓名</param>
        /// <param name="avatar">头像</param>
        /// <param name="gender">性别</param>
        /// <param name="birthday">生日</param>
        /// <returns>更新后的用户实体</returns>
        Task<User> UpdateUserInfoAsync(User user, string email, string phoneNumber, string realName, string avatar, int gender, DateTime? birthday);
        
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>操作是否成功</returns>
        Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
        
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>操作是否成功</returns>
        Task<bool> ResetPasswordAsync(Guid userId, string newPassword);
    }
}