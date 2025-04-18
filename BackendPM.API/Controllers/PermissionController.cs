using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;
using BackendPM.Application.Services;

namespace BackendPM.API.Controllers
{
    /// <summary>
    /// 权限控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class PermissionController : BaseController
    {
        private readonly IPermissionAppService _permissionAppService;
        
        public PermissionController(IPermissionAppService permissionAppService)
        {
            _permissionAppService = permissionAppService;
        }
        
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="keywords">搜索关键字</param>
        /// <param name="type">权限类型</param>
        /// <returns>权限列表</returns>
        [HttpGet]
        public async Task<IActionResult> GetPermissionList(
            [FromQuery] int pageIndex = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string keywords = null, 
            [FromQuery] string type = null)
        {
            var (data, total) = await _permissionAppService.GetPermissionListAsync(pageIndex, pageSize, keywords, type);
            return SuccessWithPagination(data, total, pageIndex, pageSize);
        }
        
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns>权限列表</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _permissionAppService.GetAllPermissionsAsync();
            return Success(permissions);
        }
        
        /// <summary>
        /// 获取权限详情
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <returns>权限详情</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermissionDetail(Guid id)
        {
            var permission = await _permissionAppService.GetPermissionDetailAsync(id);
            return Success(permission);
        }
        
        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="createPermissionDto">权限信息</param>
        /// <returns>创建的权限</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionDto createPermissionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var permission = await _permissionAppService.CreatePermissionAsync(createPermissionDto);
            return Success(permission);
        }
        
        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <param name="updatePermissionDto">更新的权限信息</param>
        /// <returns>更新后的权限</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(Guid id, [FromBody] UpdatePermissionDto updatePermissionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var permission = await _permissionAppService.UpdatePermissionAsync(id, updatePermissionDto);
            return Success(permission);
        }
        
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(Guid id)
        {
            var result = await _permissionAppService.DeletePermissionAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 启用权限
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/enable")]
        public async Task<IActionResult> EnablePermission(Guid id)
        {
            var result = await _permissionAppService.EnablePermissionAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 禁用权限
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/disable")]
        public async Task<IActionResult> DisablePermission(Guid id)
        {
            var result = await _permissionAppService.DisablePermissionAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <returns>权限树</returns>
        [HttpGet("tree")]
        public async Task<IActionResult> GetPermissionTree()
        {
            var tree = await _permissionAppService.GetPermissionTreeAsync();
            return Success(tree);
        }
        
        /// <summary>
        /// 批量创建权限
        /// </summary>
        /// <param name="batchCreateDto">批量创建权限信息</param>
        /// <returns>创建的权限</returns>
        [HttpPost("batch")]
        public async Task<IActionResult> BatchCreatePermissions([FromBody] BatchCreatePermissionsDto batchCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var permissions = await _permissionAppService.BatchCreatePermissionsAsync(batchCreateDto);
            return Success(permissions);
        }
        
        /// <summary>
        /// 通过关键字搜索权限
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>权限列表</returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchPermissions([FromQuery] string keyword)
        {
            var permissions = await _permissionAppService.SearchPermissionsAsync(keyword);
            return Success(permissions);
        }
        
        /// <summary>
        /// 获取特定类型的权限
        /// </summary>
        /// <param name="type">权限类型</param>
        /// <returns>权限列表</returns>
        [HttpGet("by-type")]
        public async Task<IActionResult> GetPermissionsByType([FromQuery] string type)
        {
            var permissions = await _permissionAppService.GetPermissionsByTypeAsync(type);
            return Success(permissions);
        }
        
        /// <summary>
        /// 检查权限编码是否存在
        /// </summary>
        /// <param name="code">权限编码</param>
        /// <returns>是否存在</returns>
        [HttpGet("check-code")]
        public async Task<IActionResult> CheckPermissionCodeExists([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("权限编码不能为空");
            }
            
            var exists = await _permissionAppService.CheckPermissionCodeExistsAsync(code);
            return Success(exists);
        }
        
        /// <summary>
        /// 获取当前用户拥有的权限
        /// </summary>
        /// <returns>权限列表</returns>
        [HttpGet("current-user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserPermissions()
        {
            var userId = CurrentUserId;
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            var permissions = await _permissionAppService.GetUserPermissionsAsync(userId);
            return Success(permissions);
        }
        
        /// <summary>
        /// 检查当前用户是否拥有某个权限
        /// </summary>
        /// <param name="code">权限编码</param>
        /// <returns>是否拥有权限</returns>
        [HttpGet("check")]
        [Authorize]
        public async Task<IActionResult> CheckCurrentUserHasPermission([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("权限编码不能为空");
            }
            
            var userId = CurrentUserId;
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            var hasPermission = await _permissionAppService.CheckUserHasPermissionAsync(userId, code);
            return Success(hasPermission);
        }
    }
}