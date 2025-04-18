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
    /// 用户应用服务实现
    /// </summary>
    public class UserAppService : IUserAppService
    {
        private readonly IUserDomainService _userDomainService;
        private readonly IRoleDomainService _roleDomainService;
        private readonly IDepartmentDomainService _departmentDomainService;
        
        public UserAppService(
            IUserDomainService userDomainService,
            IRoleDomainService roleDomainService,
            IDepartmentDomainService departmentDomainService)
        {
            _userDomainService = userDomainService;
            _roleDomainService = roleDomainService;
            _departmentDomainService = departmentDomainService;
        }

        public async Task<bool> ActivateUserAsync(Guid userId)
        {
            return await _userDomainService.ActivateUserAsync(userId);
        }

        public async Task<bool> AddRoleToUserAsync(Guid userId, Guid roleId)
        {
            return await _userDomainService.AddRoleToUserAsync(userId, roleId);
        }

        public async Task<bool> AssignDepartmentsAsync(AssignDepartmentsDto assignDepartmentsDto)
        {
            // 清除用户当前的所有部门
            var currentDepts = await _userDomainService.GetUserDepartmentsAsync(assignDepartmentsDto.UserId);
            foreach (var dept in currentDepts)
            {
                await _userDomainService.RemoveDepartmentFromUserAsync(assignDepartmentsDto.UserId, dept.Id);
            }
            
            // 添加新的部门关联
            foreach (var deptId in assignDepartmentsDto.DepartmentIds)
            {
                bool isPrimary = assignDepartmentsDto.PrimaryDepartmentId.HasValue && deptId == assignDepartmentsDto.PrimaryDepartmentId.Value;
                await _userDomainService.AddDepartmentToUserAsync(assignDepartmentsDto.UserId, deptId, isPrimary);
            }
            
            return true;
        }

        public async Task<bool> AssignRolesAsync(AssignRolesDto assignRolesDto)
        {
            // 清除用户当前的所有角色
            var currentRoles = await _userDomainService.GetUserRolesAsync(assignRolesDto.UserId);
            foreach (var role in currentRoles)
            {
                await _userDomainService.RemoveRoleFromUserAsync(assignRolesDto.UserId, role.Id);
            }
            
            // 添加新的角色关联
            foreach (var roleId in assignRolesDto.RoleIds)
            {
                await _userDomainService.AddRoleToUserAsync(assignRolesDto.UserId, roleId);
            }
            
            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto)
        {
            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            {
                throw new InvalidOperationException("新密码与确认密码不一致");
            }

            return await _userDomainService.ChangePasswordAsync(userId, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userDomainService.CheckEmailExistsAsync(email);
        }

        public async Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber)
        {
            return await _userDomainService.CheckPhoneNumberExistsAsync(phoneNumber);
        }

        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            return await _userDomainService.CheckUsernameExistsAsync(username);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // 创建用户
            var user = await _userDomainService.CreateUserAsync(
                createUserDto.Username,
                createUserDto.Password,
                createUserDto.Email,
                createUserDto.PhoneNumber,
                createUserDto.RealName,
                createUserDto.Gender);
                
            // 分配角色
            if (createUserDto.RoleIds != null && createUserDto.RoleIds.Any())
            {
                foreach (var roleId in createUserDto.RoleIds)
                {
                    await _userDomainService.AddRoleToUserAsync(user.Id, roleId);
                }
            }
            
            // 分配部门
            if (createUserDto.DepartmentIds != null && createUserDto.DepartmentIds.Any())
            {
                foreach (var deptId in createUserDto.DepartmentIds)
                {
                    bool isPrimary = createUserDto.PrimaryDepartmentId.HasValue && deptId == createUserDto.PrimaryDepartmentId.Value;
                    await _userDomainService.AddDepartmentToUserAsync(user.Id, deptId, isPrimary);
                }
            }
            
            // 设置主部门
            if (createUserDto.PrimaryDepartmentId.HasValue)
            {
                await _userDomainService.SetPrimaryDepartmentAsync(user.Id, createUserDto.PrimaryDepartmentId.Value);
            }
            
            return await GetUserDetailAsync(user.Id);
        }

        public async Task<bool> DeactivateUserAsync(Guid userId)
        {
            return await _userDomainService.DeactivateUserAsync(userId);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            return await _userDomainService.DeleteUserAsync(userId);
        }

        public async Task<UserDto> GetCurrentUserAsync(Guid userId)
        {
            var user = await _userDomainService.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"ID为 {userId} 的用户不存在");
                
            var roles = await _userDomainService.GetUserRolesAsync(userId);
            var departments = await _userDomainService.GetUserDepartmentsAsync(userId);
            var primaryDept = departments.FirstOrDefault(d => d.IsPrimary);
            
            var userDto = EntityMapper.Map<User, UserDto>(user);
            userDto.Roles = roles.Select(r => EntityMapper.Map<Role, RoleDto>(r)).ToList();
            userDto.Departments = departments.Select(d => EntityMapper.Map<Department, DepartmentDto>(d)).ToList();
            
            if (primaryDept != null)
            {
                userDto.PrimaryDepartment = EntityMapper.Map<Department, DepartmentDto>(primaryDept);
            }
            
            return userDto;
        }

        public async Task<IEnumerable<DepartmentDto>> GetUserDepartmentsAsync(Guid userId)
        {
            var departments = await _userDomainService.GetUserDepartmentsAsync(userId);
            return departments.Select(d => EntityMapper.Map<Department, DepartmentDto>(d));
        }

        public async Task<UserDto> GetUserDetailAsync(Guid userId)
        {
            return await GetCurrentUserAsync(userId);
        }

        public async Task<(IEnumerable<UserDto> Data, int Total)> GetUserListAsync(int pageIndex, int pageSize, string keywords = null)
        {
            var (users, total) = await _userDomainService.GetUserListAsync(pageIndex, pageSize, keywords);
            
            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var userDto = EntityMapper.Map<User, UserDto>(user);
                var roles = await _userDomainService.GetUserRolesAsync(user.Id);
                var departments = await _userDomainService.GetUserDepartmentsAsync(user.Id);
                var primaryDept = departments.FirstOrDefault(d => d.IsPrimary);
                
                userDto.Roles = roles.Select(r => EntityMapper.Map<Role, RoleDto>(r)).ToList();
                userDto.Departments = departments.Select(d => EntityMapper.Map<Department, DepartmentDto>(d)).ToList();
                
                if (primaryDept != null)
                {
                    userDto.PrimaryDepartment = EntityMapper.Map<Department, DepartmentDto>(primaryDept);
                }
                
                userDtos.Add(userDto);
            }
            
            return (userDtos, total);
        }

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(Guid userId)
        {
            var roles = await _userDomainService.GetUserRolesAsync(userId);
            return roles.Select(r => EntityMapper.Map<Role, RoleDto>(r));
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var (success, user, token, expiresIn) = await _userDomainService.AuthenticateAsync(loginDto.Username, loginDto.Password);
                
                if (!success)
                {
                    return new LoginResultDto
                    {
                        Success = false,
                        Error = "用户名或密码错误"
                    };
                }
                
                return new LoginResultDto
                {
                    Success = true,
                    User = await GetCurrentUserAsync(user.Id),
                    AccessToken = token,
                    ExpiresIn = expiresIn
                };
            }
            catch (Exception ex)
            {
                return new LoginResultDto
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }

        public async Task<bool> RemoveDepartmentFromUserAsync(Guid userId, Guid departmentId)
        {
            return await _userDomainService.RemoveDepartmentFromUserAsync(userId, departmentId);
        }

        public async Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId)
        {
            return await _userDomainService.RemoveRoleFromUserAsync(userId, roleId);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            return await _userDomainService.ResetPasswordAsync(resetPasswordDto.UserId, resetPasswordDto.NewPassword);
        }

        public async Task<bool> SetUserPrimaryDepartmentAsync(Guid userId, Guid departmentId)
        {
            return await _userDomainService.SetPrimaryDepartmentAsync(userId, departmentId);
        }

        public async Task<bool> UnlockUserAsync(Guid userId)
        {
            return await _userDomainService.UnlockUserAsync(userId);
        }

        public async Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
        {
            await _userDomainService.UpdateUserAsync(
                userId, 
                updateUserDto.Email, 
                updateUserDto.PhoneNumber, 
                updateUserDto.RealName, 
                updateUserDto.Gender,
                updateUserDto.Avatar,
                updateUserDto.Birthday);
                
            return await GetUserDetailAsync(userId);
        }
    }
}