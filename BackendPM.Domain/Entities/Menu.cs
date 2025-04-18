using System;
using System.Collections.Generic;

namespace BackendPM.Domain.Entities
{
    /// <summary>
    /// 菜单实体，作为菜单领域的聚合根
    /// </summary>
    public class Menu : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// 菜单编码，系统唯一
        /// </summary>
        public string Code { get; private set; }
        
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public Guid? ParentId { get; private set; }
        
        /// <summary>
        /// 菜单层级路径，用"/"分隔的ID字符串
        /// </summary>
        public string Path { get; private set; }
        
        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; private set; }
        
        /// <summary>
        /// 路由地址
        /// </summary>
        public string Route { get; private set; }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; private set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; private set; }
        
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; private set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; private set; }
        
        /// <summary>
        /// 权限标识
        /// </summary>
        public string Permissions { get; private set; }
        
        /// <summary>
        /// 菜单类型：0目录，1菜单，2按钮
        /// </summary>
        public int Type { get; private set; }
        
        /// <summary>
        /// 是否外链
        /// </summary>
        public bool IsExternal { get; private set; }
        
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool KeepAlive { get; private set; }
        
        /// <summary>
        /// 菜单对应的权限
        /// </summary>
        private readonly List<MenuPermission> _menuPermissions = new List<MenuPermission>();
        public IReadOnlyCollection<MenuPermission> MenuPermissions => _menuPermissions.AsReadOnly();
        
        protected Menu() { } // 为EF Core准备的构造函数
        
        public Menu(string name, string code, int type, string component, string route, Guid? parentId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("菜单名称不能为空", nameof(name));
                
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("菜单编码不能为空", nameof(code));
                
            Name = name;
            Code = code;
            Type = type;
            Component = component;
            Route = route;
            ParentId = parentId;
            Path = parentId.HasValue ? $"{parentId.Value}/" : "";
            Path += Id.ToString();
            IsEnabled = true;
            Visible = true;
            Sort = 0;
            IsExternal = false;
            KeepAlive = false;
        }
        
        public void Update(string name, string component, string route, string icon, string permissions)
        {
            Name = name;
            Component = component;
            Route = route;
            Icon = icon;
            Permissions = permissions;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void SetParent(Guid? parentId)
        {
            // 避免循环引用
            if (parentId == Id)
                throw new InvalidOperationException("菜单不能将自己设为父菜单");
                
            ParentId = parentId;
            UpdatePath();
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void UpdatePath(string parentPath = "")
        {
            Path = string.IsNullOrEmpty(parentPath) ? Id.ToString() : $"{parentPath}/{Id}";
            LastModifiedAt = DateTime.UtcNow;
            
            // 注意：在实际实现中，需要级联更新所有子菜单的Path
            // 这通常需要通过领域服务或应用服务来实现
        }
        
        public void SetVisibility(bool visible)
        {
            Visible = visible;
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
        
        public void SetExternalLink(bool isExternal)
        {
            IsExternal = isExternal;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void SetKeepAlive(bool keepAlive)
        {
            KeepAlive = keepAlive;
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void AddPermission(Permission permission)
        {
            if (_menuPermissions.Exists(mp => mp.PermissionId == permission.Id))
                return;
                
            _menuPermissions.Add(new MenuPermission(this.Id, permission.Id));
            LastModifiedAt = DateTime.UtcNow;
        }
        
        public void RemovePermission(Guid permissionId)
        {
            var menuPermission = _menuPermissions.Find(mp => mp.PermissionId == permissionId);
            if (menuPermission != null)
            {
                _menuPermissions.Remove(menuPermission);
                LastModifiedAt = DateTime.UtcNow;
            }
        }
        
        public void ClearPermissions()
        {
            _menuPermissions.Clear();
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