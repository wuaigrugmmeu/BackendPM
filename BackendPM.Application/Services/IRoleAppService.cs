using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;

namespace BackendPM.Application.Services
{
    /// <summary>
    /// 角色应用服务接口
    /// </summary>
    public interface IRoleAppService
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="createRoleDto">角色信息</param>
        /// <returns>角色信息</returns>
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
        
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="updateRoleDto">更新信息</param>
        /// <returns>更新后的角色信息</returns>
        Task<RoleDto> UpdateRoleAsync(Guid roleId, UpdateRoleDto updateRoleDto);
        
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteRoleAsync(Guid roleId);
        
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="keywords">搜索关键字</param>
        /// <returns>角色列表和总数</returns>
        Task<(IEnumerable<RoleDto> Data, int Total)> GetRoleListAsync(int pageIndex, int pageSize, string keywords = null);
        
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns>角色列表</returns>
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        
        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色详情</returns>
        Task<RoleDto> GetRoleDetailAsync(Guid roleId);
        
        /// <summary>
        /// 启用角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>是否成功</returns>
        Task<bool> EnableRoleAsync(Guid roleId);
        
        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DisableRoleAsync(Guid roleId);
        
        /// <summary>
        /// 设置角色父级
        /// </summary>
        /// <param name="setRoleParentDto">角色父级信息</param>
        /// <returns>更新后的角色信息</returns>
        Task<RoleDto> SetRoleParentAsync(SetRoleParentDto setRoleParentDto);
        
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="roleAssignPermissionsDto">角色权限信息</param>
        /// <returns>是否成功</returns>
        Task<bool> AssignPermissionsAsync(RoleAssignPermissionsDto roleAssignPermissionsDto);
        
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<PermissionDto>> GetRolePermissionsAsync(Guid roleId);
        
        /// <summary>
        /// 获取角色用户
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<UserDto>> GetRoleUsersAsync(Guid roleId);
        
        /// <summary>
        /// 获取系统角色
        /// </summary>
        /// <returns>系统角色列表</returns>
        Task<IEnumerable<RoleDto>> GetSystemRolesAsync();
        
        /// <summary>
        /// 获取自定义角色
        /// </summary>
        /// <returns>自定义角色列表</returns>
        Task<IEnumerable<RoleDto>> GetCustomRolesAsync();
        
        /// <summary>
        /// 检查角色编码是否存在
        /// </summary>
        /// <param name="code">角色编码</param>
        /// <returns>是否存在</returns>
        Task<bool> CheckRoleCodeExistsAsync(string code);
        
        /// <summary>
        /// 获取角色树形结构
        /// </summary>
        /// <param name="rootId">根角色ID，如果为null则返回所有角色</param>
        /// <returns>角色树形结构</returns>
        Task<IEnumerable<RoleTreeDto>> GetRoleTreeAsync(Guid? rootId = null);
    }
}