using System;
using System.Collections.Generic;

namespace BackendPM.Domain.Entities
{
    /// <summary>
    /// 部门实体，作为组织结构领域的聚合根
    /// </summary>
    public class Department : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// 部门编码，系统唯一
        /// </summary>
        public string Code { get; private set; }
        
        /// <summary>
        /// 部门描述
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// 父部门ID
        /// </summary>
        public Guid? ParentId { get; private set; }
        
        /// <summary>
        /// 部门层级路径，用"/"分隔的ID字符串
        /// </summary>
        public string Path { get; private set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; private set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; private set; }
        
        /// <summary>
        /// 部门负责人ID
        /// </summary>
        public Guid? LeaderId { get; private set; }
        
        /// <summary>
        /// 部门成员
        /// </summary>
        private readonly List<UserDepartment> _userDepartments = new List<UserDepartment>();
        public IReadOnlyCollection<UserDepartment> UserDepartments => _userDepartments.AsReadOnly();
        
        protected Department() { } // 为EF Core准备的构造函数
        
        public Department(string name, string code, string description, Guid? parentId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("部门名称不能为空", nameof(name));
                
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("部门编码不能为空", nameof(code));
                
            Name = name;
            Code = code;
            Description = description;
            ParentId = parentId;
            Path = parentId.HasValue ? $"{parentId.Value}/" : "";
            Path += Id.ToString();
            IsEnabled = true;
            Sort = 0;
        }
        
        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void SetParent(Guid? parentId)
        {
            // 避免循环引用
            if (parentId == Id)
                throw new InvalidOperationException("部门不能将自己设为父部门");
                
            ParentId = parentId;
            UpdatePath();
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void UpdatePath(string parentPath = "")
        {
            Path = string.IsNullOrEmpty(parentPath) ? Id.ToString() : $"{parentPath}/{Id}";
            LastModifiedAt = DateTime.UtcNow;
            
            // 注意：在实际实现中，需要级联更新所有子部门的Path
            // 这通常需要通过领域服务或应用服务来实现
        }
        
        public void SetLeader(Guid? leaderId)
        {
            LeaderId = leaderId;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void Enable()
        {
            IsEnabled = true;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void Disable()
        {
            IsEnabled = false;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void SetSort(int sort)
        {
            Sort = sort;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void SoftDelete()
        {
            IsDeleted = true;
            IsEnabled = false;
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}