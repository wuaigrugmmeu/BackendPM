using System;
using System.Collections.Generic;

namespace BackendPM.Application.DTOs
{
    /// <summary>
    /// 用户DTO
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        
        /// <summary>
        /// 头像URL
        /// </summary>
        public string Avatar { get; set; }
        
        /// <summary>
        /// 性别：0=未指定，1=男，2=女
        /// </summary>
        public int Gender { get; set; }
        
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        
        /// <summary>
        /// 状态：0=正常，1=未激活，2=锁定
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// 所属角色列表
        /// </summary>
        public List<RoleDto> Roles { get; set; }
        
        /// <summary>
        /// 所属部门列表
        /// </summary>
        public List<DepartmentDto> Departments { get; set; }
        
        /// <summary>
        /// 主部门
        /// </summary>
        public DepartmentDto PrimaryDepartment { get; set; }
    }
    
    /// <summary>
    /// 用户创建请求DTO
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        
        /// <summary>
        /// 性别：0=未指定，1=男，2=女
        /// </summary>
        public int Gender { get; set; }
        
        /// <summary>
        /// 初始角色ID列表
        /// </summary>
        public List<Guid> RoleIds { get; set; }
        
        /// <summary>
        /// 初始部门ID列表
        /// </summary>
        public List<Guid> DepartmentIds { get; set; }
        
        /// <summary>
        /// 主部门ID
        /// </summary>
        public Guid? PrimaryDepartmentId { get; set; }
    }
    
    /// <summary>
    /// 用户更新请求DTO
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        
        /// <summary>
        /// 头像URL
        /// </summary>
        public string Avatar { get; set; }
        
        /// <summary>
        /// 性别：0=未指定，1=男，2=女
        /// </summary>
        public int Gender { get; set; }
        
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
    }
    
    /// <summary>
    /// 用户登录请求DTO
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
    
    /// <summary>
    /// 登录结果DTO
    /// </summary>
    public class LoginResultDto
    {
        /// <summary>
        /// 是否登录成功
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }
        
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserDto User { get; set; }
        
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }
        
        /// <summary>
        /// 令牌类型
        /// </summary>
        public string TokenType { get; set; } = "Bearer";
        
        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public int ExpiresIn { get; set; }
    }
    
    /// <summary>
    /// 修改密码请求DTO
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }
        
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
        
        /// <summary>
        /// 确认新密码
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
    
    /// <summary>
    /// 重置密码请求DTO
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
    }
    
    /// <summary>
    /// 分配角色请求DTO
    /// </summary>
    public class AssignRolesDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// 角色ID列表
        /// </summary>
        public List<Guid> RoleIds { get; set; }
    }
    
    /// <summary>
    /// 分配部门请求DTO
    /// </summary>
    public class AssignDepartmentsDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// 部门ID列表
        /// </summary>
        public List<Guid> DepartmentIds { get; set; }
        
        /// <summary>
        /// 主部门ID
        /// </summary>
        public Guid? PrimaryDepartmentId { get; set; }
    }
}