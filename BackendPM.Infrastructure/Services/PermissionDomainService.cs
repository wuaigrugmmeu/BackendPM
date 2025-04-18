using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;
using BackendPM.Domain.Events;
using BackendPM.Domain.Repositories;
using BackendPM.Domain.Services;
using BackendPM.Domain.ValueObjects;

namespace BackendPM.Infrastructure.Services
{
    /// <summary>
    /// 权限领域服务实现
    /// </summary>
    public class PermissionDomainService : IPermissionDomainService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        
        public PermissionDomainService(
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository,
            IUserRepository userRepository)
        {
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException($"ID为 {roleId} 的角色不存在");
                
            await _roleRepository.AssignPermissionsToRoleAsync(roleId, permissionIds);
            
            // 触发角色权限分配事件
            var permissionAssignedEvent = new RolePermissionAssignedEvent(roleId, permissionIds);
            // TODO: 使用领域事件发布器发布事件
        }

        public async Task<bool> CheckUserApiPermissionAsync(Guid userId, string apiResource, string httpMethod)
        {
            // 获取用户所有权限
            var permissions = await _userRepository.GetUserPermissionsAsync(userId);
            
            // 检查是否有对应的API权限
            foreach (var permission in permissions)
            {
                // 对于API类型的权限，检查API路径是否匹配
                if (permission.Type == PermissionType.Api && 
                    permission.ApiResource == apiResource)
                {
                    return true;
                }
            }
            
            return false;
        }

        public async Task<bool> CheckUserPermissionAsync(Guid userId, string permissionCode)
        {
            // 获取用户所有权限
            var permissions = await _userRepository.GetUserPermissionsAsync(userId);
            
            // 检查是否包含指定编码的权限
            foreach (var permission in permissions)
            {
                if (permission.Code.Equals(permissionCode, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            
            return false;
        }

        public async Task<Permission> CreatePermissionAsync(string name, string code, PermissionType type, string description, Guid? groupId = null, string apiResource = null)
        {
            // 验证权限编码的唯一性
            if (await _permissionRepository.CodeExistsAsync(code))
                throw new InvalidOperationException($"权限编码 {code} 已存在");
                
            // 验证权限组是否存在
            if (groupId.HasValue)
            {
                var group = await _permissionRepository.GetByIdAsync(groupId.Value);
                if (group == null)
                    throw new KeyNotFoundException($"ID为 {groupId.Value} 的权限组不存在");
            }
            
            // 创建权限
            var permission = new Permission(name, code, type, description);
            
            if (groupId.HasValue)
                permission.SetGroup(groupId);
                
            if (type == PermissionType.Api && !string.IsNullOrEmpty(apiResource))
                permission.SetApiResource(apiResource);
                
            await _permissionRepository.AddAsync(permission);
            
            // 触发权限创建事件
            var permissionCreatedEvent = new PermissionCreatedEvent(permission);
            // TODO: 使用领域事件发布器发布事件
            
            return permission;
        }

        public async Task<Permission> CreatePermissionGroupAsync(string name, string code, string description)
        {
            // 验证权限编码的唯一性
            if (await _permissionRepository.CodeExistsAsync(code))
                throw new InvalidOperationException($"权限编码 {code} 已存在");
                
            // 创建权限组
            var permissionGroup = new Permission(name, code, PermissionType.Menu, description);
            await _permissionRepository.AddAsync(permissionGroup);
            
            // 触发权限创建事件
            var permissionCreatedEvent = new PermissionCreatedEvent(permissionGroup);
            // TODO: 使用领域事件发布器发布事件
            
            return permissionGroup;
        }

        public string GeneratePermissionCode(string moduleName, string actionName)
        {
            // 生成权限编码: module:action
            return $"{moduleName.ToLower()}:{actionName.ToLower()}";
        }

        public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId)
        {
            return await _roleRepository.GetRolePermissionsAsync(roleId);
        }

        public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            return await _userRepository.GetUserPermissionsAsync(userId);
        }

        public async Task<Permission> RegisterApiPermissionAsync(string apiResource, string httpMethod, string name, string description, Guid? groupId = null)
        {
            // 检查API权限是否已存在
            var existingPermission = await _permissionRepository.GetByApiResourceAsync(apiResource);
            if (existingPermission != null)
                return existingPermission;
                
            // 生成唯一的权限编码
            string code = GeneratePermissionCode("api", apiResource.Replace("/", "_"));
            
            // 确保编码唯一
            int suffix = 1;
            string originalCode = code;
            while (await _permissionRepository.CodeExistsAsync(code))
            {
                code = $"{originalCode}_{suffix}";
                suffix++;
            }
            
            // 创建API权限
            var permission = new Permission(name, code, PermissionType.Api, description);
            permission.SetApiResource(apiResource);
            
            if (groupId.HasValue)
                permission.SetGroup(groupId);
                
            await _permissionRepository.AddAsync(permission);
            
            // 触发API权限注册事件
            var apiPermissionRegisteredEvent = new ApiPermissionRegisteredEvent(permission, httpMethod);
            // TODO: 使用领域事件发布器发布事件
            
            return permission;
        }
    }
}