using System;

namespace BackendPM.Domain.Entities
{
    /// <summary>
    /// 角色权限关联实体
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; private set; }
        
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid PermissionId { get; private set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; private set; }
        
        protected RolePermission() { } // 为EF Core准备的构造函数
        
        public RolePermission(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}