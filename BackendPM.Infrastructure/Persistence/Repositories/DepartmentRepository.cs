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
    /// 部门仓储实现
    /// </summary>
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CodeExistsAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return false;

            return await _dbContext.Departments
                .AnyAsync(d => d.Code == code);
        }

        public async Task<Department> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            return await _dbContext.Departments
                .FirstOrDefaultAsync(d => d.Code == code);
        }

        public async Task<IEnumerable<Department>> GetChildrenAsync(Guid parentId)
        {
            return await _dbContext.Departments
                .Where(d => d.ParentId == parentId)
                .OrderBy(d => d.Sort)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartmentTreeAsync(Guid? rootId = null)
        {
            // 获取所有部门
            var allDepartments = await _dbContext.Departments
                .Where(d => d.IsEnabled)
                .OrderBy(d => d.Sort)
                .ToListAsync();

            // 构建树结构
            IEnumerable<Department> rootDepartments;

            if (rootId.HasValue)
            {
                // 以指定的根部门开始
                rootDepartments = allDepartments.Where(d => d.Id == rootId.Value);
            }
            else
            {
                // 获取所有顶级部门
                rootDepartments = allDepartments.Where(d => !d.ParentId.HasValue);
            }

            // 递归构建部门树
            return BuildDepartmentTree(rootDepartments, allDepartments);
        }

        private IEnumerable<Department> BuildDepartmentTree(IEnumerable<Department> parentDepts, List<Department> allDepts)
        {
            foreach (var dept in parentDepts)
            {
                // 查找当前部门的所有子部门
                var children = allDepts.Where(d => d.ParentId == dept.Id).ToList();

                // 递归构建子部门树
                if (children.Any())
                {
                    BuildDepartmentTree(children, allDepts);
                }

                yield return dept;
            }
        }

        public async Task<User> GetDepartmentLeaderAsync(Guid departmentId)
        {
            var department = await GetByIdAsync(departmentId);
            if (department == null || !department.LeaderId.HasValue)
                return null;

            return await _dbContext.Users.FindAsync(department.LeaderId.Value);
        }

        public async Task<IEnumerable<User>> GetDepartmentUsersAsync(Guid departmentId, bool includeChildDepts = false)
        {
            if (!includeChildDepts)
            {
                // 只获取当前部门的用户
                var users = await _dbContext.UserDepartments
                    .Where(ud => ud.DepartmentId == departmentId)
                    .Join(_dbContext.Users,
                        ud => ud.UserId,
                        u => u.Id,
                        (ud, u) => u)
                    .ToListAsync();

                return users;
            }
            else
            {
                // 获取当前部门及其所有子部门的用户
                
                // 首先获取当前部门及其所有子部门的ID
                var department = await GetByIdAsync(departmentId);
                if (department == null)
                    return Enumerable.Empty<User>();

                var allDepartments = await _dbContext.Departments
                    .Where(d => d.IsEnabled)
                    .ToListAsync();

                var departmentIds = new List<Guid> { departmentId };
                CollectChildDepartmentIds(departmentId, allDepartments, departmentIds);

                // 然后获取这些部门的所有用户
                var users = await _dbContext.UserDepartments
                    .Where(ud => departmentIds.Contains(ud.DepartmentId))
                    .Join(_dbContext.Users,
                        ud => ud.UserId,
                        u => u.Id,
                        (ud, u) => u)
                    .Distinct()
                    .ToListAsync();

                return users;
            }
        }

        private void CollectChildDepartmentIds(Guid parentId, List<Department> allDepartments, List<Guid> result)
        {
            var children = allDepartments.Where(d => d.ParentId == parentId);
            foreach (var child in children)
            {
                if (!result.Contains(child.Id))
                {
                    result.Add(child.Id);
                    CollectChildDepartmentIds(child.Id, allDepartments, result);
                }
            }
        }

        public async Task<IEnumerable<Department>> GetRootDepartmentsAsync()
        {
            return await _dbContext.Departments
                .Where(d => !d.ParentId.HasValue)
                .OrderBy(d => d.Sort)
                .ToListAsync();
        }

        public async Task SetDepartmentLeaderAsync(Guid departmentId, Guid userId)
        {
            var department = await GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"ID为 {userId} 的用户不存在");

            department.SetLeader(userId);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateChildrenPathsAsync(Guid parentId, string parentPath)
        {
            // 获取所有子部门
            var children = await _dbContext.Departments
                .Where(d => d.ParentId == parentId)
                .ToListAsync();

            foreach (var child in children)
            {
                // 更新子部门路径
                string newPath = parentPath + "/" + child.Id;
                child.UpdatePath(parentPath);
                
                // 递归更新子部门的子部门路径
                await UpdateChildrenPathsAsync(child.Id, newPath);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePathAsync(Guid departmentId, string newPath)
        {
            var department = await GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");

            department.UpdatePath(newPath);
            await _dbContext.SaveChangesAsync();
        }
    }
}