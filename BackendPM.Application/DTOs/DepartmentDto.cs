using System;
using System.Collections.Generic;

namespace BackendPM.Application.DTOs
{
    /// <summary>
    /// 部门DTO
    /// </summary>
    public class DepartmentDto
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 部门编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 部门描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 父部门ID
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// 部门层级路径
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// 部门负责人ID
        /// </summary>
        public Guid? LeaderId { get; set; }
        
        /// <summary>
        /// 部门负责人姓名
        /// </summary>
        public string LeaderName { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// 子部门列表
        /// </summary>
        public List<DepartmentDto> Children { get; set; } = new List<DepartmentDto>();
    }
    
    /// <summary>
    /// 部门创建请求DTO
    /// </summary>
    public class CreateDepartmentDto
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 部门编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 部门描述
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 父部门ID
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// 部门负责人ID
        /// </summary>
        public Guid? LeaderId { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;
    }
    
    /// <summary>
    /// 部门更新请求DTO
    /// </summary>
    public class UpdateDepartmentDto
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 部门描述
        /// </summary>
        public string Description { get; set; }
    }
    
    /// <summary>
    /// 部门树节点DTO
    /// </summary>
    public class DepartmentTreeDto
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 部门编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// 部门负责人ID
        /// </summary>
        public Guid? LeaderId { get; set; }
        
        /// <summary>
        /// 部门负责人姓名
        /// </summary>
        public string LeaderName { get; set; }
        
        /// <summary>
        /// 子部门列表
        /// </summary>
        public List<DepartmentTreeDto> Children { get; set; } = new List<DepartmentTreeDto>();
    }
    
    /// <summary>
    /// 设置部门负责人请求DTO
    /// </summary>
    public class SetDepartmentLeaderDto
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DepartmentId { get; set; }
        
        /// <summary>
        /// 负责人ID，null表示取消负责人
        /// </summary>
        public Guid? LeaderId { get; set; }
    }
    
    /// <summary>
    /// 设置部门父级请求DTO
    /// </summary>
    public class SetDepartmentParentDto
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DepartmentId { get; set; }
        
        /// <summary>
        /// 父部门ID，null表示设置为顶级部门
        /// </summary>
        public Guid? ParentId { get; set; }
    }
    
    /// <summary>
    /// 部门用户请求DTO
    /// </summary>
    public class DepartmentUserDto
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public Guid DepartmentId { get; set; }
        
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// 是否是主部门
        /// </summary>
        public bool IsPrimary { get; set; }
    }
}