namespace BackendPM.Domain.ValueObjects
{
    /// <summary>
    /// 权限类型枚举
    /// </summary>
    public enum PermissionType
    {
        /// <summary>
        /// 菜单权限
        /// </summary>
        Menu = 0,
        
        /// <summary>
        /// 操作权限
        /// </summary>
        Operation = 1,
        
        /// <summary>
        /// API权限
        /// </summary>
        Api = 2,
        
        /// <summary>
        /// 数据权限
        /// </summary>
        Data = 3
    }
}