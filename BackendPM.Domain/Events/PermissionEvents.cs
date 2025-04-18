using System;
using BackendPM.Domain.Entities;
using BackendPM.Domain.ValueObjects;

namespace BackendPM.Domain.Events
{
    /// <summary>
    /// 权限创建事件
    /// </summary>
    public class PermissionCreatedEvent : DomainEvent
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid PermissionId { get; }
        
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; }
        
        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; }
        
        public PermissionCreatedEvent(Permission permission)
        {
            PermissionId = permission.Id;
            Name = permission.Name;
            Code = permission.Code;
            Type = permission.Type;
        }
    }
    
    /// <summary>
    /// 权限更新事件
    /// </summary>
    public class PermissionUpdatedEvent : DomainEvent
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid PermissionId { get; }
        
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; }
        
        public PermissionUpdatedEvent(Permission permission)
        {
            PermissionId = permission.Id;
            Name = permission.Name;
        }
    }
    
    /// <summary>
    /// 权限删除事件
    /// </summary>
    public class PermissionDeletedEvent : DomainEvent
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid PermissionId { get; }
        
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; }
        
        public PermissionDeletedEvent(Permission permission)
        {
            PermissionId = permission.Id;
            Name = permission.Name;
            Code = permission.Code;
        }
    }
    
    /// <summary>
    /// 权限状态变更事件
    /// </summary>
    public class PermissionStatusChangedEvent : DomainEvent
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid PermissionId { get; }
        
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; }
        
        public PermissionStatusChangedEvent(Permission permission, bool isEnabled)
        {
            PermissionId = permission.Id;
            Name = permission.Name;
            IsEnabled = isEnabled;
        }
    }
    
    /// <summary>
    /// API权限注册事件
    /// </summary>
    public class ApiPermissionRegisteredEvent : DomainEvent
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid PermissionId { get; }
        
        /// <summary>
        /// API资源路径
        /// </summary>
        public string ApiResource { get; }
        
        /// <summary>
        /// HTTP方法
        /// </summary>
        public string HttpMethod { get; }
        
        public ApiPermissionRegisteredEvent(Permission permission, string httpMethod)
        {
            PermissionId = permission.Id;
            ApiResource = permission.ApiResource;
            HttpMethod = httpMethod;
        }
    }
    
    /// <summary>
    /// 权限分组变更事件
    /// </summary>
    public class PermissionGroupChangedEvent : DomainEvent
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid PermissionId { get; }
        
        /// <summary>
        /// 旧权限组ID
        /// </summary>
        public Guid? OldGroupId { get; }
        
        /// <summary>
        /// 新权限组ID
        /// </summary>
        public Guid? NewGroupId { get; }
        
        public PermissionGroupChangedEvent(Guid permissionId, Guid? oldGroupId, Guid? newGroupId)
        {
            PermissionId = permissionId;
            OldGroupId = oldGroupId;
            NewGroupId = newGroupId;
        }
    }
}