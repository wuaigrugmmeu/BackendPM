using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;

namespace BackendPM.Application.Services
{
    /// <summary>
    /// 部门应用服务接口
    /// </summary>
    public interface IDepartmentAppService
    {
        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="createDepartmentDto">部门信息</param>
        /// <returns>部门信息</returns>
        Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto);
        
        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="updateDepartmentDto">更新信息</param>
        /// <returns>更新后的部门信息</returns>
        Task<DepartmentDto> UpdateDepartmentAsync(Guid departmentId, UpdateDepartmentDto updateDepartmentDto);
        
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteDepartmentAsync(Guid departmentId);
        
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="keywords">搜索关键字</param>
        /// <returns>部门列表和总数</returns>
        Task<(IEnumerable<DepartmentDto> Data, int Total)> GetDepartmentListAsync(int pageIndex, int pageSize, string keywords = null);
        
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns>部门列表</returns>
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
        
        /// <summary>
        /// 获取部门详情
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>部门详情</returns>
        Task<DepartmentDto> GetDepartmentDetailAsync(Guid departmentId);
        
        /// <summary>
        /// 启用部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>是否成功</returns>
        Task<bool> EnableDepartmentAsync(Guid departmentId);
        
        /// <summary>
        /// 禁用部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DisableDepartmentAsync(Guid departmentId);
        
        /// <summary>
        /// 设置部门排序
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="sort">排序值</param>
        /// <returns>是否成功</returns>
        Task<bool> SetDepartmentSortAsync(Guid departmentId, int sort);
        
        /// <summary>
        /// 设置部门父级
        /// </summary>
        /// <param name="setDepartmentParentDto">部门父级信息</param>
        /// <returns>更新后的部门信息</returns>
        Task<DepartmentDto> SetDepartmentParentAsync(SetDepartmentParentDto setDepartmentParentDto);
        
        /// <summary>
        /// 设置部门负责人
        /// </summary>
        /// <param name="setDepartmentLeaderDto">部门负责人信息</param>
        /// <returns>是否成功</returns>
        Task<bool> SetDepartmentLeaderAsync(SetDepartmentLeaderDto setDepartmentLeaderDto);
        
        /// <summary>
        /// 获取部门负责人
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>用户信息</returns>
        Task<UserDto> GetDepartmentLeaderAsync(Guid departmentId);
        
        /// <summary>
        /// 获取部门树形结构
        /// </summary>
        /// <param name="rootId">根部门ID，如果为null则返回所有部门</param>
        /// <returns>部门树形结构</returns>
        Task<IEnumerable<DepartmentTreeDto>> GetDepartmentTreeAsync(Guid? rootId = null);
        
        /// <summary>
        /// 获取部门的用户
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="includeChildDepts">是否包含子部门的用户</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<UserDto>> GetDepartmentUsersAsync(Guid departmentId, bool includeChildDepts = false);
        
        /// <summary>
        /// 添加用户到部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="isPrimary">是否为主部门</param>
        /// <returns>是否成功</returns>
        Task<bool> AddUserToDepartmentAsync(Guid departmentId, Guid userId, bool isPrimary = false);
        
        /// <summary>
        /// 从部门中移除用户
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否成功</returns>
        Task<bool> RemoveUserFromDepartmentAsync(Guid departmentId, Guid userId);
        
        /// <summary>
        /// 设置用户的主部门
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="departmentId">部门ID</param>
        /// <returns>是否成功</returns>
        Task<bool> SetUserPrimaryDepartmentAsync(Guid userId, Guid departmentId);
        
        /// <summary>
        /// 检查部门编码是否存在
        /// </summary>
        /// <param name="code">部门编码</param>
        /// <returns>是否存在</returns>
        Task<bool> CheckDepartmentCodeExistsAsync(string code);
    }
}