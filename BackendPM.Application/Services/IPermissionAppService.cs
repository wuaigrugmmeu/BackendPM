using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;

namespace BackendPM.Application.Services
{
    /// <summary>
    /// 权限应用服务接口
    /// </summary>
    public interface IPermissionAppService
    {
        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="createPermissionDto">权限信息</param>
        /// <returns>权限信息</returns>
        Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto createPermissionDto);
        
        /// <summary>
        /// 创建权限组
        /// </summary>
        /// <param name="createPermissionGroupDto">权限组信息</param>
        /// <returns>权限组信息</returns>
        Task<PermissionDto> CreatePermissionGroupAsync(CreatePermissionGroupDto createPermissionGroupDto);
        
        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="permissionId">权限ID</param>
        /// <param name="updatePermissionDto">更新信息</param>
        /// <returns>更新后的权限信息</returns>
        Task<PermissionDto> UpdatePermissionAsync(Guid permissionId, UpdatePermissionDto updatePermissionDto);
        
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permissionId">权限ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DeletePermissionAsync(Guid permissionId);
        
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="keywords">搜索关键字</param>
        /// <returns>权限列表和总数</returns>
        Task<(IEnumerable<PermissionDto> Data, int Total)> GetPermissionListAsync(int pageIndex, int pageSize, string keywords = null);
        
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns>权限列表</returns>
        Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
        
        /// <summary>
        /// 获取权限详情
        /// </summary>
        /// <param name="permissionId">权限ID</param>
        /// <returns>权限详情</returns>
        Task<PermissionDto> GetPermissionDetailAsync(Guid permissionId);
        
        /// <summary>
        /// 启用权限
        /// </summary>
        /// <param name="permissionId">权限ID</param>
        /// <returns>是否成功</returns>
        Task<bool> EnablePermissionAsync(Guid permissionId);
        
        /// <summary>
        /// 禁用权限
        /// </summary>
        /// <param name="permissionId">权限ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DisablePermissionAsync(Guid permissionId);
        
        /// <summary>
        /// 获取权限组下的权限
        /// </summary>
        /// <param name="groupId">权限组ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<PermissionDto>> GetPermissionsByGroupAsync(Guid groupId);
        
        /// <summary>
        /// 获取所有权限组
        /// </summary>
        /// <returns>权限组列表</returns>
        Task<IEnumerable<PermissionDto>> GetAllGroupsAsync();
        
        /// <summary>
        /// 根据类型获取权限
        /// </summary>
        /// <param name="type">权限类型</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<PermissionDto>> GetPermissionsByTypeAsync(int type);
        
        /// <summary>
        /// 检查用户是否有指定权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionCode">权限编码</param>
        /// <returns>是否有权限</returns>
        Task<bool> CheckUserPermissionAsync(Guid userId, string permissionCode);
        
        /// <summary>
        /// 检查用户是否有指定API权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="apiResource">API资源路径</param>
        /// <param name="httpMethod">HTTP方法</param>
        /// <returns>是否有权限</returns>
        Task<bool> CheckUserApiPermissionAsync(Guid userId, string apiResource, string httpMethod);
        
        /// <summary>
        /// 注册API权限
        /// </summary>
        /// <param name="registerApiPermissionDto">API权限信息</param>
        /// <returns>权限信息</returns>
        Task<PermissionDto> RegisterApiPermissionAsync(RegisterApiPermissionDto registerApiPermissionDto);
        
        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(Guid userId);
        
        /// <summary>
        /// 获取权限树形结构
        /// </summary>
        /// <returns>权限树形结构</returns>
        Task<IEnumerable<PermissionTreeDto>> GetPermissionTreeAsync();
        
        /// <summary>
        /// 检查权限编码是否存在
        /// </summary>
        /// <param name="code">权限编码</param>
        /// <returns>是否存在</returns>
        Task<bool> CheckPermissionCodeExistsAsync(string code);
    }
}