using System;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Events
{
    /// <summary>
    /// 用户创建事件
    /// </summary>
    public class UserCreatedEvent : DomainEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; }
        
        public UserCreatedEvent(User user)
        {
            UserId = user.Id;
            Username = user.Username;
            Email = user.Email;
        }
    }
    
    /// <summary>
    /// 用户信息更新事件
    /// </summary>
    public class UserUpdatedEvent : DomainEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; }
        
        public UserUpdatedEvent(User user)
        {
            UserId = user.Id;
            Username = user.Username;
        }
    }
    
    /// <summary>
    /// 用户删除事件
    /// </summary>
    public class UserDeletedEvent : DomainEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; }
        
        public UserDeletedEvent(User user)
        {
            UserId = user.Id;
            Username = user.Username;
        }
    }
    
    /// <summary>
    /// 用户状态变更事件
    /// </summary>
    public class UserStatusChangedEvent : DomainEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; }
        
        /// <summary>
        /// 新状态
        /// </summary>
        public string NewStatus { get; }
        
        /// <summary>
        /// 旧状态
        /// </summary>
        public string OldStatus { get; }
        
        public UserStatusChangedEvent(User user, string oldStatus, string newStatus)
        {
            UserId = user.Id;
            Username = user.Username;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
    
    /// <summary>
    /// 用户登录事件
    /// </summary>
    public class UserLoggedInEvent : DomainEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; }
        
        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; }
        
        /// <summary>
        /// 用户代理
        /// </summary>
        public string UserAgent { get; }
        
        public UserLoggedInEvent(User user, string ipAddress, string userAgent)
        {
            UserId = user.Id;
            Username = user.Username;
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }
    }
    
    /// <summary>
    /// 用户登出事件
    /// </summary>
    public class UserLoggedOutEvent : DomainEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; }
        
        public UserLoggedOutEvent(User user)
        {
            UserId = user.Id;
            Username = user.Username;
        }
    }
    
    /// <summary>
    /// 用户登录失败事件
    /// </summary>
    public class UserLoginFailedEvent : DomainEvent
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; }
        
        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; }
        
        /// <summary>
        /// 用户代理
        /// </summary>
        public string UserAgent { get; }
        
        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailReason { get; }
        
        public UserLoginFailedEvent(string username, string ipAddress, string userAgent, string failReason)
        {
            Username = username;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            FailReason = failReason;
        }
    }
    
    /// <summary>
    /// 用户角色分配事件
    /// </summary>
    public class UserRoleAssignedEvent : DomainEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; }
        
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; }
        
        public UserRoleAssignedEvent(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
    
    /// <summary>
    /// 用户角色移除事件
    /// </summary>
    public class UserRoleRemovedEvent : DomainEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; }
        
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; }
        
        public UserRoleRemovedEvent(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
    
    /// <summary>
    /// 用户密码变更事件
    /// </summary>
    public class UserPasswordChangedEvent : DomainEvent
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; }
        
        public UserPasswordChangedEvent(User user)
        {
            UserId = user.Id;
            Username = user.Username;
        }
    }
}