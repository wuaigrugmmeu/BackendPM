using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;
using BackendPM.Domain.ValueObjects;

namespace BackendPM.Domain.Services
{
    /// <summary>
    /// 权限领域服务接口
    /// </summary>
    public interface IPermissionDomainService
    {
        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="name">权限名称</param>
        /// <param name="code">权限编码</param>
        /// <param name="type">权限类型</param>
        /// <param name="description">权限描述</param>
        /// <param name="groupId">权限组ID</param>
        /// <param name="apiResource">API资源路径</param>
        /// <returns>创建的权限实体</returns>
        Task<Permission> CreatePermissionAsync(string name, string code, PermissionType type, string description, Guid? groupId = null, string apiResource = null);
        
        /// <summary>
        /// 创建权限组
        /// </summary>
        /// <param name="name">权限组名称</param>
        /// <param name="code">权限组编码</param>
        /// <param name="description">权限组描述</param>
        /// <returns>创建的权限组实体</returns>
        Task<Permission> CreatePermissionGroupAsync(string name, string code, string description);
        
        /// <summary>
        /// 将权限分配给角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permissionIds">权限ID列表</param>
        Task AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds);
        
        /// <summary>
        /// 检查用户是否拥有指定权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionCode">权限编码</param>
        /// <returns>是否拥有权限</returns>
        Task<bool> CheckUserPermissionAsync(Guid userId, string permissionCode);
        
        /// <summary>
        /// 检查用户是否拥有指定API资源的访问权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="apiResource">API资源路径</param>
        /// <param name="httpMethod">HTTP方法</param>
        /// <returns>是否拥有权限</returns>
        Task<bool> CheckUserApiPermissionAsync(Guid userId, string apiResource, string httpMethod);
        
        /// <summary>
        /// 获取用户的所有权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId);
        
        /// <summary>
        /// 获取角色的所有权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId);
        
        /// <summary>
        /// 生成权限编码
        /// </summary>
        /// <param name="moduleName">模块名</param>
        /// <param name="actionName">操作名</param>
        /// <returns>权限编码</returns>
        string GeneratePermissionCode(string moduleName, string actionName);
        
        /// <summary>
        /// 将API接口注册为权限
        /// </summary>
        /// <param name="apiResource">API资源路径</param>
        /// <param name="httpMethod">HTTP方法</param>
        /// <param name="name">权限名称</param>
        /// <param name="description">权限描述</param>
        /// <param name="groupId">权限组ID</param>
        /// <returns>创建的权限实体</returns>
        Task<Permission> RegisterApiPermissionAsync(string apiResource, string httpMethod, string name, string description, Guid? groupId = null);
    }
}