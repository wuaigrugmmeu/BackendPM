using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Repositories
{
    /// <summary>
    /// 角色仓储接口
    /// </summary>
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        /// 根据角色编码获取角色
        /// </summary>
        /// <param name="code">角色编码</param>
        /// <returns>角色实体</returns>
        Task<Role> GetByCodeAsync(string code);
        
        /// <summary>
        /// 检查角色编码是否存在
        /// </summary>
        /// <param name="code">角色编码</param>
        /// <returns>是否存在</returns>
        Task<bool> CodeExistsAsync(string code);
        
        /// <summary>
        /// 获取角色的所有权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId);
        
        /// <summary>
        /// 分配权限给角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permissionIds">权限ID列表</param>
        Task AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds);
        
        /// <summary>
        /// 删除角色的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permissionId">权限ID</param>
        Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);
        
        /// <summary>
        /// 清空角色的所有权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        Task ClearRolePermissionsAsync(Guid roleId);
        
        /// <summary>
        /// 获取角色的所有用户
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<User>> GetRoleUsersAsync(Guid roleId);
        
        /// <summary>
        /// 获取子角色列表
        /// </summary>
        /// <param name="parentId">父角色ID</param>
        /// <returns>子角色列表</returns>
        Task<IEnumerable<Role>> GetChildRolesAsync(Guid parentId);
        
        /// <summary>
        /// 获取角色继承链
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色继承链</returns>
        Task<IEnumerable<Role>> GetRoleInheritanceChainAsync(Guid roleId);
    }
}