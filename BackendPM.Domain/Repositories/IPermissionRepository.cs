using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;
using BackendPM.Domain.ValueObjects;

namespace BackendPM.Domain.Repositories
{
    /// <summary>
    /// 权限仓储接口
    /// </summary>
    public interface IPermissionRepository : IRepository<Permission>
    {
        /// <summary>
        /// 根据权限编码获取权限
        /// </summary>
        /// <param name="code">权限编码</param>
        /// <returns>权限实体</returns>
        Task<Permission> GetByCodeAsync(string code);
        
        /// <summary>
        /// 检查权限编码是否存在
        /// </summary>
        /// <param name="code">权限编码</param>
        /// <returns>是否存在</returns>
        Task<bool> CodeExistsAsync(string code);
        
        /// <summary>
        /// 获取指定类型的所有权限
        /// </summary>
        /// <param name="type">权限类型</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<Permission>> GetByTypeAsync(PermissionType type);
        
        /// <summary>
        /// 获取指定权限组的所有权限
        /// </summary>
        /// <param name="groupId">权限组ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<Permission>> GetByGroupAsync(Guid groupId);
        
        /// <summary>
        /// 获取指定API资源对应的权限
        /// </summary>
        /// <param name="apiResource">API资源路径</param>
        /// <returns>权限实体</returns>
        Task<Permission> GetByApiResourceAsync(string apiResource);
        
        /// <summary>
        /// 获取所有权限组（根据GroupId分组）
        /// </summary>
        /// <returns>权限组列表</returns>
        Task<IEnumerable<Permission>> GetAllGroupsAsync();
        
        /// <summary>
        /// 获取拥有该权限的所有角色
        /// </summary>
        /// <param name="permissionId">权限ID</param>
        /// <returns>角色列表</returns>
        Task<IEnumerable<Role>> GetPermissionRolesAsync(Guid permissionId);
        
        /// <summary>
        /// 获取与该权限关联的所有菜单
        /// </summary>
        /// <param name="permissionId">权限ID</param>
        /// <returns>菜单列表</returns>
        Task<IEnumerable<Menu>> GetPermissionMenusAsync(Guid permissionId);
    }
}