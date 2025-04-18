using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;

namespace BackendPM.Application.Services
{
    /// <summary>
    /// 用户应用服务接口
    /// </summary>
    public interface IUserAppService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginDto">登录信息</param>
        /// <returns>登录结果</returns>
        Task<LoginResultDto> LoginAsync(LoginDto loginDto);
        
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户信息</returns>
        Task<UserDto> GetCurrentUserAsync(Guid userId);
        
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="keywords">搜索关键字</param>
        /// <returns>用户列表和总数</returns>
        Task<(IEnumerable<UserDto> Data, int Total)> GetUserListAsync(int pageIndex, int pageSize, string keywords = null);
        
        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户详情</returns>
        Task<UserDto> GetUserDetailAsync(Guid userId);
        
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="createUserDto">用户信息</param>
        /// <returns>用户信息</returns>
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        
        /// <summary>
        /// 更新用户基本信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="updateUserDto">更新信息</param>
        /// <returns>更新后的用户信息</returns>
        Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto);
        
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteUserAsync(Guid userId);
        
        /// <summary>
        /// 激活用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否成功</returns>
        Task<bool> ActivateUserAsync(Guid userId);
        
        /// <summary>
        /// 停用用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DeactivateUserAsync(Guid userId);
        
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="changePasswordDto">修改密码信息</param>
        /// <returns>是否成功</returns>
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
        
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="resetPasswordDto">重置密码信息</param>
        /// <returns>是否成功</returns>
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        
        /// <summary>
        /// 分配角色
        /// </summary>
        /// <param name="assignRolesDto">分配角色信息</param>
        /// <returns>是否成功</returns>
        Task<bool> AssignRolesAsync(AssignRolesDto assignRolesDto);
        
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>角色列表</returns>
        Task<IEnumerable<RoleDto>> GetUserRolesAsync(Guid userId);
        
        /// <summary>
        /// 分配部门
        /// </summary>
        /// <param name="assignDepartmentsDto">分配部门信息</param>
        /// <returns>是否成功</returns>
        Task<bool> AssignDepartmentsAsync(AssignDepartmentsDto assignDepartmentsDto);
        
        /// <summary>
        /// 获取用户部门
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>部门列表</returns>
        Task<IEnumerable<DepartmentDto>> GetUserDepartmentsAsync(Guid userId);
        
        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>是否存在</returns>
        Task<bool> CheckUsernameExistsAsync(string username);
        
        /// <summary>
        /// 检查邮箱是否存在
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns>是否存在</returns>
        Task<bool> CheckEmailExistsAsync(string email);
        
        /// <summary>
        /// 检查手机号是否存在
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>是否存在</returns>
        Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber);
        
        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>是否成功</returns>
        Task<bool> UnlockUserAsync(Guid userId);
    }
}