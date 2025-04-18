using System;

namespace BackendPM.Domain.Entities
{
    /// <summary>
    /// 用户角色关联实体
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; private set; }
        
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; private set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; private set; }
        
        protected UserRole() { } // 为EF Core准备的构造函数
        
        public UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}