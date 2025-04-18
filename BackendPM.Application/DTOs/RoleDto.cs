using System;
using System.Collections.Generic;

namespace BackendPM.Application.DTOs
{
    /// <summary>
    /// 角色DTO
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 角色编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 父角色ID
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// 是否是系统角色
        /// </summary>
        public bool IsSystem { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// 子角色列表
        /// </summary>
        public List<RoleDto> Children { get; set; } = new List<RoleDto>();
    }
    
    /// <summary>
    /// 角色创建请求DTO
    /// </summary>
    public class CreateRoleDto
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 角色编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 父角色ID
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// 是否是系统角色
        /// </summary>
        public bool IsSystem { get; set; } = false;
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;
    }
    
    /// <summary>
    /// 角色更新请求DTO
    /// </summary>
    public class UpdateRoleDto
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }
    }
    
    /// <summary>
    /// 角色树节点DTO
    /// </summary>
    public class RoleTreeDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 角色编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 是否是系统角色
        /// </summary>
        public bool IsSystem { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// 子角色列表
        /// </summary>
        public List<RoleTreeDto> Children { get; set; } = new List<RoleTreeDto>();
    }
    
    /// <summary>
    /// 设置角色父级请求DTO
    /// </summary>
    public class SetRoleParentDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        
        /// <summary>
        /// 父角色ID，null表示设置为顶级角色
        /// </summary>
        public Guid? ParentId { get; set; }
    }
    
    /// <summary>
    /// 角色分配权限请求DTO
    /// </summary>
    public class RoleAssignPermissionsDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }
        
        /// <summary>
        /// 权限ID列表
        /// </summary>
        public List<Guid> PermissionIds { get; set; }
    }
}