using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Services
{
    /// <summary>
    /// 菜单领域服务接口
    /// </summary>
    public interface IMenuDomainService
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="name">菜单名称</param>
        /// <param name="code">菜单编码</param>
        /// <param name="type">菜单类型：0目录，1菜单，2按钮</param>
        /// <param name="component">组件路径</param>
        /// <param name="route">路由地址</param>
        /// <param name="parentId">父菜单ID</param>
        /// <param name="icon">图标</param>
        /// <param name="permissions">权限标识</param>
        /// <returns>创建的菜单实体</returns>
        Task<Menu> CreateMenuAsync(string name, string code, int type, string component, string route, Guid? parentId = null, string icon = null, string permissions = null);
        
        /// <summary>
        /// 更新菜单信息
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="name">菜单名称</param>
        /// <param name="component">组件路径</param>
        /// <param name="route">路由地址</param>
        /// <param name="icon">图标</param>
        /// <param name="permissions">权限标识</param>
        /// <returns>更新后的菜单实体</returns>
        Task<Menu> UpdateMenuAsync(Guid menuId, string name, string component, string route, string icon, string permissions);
        
        /// <summary>
        /// 设置菜单父级
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="parentId">父菜单ID</param>
        /// <returns>更新后的菜单实体</returns>
        Task<Menu> SetMenuParentAsync(Guid menuId, Guid? parentId);
        
        /// <summary>
        /// 设置菜单可见性
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="visible">是否可见</param>
        /// <returns>更新后的菜单实体</returns>
        Task<Menu> SetMenuVisibilityAsync(Guid menuId, bool visible);
        
        /// <summary>
        /// 启用菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>更新后的菜单实体</returns>
        Task<Menu> EnableMenuAsync(Guid menuId);
        
        /// <summary>
        /// 禁用菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>更新后的菜单实体</returns>
        Task<Menu> DisableMenuAsync(Guid menuId);
        
        /// <summary>
        /// 设置菜单排序
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="sort">排序序号</param>
        /// <returns>更新后的菜单实体</returns>
        Task<Menu> SetMenuSortAsync(Guid menuId, int sort);
        
        /// <summary>
        /// 将权限分配给菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="permissionIds">权限ID列表</param>
        /// <returns>操作是否成功</returns>
        Task<bool> AssignPermissionsToMenuAsync(Guid menuId, IEnumerable<Guid> permissionIds);
        
        /// <summary>
        /// 移除菜单的权限
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="permissionId">权限ID</param>
        /// <returns>操作是否成功</returns>
        Task<bool> RemovePermissionFromMenuAsync(Guid menuId, Guid permissionId);
        
        /// <summary>
        /// 清空菜单的所有权限
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>操作是否成功</returns>
        Task<bool> ClearMenuPermissionsAsync(Guid menuId);
        
        /// <summary>
        /// 获取菜单的所有权限
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<Permission>> GetMenuPermissionsAsync(Guid menuId);
        
        /// <summary>
        /// 获取用户可访问的菜单树
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>菜单树</returns>
        Task<IEnumerable<Menu>> GetUserMenuTreeAsync(Guid userId);
        
        /// <summary>
        /// 获取角色可访问的菜单树
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>菜单树</returns>
        Task<IEnumerable<Menu>> GetRoleMenuTreeAsync(Guid roleId);
        
        /// <summary>
        /// 获取完整的菜单树
        /// </summary>
        /// <param name="rootId">根菜单ID，为null则获取所有根菜单</param>
        /// <returns>菜单树</returns>
        Task<IEnumerable<Menu>> GetFullMenuTreeAsync(Guid? rootId = null);
        
        /// <summary>
        /// 检查菜单是否有子菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>是否有子菜单</returns>
        Task<bool> HasChildrenAsync(Guid menuId);
    }
}