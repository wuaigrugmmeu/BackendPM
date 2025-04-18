using System;

namespace BackendPM.Domain.Entities
{
    /// <summary>
    /// 基础实体类，提供所有实体共有的属性
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// 实体唯一标识符
        /// </summary>
        public Guid Id { get; protected set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; protected set; }
        
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastModifiedAt { get; protected set; }
        
        /// <summary>
        /// 是否已删除(软删除标记)
        /// </summary>
        public bool IsDeleted { get; protected set; }
        
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}