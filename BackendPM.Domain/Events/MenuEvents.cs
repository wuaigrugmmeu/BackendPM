using System;
using System.Collections.Generic;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Events
{
    /// <summary>
    /// 菜单创建事件
    /// </summary>
    public class MenuCreatedEvent : DomainEvent
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string Code { get; }
        
        /// <summary>
        /// 菜单类型
        /// </summary>
        public int Type { get; }
        
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public Guid? ParentId { get; }
        
        public MenuCreatedEvent(Menu menu)
        {
            MenuId = menu.Id;
            Name = menu.Name;
            Code = menu.Code;
            Type = menu.Type;
            ParentId = menu.ParentId;
        }
    }
    
    /// <summary>
    /// 菜单更新事件
    /// </summary>
    public class MenuUpdatedEvent : DomainEvent
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; }
        
        public MenuUpdatedEvent(Menu menu)
        {
            MenuId = menu.Id;
            Name = menu.Name;
        }
    }
    
    /// <summary>
    /// 菜单删除事件
    /// </summary>
    public class MenuDeletedEvent : DomainEvent
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 菜单编码
        /// </summary>
        public string Code { get; }
        
        public MenuDeletedEvent(Menu menu)
        {
            MenuId = menu.Id;
            Name = menu.Name;
            Code = menu.Code;
        }
    }
    
    /// <summary>
    /// 菜单状态变更事件
    /// </summary>
    public class MenuStatusChangedEvent : DomainEvent
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; }
        
        public MenuStatusChangedEvent(Menu menu, bool isEnabled)
        {
            MenuId = menu.Id;
            Name = menu.Name;
            IsEnabled = isEnabled;
        }
    }
    
    /// <summary>
    /// 菜单可见性变更事件
    /// </summary>
    public class MenuVisibilityChangedEvent : DomainEvent
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; }
        
        public MenuVisibilityChangedEvent(Menu menu, bool isVisible)
        {
            MenuId = menu.Id;
            Name = menu.Name;
            IsVisible = isVisible;
        }
    }
    
    /// <summary>
    /// 菜单层级变更事件
    /// </summary>
    public class MenuHierarchyChangedEvent : DomainEvent
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; }
        
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 旧父菜单ID
        /// </summary>
        public Guid? OldParentId { get; }
        
        /// <summary>
        /// 新父菜单ID
        /// </summary>
        public Guid? NewParentId { get; }
        
        public MenuHierarchyChangedEvent(Menu menu, Guid? oldParentId, Guid? newParentId)
        {
            MenuId = menu.Id;
            Name = menu.Name;
            OldParentId = oldParentId;
            NewParentId = newParentId;
        }
    }
    
    /// <summary>
    /// 菜单权限分配事件
    /// </summary>
    public class MenuPermissionAssignedEvent : DomainEvent
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; }
        
        /// <summary>
        /// 权限ID列表
        /// </summary>
        public IEnumerable<Guid> PermissionIds { get; }
        
        public MenuPermissionAssignedEvent(Guid menuId, IEnumerable<Guid> permissionIds)
        {
            MenuId = menuId;
            PermissionIds = permissionIds;
        }
    }
    
    /// <summary>
    /// 菜单权限移除事件
    /// </summary>
    public class MenuPermissionRemovedEvent : DomainEvent
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; }
        
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid PermissionId { get; }
        
        public MenuPermissionRemovedEvent(Guid menuId, Guid permissionId)
        {
            MenuId = menuId;
            PermissionId = permissionId;
        }
    }
}