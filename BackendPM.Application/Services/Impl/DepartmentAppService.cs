using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendPM.Application.DTOs;
using BackendPM.Application.Mappers;
using BackendPM.Domain.Repositories;
using BackendPM.Domain.Services;

namespace BackendPM.Application.Services.Impl
{
    /// <summary>
    /// 部门应用服务实现
    /// </summary>
    public class DepartmentAppService : IDepartmentAppService
    {
        private readonly IDepartmentDomainService _departmentDomainService;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        
        public DepartmentAppService(
            IDepartmentDomainService departmentDomainService,
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository)
        {
            _departmentDomainService = departmentDomainService;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> AddUsersToDepartmentAsync(AddUsersToDeptDto addUsersToDeptDto)
        {
            return await _departmentDomainService.AddUsersToDepartmentAsync(
                addUsersToDeptDto.DepartmentId,
                addUsersToDeptDto.UserIds);
        }

        public async Task<bool> CheckDepartmentCodeExistsAsync(string code)
        {
            return await _departmentRepository.CodeExistsAsync(code);
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto)
        {
            // 检查编码唯一性
            if (await CheckDepartmentCodeExistsAsync(createDepartmentDto.Code))
                throw new InvalidOperationException($"部门编码 {createDepartmentDto.Code} 已存在");
                
            // 创建部门
            var department = await _departmentDomainService.CreateDepartmentAsync(
                createDepartmentDto.Name,
                createDepartmentDto.Code,
                createDepartmentDto.ParentId);
                
            return EntityMapper.MapToDepartmentDto(department);
        }

        public async Task<bool> DeleteDepartmentAsync(Guid departmentId)
        {
            return await _departmentDomainService.DeleteDepartmentAsync(departmentId);
        }

        public async Task<(IEnumerable<DepartmentDto> Data, int Total)> GetDepartmentListAsync(int pageIndex, int pageSize, string keywords = null)
        {
            var (departments, total) = await _departmentRepository.GetPaginatedListAsync(
                pageIndex: pageIndex,
                pageSize: pageSize,
                predicate: string.IsNullOrEmpty(keywords) ? null : d => 
                    d.Name.Contains(keywords) || 
                    d.Code.Contains(keywords)
            );
            
            // 转换为DTO
            var departmentDtos = departments.Select(d => EntityMapper.MapToDepartmentDto(d)).ToList();
            
            return (departmentDtos, total);
        }

        public async Task<DepartmentDto> GetDepartmentDetailAsync(Guid departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            if (department == null)
                throw new KeyNotFoundException($"ID为 {departmentId} 的部门不存在");
                
            return EntityMapper.MapToDepartmentDto(department);
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return departments.Select(d => EntityMapper.MapToDepartmentDto(d));
        }

        public async Task<IEnumerable<DepartmentTreeDto>> GetDepartmentTreeAsync(Guid? rootId = null)
        {
            // 获取所有部门
            var allDepartments = await _departmentRepository.GetAllAsync();
            
            // 筛选出根部门
            var rootDepartments = rootId.HasValue 
                ? allDepartments.Where(d => d.Id == rootId.Value).ToList() 
                : allDepartments.Where(d => d.ParentId == null).ToList();
                
            // 构建部门树
            var result = BuildDepartmentTree(rootDepartments, allDepartments);
            
            return result;
        }

        private IEnumerable<DepartmentTreeDto> BuildDepartmentTree(IEnumerable<Domain.Entities.Department> rootDepartments, IEnumerable<Domain.Entities.Department> allDepartments)
        {
            var result = new List<DepartmentTreeDto>();
            
            foreach (var department in rootDepartments)
            {
                var departmentTreeDto = new DepartmentTreeDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Code = department.Code
                };
                
                var children = allDepartments.Where(d => d.ParentId == department.Id).ToList();
                if (children.Any())
                {
                    departmentTreeDto.Children.AddRange(BuildDepartmentTree(children, allDepartments));
                }
                
                result.Add(departmentTreeDto);
            }
            
            return result;
        }

        public async Task<IEnumerable<UserDto>> GetDepartmentUsersAsync(Guid departmentId)
        {
            var users = await _departmentRepository.GetDepartmentUsersAsync(departmentId);
            return users.Select(u => EntityMapper.MapToUserDto(u));
        }

        public async Task<bool> RemoveUsersFromDepartmentAsync(RemoveUsersFromDeptDto removeUsersFromDeptDto)
        {
            return await _departmentDomainService.RemoveUsersFromDepartmentAsync(
                removeUsersFromDeptDto.DepartmentId,
                removeUsersFromDeptDto.UserIds);
        }

        public async Task<DepartmentDto> SetDepartmentParentAsync(SetDepartmentParentDto setDepartmentParentDto)
        {
            var department = await _departmentDomainService.SetDepartmentParentAsync(
                setDepartmentParentDto.DepartmentId, 
                setDepartmentParentDto.ParentId);
                
            return EntityMapper.MapToDepartmentDto(department);
        }

        public async Task<DepartmentDto> UpdateDepartmentAsync(Guid departmentId, UpdateDepartmentDto updateDepartmentDto)
        {
            var department = await _departmentDomainService.UpdateDepartmentAsync(
                departmentId,
                updateDepartmentDto.Name);
                
            return EntityMapper.MapToDepartmentDto(department);
        }
    }
}