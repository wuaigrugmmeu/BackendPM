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
    /// 菜单应用服务实现
    /// </summary>
    public class MenuAppService : IMenuAppService
    {
        private readonly IMenuDomainService _menuDomainService;
        private readonly IPermissionDomainService _permissionDomainService;
        private readonly IUserDomainService _userDomainService;
        
        public MenuAppService(
            IMenuDomainService menuDomainService,
            IPermissionDomainService permissionDomainService,
            IUserDomainService userDomainService)
        {
            _menuDomainService = menuDomainService;
            _permissionDomainService = permissionDomainService;
            _userDomainService = userDomainService;
        }

        public async Task<bool> AssignPermissionsToMenuAsync(MenuAssignPermissionsDto menuAssignPermissionsDto)
        {
            // 清除菜单当前的所有权限
            var currentPermissions = await _menuDomainService.GetMenuPermissionsAsync(menuAssignPermissionsDto.MenuId);
            foreach (var permission in currentPermissions)
            {
                await _menuDomainService.RemovePermissionFromMenuAsync(menuAssignPermissionsDto.MenuId, permission.Id);
            }
            
            // 添加新的权限关联
            foreach (var permissionId in menuAssignPermissionsDto.PermissionIds)
            {
                await _menuDomainService.AddPermissionToMenuAsync(menuAssignPermissionsDto.MenuId, permissionId);
            }
            
            return true;
        }

        public async Task<MenuDto> CreateMenuAsync(CreateMenuDto createMenuDto)
        {
            var menu = await _menuDomainService.CreateMenuAsync(
                createMenuDto.Name,
                createMenuDto.Path,
                createMenuDto.Component,
                createMenuDto.Icon,
                createMenuDto.Sort,
                createMenuDto.IsHidden,
                createMenuDto.ParentId);
                
            // 分配权限
            if (createMenuDto.PermissionIds != null && createMenuDto.PermissionIds.Any())
            {
                foreach (var permissionId in createMenuDto.PermissionIds)
                {
                    await _menuDomainService.AddPermissionToMenuAsync(menu.Id, permissionId);
                }
            }
            
            return await GetMenuDetailAsync(menu.Id);
        }

        public async Task<bool> DeleteMenuAsync(Guid menuId)
        {
            return await _menuDomainService.DeleteMenuAsync(menuId);
        }

        public async Task<bool> DisableMenuAsync(Guid menuId)
        {
            return await _menuDomainService.DisableMenuAsync(menuId);
        }

        public async Task<bool> EnableMenuAsync(Guid menuId)
        {
            return await _menuDomainService.EnableMenuAsync(menuId);
        }

        public async Task<IEnumerable<MenuDto>> GetAllMenusAsync()
        {
            var menus = await _menuDomainService.GetAllMenusAsync();
            return menus.Select(m => EntityMapper.Map<Menu, MenuDto>(m));
        }

        public async Task<MenuDto> GetMenuDetailAsync(Guid menuId)
        {
            var menu = await _menuDomainService.GetByIdAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"ID为 {menuId} 的菜单不存在");
                
            var permissions = await _menuDomainService.GetMenuPermissionsAsync(menuId);
            
            var menuDto = EntityMapper.Map<Menu, MenuDto>(menu);
            menuDto.Permissions = permissions.Select(p => EntityMapper.Map<Permission, PermissionDto>(p)).ToList();
            
            if (menu.ParentId.HasValue)
            {
                var parentMenu = await _menuDomainService.GetByIdAsync(menu.ParentId.Value);
                if (parentMenu != null)
                {
                    menuDto.ParentName = parentMenu.Name;
                }
            }
            
            return menuDto;
        }

        public async Task<(IEnumerable<MenuDto> Data, int Total)> GetMenuListAsync(int pageIndex, int pageSize, string keywords = null)
        {
            var (menus, total) = await _menuDomainService.GetMenuListAsync(pageIndex, pageSize, keywords);
            
            var menuDtos = new List<MenuDto>();
            foreach (var menu in menus)
            {
                var menuDto = EntityMapper.Map<Menu, MenuDto>(menu);
                
                if (menu.ParentId.HasValue)
                {
                    var parentMenu = await _menuDomainService.GetByIdAsync(menu.ParentId.Value);
                    if (parentMenu != null)
                    {
                        menuDto.ParentName = parentMenu.Name;
                    }
                }
                
                menuDtos.Add(menuDto);
            }
            
            return (menuDtos, total);
        }

        public async Task<IEnumerable<PermissionDto>> GetMenuPermissionsAsync(Guid menuId)
        {
            var permissions = await _menuDomainService.GetMenuPermissionsAsync(menuId);
            return permissions.Select(p => EntityMapper.Map<Permission, PermissionDto>(p));
        }

        public async Task<IEnumerable<MenuTreeDto>> GetMenuTreeAsync(Guid? rootId = null)
        {
            var menus = await _menuDomainService.GetMenuTreeAsync(rootId);
            return MapToMenuTreeDto(menus);
        }

        public async Task<IEnumerable<MenuTreeDto>> GetUserMenuTreeAsync(Guid userId)
        {
            var userMenus = await _menuDomainService.GetUserMenuTreeAsync(userId);
            return MapToMenuTreeDto(userMenus);
        }

        public async Task<MenuDto> SetMenuParentAsync(SetMenuParentDto setMenuParentDto)
        {
            await _menuDomainService.SetMenuParentAsync(setMenuParentDto.MenuId, setMenuParentDto.ParentId);
            return await GetMenuDetailAsync(setMenuParentDto.MenuId);
        }

        public async Task<MenuDto> UpdateMenuAsync(Guid menuId, UpdateMenuDto updateMenuDto)
        {
            await _menuDomainService.UpdateMenuAsync(
                menuId,
                updateMenuDto.Name,
                updateMenuDto.Path,
                updateMenuDto.Component,
                updateMenuDto.Icon,
                updateMenuDto.Sort,
                updateMenuDto.IsHidden);
                
            return await GetMenuDetailAsync(menuId);
        }
        
        private IEnumerable<MenuTreeDto> MapToMenuTreeDto(IEnumerable<Menu> menus)
        {
            if (menus == null)
                return Enumerable.Empty<MenuTreeDto>();
                
            return menus.Select(m => new MenuTreeDto
            {
                Id = m.Id,
                Name = m.Name,
                Path = m.Path,
                Component = m.Component,
                Icon = m.Icon,
                Sort = m.Sort,
                IsHidden = m.IsHidden,
                IsEnabled = m.IsEnabled,
                Children = MapToMenuTreeDto(m.Children)
            });
        }
    }
}