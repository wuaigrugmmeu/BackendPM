using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;
using BackendPM.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BackendPM.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// 用户仓储实现
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AssignDepartmentToUserAsync(Guid userId, Guid departmentId, bool isPrimary)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"ID为 {userId} 的用户不存在");

            var department = await _dbContext.Departments.FindAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");

            // 检查关联是否已存在
            var existingAssociation = await _dbContext.UserDepartments
                .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DepartmentId == departmentId);

            if (existingAssociation != null)
            {
                // 如果关联已存在且设置为主部门，需要将其他部门设为非主部门
                if (isPrimary && !existingAssociation.IsPrimary)
                {
                    // 先将所有部门设置为非主部门
                    var userDepartments = await _dbContext.UserDepartments
                        .Where(ud => ud.UserId == userId)
                        .ToListAsync();

                    foreach (var ud in userDepartments)
                    {
                        ud.SetPrimary(false);
                    }

                    // 设置当前部门为主部门
                    existingAssociation.SetPrimary(true);
                }
            }
            else
            {
                // 如果设置为主部门，需要将其他部门设为非主部门
                if (isPrimary)
                {
                    var userDepartments = await _dbContext.UserDepartments
                        .Where(ud => ud.UserId == userId)
                        .ToListAsync();

                    foreach (var ud in userDepartments)
                    {
                        ud.SetPrimary(false);
                    }
                }

                // 添加新的关联
                var userDepartment = new UserDepartment(userId, departmentId, isPrimary);
                await _dbContext.UserDepartments.AddAsync(userDepartment);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task AssignRolesToUserAsync(Guid userId, IEnumerable<Guid> roleIds)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"ID为 {userId} 的用户不存在");

            // 获取当前用户的所有角色
            var currentRoles = await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            // 计算需要添加的角色
            var rolesToAdd = roleIds.Except(currentRoles.Select(ur => ur.RoleId));

            // 添加新角色
            foreach (var roleId in rolesToAdd)
            {
                // 检查角色是否存在
                var role = await _dbContext.Roles.FindAsync(roleId);
                if (role == null)
                    throw new KeyNotFoundException($"ID为 {roleId} 的角色不存在");

                await _dbContext.UserRoles.AddAsync(new UserRole(userId, roleId));
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearUserDepartmentsAsync(Guid userId)
        {
            var userDepartments = await _dbContext.UserDepartments
                .Where(ud => ud.UserId == userId)
                .ToListAsync();

            _dbContext.UserDepartments.RemoveRange(userDepartments);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearUserRolesAsync(Guid userId)
        {
            var userRoles = await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            _dbContext.UserRoles.RemoveRange(userRoles);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return await _dbContext.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByPhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return null;

            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<Department>> GetUserDepartmentsAsync(Guid userId)
        {
            var departments = await _dbContext.UserDepartments
                .Where(ud => ud.UserId == userId)
                .Join(_dbContext.Departments,
                    ud => ud.DepartmentId,
                    d => d.Id,
                    (ud, d) => d)
                .ToListAsync();

            return departments;
        }

        public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            // 获取用户直接分配的角色的权限
            var permissions = await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_dbContext.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.PermissionId)
                .Distinct()
                .Join(_dbContext.Permissions,
                    permissionId => permissionId,
                    permission => permission.Id,
                    (permissionId, permission) => permission)
                .Where(p => p.IsEnabled) // 只返回已启用的权限
                .ToListAsync();

            return permissions;
        }

        public async Task<Department> GetUserPrimaryDepartmentAsync(Guid userId)
        {
            var primaryDepartment = await _dbContext.UserDepartments
                .Where(ud => ud.UserId == userId && ud.IsPrimary)
                .Join(_dbContext.Departments,
                    ud => ud.DepartmentId,
                    d => d.Id,
                    (ud, d) => d)
                .FirstOrDefaultAsync();

            return primaryDepartment;
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId)
        {
            var roles = await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_dbContext.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => r)
                .Where(r => r.IsEnabled) // 只返回已启用的角色
                .ToListAsync();

            return roles;
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            return await _dbContext.Users
                .AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task RemoveDepartmentFromUserAsync(Guid userId, Guid departmentId)
        {
            var userDepartment = await _dbContext.UserDepartments
                .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DepartmentId == departmentId);

            if (userDepartment != null)
            {
                _dbContext.UserDepartments.Remove(userDepartment);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveRoleFromUserAsync(Guid userId, Guid roleId)
        {
            var userRole = await _dbContext.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole != null)
            {
                _dbContext.UserRoles.Remove(userRole);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SetUserPrimaryDepartmentAsync(Guid userId, Guid departmentId)
        {
            // 首先确保用户和部门存在
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"ID为 {userId} 的用户不存在");

            var department = await _dbContext.Departments.FindAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");

            // 确保用户在该部门中
            var userDepartment = await _dbContext.UserDepartments
                .FirstOrDefaultAsync(ud => ud.UserId == userId && ud.DepartmentId == departmentId);

            if (userDepartment == null)
                throw new InvalidOperationException($"用户不属于ID为 {departmentId} 的部门");

            // 将所有部门设置为非主部门
            var userDepartments = await _dbContext.UserDepartments
                .Where(ud => ud.UserId == userId)
                .ToListAsync();

            foreach (var ud in userDepartments)
            {
                ud.SetPrimary(ud.DepartmentId == departmentId);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            return await _dbContext.Users
                .AnyAsync(u => u.Username == username);
        }
    }
}