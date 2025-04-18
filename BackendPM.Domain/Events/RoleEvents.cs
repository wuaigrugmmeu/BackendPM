using System;
using System.Collections.Generic;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Events
{
    /// <summary>
    /// 角色创建事件
    /// </summary>
    public class RoleCreatedEvent : DomainEvent
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 角色编码
        /// </summary>
        public string Code { get; }
        
        /// <summary>
        /// 是否是系统角色
        /// </summary>
        public bool IsSystem { get; }
        
        public RoleCreatedEvent(Role role)
        {
            RoleId = role.Id;
            Name = role.Name;
            Code = role.Code;
            IsSystem = role.IsSystem;
        }
    }
    
    /// <summary>
    /// 角色更新事件
    /// </summary>
    public class RoleUpdatedEvent : DomainEvent
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; }
        
        public RoleUpdatedEvent(Role role)
        {
            RoleId = role.Id;
            Name = role.Name;
        }
    }
    
    /// <summary>
    /// 角色删除事件
    /// </summary>
    public class RoleDeletedEvent : DomainEvent
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; }
        
        public RoleDeletedEvent(Role role)
        {
            RoleId = role.Id;
            Name = role.Name;
        }
    }
    
    /// <summary>
    /// 角色状态变更事件
    /// </summary>
    public class RoleStatusChangedEvent : DomainEvent
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; }
        
        public RoleStatusChangedEvent(Role role, bool isEnabled)
        {
            RoleId = role.Id;
            Name = role.Name;
            IsEnabled = isEnabled;
        }
    }
    
    /// <summary>
    /// 角色权限分配事件
    /// </summary>
    public class RolePermissionAssignedEvent : DomainEvent
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; }
        
        /// <summary>
        /// 权限ID列表
        /// </summary>
        public IEnumerable<Guid> PermissionIds { get; }
        
        public RolePermissionAssignedEvent(Guid roleId, IEnumerable<Guid> permissionIds)
        {
            RoleId = roleId;
            PermissionIds = permissionIds;
        }
    }
    
    /// <summary>
    /// 角色权限移除事件
    /// </summary>
    public class RolePermissionRemovedEvent : DomainEvent
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; }
        
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid PermissionId { get; }
        
        public RolePermissionRemovedEvent(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
    
    /// <summary>
    /// 角色权限清空事件
    /// </summary>
    public class RolePermissionsClearedEvent : DomainEvent
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; }
        
        public RolePermissionsClearedEvent(Guid roleId)
        {
            RoleId = roleId;
        }
    }
}