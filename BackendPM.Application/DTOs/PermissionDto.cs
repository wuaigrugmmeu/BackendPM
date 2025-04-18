using System;
using System.Collections.Generic;

namespace BackendPM.Application.DTOs
{
    /// <summary>
    /// 权限DTO
    /// </summary>
    public class PermissionDto
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 权限类型：0=菜单，1=按钮，2=API
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 权限组ID
        /// </summary>
        public Guid? GroupId { get; set; }
        
        /// <summary>
        /// API资源路径
        /// </summary>
        public string ApiResource { get; set; }
        
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
        /// 包含的子权限列表（如果是权限组）
        /// </summary>
        public List<PermissionDto> Children { get; set; } = new List<PermissionDto>();
    }
    
    /// <summary>
    /// 权限创建请求DTO
    /// </summary>
    public class CreatePermissionDto
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 权限类型：0=菜单，1=按钮，2=API
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 权限组ID
        /// </summary>
        public Guid? GroupId { get; set; }
        
        /// <summary>
        /// API资源路径
        /// </summary>
        public string ApiResource { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;
    }
    
    /// <summary>
    /// 权限更新请求DTO
    /// </summary>
    public class UpdatePermissionDto
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; set; }
    }
    
    /// <summary>
    /// 权限组创建请求DTO
    /// </summary>
    public class CreatePermissionGroupDto
    {
        /// <summary>
        /// 权限组名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 权限组编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 权限组描述
        /// </summary>
        public string Description { get; set; }
    }
    
    /// <summary>
    /// 权限树节点DTO
    /// </summary>
    public class PermissionTreeDto
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 权限类型：0=菜单，1=按钮，2=API
        /// </summary>
        public int Type { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// 子权限列表
        /// </summary>
        public List<PermissionTreeDto> Children { get; set; } = new List<PermissionTreeDto>();
    }
    
    /// <summary>
    /// API权限注册请求DTO
    /// </summary>
    public class RegisterApiPermissionDto
    {
        /// <summary>
        /// API资源路径
        /// </summary>
        public string ApiResource { get; set; }
        
        /// <summary>
        /// HTTP方法
        /// </summary>
        public string HttpMethod { get; set; }
        
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 权限组ID
        /// </summary>
        public Guid? GroupId { get; set; }
    }
}