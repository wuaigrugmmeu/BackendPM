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
    /// 菜单领域服务实现
    /// </summary>
    public class MenuDomainService : IMenuDomainService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        
        public MenuDomainService(
            IMenuRepository menuRepository,
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository,
            IUserRepository userRepository)
        {
            _menuRepository = menuRepository;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> AssignPermissionsToMenuAsync(Guid menuId, IEnumerable<Guid> permissionIds)
        {
            var menu = await _menuRepository.GetByIdAsync(menuId);
            if (menu == null)
                return false;
                
            await _menuRepository.AssignPermissionsToMenuAsync(menuId, permissionIds);
            
            // 触发菜单权限分配事件
            var permissionAssignedEvent = new MenuPermissionAssignedEvent(menuId, permissionIds);
            // TODO: 使用领域事件发布器发布事件
            
            return true;
        }

        public async Task<bool> ClearMenuPermissionsAsync(Guid menuId)
        {
            var menu = await _menuRepository.GetByIdAsync(menuId);
            if (menu == null)
                return false;
                
            await _menuRepository.ClearMenuPermissionsAsync(menuId);
            
            return true;
        }

        public async Task<Menu> CreateMenuAsync(string name, string code, int type, string component, string route, Guid? parentId = null, string icon = null, string permissions = null)
        {
            // 验证菜单编码的唯一性
            if (await _menuRepository.CodeExistsAsync(code))
                throw new InvalidOperationException($"菜单编码 {code} 已存在");
                
            // 验证父菜单是否存在
            if (parentId.HasValue)
            {
                var parentMenu = await _menuRepository.GetByIdAsync(parentId.Value);
                if (parentMenu == null)
                    throw new KeyNotFoundException($"ID为 {parentId.Value} 的父菜单不存在");
            }
            
            // 创建菜单
            var menu = new Menu(name, code, type, component, route, parentId);
            
            if (!string.IsNullOrEmpty(icon))
                menu.Update(name, component, route, icon, permissions);
                
            await _menuRepository.AddAsync(menu);
            
            // 触发菜单创建事件
            var menuCreatedEvent = new MenuCreatedEvent(menu);
            // TODO: 使用领域事件发布器发布事件
            
            return menu;
        }

        public async Task<Menu> DisableMenuAsync(Guid menuId)
        {
            var menu = await _menuRepository.GetByIdAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"ID为 {menuId} 的菜单不存在");
                
            menu.Disable();
            await _menuRepository.UpdateAsync(menu);
            
            // 触发菜单状态变更事件
            var statusChangedEvent = new MenuStatusChangedEvent(menu, false);
            // TODO: 使用领域事件发布器发布事件
            
            return menu;
        }

        public async Task<Menu> EnableMenuAsync(Guid menuId)
        {
            var menu = await _menuRepository.GetByIdAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"ID为 {menuId} 的菜单不存在");
                
            menu.Enable();
            await _menuRepository.UpdateAsync(menu);
            
            // 触发菜单状态变更事件
            var statusChangedEvent = new MenuStatusChangedEvent(menu, true);
            // TODO: 使用领域事件发布器发布事件
            
            return menu;
        }

        public async Task<IEnumerable<Menu>> GetFullMenuTreeAsync(Guid? rootId = null)
        {
            return await _menuRepository.GetMenuTreeAsync(rootId);
        }

        public async Task<IEnumerable<Permission>> GetMenuPermissionsAsync(Guid menuId)
        {
            return await _menuRepository.GetMenuPermissionsAsync(menuId);
        }

        public async Task<IEnumerable<Menu>> GetRoleMenuTreeAsync(Guid roleId)
        {
            // 获取角色可访问的菜单
            var menus = await _menuRepository.GetRoleMenusAsync(roleId);
            
            // 将菜单列表转换为树结构
            return BuildMenuTree(menus);
        }

        public async Task<IEnumerable<Menu>> GetUserMenuTreeAsync(Guid userId)
        {
            // 获取用户可访问的菜单
            var menus = await _menuRepository.GetUserMenusAsync(userId);
            
            // 将菜单列表转换为树结构
            return BuildMenuTree(menus);
        }

        private IEnumerable<Menu> BuildMenuTree(IEnumerable<Menu> menus)
        {
            var menuList = menus.ToList();
            var rootMenus = menuList.Where(m => !m.ParentId.HasValue).ToList();
            
            foreach (var rootMenu in rootMenus)
            {
                BuildMenuTreeRecursively(rootMenu, menuList);
            }
            
            return rootMenus;
        }
        
        private void BuildMenuTreeRecursively(Menu parentMenu, List<Menu> allMenus)
        {
            var children = allMenus.Where(m => m.ParentId == parentMenu.Id).ToList();
            
            foreach (var child in children)
            {
                BuildMenuTreeRecursively(child, allMenus);
            }
        }

        public async Task<bool> HasChildrenAsync(Guid menuId)
        {
            var children = await _menuRepository.GetChildrenAsync(menuId);
            return children.Any();
        }

        public async Task<bool> RemovePermissionFromMenuAsync(Guid menuId, Guid permissionId)
        {
            var menu = await _menuRepository.GetByIdAsync(menuId);
            if (menu == null)
                return false;
                
            var permission = await _permissionRepository.GetByIdAsync(permissionId);
            if (permission == null)
                return false;
                
            await _menuRepository.RemovePermissionFromMenuAsync(menuId, permissionId);
            
            // 触发菜单权限移除事件
            var permissionRemovedEvent = new MenuPermissionRemovedEvent(menuId, permissionId);
            // TODO: 使用领域事件发布器发布事件
            
            return true;
        }

        public async Task<Menu> SetMenuParentAsync(Guid menuId, Guid? parentId)
        {
            var menu = await _menuRepository.GetByIdAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"ID为 {menuId} 的菜单不存在");
                
            // 验证父菜单是否存在
            if (parentId.HasValue)
            {
                if (parentId.Value == menuId)
                    throw new InvalidOperationException("不能将菜单自身设置为父菜单");
                    
                var parentMenu = await _menuRepository.GetByIdAsync(parentId.Value);
                if (parentMenu == null)
                    throw new KeyNotFoundException($"ID为 {parentId.Value} 的父菜单不存在");
                
                // 避免循环引用 - 检查新父菜单是否是当前菜单的子菜单
                var children = await _menuRepository.GetChildrenAsync(menuId);
                if (children.Any(c => c.Id == parentId.Value))
                {
                    throw new InvalidOperationException("设置父菜单会导致循环引用");
                }
            }
            
            // 存储旧的父菜单ID用于事件
            var oldParentId = menu.ParentId;
            
            // 更新菜单的父菜单ID
            menu.SetParent(parentId);
            await _menuRepository.UpdateAsync(menu);
            
            // 更新路径
            if (parentId.HasValue)
            {
                var parentMenu = await _menuRepository.GetByIdAsync(parentId.Value);
                await _menuRepository.UpdatePathAsync(menuId, parentMenu.Path);
            }
            else
            {
                await _menuRepository.UpdatePathAsync(menuId, string.Empty);
            }
            
            // 递归更新子菜单的路径
            await _menuRepository.UpdateChildrenPathsAsync(menuId, menu.Path);
            
            // 触发菜单层级变更事件
            var hierarchyChangedEvent = new MenuHierarchyChangedEvent(menu, oldParentId, parentId);
            // TODO: 使用领域事件发布器发布事件
            
            return menu;
        }

        public async Task<Menu> SetMenuSortAsync(Guid menuId, int sort)
        {
            var menu = await _menuRepository.GetByIdAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"ID为 {menuId} 的菜单不存在");
                
            menu.SetSort(sort);
            await _menuRepository.UpdateAsync(menu);
            
            return menu;
        }

        public async Task<Menu> SetMenuVisibilityAsync(Guid menuId, bool visible)
        {
            var menu = await _menuRepository.GetByIdAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"ID为 {menuId} 的菜单不存在");
                
            menu.SetVisibility(visible);
            await _menuRepository.UpdateAsync(menu);
            
            // 触发菜单可见性变更事件
            var visibilityChangedEvent = new MenuVisibilityChangedEvent(menu, visible);
            // TODO: 使用领域事件发布器发布事件
            
            return menu;
        }

        public async Task<Menu> UpdateMenuAsync(Guid menuId, string name, string component, string route, string icon, string permissions)
        {
            var menu = await _menuRepository.GetByIdAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"ID为 {menuId} 的菜单不存在");
                
            menu.Update(name, component, route, icon, permissions);
            await _menuRepository.UpdateAsync(menu);
            
            // 触发菜单更新事件
            var menuUpdatedEvent = new MenuUpdatedEvent(menu);
            // TODO: 使用领域事件发布器发布事件
            
            return menu;
        }
    }
}