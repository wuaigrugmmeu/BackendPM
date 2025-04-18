using System;
using System.Collections.Generic;

namespace BackendPM.Domain.Entities
{
    /// <summary>
    /// 角色实体，作为角色领域的聚合根
    /// </summary>
    public class Role : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// 角色编码，系统唯一
        /// </summary>
        public string Code { get; private set; }
        
        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// 是否是系统角色
        /// </summary>
        public bool IsSystem { get; private set; }
        
        /// <summary>
        /// 父角色ID，用于角色继承
        /// </summary>
        public Guid? ParentId { get; private set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; private set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; private set; }
        
        /// <summary>
        /// 角色所包含的权限
        /// </summary>
        private readonly List<RolePermission> _rolePermissions = new List<RolePermission>();
        public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();
        
        protected Role() { } // 为EF Core准备的构造函数
        
        public Role(string name, string code, string description, bool isSystem = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("角色名称不能为空", nameof(name));
                
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("角色编码不能为空", nameof(code));
                
            Name = name;
            Code = code;
            Description = description;
            IsSystem = isSystem;
            IsEnabled = true;
            Sort = 0;
        }
        
        public void Update(string name, string description)
        {
            if (!IsSystem)
            {
                Name = name;
                Description = description;
                LastModifiedAt = DateTime.UtcNow;
            }
            else
            {
                throw new InvalidOperationException("系统角色不能修改基本信息");
            }
        }
        
        public void SetParent(Guid? parentId)
        {
            // 避免循环引用
            if (parentId == Id)
                throw new InvalidOperationException("角色不能将自己设为父角色");
                
            ParentId = parentId;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void Enable()
        {
            IsEnabled = true;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void Disable()
        {
            if (!IsSystem)
            {
                IsEnabled = false;
                LastModifiedAt = DateTime.UtcNow;
            }
            else
            {
                throw new InvalidOperationException("系统角色不能禁用");
            }
        }
        
        public void SetSort(int sort)
        {
            Sort = sort;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void AddPermission(Permission permission)
        {
            if (_rolePermissions.Exists(rp => rp.PermissionId == permission.Id))
                return;
                
            _rolePermissions.Add(new RolePermission(this.Id, permission.Id));
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void RemovePermission(Guid permissionId)
        {
            var rolePermission = _rolePermissions.Find(rp => rp.PermissionId == permissionId);
            if (rolePermission != null)
            {
                _rolePermissions.Remove(rolePermission);
                LastModifiedAt = DateTime.UtcNow;
            }
        }
        
        public void ClearPermissions()
        {
            _rolePermissions.Clear();
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void SoftDelete()
        {
            if (!IsSystem)
            {
                IsDeleted = true;
                IsEnabled = false;
                LastModifiedAt = DateTime.UtcNow;
            }
            else
            {
                throw new InvalidOperationException("系统角色不能删除");
            }
        }
    }
}