using System;
using System.Collections.Generic;
using BackendPM.Domain.ValueObjects;

namespace BackendPM.Domain.Entities
{
    /// <summary>
    /// 权限实体，作为权限领域的聚合根
    /// </summary>
    public class Permission : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// 权限编码，系统唯一
        /// </summary>
        public string Code { get; private set; }
        
        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; private set; }
        
        /// <summary>
        /// 权限描述
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// 权限组ID
        /// </summary>
        public Guid? GroupId { get; private set; }
        
        /// <summary>
        /// API资源路径，仅当Type为Api时有效
        /// </summary>
        public string ApiResource { get; private set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; private set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; private set; }
        
        protected Permission() { } // 为EF Core准备的构造函数
        
        public Permission(string name, string code, PermissionType type, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("权限名称不能为空", nameof(name));
                
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("权限编码不能为空", nameof(code));
                
            Name = name;
            Code = code;
            Type = type;
            Description = description;
            IsEnabled = true;
            Sort = 0;
        }
        
        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void SetGroup(Guid? groupId)
        {
            GroupId = groupId;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void SetApiResource(string apiResource)
        {
            if (Type == PermissionType.Api)
            {
                ApiResource = apiResource;
                LastModifiedAt = DateTime.UtcNow;
            }
            else
            {
                throw new InvalidOperationException("只有API类型的权限才能设置API资源路径");
            }
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