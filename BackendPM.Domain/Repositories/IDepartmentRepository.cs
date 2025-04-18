using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Repositories
{
    /// <summary>
    /// 部门仓储接口
    /// </summary>
    public interface IDepartmentRepository : IRepository<Department>
    {
        /// <summary>
        /// 根据部门编码获取部门
        /// </summary>
        /// <param name="code">部门编码</param>
        /// <returns>部门实体</returns>
        Task<Department> GetByCodeAsync(string code);
        
        /// <summary>
        /// 检查部门编码是否存在
        /// </summary>
        /// <param name="code">部门编码</param>
        /// <returns>是否存在</returns>
        Task<bool> CodeExistsAsync(string code);
        
        /// <summary>
        /// 获取子部门列表
        /// </summary>
        /// <param name="parentId">父部门ID</param>
        /// <returns>子部门列表</returns>
        Task<IEnumerable<Department>> GetChildrenAsync(Guid parentId);
        
        /// <summary>
        /// 获取所有根部门（没有父部门的部门）
        /// </summary>
        /// <returns>根部门列表</returns>
        Task<IEnumerable<Department>> GetRootDepartmentsAsync();
        
        /// <summary>
        /// 获取部门树（递归构建的层级结构）
        /// </summary>
        /// <param name="rootId">根部门ID，为null则从所有根部门开始</param>
        /// <returns>部门树结构</returns>
        Task<IEnumerable<Department>> GetDepartmentTreeAsync(Guid? rootId = null);
        
        /// <summary>
        /// 获取部门的所有用户
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="includeChildDepts">是否包含子部门的用户</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<User>> GetDepartmentUsersAsync(Guid departmentId, bool includeChildDepts = false);
        
        /// <summary>
        /// 更新部门路径
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="newPath">新路径</param>
        Task UpdatePathAsync(Guid departmentId, string newPath);
        
        /// <summary>
        /// 批量更新子部门路径（当父部门路径变更时）
        /// </summary>
        /// <param name="parentId">父部门ID</param>
        /// <param name="parentPath">父部门新路径</param>
        Task UpdateChildrenPathsAsync(Guid parentId, string parentPath);
        
        /// <summary>
        /// 获取部门的领导用户
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>领导用户实体</returns>
        Task<User> GetDepartmentLeaderAsync(Guid departmentId);
        
        /// <summary>
        /// 设置部门领导
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="userId">用户ID</param>
        Task SetDepartmentLeaderAsync(Guid departmentId, Guid userId);
    }
}