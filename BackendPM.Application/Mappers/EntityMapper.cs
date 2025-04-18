using System.Collections.Generic;
using System.Linq;
using BackendPM.Application.DTOs;
using BackendPM.Domain.Entities;

namespace BackendPM.Application.Mappers
{
    /// <summary>
    /// 实体映射工具类 - 负责在实体和DTO之间进行转换
    /// </summary>
    public static class EntityMapper
    {
        #region User Mappers
        
        /// <summary>
        /// 将用户实体转换为用户DTO
        /// </summary>
        public static UserDto ToUserDto(this User entity, IEnumerable<Role> roles = null, IEnumerable<Department> departments = null, Department primaryDepartment = null)
        {
            if (entity == null)
                return null;
                
            var userDto = new UserDto
            {
                Id = entity.Id,
                Username = entity.Username,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                RealName = entity.RealName,
                Avatar = entity.Avatar,
                Gender = (int)entity.Gender,
                Birthday = entity.Birthday,
                Status = (int)entity.Status,
                LastLoginTime = entity.LastLoginTime,
                CreatedAt = entity.CreatedAt
            };
            
            if (roles != null)
            {
                userDto.Roles = roles.Select(r => r.ToRoleDto()).ToList();
            }
            
            if (departments != null)
            {
                userDto.Departments = departments.Select(d => d.ToDepartmentDto()).ToList();
            }
            
            if (primaryDepartment != null)
            {
                userDto.PrimaryDepartment = primaryDepartment.ToDepartmentDto();
            }
            
            return userDto;
        }
        
        #endregion
        
        #region Role Mappers
        
        /// <summary>
        /// 将角色实体转换为角色DTO
        /// </summary>
        public static RoleDto ToRoleDto(this Role entity)
        {
            if (entity == null)
                return null;
                
            return new RoleDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                ParentId = entity.ParentId,
                IsSystem = entity.IsSystem,
                IsEnabled = entity.IsEnabled,
                Sort = entity.Sort,
                CreatedAt = entity.CreatedAt
            };
        }
        
        /// <summary>
        /// 将角色实体转换为角色树节点DTO
        /// </summary>
        public static RoleTreeDto ToRoleTreeDto(this Role entity)
        {
            if (entity == null)
                return null;
                
            return new RoleTreeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Sort = entity.Sort,
                IsSystem = entity.IsSystem,
                IsEnabled = entity.IsEnabled
            };
        }
        
        #endregion
        
        #region Permission Mappers
        
        /// <summary>
        /// 将权限实体转换为权限DTO
        /// </summary>
        public static PermissionDto ToPermissionDto(this Permission entity)
        {
            if (entity == null)
                return null;
                
            return new PermissionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Type = (int)entity.Type,
                Description = entity.Description,
                GroupId = entity.GroupId,
                ApiResource = entity.ApiResource,
                IsEnabled = entity.IsEnabled,
                Sort = entity.Sort,
                CreatedAt = entity.CreatedAt
            };
        }
        
        /// <summary>
        /// 将权限实体转换为权限树节点DTO
        /// </summary>
        public static PermissionTreeDto ToPermissionTreeDto(this Permission entity)
        {
            if (entity == null)
                return null;
                
            return new PermissionTreeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Type = (int)entity.Type,
                Sort = entity.Sort,
                IsEnabled = entity.IsEnabled
            };
        }
        
        #endregion
        
        #region Menu Mappers
        
        /// <summary>
        /// 将菜单实体转换为菜单DTO
        /// </summary>
        public static MenuDto ToMenuDto(this Menu entity)
        {
            if (entity == null)
                return null;
                
            return new MenuDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                ParentId = entity.ParentId,
                Path = entity.Path,
                Component = entity.Component,
                Route = entity.Route,
                Icon = entity.Icon,
                Sort = entity.Sort,
                Visible = entity.Visible,
                IsEnabled = entity.IsEnabled,
                Permissions = entity.Permissions,
                Type = entity.Type,
                IsExternal = entity.IsExternal,
                KeepAlive = entity.KeepAlive,
                CreatedAt = entity.CreatedAt
            };
        }
        
        /// <summary>
        /// 将菜单实体转换为菜单树节点DTO
        /// </summary>
        public static MenuTreeDto ToMenuTreeDto(this Menu entity)
        {
            if (entity == null)
                return null;
                
            return new MenuTreeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Route = entity.Route,
                Icon = entity.Icon,
                Sort = entity.Sort,
                Type = entity.Type
            };
        }
        
        /// <summary>
        /// 将菜单实体转换为前端路由菜单DTO
        /// </summary>
        public static RouterMenuDto ToRouterMenuDto(this Menu entity)
        {
            if (entity == null)
                return null;
                
            var routerMenu = new RouterMenuDto
            {
                Name = entity.Code,
                Path = entity.Route,
                Component = entity.Component,
                Hidden = !entity.Visible,
                Meta = new MenuMeta
                {
                    Title = entity.Name,
                    Icon = entity.Icon,
                    KeepAlive = entity.KeepAlive
                }
            };
            
            if (!string.IsNullOrEmpty(entity.Permissions))
            {
                routerMenu.Meta.Permissions = entity.Permissions.Split(',').Select(p => p.Trim()).ToList();
            }
            else
            {
                routerMenu.Meta.Permissions = new List<string>();
            }
            
            return routerMenu;
        }
        
        #endregion
        
        #region Department Mappers
        
        /// <summary>
        /// 将部门实体转换为部门DTO
        /// </summary>
        public static DepartmentDto ToDepartmentDto(this Department entity, User leader = null)
        {
            if (entity == null)
                return null;
                
            var result = new DepartmentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                ParentId = entity.ParentId,
                Path = entity.Path,
                Sort = entity.Sort,
                IsEnabled = entity.IsEnabled,
                LeaderId = entity.LeaderId,
                CreatedAt = entity.CreatedAt
            };
            
            if (leader != null)
            {
                result.LeaderName = leader.RealName;
            }
            
            return result;
        }
        
        /// <summary>
        /// 将部门实体转换为部门树节点DTO
        /// </summary>
        public static DepartmentTreeDto ToDepartmentTreeDto(this Department entity, string leaderName = null)
        {
            if (entity == null)
                return null;
                
            return new DepartmentTreeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Sort = entity.Sort,
                IsEnabled = entity.IsEnabled,
                LeaderId = entity.LeaderId,
                LeaderName = leaderName
            };
        }
        
        #endregion
    }
}