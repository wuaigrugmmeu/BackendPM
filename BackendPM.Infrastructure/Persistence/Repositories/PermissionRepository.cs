using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;
using BackendPM.Domain.Repositories;
using BackendPM.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BackendPM.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// 权限仓储实现
    /// </summary>
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            return await _dbContext.Permissions
                .AnyAsync(p => p.Code == code);
        }

        public async Task<IEnumerable<Permission>> GetAllGroupsAsync()
        {
            // 权限组应该是没有GroupId的权限
            var groups = await _dbContext.Permissions
                .Where(p => !p.GroupId.HasValue)
                .ToListAsync();

            return groups;
        }

        public async Task<Permission> GetByApiResourceAsync(string apiResource)
        {
            if (string.IsNullOrWhiteSpace(apiResource))
                return null;

            return await _dbContext.Permissions
                .FirstOrDefaultAsync(p => p.ApiResource == apiResource && p.Type == PermissionType.Api);
        }

        public async Task<Permission> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            return await _dbContext.Permissions
                .FirstOrDefaultAsync(p => p.Code == code);
        }

        public async Task<IEnumerable<Permission>> GetByGroupAsync(Guid groupId)
        {
            return await _dbContext.Permissions
                .Where(p => p.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetByTypeAsync(PermissionType type)
        {
            return await _dbContext.Permissions
                .Where(p => p.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Menu>> GetPermissionMenusAsync(Guid permissionId)
        {
            var menus = await _dbContext.MenuPermissions
                .Where(mp => mp.PermissionId == permissionId)
                .Join(_dbContext.Menus,
                    mp => mp.MenuId,
                    m => m.Id,
                    (mp, m) => m)
                .Where(m => m.IsEnabled)
                .ToListAsync();

            return menus;
        }

        public async Task<IEnumerable<Role>> GetPermissionRolesAsync(Guid permissionId)
        {
            var roles = await _dbContext.RolePermissions
                .Where(rp => rp.PermissionId == permissionId)
                .Join(_dbContext.Roles,
                    rp => rp.RoleId,
                    r => r.Id,
                    (rp, r) => r)
                .Where(r => r.IsEnabled)
                .ToListAsync();

            return roles;
        }
    }
}