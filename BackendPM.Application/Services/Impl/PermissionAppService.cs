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
    /// 权限应用服务实现
    /// </summary>
    public class PermissionAppService : IPermissionAppService
    {
        private readonly IPermissionDomainService _permissionDomainService;
        private readonly IUserDomainService _userDomainService;
        
        public PermissionAppService(
            IPermissionDomainService permissionDomainService,
            IUserDomainService userDomainService)
        {
            _permissionDomainService = permissionDomainService;
            _userDomainService = userDomainService;
        }

        public async Task<bool> CheckPermissionCodeExistsAsync(string code)
        {
            return await _permissionDomainService.CheckPermissionCodeExistsAsync(code);
        }

        public async Task<bool> CheckUserApiPermissionAsync(Guid userId, string apiResource, string httpMethod)
        {
            return await _permissionDomainService.CheckUserApiPermissionAsync(userId, apiResource, httpMethod);
        }

        public async Task<bool> CheckUserPermissionAsync(Guid userId, string permissionCode)
        {
            return await _permissionDomainService.CheckUserPermissionAsync(userId, permissionCode);
        }

        public async Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto createPermissionDto)
        {
            var permission = await _permissionDomainService.CreatePermissionAsync(
                createPermissionDto.Name,
                createPermissionDto.Code,
                createPermissionDto.Type,
                createPermissionDto.Description,
                createPermissionDto.GroupId);
                
            return await GetPermissionDetailAsync(permission.Id);
        }

        public async Task<PermissionDto> CreatePermissionGroupAsync(CreatePermissionGroupDto createPermissionGroupDto)
        {
            var group = await _permissionDomainService.CreatePermissionGroupAsync(
                createPermissionGroupDto.Name,
                createPermissionGroupDto.Code,
                createPermissionGroupDto.ParentId,
                createPermissionGroupDto.Description);
                
            return await GetPermissionDetailAsync(group.Id);
        }

        public async Task<bool> DeletePermissionAsync(Guid permissionId)
        {
            return await _permissionDomainService.DeletePermissionAsync(permissionId);
        }

        public async Task<bool> DisablePermissionAsync(Guid permissionId)
        {
            return await _permissionDomainService.DisablePermissionAsync(permissionId);
        }

        public async Task<bool> EnablePermissionAsync(Guid permissionId)
        {
            return await _permissionDomainService.EnablePermissionAsync(permissionId);
        }

        public async Task<IEnumerable<PermissionDto>> GetAllGroupsAsync()
        {
            var groups = await _permissionDomainService.GetAllGroupsAsync();
            return groups.Select(p => EntityMapper.Map<Permission, PermissionDto>(p));
        }

        public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
        {
            var permissions = await _permissionDomainService.GetAllPermissionsAsync();
            return permissions.Select(p => EntityMapper.Map<Permission, PermissionDto>(p));
        }

        public async Task<PermissionDto> GetPermissionDetailAsync(Guid permissionId)
        {
            var permission = await _permissionDomainService.GetByIdAsync(permissionId);
            if (permission == null)
                throw new KeyNotFoundException($"ID为 {permissionId} 的权限不存在");
                
            var permissionDto = EntityMapper.Map<Permission, PermissionDto>(permission);
            
            if (permission.GroupId.HasValue)
            {
                var group = await _permissionDomainService.GetByIdAsync(permission.GroupId.Value);
                if (group != null)
                {
                    permissionDto.GroupName = group.Name;
                }
            }
            
            return permissionDto;
        }

        public async Task<(IEnumerable<PermissionDto> Data, int Total)> GetPermissionListAsync(int pageIndex, int pageSize, string keywords = null)
        {
            var (permissions, total) = await _permissionDomainService.GetPermissionListAsync(pageIndex, pageSize, keywords);
            
            var permissionDtos = new List<PermissionDto>();
            foreach (var permission in permissions)
            {
                var permissionDto = EntityMapper.Map<Permission, PermissionDto>(permission);
                
                if (permission.GroupId.HasValue)
                {
                    var group = await _permissionDomainService.GetByIdAsync(permission.GroupId.Value);
                    if (group != null)
                    {
                        permissionDto.GroupName = group.Name;
                    }
                }
                
                permissionDtos.Add(permissionDto);
            }
            
            return (permissionDtos, total);
        }

        public async Task<IEnumerable<PermissionDto>> GetPermissionsByGroupAsync(Guid groupId)
        {
            var permissions = await _permissionDomainService.GetPermissionsByGroupAsync(groupId);
            return permissions.Select(p => EntityMapper.Map<Permission, PermissionDto>(p));
        }

        public async Task<IEnumerable<PermissionDto>> GetPermissionsByTypeAsync(int type)
        {
            var permissions = await _permissionDomainService.GetPermissionsByTypeAsync(type);
            return permissions.Select(p => EntityMapper.Map<Permission, PermissionDto>(p));
        }

        public async Task<IEnumerable<PermissionTreeDto>> GetPermissionTreeAsync()
        {
            var permissions = await _permissionDomainService.GetPermissionTreeAsync();
            return MapToPermissionTreeDto(permissions);
        }

        public async Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(Guid userId)
        {
            var permissions = await _permissionDomainService.GetUserPermissionsAsync(userId);
            return permissions.Select(p => EntityMapper.Map<Permission, PermissionDto>(p));
        }

        public async Task<PermissionDto> RegisterApiPermissionAsync(RegisterApiPermissionDto registerApiPermissionDto)
        {
            var permission = await _permissionDomainService.RegisterApiPermissionAsync(
                registerApiPermissionDto.Name,
                registerApiPermissionDto.Code,
                registerApiPermissionDto.ApiResource,
                registerApiPermissionDto.HttpMethod,
                registerApiPermissionDto.Description,
                registerApiPermissionDto.GroupId);
                
            return await GetPermissionDetailAsync(permission.Id);
        }

        public async Task<PermissionDto> UpdatePermissionAsync(Guid permissionId, UpdatePermissionDto updatePermissionDto)
        {
            await _permissionDomainService.UpdatePermissionAsync(
                permissionId,
                updatePermissionDto.Name,
                updatePermissionDto.Description,
                updatePermissionDto.GroupId);
                
            return await GetPermissionDetailAsync(permissionId);
        }
        
        private IEnumerable<PermissionTreeDto> MapToPermissionTreeDto(IEnumerable<Permission> permissions)
        {
            if (permissions == null)
                return Enumerable.Empty<PermissionTreeDto>();
                
            return permissions.Select(p => new PermissionTreeDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Type = p.Type,
                Description = p.Description,
                IsEnabled = p.IsEnabled,
                Sort = p.Sort,
                Children = MapToPermissionTreeDto(p.Children)
            });
        }
    }
}