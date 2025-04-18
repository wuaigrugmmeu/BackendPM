using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Repositories
{
    /// <summary>
    /// 菜单仓储接口
    /// </summary>
    public interface IMenuRepository : IRepository<Menu>
    {
        /// <summary>
        /// 根据菜单编码获取菜单
        /// </summary>
        /// <param name="code">菜单编码</param>
        /// <returns>菜单实体</returns>
        Task<Menu> GetByCodeAsync(string code);
        
        /// <summary>
        /// 检查菜单编码是否存在
        /// </summary>
        /// <param name="code">菜单编码</param>
        /// <returns>是否存在</returns>
        Task<bool> CodeExistsAsync(string code);
        
        /// <summary>
        /// 获取子菜单列表
        /// </summary>
        /// <param name="parentId">父菜单ID</param>
        /// <returns>子菜单列表</returns>
        Task<IEnumerable<Menu>> GetChildrenAsync(Guid parentId);
        
        /// <summary>
        /// 获取所有根菜单（没有父菜单的菜单）
        /// </summary>
        /// <returns>根菜单列表</returns>
        Task<IEnumerable<Menu>> GetRootMenusAsync();
        
        /// <summary>
        /// 获取菜单树（递归构建的层级结构）
        /// </summary>
        /// <param name="rootId">根菜单ID，为null则从所有根菜单开始</param>
        /// <returns>菜单树结构</returns>
        Task<IEnumerable<Menu>> GetMenuTreeAsync(Guid? rootId = null);
        
        /// <summary>
        /// 获取指定类型的菜单
        /// </summary>
        /// <param name="type">菜单类型：0目录，1菜单，2按钮</param>
        /// <returns>菜单列表</returns>
        Task<IEnumerable<Menu>> GetByTypeAsync(int type);
        
        /// <summary>
        /// 获取菜单的所有权限
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<Permission>> GetMenuPermissionsAsync(Guid menuId);
        
        /// <summary>
        /// 分配权限给菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="permissionIds">权限ID列表</param>
        Task AssignPermissionsToMenuAsync(Guid menuId, IEnumerable<Guid> permissionIds);
        
        /// <summary>
        /// 删除菜单的权限
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="permissionId">权限ID</param>
        Task RemovePermissionFromMenuAsync(Guid menuId, Guid permissionId);
        
        /// <summary>
        /// 清空菜单的所有权限
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        Task ClearMenuPermissionsAsync(Guid menuId);
        
        /// <summary>
        /// 更新菜单路径
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="newPath">新路径</param>
        Task UpdatePathAsync(Guid menuId, string newPath);
        
        /// <summary>
        /// 批量更新子菜单路径（当父菜单路径变更时）
        /// </summary>
        /// <param name="parentId">父菜单ID</param>
        /// <param name="parentPath">父菜单新路径</param>
        Task UpdateChildrenPathsAsync(Guid parentId, string parentPath);
        
        /// <summary>
        /// 获取用户可访问的菜单列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>菜单列表</returns>
        Task<IEnumerable<Menu>> GetUserMenusAsync(Guid userId);
        
        /// <summary>
        /// 获取角色可访问的菜单列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>菜单列表</returns>
        Task<IEnumerable<Menu>> GetRoleMenusAsync(Guid roleId);
    }
}