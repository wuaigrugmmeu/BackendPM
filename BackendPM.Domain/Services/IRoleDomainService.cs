using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Services
{
    /// <summary>
    /// 角色领域服务接口
    /// </summary>
    public interface IRoleDomainService
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name">角色名称</param>
        /// <param name="code">角色编码</param>
        /// <param name="description">角色描述</param>
        /// <param name="isSystem">是否是系统角色</param>
        /// <param name="parentId">父角色ID</param>
        /// <returns>创建的角色实体</returns>
        Task<Role> CreateRoleAsync(string name, string code, string description, bool isSystem = false, Guid? parentId = null);
        
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="name">角色名称</param>
        /// <param name="description">角色描述</param>
        /// <returns>更新后的角色实体</returns>
        Task<Role> UpdateRoleAsync(Guid roleId, string name, string description);
        
        /// <summary>
        /// 设置角色父级
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="parentId">父角色ID</param>
        /// <returns>更新后的角色实体</returns>
        Task<Role> SetRoleParentAsync(Guid roleId, Guid? parentId);
        
        /// <summary>
        /// 启用角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>更新后的角色实体</returns>
        Task<Role> EnableRoleAsync(Guid roleId);
        
        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>更新后的角色实体</returns>
        Task<Role> DisableRoleAsync(Guid roleId);
        
        /// <summary>
        /// 将权限分配给角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permissionIds">权限ID列表</param>
        /// <returns>操作是否成功</returns>
        Task<bool> AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds);
        
        /// <summary>
        /// 移除角色的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permissionId">权限ID</param>
        /// <returns>操作是否成功</returns>
        Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);
        
        /// <summary>
        /// 清空角色的所有权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>操作是否成功</returns>
        Task<bool> ClearRolePermissionsAsync(Guid roleId);
        
        /// <summary>
        /// 获取角色的所有权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId);
        
        /// <summary>
        /// 获取所有系统角色
        /// </summary>
        /// <returns>系统角色列表</returns>
        Task<IEnumerable<Role>> GetSystemRolesAsync();
        
        /// <summary>
        /// 获取所有自定义角色
        /// </summary>
        /// <returns>自定义角色列表</returns>
        Task<IEnumerable<Role>> GetCustomRolesAsync();
        
        /// <summary>
        /// 获取角色的所有用户
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<User>> GetRoleUsersAsync(Guid roleId);
        
        /// <summary>
        /// 获取角色继承树
        /// </summary>
        /// <param name="rootId">根角色ID，为null则获取所有根角色</param>
        /// <returns>角色继承树</returns>
        Task<IEnumerable<Role>> GetRoleTreeAsync(Guid? rootId = null);
    }
}