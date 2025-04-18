using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;
using BackendPM.Application.Services;

namespace BackendPM.API.Controllers
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RoleController : BaseController
    {
        private readonly IRoleAppService _roleAppService;
        
        public RoleController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }
        
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="keywords">搜索关键字</param>
        /// <returns>角色列表</returns>
        [HttpGet]
        public async Task<IActionResult> GetRoleList([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string keywords = null)
        {
            var (data, total) = await _roleAppService.GetRoleListAsync(pageIndex, pageSize, keywords);
            return SuccessWithPagination(data, total, pageIndex, pageSize);
        }
        
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns>角色列表</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleAppService.GetAllRolesAsync();
            return Success(roles);
        }
        
        /// <summary>
        /// 获取系统角色
        /// </summary>
        /// <returns>系统角色列表</returns>
        [HttpGet("system")]
        public async Task<IActionResult> GetSystemRoles()
        {
            var roles = await _roleAppService.GetSystemRolesAsync();
            return Success(roles);
        }
        
        /// <summary>
        /// 获取自定义角色
        /// </summary>
        /// <returns>自定义角色列表</returns>
        [HttpGet("custom")]
        public async Task<IActionResult> GetCustomRoles()
        {
            var roles = await _roleAppService.GetCustomRolesAsync();
            return Success(roles);
        }
        
        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>角色详情</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleDetail(Guid id)
        {
            var role = await _roleAppService.GetRoleDetailAsync(id);
            return Success(role);
        }
        
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="createRoleDto">角色信息</param>
        /// <returns>创建的角色</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var role = await _roleAppService.CreateRoleAsync(createRoleDto);
            return Success(role);
        }
        
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="updateRoleDto">更新的角色信息</param>
        /// <returns>更新后的角色</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var role = await _roleAppService.UpdateRoleAsync(id, updateRoleDto);
            return Success(role);
        }
        
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var result = await _roleAppService.DeleteRoleAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 启用角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/enable")]
        public async Task<IActionResult> EnableRole(Guid id)
        {
            var result = await _roleAppService.EnableRoleAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 禁用角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/disable")]
        public async Task<IActionResult> DisableRole(Guid id)
        {
            var result = await _roleAppService.DisableRoleAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="roleAssignPermissionsDto">权限分配信息</param>
        /// <returns>操作结果</returns>
        [HttpPost("assign-permissions")]
        public async Task<IActionResult> AssignPermissions([FromBody] RoleAssignPermissionsDto roleAssignPermissionsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _roleAppService.AssignPermissionsAsync(roleAssignPermissionsDto);
            return Success(result);
        }
        
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>权限列表</returns>
        [HttpGet("{id}/permissions")]
        public async Task<IActionResult> GetRolePermissions(Guid id)
        {
            var permissions = await _roleAppService.GetRolePermissionsAsync(id);
            return Success(permissions);
        }
        
        /// <summary>
        /// 获取角色用户
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>用户列表</returns>
        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetRoleUsers(Guid id)
        {
            var users = await _roleAppService.GetRoleUsersAsync(id);
            return Success(users);
        }
        
        /// <summary>
        /// 获取角色树
        /// </summary>
        /// <param name="rootId">根角色ID，如果为null则返回所有角色</param>
        /// <returns>角色树</returns>
        [HttpGet("tree")]
        public async Task<IActionResult> GetRoleTree([FromQuery] Guid? rootId = null)
        {
            var tree = await _roleAppService.GetRoleTreeAsync(rootId);
            return Success(tree);
        }
        
        /// <summary>
        /// 设置角色父级
        /// </summary>
        /// <param name="setRoleParentDto">角色父级信息</param>
        /// <returns>更新后的角色</returns>
        [HttpPost("set-parent")]
        public async Task<IActionResult> SetRoleParent([FromBody] SetRoleParentDto setRoleParentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var role = await _roleAppService.SetRoleParentAsync(setRoleParentDto);
            return Success(role);
        }
        
        /// <summary>
        /// 检查角色编码是否存在
        /// </summary>
        /// <param name="code">角色编码</param>
        /// <returns>是否存在</returns>
        [HttpGet("check-code")]
        public async Task<IActionResult> CheckRoleCodeExists([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("角色编码不能为空");
            }
            
            var exists = await _roleAppService.CheckRoleCodeExistsAsync(code);
            return Success(exists);
        }
    }
}