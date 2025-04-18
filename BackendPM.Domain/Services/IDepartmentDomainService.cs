using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Services
{
    /// <summary>
    /// 部门领域服务接口
    /// </summary>
    public interface IDepartmentDomainService
    {
        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="name">部门名称</param>
        /// <param name="code">部门编码</param>
        /// <param name="description">部门描述</param>
        /// <param name="parentId">父部门ID</param>
        /// <returns>创建的部门实体</returns>
        Task<Department> CreateDepartmentAsync(string name, string code, string description, Guid? parentId = null);
        
        /// <summary>
        /// 更新部门信息
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="name">部门名称</param>
        /// <param name="description">部门描述</param>
        /// <returns>更新后的部门实体</returns>
        Task<Department> UpdateDepartmentAsync(Guid departmentId, string name, string description);
        
        /// <summary>
        /// 设置部门父级
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="parentId">父部门ID</param>
        /// <returns>更新后的部门实体</returns>
        Task<Department> SetDepartmentParentAsync(Guid departmentId, Guid? parentId);
        
        /// <summary>
        /// 启用部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>更新后的部门实体</returns>
        Task<Department> EnableDepartmentAsync(Guid departmentId);
        
        /// <summary>
        /// 禁用部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>更新后的部门实体</returns>
        Task<Department> DisableDepartmentAsync(Guid departmentId);
        
        /// <summary>
        /// 设置部门排序
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="sort">排序序号</param>
        /// <returns>更新后的部门实体</returns>
        Task<Department> SetDepartmentSortAsync(Guid departmentId, int sort);
        
        /// <summary>
        /// 设置部门领导
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="leaderId">领导用户ID</param>
        /// <returns>更新后的部门实体</returns>
        Task<Department> SetDepartmentLeaderAsync(Guid departmentId, Guid? leaderId);
        
        /// <summary>
        /// 获取部门的所有用户
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="includeChildDepts">是否包含子部门的用户</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<User>> GetDepartmentUsersAsync(Guid departmentId, bool includeChildDepts = false);
        
        /// <summary>
        /// 获取部门领导
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>领导用户</returns>
        Task<User> GetDepartmentLeaderAsync(Guid departmentId);
        
        /// <summary>
        /// 添加用户到部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="isPrimary">是否为主部门</param>
        /// <returns>操作是否成功</returns>
        Task<bool> AddUserToDepartmentAsync(Guid departmentId, Guid userId, bool isPrimary = false);
        
        /// <summary>
        /// 从部门移除用户
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>操作是否成功</returns>
        Task<bool> RemoveUserFromDepartmentAsync(Guid departmentId, Guid userId);
        
        /// <summary>
        /// 设置用户的主部门
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <returns>操作是否成功</returns>
        Task<bool> SetUserPrimaryDepartmentAsync(Guid userId, Guid departmentId);
        
        /// <summary>
        /// 获取部门树
        /// </summary>
        /// <param name="rootId">根部门ID，为null则获取所有根部门</param>
        /// <returns>部门树</returns>
        Task<IEnumerable<Department>> GetDepartmentTreeAsync(Guid? rootId = null);
        
        /// <summary>
        /// 检查部门是否有子部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>是否有子部门</returns>
        Task<bool> HasChildrenAsync(Guid departmentId);
        
        /// <summary>
        /// 批量更新子部门路径
        /// </summary>
        /// <param name="parentId">父部门ID</param>
        /// <param name="parentPath">父部门路径</param>
        /// <returns>操作是否成功</returns>
        Task<bool> UpdateChildrenPathsAsync(Guid parentId, string parentPath);
    }
}