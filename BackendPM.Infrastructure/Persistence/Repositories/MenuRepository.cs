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
    /// 菜单仓储实现
    /// </summary>
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        public MenuRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AssignPermissionsToMenuAsync(Guid menuId, IEnumerable<Guid> permissionIds)
        {
            var menu = await GetByIdAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"ID为 {menuId} 的菜单不存在");

            // 获取当前菜单的所有权限
            var currentPermissions = await _dbContext.MenuPermissions
                .Where(mp => mp.MenuId == menuId)
                .ToListAsync();

            // 计算需要添加的权限
            var permissionIdsToAdd = permissionIds.Except(currentPermissions.Select(mp => mp.PermissionId));

            // 添加新权限
            foreach (var permissionId in permissionIdsToAdd)
            {
                // 检查权限是否存在
                var permission = await _dbContext.Permissions.FindAsync(permissionId);
                if (permission == null)
                    throw new KeyNotFoundException($"ID为 {permissionId} 的权限不存在");

                await _dbContext.MenuPermissions.AddAsync(new MenuPermission(menuId, permissionId));
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearMenuPermissionsAsync(Guid menuId)
        {
            var menuPermissions = await _dbContext.MenuPermissions
                .Where(mp => mp.MenuId == menuId)
                .ToListAsync();

            _dbContext.MenuPermissions.RemoveRange(menuPermissions);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            return await _dbContext.Menus
                .AnyAsync(m => m.Code == code);
        }

        public async Task<IEnumerable<Menu>> GetByTypeAsync(int type)
        {
            return await _dbContext.Menus
                .Where(m => m.Type == type)
                .ToListAsync();
        }

        public async Task<Menu> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            return await _dbContext.Menus
                .FirstOrDefaultAsync(m => m.Code == code);
        }

        public async Task<IEnumerable<Menu>> GetChildrenAsync(Guid parentId)
        {
            return await _dbContext.Menus
                .Where(m => m.ParentId == parentId)
                .OrderBy(m => m.Sort)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetMenuPermissionsAsync(Guid menuId)
        {
            var permissions = await _dbContext.MenuPermissions
                .Where(mp => mp.MenuId == menuId)
                .Join(_dbContext.Permissions,
                    mp => mp.PermissionId,
                    p => p.Id,
                    (mp, p) => p)
                .Where(p => p.IsEnabled)
                .ToListAsync();

            return permissions;
        }

        public async Task<IEnumerable<Menu>> GetMenuTreeAsync(Guid? rootId = null)
        {
            // 获取所有菜单
            var allMenus = await _dbContext.Menus
                .Where(m => m.IsEnabled)
                .OrderBy(m => m.Sort)
                .ToListAsync();

            // 构建树结构
            IEnumerable<Menu> rootMenus;

            if (rootId.HasValue)
            {
                // 以指定的根菜单开始
                rootMenus = allMenus.Where(m => m.Id == rootId.Value);
            }
            else
            {
                // 获取所有顶级菜单
                rootMenus = allMenus.Where(m => !m.ParentId.HasValue);
            }

            // 递归构建菜单树
            return BuildMenuTree(rootMenus, allMenus);
        }

        private IEnumerable<Menu> BuildMenuTree(IEnumerable<Menu> parentMenus, List<Menu> allMenus)
        {
            foreach (var menu in parentMenus)
            {
                // 查找当前菜单的所有子菜单
                var children = allMenus.Where(m => m.ParentId == menu.Id).ToList();

                // 递归构建子菜单树
                if (children.Any())
                {
                    // 在这里我们不能直接修改菜单实体，因为EF Core会跟踪这些变化
                    // 所以我们只返回带有正确层次结构的菜单列表
                    BuildMenuTree(children, allMenus);
                }

                yield return menu;
            }
        }

        public async Task<IEnumerable<Menu>> GetRoleMenusAsync(Guid roleId)
        {
            // 获取角色所有权限
            var permissionIds = await _dbContext.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            // 获取这些权限关联的菜单
            var menus = await _dbContext.MenuPermissions
                .Where(mp => permissionIds.Contains(mp.PermissionId))
                .Join(_dbContext.Menus,
                    mp => mp.MenuId,
                    m => m.Id,
                    (mp, m) => m)
                .Where(m => m.IsEnabled)
                .Distinct()
                .OrderBy(m => m.Sort)
                .ToListAsync();

            return menus;
        }

        public async Task<IEnumerable<Menu>> GetRootMenusAsync()
        {
            return await _dbContext.Menus
                .Where(m => !m.ParentId.HasValue)
                .OrderBy(m => m.Sort)
                .ToListAsync();
        }

        public async Task<IEnumerable<Menu>> GetUserMenusAsync(Guid userId)
        {
            // 获取用户所有角色
            var roleIds = await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_dbContext.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => r.Id)
                .Where(r => _dbContext.Roles.First(role => role.Id == r).IsEnabled)
                .ToListAsync();

            // 获取这些角色所有权限
            var permissionIds = await _dbContext.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId))
                .Select(rp => rp.PermissionId)
                .Distinct()
                .ToListAsync();

            // 获取这些权限关联的菜单
            var menus = await _dbContext.MenuPermissions
                .Where(mp => permissionIds.Contains(mp.PermissionId))
                .Join(_dbContext.Menus,
                    mp => mp.MenuId,
                    m => m.Id,
                    (mp, m) => m)
                .Where(m => m.IsEnabled && m.Visible)
                .Distinct()
                .OrderBy(m => m.Sort)
                .ToListAsync();

            return menus;
        }

        public async Task RemovePermissionFromMenuAsync(Guid menuId, Guid permissionId)
        {
            var menuPermission = await _dbContext.MenuPermissions
                .FirstOrDefaultAsync(mp => mp.MenuId == menuId && mp.PermissionId == permissionId);

            if (menuPermission != null)
            {
                _dbContext.MenuPermissions.Remove(menuPermission);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateChildrenPathsAsync(Guid parentId, string parentPath)
        {
            // 获取所有子菜单
            var children = await _dbContext.Menus
                .Where(m => m.ParentId == parentId)
                .ToListAsync();

            foreach (var child in children)
            {
                // 更新子菜单路径
                string newPath = parentPath + "/" + child.Id;
                child.UpdatePath(parentPath);
                
                // 递归更新子菜单的子菜单路径
                await UpdateChildrenPathsAsync(child.Id, newPath);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePathAsync(Guid menuId, string newPath)
        {
            var menu = await GetByIdAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"ID为 {menuId} 的菜单不存在");

            menu.UpdatePath(newPath);
            await _dbContext.SaveChangesAsync();
        }
    }
}