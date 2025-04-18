using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;

namespace BackendPM.Application.Services
{
    /// <summary>
    /// 菜单应用服务接口
    /// </summary>
    public interface IMenuAppService
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="createMenuDto">菜单信息</param>
        /// <returns>菜单信息</returns>
        Task<MenuDto> CreateMenuAsync(CreateMenuDto createMenuDto);
        
        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="updateMenuDto">更新信息</param>
        /// <returns>更新后的菜单信息</returns>
        Task<MenuDto> UpdateMenuAsync(Guid menuId, UpdateMenuDto updateMenuDto);
        
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteMenuAsync(Guid menuId);
        
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="keywords">搜索关键字</param>
        /// <returns>菜单列表和总数</returns>
        Task<(IEnumerable<MenuDto> Data, int Total)> GetMenuListAsync(int pageIndex, int pageSize, string keywords = null);
        
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns>菜单列表</returns>
        Task<IEnumerable<MenuDto>> GetAllMenusAsync();
        
        /// <summary>
        /// 获取菜单详情
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>菜单详情</returns>
        Task<MenuDto> GetMenuDetailAsync(Guid menuId);
        
        /// <summary>
        /// 启用菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>是否成功</returns>
        Task<bool> EnableMenuAsync(Guid menuId);
        
        /// <summary>
        /// 禁用菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DisableMenuAsync(Guid menuId);
        
        /// <summary>
        /// 设置菜单显示状态
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="visible">是否显示</param>
        /// <returns>是否成功</returns>
        Task<bool> SetMenuVisibilityAsync(Guid menuId, bool visible);
        
        /// <summary>
        /// 设置菜单排序
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="sort">排序值</param>
        /// <returns>是否成功</returns>
        Task<bool> SetMenuSortAsync(Guid menuId, int sort);
        
        /// <summary>
        /// 设置菜单父级
        /// </summary>
        /// <param name="setMenuParentDto">菜单父级信息</param>
        /// <returns>更新后的菜单信息</returns>
        Task<MenuDto> SetMenuParentAsync(SetMenuParentDto setMenuParentDto);
        
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="menuAssignPermissionsDto">菜单权限信息</param>
        /// <returns>是否成功</returns>
        Task<bool> AssignPermissionsAsync(MenuAssignPermissionsDto menuAssignPermissionsDto);
        
        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<PermissionDto>> GetMenuPermissionsAsync(Guid menuId);
        
        /// <summary>
        /// 获取菜单树形结构
        /// </summary>
        /// <param name="rootId">根菜单ID，如果为null则返回所有菜单</param>
        /// <returns>菜单树形结构</returns>
        Task<IEnumerable<MenuTreeDto>> GetMenuTreeAsync(Guid? rootId = null);
        
        /// <summary>
        /// 获取用户菜单树
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>菜单树形结构</returns>
        Task<IEnumerable<MenuTreeDto>> GetUserMenuTreeAsync(Guid userId);
        
        /// <summary>
        /// 获取角色菜单树
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>菜单树形结构</returns>
        Task<IEnumerable<MenuTreeDto>> GetRoleMenuTreeAsync(Guid roleId);
        
        /// <summary>
        /// 获取用户前端路由菜单
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>前端路由菜单</returns>
        Task<IEnumerable<RouterMenuDto>> GetUserRouterMenusAsync(Guid userId);
        
        /// <summary>
        /// 检查菜单编码是否存在
        /// </summary>
        /// <param name="code">菜单编码</param>
        /// <returns>是否存在</returns>
        Task<bool> CheckMenuCodeExistsAsync(string code);
        
        /// <summary>
        /// 获取指定类型的菜单
        /// </summary>
        /// <param name="type">菜单类型</param>
        /// <returns>菜单列表</returns>
        Task<IEnumerable<MenuDto>> GetMenusByTypeAsync(int type);
    }
}