using System;

namespace BackendPM.Domain.ValueObjects
{
    /// <summary>
    /// 用户个人资料值对象
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// 关联的用户ID
        /// </summary>
        public Guid UserId { get; private set; }
        
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; private set; }
        
        /// <summary>
        /// 头像URL
        /// </summary>
        public string Avatar { get; private set; }
        
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; private set; }
        
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; private set; }

        protected UserProfile() { } // 为EF Core准备的构造函数
        
        public UserProfile(Guid userId)
        {
            UserId = userId;
            Gender = Gender.Unspecified;
        }
        
        public void Update(string realName, string avatar, Gender gender, DateTime? birthday)
        {
            RealName = realName;
            Avatar = avatar;
            Gender = gender;
            Birthday = birthday;
        }
    }
}