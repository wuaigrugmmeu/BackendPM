using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;
using BackendPM.Domain.Repositories;
using BackendPM.Domain.Services;

namespace BackendPM.Infrastructure.Services
{
    /// <summary>
    /// 部门领域服务实现
    /// </summary>
    public class DepartmentDomainService : IDepartmentDomainService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        
        public DepartmentDomainService(
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository)
        {
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> AddUserToDepartmentAsync(Guid departmentId, Guid userId, bool isPrimary = false)
        {
            // 验证部门和用户是否存在
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");
                
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"ID为 {userId} 的用户不存在");
                
            // 将用户添加到部门
            await _userRepository.AssignDepartmentToUserAsync(userId, departmentId, isPrimary);
            
            return true;
        }

        public async Task<Department> CreateDepartmentAsync(string name, string code, string description, Guid? parentId = null)
        {
            // 验证部门编码的唯一性
            if (await _departmentRepository.CodeExistsAsync(code))
                throw new InvalidOperationException($"部门编码 {code} 已存在");
                
            // 验证父部门是否存在
            if (parentId.HasValue)
            {
                var parentDepartment = await _departmentRepository.GetByIdAsync(parentId.Value);
                if (parentDepartment == null)
                    throw new KeyNotFoundException($"ID为 {parentId.Value} 的父部门不存在");
            }
            
            // 创建部门
            var department = new Department(name, code, description, parentId);
            await _departmentRepository.AddAsync(department);
            
            return department;
        }

        public async Task<Department> DisableDepartmentAsync(Guid departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");
                
            department.Disable();
            await _departmentRepository.UpdateAsync(department);
            
            return department;
        }

        public async Task<Department> EnableDepartmentAsync(Guid departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");
                
            department.Enable();
            await _departmentRepository.UpdateAsync(department);
            
            return department;
        }

        public async Task<User> GetDepartmentLeaderAsync(Guid departmentId)
        {
            return await _departmentRepository.GetDepartmentLeaderAsync(departmentId);
        }

        public async Task<IEnumerable<Department>> GetDepartmentTreeAsync(Guid? rootId = null)
        {
            return await _departmentRepository.GetDepartmentTreeAsync(rootId);
        }

        public async Task<IEnumerable<User>> GetDepartmentUsersAsync(Guid departmentId, bool includeChildDepts = false)
        {
            return await _departmentRepository.GetDepartmentUsersAsync(departmentId, includeChildDepts);
        }

        public async Task<bool> HasChildrenAsync(Guid departmentId)
        {
            var children = await _departmentRepository.GetChildrenAsync(departmentId);
            return children.Any();
        }

        public async Task<bool> RemoveUserFromDepartmentAsync(Guid departmentId, Guid userId)
        {
            // 验证部门和用户是否存在
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");
                
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"ID为 {userId} 的用户不存在");
                
            // 将用户从部门中移除
            await _userRepository.RemoveDepartmentFromUserAsync(userId, departmentId);
            
            return true;
        }

        public async Task<Department> SetDepartmentLeaderAsync(Guid departmentId, Guid? leaderId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");
                
            // 验证领导者是否存在
            if (leaderId.HasValue)
            {
                var leader = await _userRepository.GetByIdAsync(leaderId.Value);
                if (leader == null)
                    throw new KeyNotFoundException($"ID为 {leaderId.Value} 的用户不存在");
                    
                // 确保该用户在此部门中
                var userDepartments = await _userRepository.GetUserDepartmentsAsync(leaderId.Value);
                bool isInDepartment = false;
                foreach (var userDept in userDepartments)
                {
                    if (userDept.Id == departmentId)
                    {
                        isInDepartment = true;
                        break;
                    }
                }
                
                if (!isInDepartment)
                {
                    // 自动将用户添加到部门
                    await _userRepository.AssignDepartmentToUserAsync(leaderId.Value, departmentId, false);
                }
            }
            
            department.SetLeader(leaderId);
            await _departmentRepository.UpdateAsync(department);
            
            return department;
        }

        public async Task<Department> SetDepartmentParentAsync(Guid departmentId, Guid? parentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");
                
            // 验证父部门是否存在
            if (parentId.HasValue)
            {
                if (parentId.Value == departmentId)
                    throw new InvalidOperationException("不能将部门自身设置为父部门");
                    
                var parentDepartment = await _departmentRepository.GetByIdAsync(parentId.Value);
                if (parentDepartment == null)
                    throw new KeyNotFoundException($"ID为 {parentId.Value} 的父部门不存在");
                
                // 避免循环引用 - 检查新父部门是否是当前部门的子部门
                var children = await _departmentRepository.GetChildrenAsync(departmentId);
                if (children.Any(c => c.Id == parentId.Value))
                {
                    throw new InvalidOperationException("设置父部门会导致循环引用");
                }
            }
            
            // 更新部门的父部门ID
            department.SetParent(parentId);
            await _departmentRepository.UpdateAsync(department);
            
            // 更新路径
            if (parentId.HasValue)
            {
                var parentDepartment = await _departmentRepository.GetByIdAsync(parentId.Value);
                await _departmentRepository.UpdatePathAsync(departmentId, parentDepartment.Path);
            }
            else
            {
                await _departmentRepository.UpdatePathAsync(departmentId, string.Empty);
            }
            
            // 递归更新子部门的路径
            await _departmentRepository.UpdateChildrenPathsAsync(departmentId, department.Path);
            
            return department;
        }

        public async Task<Department> SetDepartmentSortAsync(Guid departmentId, int sort)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");
                
            department.SetSort(sort);
            await _departmentRepository.UpdateAsync(department);
            
            return department;
        }

        public async Task<bool> SetUserPrimaryDepartmentAsync(Guid userId, Guid departmentId)
        {
            // 验证部门和用户是否存在
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");
                
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"ID为 {userId} 的用户不存在");
                
            // 设置用户的主部门
            await _userRepository.SetUserPrimaryDepartmentAsync(userId, departmentId);
            
            return true;
        }

        public async Task<bool> UpdateChildrenPathsAsync(Guid parentId, string parentPath)
        {
            await _departmentRepository.UpdateChildrenPathsAsync(parentId, parentPath);
            return true;
        }

        public async Task<Department> UpdateDepartmentAsync(Guid departmentId, string name, string description)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");
                
            department.Update(name, description);
            await _departmentRepository.UpdateAsync(department);
            
            return department;
        }
    }
}