using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;
using BackendPM.Domain.Events;
using BackendPM.Domain.Repositories;
using BackendPM.Domain.Services;

namespace BackendPM.Infrastructure.Services
{
    /// <summary>
    /// 角色领域服务实现
    /// </summary>
    public class RoleDomainService : IRoleDomainService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;
        
        public RoleDomainService(
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return false;
                
            await _roleRepository.AssignPermissionsToRoleAsync(roleId, permissionIds);
            
            // 触发角色权限分配事件
            var permissionAssignedEvent = new RolePermissionAssignedEvent(roleId, permissionIds);
            // TODO: 使用领域事件发布器发布事件
            
            return true;
        }

        public async Task<bool> ClearRolePermissionsAsync(Guid roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return false;
                
            await _roleRepository.ClearRolePermissionsAsync(roleId);
            
            // 触发角色权限清空事件
            var permissionsClearedEvent = new RolePermissionsClearedEvent(roleId);
            // TODO: 使用领域事件发布器发布事件
            
            return true;
        }

        public async Task<Role> CreateRoleAsync(string name, string code, string description, bool isSystem = false, Guid? parentId = null)
        {
            // 验证角色编码的唯一性
            if (await _roleRepository.CodeExistsAsync(code))
                throw new InvalidOperationException($"角色编码 {code} 已存在");
                
            // 验证父角色是否存在
            if (parentId.HasValue)
            {
                var parentRole = await _roleRepository.GetByIdAsync(parentId.Value);
                if (parentRole == null)
                    throw new InvalidOperationException($"ID为 {parentId.Value} 的父角色不存在");
            }
            
            // 创建角色
            var role = new Role(name, code, description, isSystem);
            
            if (parentId.HasValue)
                role.SetParent(parentId);
                
            await _roleRepository.AddAsync(role);
            
            // 触发角色创建事件
            var roleCreatedEvent = new RoleCreatedEvent(role);
            // TODO: 使用领域事件发布器发布事件
            
            return role;
        }

        public async Task<Role> DisableRoleAsync(Guid roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException($"ID为 {roleId} 的角色不存在");
                
            role.Disable();
            await _roleRepository.UpdateAsync(role);
            
            // 触发角色状态变更事件
            var statusChangedEvent = new RoleStatusChangedEvent(role, false);
            // TODO: 使用领域事件发布器发布事件
            
            return role;
        }

        public async Task<Role> EnableRoleAsync(Guid roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException($"ID为 {roleId} 的角色不存在");
                
            role.Enable();
            await _roleRepository.UpdateAsync(role);
            
            // 触发角色状态变更事件
            var statusChangedEvent = new RoleStatusChangedEvent(role, true);
            // TODO: 使用领域事件发布器发布事件
            
            return role;
        }

        public async Task<IEnumerable<Role>> GetCustomRolesAsync()
        {
            // 获取所有非系统角色
            var roles = await _roleRepository.FindAsync(r => !r.IsSystem);
            return roles;
        }

        public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId)
        {
            return await _roleRepository.GetRolePermissionsAsync(roleId);
        }

        public async Task<IEnumerable<Role>> GetRoleTreeAsync(Guid? rootId = null)
        {
            if (rootId.HasValue)
            {
                var roleExist = await _roleRepository.ExistsAsync(r => r.Id == rootId.Value);
                if (!roleExist)
                    throw new KeyNotFoundException($"ID为 {rootId.Value} 的角色不存在");
            }
            
            // 获取所有角色
            List<Role> roles = (List<Role>)await _roleRepository.GetAllAsync();
            
            // 构建角色树
            var result = new List<Role>();
            
            if (rootId.HasValue)
            {
                var rootRole = roles.Find(r => r.Id == rootId.Value);
                if (rootRole != null)
                {
                    result.Add(rootRole);
                    await AddChildRolesRecursivelyAsync(result, rootRole.Id, roles);
                }
            }
            else
            {
                // 获取所有顶级角色
                var rootRoles = roles.FindAll(r => !r.ParentId.HasValue);
                result.AddRange(rootRoles);
                
                foreach (var rootRole in rootRoles)
                {
                    await AddChildRolesRecursivelyAsync(result, rootRole.Id, roles);
                }
            }
            
            return result;
        }

        private async Task AddChildRolesRecursivelyAsync(List<Role> result, Guid parentId, List<Role> allRoles)
        {
            var childRoles = allRoles.FindAll(r => r.ParentId.HasValue && r.ParentId.Value == parentId);
            if (childRoles.Count > 0)
            {
                result.AddRange(childRoles);
                foreach (var childRole in childRoles)
                {
                    await AddChildRolesRecursivelyAsync(result, childRole.Id, allRoles);
                }
            }
        }

        public async Task<IEnumerable<User>> GetRoleUsersAsync(Guid roleId)
        {
            return await _roleRepository.GetRoleUsersAsync(roleId);
        }

        public async Task<IEnumerable<Role>> GetSystemRolesAsync()
        {
            // 获取所有系统角色
            var roles = await _roleRepository.FindAsync(r => r.IsSystem);
            return roles;
        }

        public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                return false;
                
            var permission = await _permissionRepository.GetByIdAsync(permissionId);
            if (permission == null)
                return false;
                
            await _roleRepository.RemovePermissionFromRoleAsync(roleId, permissionId);
            
            // 触发角色权限移除事件
            var permissionRemovedEvent = new RolePermissionRemovedEvent(roleId, permissionId);
            // TODO: 使用领域事件发布器发布事件
            
            return true;
        }

        public async Task<Role> SetRoleParentAsync(Guid roleId, Guid? parentId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException($"ID为 {roleId} 的角色不存在");
                
            // 验证父角色是否存在
            if (parentId.HasValue)
            {
                if (parentId.Value == roleId)
                    throw new InvalidOperationException("不能将角色自身设置为父角色");
                    
                var parentRole = await _roleRepository.GetByIdAsync(parentId.Value);
                if (parentRole == null)
                    throw new KeyNotFoundException($"ID为 {parentId.Value} 的父角色不存在");
                    
                // 避免循环引用
                var parentChain = await _roleRepository.GetRoleInheritanceChainAsync(parentId.Value);
                foreach (var ancestor in parentChain)
                {
                    if (ancestor.Id == roleId)
                        throw new InvalidOperationException("设置父角色会导致循环引用");
                }
            }
            
            role.SetParent(parentId);
            await _roleRepository.UpdateAsync(role);
            
            return role;
        }

        public async Task<Role> UpdateRoleAsync(Guid roleId, string name, string description)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException($"ID为 {roleId} 的角色不存在");
                
            role.Update(name, description);
            await _roleRepository.UpdateAsync(role);
            
            // 触发角色更新事件
            var roleUpdatedEvent = new RoleUpdatedEvent(role);
            // TODO: 使用领域事件发布器发布事件
            
            return role;
        }
    }
}