using System;
using System.Threading;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendPM.Infrastructure.Persistence
{
    /// <summary>
    /// 应用数据库上下文
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<User> Users { get; set; }
        
        /// <summary>
        /// 角色表
        /// </summary>
        public DbSet<Role> Roles { get; set; }
        
        /// <summary>
        /// 权限表
        /// </summary>
        public DbSet<Permission> Permissions { get; set; }
        
        /// <summary>
        /// 部门表
        /// </summary>
        public DbSet<Department> Departments { get; set; }
        
        /// <summary>
        /// 菜单表
        /// </summary>
        public DbSet<Menu> Menus { get; set; }
        
        /// <summary>
        /// 用户角色关联表
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }
        
        /// <summary>
        /// 角色权限关联表
        /// </summary>
        public DbSet<RolePermission> RolePermissions { get; set; }
        
        /// <summary>
        /// 用户部门关联表
        /// </summary>
        public DbSet<UserDepartment> UserDepartments { get; set; }
        
        /// <summary>
        /// 菜单权限关联表
        /// </summary>
        public DbSet<MenuPermission> MenuPermissions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // 软删除过滤器
            modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Permission>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Department>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Menu>().HasQueryFilter(e => !e.IsDeleted);
            
            // 配置用户实体
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
                
                // 用户个人资料作为拥有实体
                entity.OwnsOne(e => e.Profile, profile =>
                {
                    profile.Property(p => p.RealName).HasMaxLength(50);
                    profile.Property(p => p.Avatar).HasMaxLength(500);
                    profile.Property(p => p.Gender).HasConversion<int>();
                    profile.Property(p => p.Birthday).IsRequired(false);
                });
            });
            
            // 配置角色实体
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasIndex(e => e.Code).IsUnique();
            });
            
            // 配置权限实体
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).HasConversion<int>();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ApiResource).HasMaxLength(500);
                entity.HasIndex(e => e.Code).IsUnique();
            });
            
            // 配置部门实体
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Departments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Path).HasMaxLength(1000);
                entity.HasIndex(e => e.Code).IsUnique();
            });
            
            // 配置菜单实体
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menus");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Path).HasMaxLength(1000);
                entity.Property(e => e.Component).HasMaxLength(500);
                entity.Property(e => e.Route).HasMaxLength(500);
                entity.Property(e => e.Icon).HasMaxLength(100);
                entity.Property(e => e.Permissions).HasMaxLength(500);
                entity.HasIndex(e => e.Code).IsUnique();
            });
            
            // 配置用户角色关联
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.HasKey(e => new { e.UserId, e.RoleId });
                
                entity.HasOne<User>()
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne<Role>()
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // 配置角色权限关联
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("RolePermissions");
                entity.HasKey(e => new { e.RoleId, e.PermissionId });
                
                entity.HasOne<Role>()
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne<Permission>()
                    .WithMany()
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // 配置用户部门关联
            modelBuilder.Entity<UserDepartment>(entity =>
            {
                entity.ToTable("UserDepartments");
                entity.HasKey(e => new { e.UserId, e.DepartmentId });
                
                entity.HasOne<User>()
                    .WithMany(u => u.UserDepartments)
                    .HasForeignKey(ud => ud.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne<Department>()
                    .WithMany(d => d.UserDepartments)
                    .HasForeignKey(ud => ud.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // 配置菜单权限关联
            modelBuilder.Entity<MenuPermission>(entity =>
            {
                entity.ToTable("MenuPermissions");
                entity.HasKey(e => new { e.MenuId, e.PermissionId });
                
                entity.HasOne<Menu>()
                    .WithMany(m => m.MenuPermissions)
                    .HasForeignKey(mp => mp.MenuId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne<Permission>()
                    .WithMany()
                    .HasForeignKey(mp => mp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteAndTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteAndTimestamps();
            return base.SaveChanges();
        }

        private void UpdateSoftDeleteAndTimestamps()
        {
            var entries = ChangeTracker.Entries();
            
            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.CreatedAt = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            entity.LastModifiedAt = DateTime.UtcNow;
                            break;
                    }
                }
            }
        }
    }
}