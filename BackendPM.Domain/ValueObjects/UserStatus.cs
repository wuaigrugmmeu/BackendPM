namespace BackendPM.Domain.ValueObjects
{
    /// <summary>
    /// 用户状态枚举
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 激活状态
        /// </summary>
        Active = 0,
        
        /// <summary>
        /// 未激活状态
        /// </summary>
        Inactive = 1,
        
        /// <summary>
        /// 锁定状态
        /// </summary>
        Locked = 2
    }
}