using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;
using BackendPM.Domain.Entities;
using BackendPM.Domain.Services;
using BackendPM.Application.Mappers;

namespace BackendPM.Application.Services.Impl
{
    /// <summary>
    /// 角色应用服务实现
    /// </summary>
    public class RoleAppService : IRoleAppService
    {
        private readonly IRoleDomainService _roleDomainService;
        private readonly IPermissionDomainService _permissionDomainService;
        
        public RoleAppService(
            IRoleDomainService roleDomainService,
            IPermissionDomainService permissionDomainService)
        {
            _roleDomainService = roleDomainService;
            _permissionDomainService = permissionDomainService;
        }

        public async Task<bool> AssignPermissionsAsync(RoleAssignPermissionsDto roleAssignPermissionsDto)
        {
            // 清除角色当前的所有权限
            var currentPermissions = await _roleDomainService.GetRolePermissionsAsync(roleAssignPermissionsDto.RoleId);
            foreach (var permission in currentPermissions)
            {
                await _roleDomainService.RemovePermissionFromRoleAsync(roleAssignPermissionsDto.RoleId, permission.Id);
            }
            
            // 添加新的权限关联
            foreach (var permissionId in roleAssignPermissionsDto.PermissionIds)
            {
                await _roleDomainService.AddPermissionToRoleAsync(roleAssignPermissionsDto.RoleId, permissionId);
            }
            
            return true;
        }

        public async Task<bool> CheckRoleCodeExistsAsync(string code)
        {
            return await _roleDomainService.CheckRoleCodeExistsAsync(code);
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            // 创建角色
            var role = await _roleDomainService.CreateRoleAsync(
                createRoleDto.Name,
                createRoleDto.Code,
                createRoleDto.Description,
                createRoleDto.IsSystemRole,
                createRoleDto.ParentId);
                
            // 分配权限
            if (createRoleDto.PermissionIds != null && createRoleDto.PermissionIds.Any())
            {
                foreach (var permissionId in createRoleDto.PermissionIds)
                {
                    await _roleDomainService.AddPermissionToRoleAsync(role.Id, permissionId);
                }
            }
            
            return await GetRoleDetailAsync(role.Id);
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            return await _roleDomainService.DeleteRoleAsync(roleId);
        }

        public async Task<bool> DisableRoleAsync(Guid roleId)
        {
            return await _roleDomainService.DisableRoleAsync(roleId);
        }

        public async Task<bool> EnableRoleAsync(Guid roleId)
        {
            return await _roleDomainService.EnableRoleAsync(roleId);
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleDomainService.GetAllRolesAsync();
            return roles.Select(r => EntityMapper.Map<Role, RoleDto>(r));
        }

        public async Task<IEnumerable<RoleDto>> GetCustomRolesAsync()
        {
            var roles = await _roleDomainService.GetCustomRolesAsync();
            return roles.Select(r => EntityMapper.Map<Role, RoleDto>(r));
        }

        public async Task<RoleDto> GetRoleDetailAsync(Guid roleId)
        {
            var role = await _roleDomainService.GetByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException($"ID为 {roleId} 的角色不存在");
                
            var permissions = await _roleDomainService.GetRolePermissionsAsync(roleId);
            
            var roleDto = EntityMapper.Map<Role, RoleDto>(role);
            roleDto.Permissions = permissions.Select(p => EntityMapper.Map<Permission, PermissionDto>(p)).ToList();
            
            if (role.ParentId.HasValue)
            {
                var parentRole = await _roleDomainService.GetByIdAsync(role.ParentId.Value);
                if (parentRole != null)
                {
                    roleDto.ParentName = parentRole.Name;
                }
            }
            
            return roleDto;
        }

        public async Task<(IEnumerable<RoleDto> Data, int Total)> GetRoleListAsync(int pageIndex, int pageSize, string keywords = null)
        {
            var (roles, total) = await _roleDomainService.GetRoleListAsync(pageIndex, pageSize, keywords);
            
            var roleDtos = new List<RoleDto>();
            foreach (var role in roles)
            {
                var roleDto = EntityMapper.Map<Role, RoleDto>(role);
                
                if (role.ParentId.HasValue)
                {
                    var parentRole = await _roleDomainService.GetByIdAsync(role.ParentId.Value);
                    if (parentRole != null)
                    {
                        roleDto.ParentName = parentRole.Name;
                    }
                }
                
                roleDtos.Add(roleDto);
            }
            
            return (roleDtos, total);
        }

        public async Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(Guid roleId)
        {
            var permissions = await _roleDomainService.GetRolePermissionsAsync(roleId);
            return permissions.Select(p => EntityMapper.Map<Permission, PermissionDto>(p));
        }

        public async Task<IEnumerable<RoleTreeDto>> GetRoleTreeAsync(Guid? rootId = null)
        {
            var roles = await _roleDomainService.GetRoleTreeAsync(rootId);
            return MapToRoleTreeDto(roles);
        }

        public async Task<IEnumerable<UserDto>> GetRoleUsersAsync(Guid roleId)
        {
            var users = await _roleDomainService.GetRoleUsersAsync(roleId);
            return users.Select(u => EntityMapper.Map<User, UserDto>(u));
        }

        public async Task<IEnumerable<RoleDto>> GetSystemRolesAsync()
        {
            var roles = await _roleDomainService.GetSystemRolesAsync();
            return roles.Select(r => EntityMapper.Map<Role, RoleDto>(r));
        }

        public async Task<RoleDto> SetRoleParentAsync(SetRoleParentDto setRoleParentDto)
        {
            await _roleDomainService.SetRoleParentAsync(setRoleParentDto.RoleId, setRoleParentDto.ParentId);
            return await GetRoleDetailAsync(setRoleParentDto.RoleId);
        }

        public async Task<RoleDto> UpdateRoleAsync(Guid roleId, UpdateRoleDto updateRoleDto)
        {
            await _roleDomainService.UpdateRoleAsync(
                roleId,
                updateRoleDto.Name,
                updateRoleDto.Description);
                
            return await GetRoleDetailAsync(roleId);
        }
        
        private IEnumerable<RoleTreeDto> MapToRoleTreeDto(IEnumerable<Role> roles)
        {
            if (roles == null)
                return Enumerable.Empty<RoleTreeDto>();
                
            return roles.Select(r => new RoleTreeDto
            {
                Id = r.Id,
                Name = r.Name,
                Code = r.Code,
                Description = r.Description,
                IsSystemRole = r.IsSystemRole,
                IsEnabled = r.IsEnabled,
                Sort = r.Sort,
                Children = MapToRoleTreeDto(r.Children)
            });
        }
    }
}