using System;
using System.Collections.Generic;

namespace BackendPM.Application.DTOs
{
    /// <summary>
    /// 菜单DTO
    /// </summary>
    public class MenuDto
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// 菜单层级路径
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; }
        
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Route { get; set; }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// 权限标识
        /// </summary>
        public string Permissions { get; set; }
        
        /// <summary>
        /// 菜单类型：0目录，1菜单，2按钮
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 是否外链
        /// </summary>
        public bool IsExternal { get; set; }
        
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool KeepAlive { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// 子菜单列表
        /// </summary>
        public List<MenuDto> Children { get; set; } = new List<MenuDto>();
    }
    
    /// <summary>
    /// 菜单创建请求DTO
    /// </summary>
    public class CreateMenuDto
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; }
        
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Route { get; set; }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }
        
        /// <summary>
        /// 权限标识
        /// </summary>
        public string Permissions { get; set; }
        
        /// <summary>
        /// 菜单类型：0目录，1菜单，2按钮
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 是否外链
        /// </summary>
        public bool IsExternal { get; set; } = false;
        
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool KeepAlive { get; set; } = false;
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;
    }
    
    /// <summary>
    /// 菜单更新请求DTO
    /// </summary>
    public class UpdateMenuDto
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; }
        
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Route { get; set; }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }
        
        /// <summary>
        /// 权限标识
        /// </summary>
        public string Permissions { get; set; }
    }
    
    /// <summary>
    /// 菜单树节点DTO
    /// </summary>
    public class MenuTreeDto
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Route { get; set; }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 菜单类型：0目录，1菜单，2按钮
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 子菜单列表
        /// </summary>
        public List<MenuTreeDto> Children { get; set; } = new List<MenuTreeDto>();
    }
    
    /// <summary>
    /// 前端路由菜单DTO
    /// </summary>
    public class RouterMenuDto
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; }
        
        /// <summary>
        /// 菜单元数据
        /// </summary>
        public MenuMeta Meta { get; set; }
        
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Hidden { get; set; }
        
        /// <summary>
        /// 子菜单列表
        /// </summary>
        public List<RouterMenuDto> Children { get; set; } = new List<RouterMenuDto>();
    }
    
    /// <summary>
    /// 菜单元数据
    /// </summary>
    public class MenuMeta
    {
        /// <summary>
        /// 菜单标题
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }
        
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool KeepAlive { get; set; }
        
        /// <summary>
        /// 权限标识
        /// </summary>
        public List<string> Permissions { get; set; }
    }
    
    /// <summary>
    /// 菜单分配权限请求DTO
    /// </summary>
    public class MenuAssignPermissionsDto
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; set; }
        
        /// <summary>
        /// 权限ID列表
        /// </summary>
        public List<Guid> PermissionIds { get; set; }
    }
    
    /// <summary>
    /// 设置菜单父级请求DTO
    /// </summary>
    public class SetMenuParentDto
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; set; }
        
        /// <summary>
        /// 父菜单ID，null表示设置为顶级菜单
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}