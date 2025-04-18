using System;

namespace BackendPM.Domain.Entities
{
    /// <summary>
    /// 菜单权限关联实体
    /// </summary>
    public class MenuPermission
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; private set; }
        
        /// <summary>
        /// 权限ID
        /// </summary>
        public Guid PermissionId { get; private set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; private set; }
        
        protected MenuPermission() { } // 为EF Core准备的构造函数
        
        public MenuPermission(Guid menuId, Guid permissionId)
        {
            MenuId = menuId;
            PermissionId = permissionId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}