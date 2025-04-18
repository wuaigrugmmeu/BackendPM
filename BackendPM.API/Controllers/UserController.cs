using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;
using BackendPM.Application.Services;

namespace BackendPM.API.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserAppService _userAppService;
        
        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }
        
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginRequest">登录信息</param>
        /// <returns>登录结果</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _userAppService.LoginAsync(loginRequest);
            return Success(result);
        }
        
                return Success(result);
            }
            else
            {
                return Fail(result.Error);
            }
        }
        
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = CurrentUserId;
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            
            var user = await _userAppService.GetCurrentUserAsync(userId);
            return Success(user);
        }
        
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="keywords">搜索关键字</param>
        /// <returns>用户列表</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserList([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string keywords = null)
        {
            var (data, total) = await _userAppService.GetUserListAsync(pageIndex, pageSize, keywords);
            return SuccessWithPagination(data, total, pageIndex, pageSize);
        }
        
        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>用户详情</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserDetail(Guid id)
        {
            var user = await _userAppService.GetUserDetailAsync(id);
            return Success(user);
        }
        
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="createUserDto">用户信息</param>
        /// <returns>创建的用户</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await _userAppService.CreateUserAsync(createUserDto);
            return Success(user);
        }
        
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="updateUserDto">更新的用户信息</param>
        /// <returns>更新后的用户</returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // 检查是否是管理员或当前用户
            if (CurrentUserId != id && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            
            var user = await _userAppService.UpdateUserAsync(id, updateUserDto);
            return Success(user);
        }
        
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userAppService.DeleteUserAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUser(Guid id)
        {
            var result = await _userAppService.ActivateUserAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateUser(Guid id)
        {
            var result = await _userAppService.DeactivateUserAsync(id);
            return Success(result);
        }
        
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="changePasswordDto">密码信息</param>
        /// <returns>操作结果</returns>
        [HttpPut("{id}/password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // 检查是否是当前用户
            if (CurrentUserId != id)
            {
                return Forbid();
            }
            
            var result = await _userAppService.ChangePasswordAsync(id, changePasswordDto);
            return Success(result);
        }
        
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="resetPasswordDto">重置密码信息</param>
        /// <returns>操作结果</returns>
        [HttpPost("reset-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _userAppService.ResetPasswordAsync(resetPasswordDto);
            return Success(result);
        }
        
        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="assignRolesDto">角色分配信息</param>
        /// <returns>操作结果</returns>
        [HttpPost("assign-roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoles([FromBody] AssignRolesDto assignRolesDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _userAppService.AssignRolesAsync(assignRolesDto);
            return Success(result);
        }
        
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>角色列表</returns>
        [HttpGet("{id}/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRoles(Guid id)
        {
            var roles = await _userAppService.GetUserRolesAsync(id);
            return Success(roles);
        }
        
        /// <summary>
        /// 分配部门
        /// </summary>
        /// <param name="assignDepartmentsDto">部门分配信息</param>
        /// <returns>操作结果</returns>
        [HttpPost("assign-departments")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignDepartments([FromBody] AssignDepartmentsDto assignDepartmentsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _userAppService.AssignDepartmentsAsync(assignDepartmentsDto);
            return Success(result);
        }
        
        /// <summary>
        /// 获取用户部门
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>部门列表</returns>
        [HttpGet("{id}/departments")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserDepartments(Guid id)
        {
            var departments = await _userAppService.GetUserDepartmentsAsync(id);
            return Success(departments);
        }
        
        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>是否存在</returns>
        [HttpGet("check-username")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUsernameExists([FromQuery] string username)
        {
            var exists = await _userAppService.CheckUsernameExistsAsync(username);
            return Success(exists);
        }
        
        /// <summary>
        /// 检查邮箱是否存在
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns>是否存在</returns>
        [HttpGet("check-email")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            var exists = await _userAppService.CheckEmailExistsAsync(email);
            return Success(exists);
        }
        
        /// <summary>
        /// 检查手机号是否存在
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>是否存在</returns>
        [HttpGet("check-phone")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckPhoneNumberExists([FromQuery] string phoneNumber)
        {
            var exists = await _userAppService.CheckPhoneNumberExistsAsync(phoneNumber);
            return Success(exists);
        }
        
        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>操作结果</returns>
        [HttpPatch("{id}/unlock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnlockUser(Guid id)
        {
            var result = await _userAppService.UnlockUserAsync(id);
            return Success(result);
        }
    }
}