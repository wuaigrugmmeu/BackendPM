using System;
using System.Collections.Generic;
using BackendPM.Domain.ValueObjects;

namespace BackendPM.Domain.Entities
{
    /// <summary>
    /// 用户实体，作为用户领域的聚合根
    /// </summary>
    public class User : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 用户名，系统唯一
        /// </summary>
        public string Username { get; private set; }
        
        /// <summary>
        /// 密码哈希
        /// </summary>
        public string PasswordHash { get; private set; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; private set; }
        
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; private set; }
        
        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatus Status { get; private set; }
        
        /// <summary>
        /// 用户个人信息
        /// </summary>
        public UserProfile Profile { get; private set; }
        
        /// <summary>
        /// 用户所属角色集合
        /// </summary>
        private readonly List<UserRole> _userRoles = new List<UserRole>();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
        
        /// <summary>
        /// 用户所属部门集合
        /// </summary>
        private readonly List<UserDepartment> _userDepartments = new List<UserDepartment>();
        public IReadOnlyCollection<UserDepartment> UserDepartments => _userDepartments.AsReadOnly();
        
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; private set; }
        
        /// <summary>
        /// 登录失败次数
        /// </summary>
        public int LoginFailedCount { get; private set; }
        
        /// <summary>
        /// 锁定结束时间
        /// </summary>
        public DateTime? LockoutEnd { get; private set; }

        protected User() { } // 为EF Core准备的构造函数

        public User(string username, string passwordHash, string email, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("用户名不能为空", nameof(username));
                
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("密码不能为空", nameof(passwordHash));
            
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
            PhoneNumber = phoneNumber;
            Status = UserStatus.Active;
            LoginFailedCount = 0;
            Profile = new UserProfile(this.Id);
        }

        public void UpdateProfile(string realName, string avatar, Gender gender, DateTime? birthday)
        {
            Profile.Update(realName, avatar, gender, birthday);
            LastModifiedAt = DateTime.UtcNow;
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("新密码不能为空", nameof(newPasswordHash));
                
            PasswordHash = newPasswordHash;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateEmail(string email)
        {
            Email = email;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            Status = UserStatus.Active;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            Status = UserStatus.Inactive;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void Lock(TimeSpan duration)
        {
            Status = UserStatus.Locked;
            LockoutEnd = DateTime.UtcNow.Add(duration);
            LastModifiedAt = DateTime.UtcNow;
        }

        public void Unlock()
        {
            Status = UserStatus.Active;
            LockoutEnd = null;
            LoginFailedCount = 0;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void RecordLoginSuccess()
        {
            LastLoginTime = DateTime.UtcNow;
            LoginFailedCount = 0;
        }

        public void RecordLoginFailure()
        {
            LoginFailedCount++;
            // 可以添加自动锁定逻辑，例如失败5次后锁定账户
            if (LoginFailedCount >= 5)
            {
                Lock(TimeSpan.FromMinutes(30));
            }
        }

        public void AddRole(Role role)
        {
            if (_userRoles.Exists(ur => ur.RoleId == role.Id))
                return;
                
            _userRoles.Add(new UserRole(this.Id, role.Id));
            LastModifiedAt = DateTime.UtcNow;
        }

        public void RemoveRole(Guid roleId)
        {
            var userRole = _userRoles.Find(ur => ur.RoleId == roleId);
            if (userRole != null)
            {
                _userRoles.Remove(userRole);
                LastModifiedAt = DateTime.UtcNow;
            }
        }

        public void AddDepartment(Department department)
        {
            if (_userDepartments.Exists(ud => ud.DepartmentId == department.Id))
                return;
                
            _userDepartments.Add(new UserDepartment(this.Id, department.Id));
            LastModifiedAt = DateTime.UtcNow;
        }

        public void RemoveDepartment(Guid departmentId)
        {
            var userDepartment = _userDepartments.Find(ud => ud.DepartmentId == departmentId);
            if (userDepartment != null)
            {
                _userDepartments.Remove(userDepartment);
                LastModifiedAt = DateTime.UtcNow;
            }
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            Status = UserStatus.Inactive;
            LastModifiedAt = DateTime.UtcNow;
        }

        public bool IsLocked()
        {
            return Status == UserStatus.Locked && (LockoutEnd == null || LockoutEnd > DateTime.UtcNow);
        }
    }
}