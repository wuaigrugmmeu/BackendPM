using System;

namespace BackendPM.Domain.Events
{
    /// <summary>
    /// 领域事件基类
    /// </summary>
    public abstract class DomainEvent
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public Guid Id { get; }
        
        /// <summary>
        /// 事件发生时间
        /// </summary>
        public DateTime OccurredOn { get; }
        
        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string EventTypeName { get; }
        
        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            EventTypeName = GetType().Name;
        }
    }
}