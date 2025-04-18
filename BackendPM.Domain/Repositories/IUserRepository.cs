using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Repositories
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>用户实体</returns>
        Task<User> GetByUsernameAsync(string username);
        
        /// <summary>
        /// 根据邮箱获取用户
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns>用户实体</returns>
        Task<User> GetByEmailAsync(string email);
        
        /// <summary>
        /// 根据手机号获取用户
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>用户实体</returns>
        Task<User> GetByPhoneNumberAsync(string phoneNumber);
        
        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>是否存在</returns>
        Task<bool> UsernameExistsAsync(string username);
        
        /// <summary>
        /// 检查邮箱是否存在
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns>是否存在</returns>
        Task<bool> EmailExistsAsync(string email);
        
        /// <summary>
        /// 检查手机号是否存在
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>是否存在</returns>
        Task<bool> PhoneNumberExistsAsync(string phoneNumber);
        
        /// <summary>
        /// 获取用户的角色列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>角色列表</returns>
        Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId);
        
        /// <summary>
        /// 获取用户的权限列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId);
        
        /// <summary>
        /// 获取用户的部门列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>部门列表</returns>
        Task<IEnumerable<Department>> GetUserDepartmentsAsync(Guid userId);
        
        /// <summary>
        /// 获取用户的主部门
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>主部门</returns>
        Task<Department> GetUserPrimaryDepartmentAsync(Guid userId);
        
        /// <summary>
        /// 分配角色给用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleIds">角色ID列表</param>
        Task AssignRolesToUserAsync(Guid userId, IEnumerable<Guid> roleIds);
        
        /// <summary>
        /// 删除用户的角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        Task RemoveRoleFromUserAsync(Guid userId, Guid roleId);
        
        /// <summary>
        /// 清空用户的所有角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        Task ClearUserRolesAsync(Guid userId);
        
        /// <summary>
        /// 分配部门给用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <param name="isPrimary">是否为主部门</param>
        Task AssignDepartmentToUserAsync(Guid userId, Guid departmentId, bool isPrimary);
        
        /// <summary>
        /// 删除用户的部门
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="departmentId">部门ID</param>
        Task RemoveDepartmentFromUserAsync(Guid userId, Guid departmentId);
        
        /// <summary>
        /// 设置用户的主部门
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="departmentId">部门ID</param>
        Task SetUserPrimaryDepartmentAsync(Guid userId, Guid departmentId);
        
        /// <summary>
        /// 清空用户的所有部门
        /// </summary>
        /// <param name="userId">用户ID</param>
        Task ClearUserDepartmentsAsync(Guid userId);
    }
}