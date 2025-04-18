using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;
using BackendPM.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BackendPM.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// 角色仓储实现
    /// </summary>
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AssignPermissionsToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds)
        {
            var role = await GetByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException($"ID为 {roleId} 的角色不存在");

            // 获取当前角色的所有权限
            var currentPermissions = await _dbContext.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();

            // 计算需要添加的权限
            var permissionIdsToAdd = permissionIds.Except(currentPermissions.Select(rp => rp.PermissionId));

            // 添加新权限
            foreach (var permissionId in permissionIdsToAdd)
            {
                // 检查权限是否存在
                var permission = await _dbContext.Permissions.FindAsync(permissionId);
                if (permission == null)
                    throw new KeyNotFoundException($"ID为 {permissionId} 的权限不存在");

                await _dbContext.RolePermissions.AddAsync(new RolePermission(roleId, permissionId));
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearRolePermissionsAsync(Guid roleId)
        {
            var role = await GetByIdAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException($"ID为 {roleId} 的角色不存在");

            var rolePermissions = await _dbContext.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();

            _dbContext.RolePermissions.RemoveRange(rolePermissions);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            return await _dbContext.Roles
                .AnyAsync(r => r.Code == code);
        }

        public async Task<IEnumerable<Role>> GetChildRolesAsync(Guid parentId)
        {
            return await _dbContext.Roles
                .Where(r => r.ParentId == parentId)
                .ToListAsync();
        }

        public async Task<Role> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            return await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Code == code);
        }

        public async Task<IEnumerable<Role>> GetRoleInheritanceChainAsync(Guid roleId)
        {
            var roles = new List<Role>();
            var currentRole = await GetByIdAsync(roleId);
            
            while (currentRole != null)
            {
                roles.Add(currentRole);
                
                if (currentRole.ParentId.HasValue)
                {
                    currentRole = await GetByIdAsync(currentRole.ParentId.Value);
                }
                else
                {
                    currentRole = null;
                }
            }
            
            return roles;
        }

        public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId)
        {
            var permissions = await _dbContext.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Join(_dbContext.Permissions,
                    rp => rp.PermissionId,
                    p => p.Id,
                    (rp, p) => p)
                .Where(p => p.IsEnabled)
                .ToListAsync();

            return permissions;
        }

        public async Task<IEnumerable<User>> GetRoleUsersAsync(Guid roleId)
        {
            var users = await _dbContext.UserRoles
                .Where(ur => ur.RoleId == roleId)
                .Join(_dbContext.Users,
                    ur => ur.UserId,
                    u => u.Id,
                    (ur, u) => u)
                .ToListAsync();

            return users;
        }

        public async Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
        {
            var rolePermission = await _dbContext.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission != null)
            {
                _dbContext.RolePermissions.Remove(rolePermission);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}