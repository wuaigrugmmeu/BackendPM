using System;

namespace BackendPM.Domain.Entities
{
    /// <summary>
    /// 用户部门关联实体
    /// </summary>
    public class UserDepartment
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; private set; }
        
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DepartmentId { get; private set; }
        
        /// <summary>
        /// 是否为主部门
        /// </summary>
        public bool IsPrimary { get; private set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; private set; }
        
        protected UserDepartment() { } // 为EF Core准备的构造函数
        
        public UserDepartment(Guid userId, Guid departmentId, bool isPrimary = false)
        {
            UserId = userId;
            DepartmentId = departmentId;
            IsPrimary = isPrimary;
            CreatedAt = DateTime.UtcNow;
        }
        
        public void SetPrimary(bool isPrimary)
        {
            IsPrimary = isPrimary;
        }
    }
}