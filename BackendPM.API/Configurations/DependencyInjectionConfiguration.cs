using BackendPM.Application.Commands;
using BackendPM.Application.Queries;
using BackendPM.Application.Services;
using BackendPM.Application.Services.Impl;
using BackendPM.Domain.Repositories;
using BackendPM.Domain.Services;
using BackendPM.Infrastructure.Persistence;
using BackendPM.Infrastructure.Persistence.Repositories;
using BackendPM.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BackendPM.API.Configurations
{
    /// <summary>
    /// 依赖注入配置
    /// </summary>
    public static class DependencyInjectionConfiguration
    {
        /// <summary>
        /// 添加依赖注入服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">配置</param>
        public static void AddDependencyInjectionServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 添加数据库上下文
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            
            // 注册存储库
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            
            // 注册领域服务
            services.AddScoped<IUserDomainService, UserDomainService>();
            services.AddScoped<IRoleDomainService, RoleDomainService>();
            services.AddScoped<IPermissionDomainService, PermissionDomainService>();
            services.AddScoped<IMenuDomainService, MenuDomainService>();
            services.AddScoped<IDepartmentDomainService, DepartmentDomainService>();
            
            // 注册应用服务
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IRoleAppService, RoleAppService>();
            services.AddScoped<IPermissionAppService, PermissionAppService>();
            services.AddScoped<IMenuAppService, MenuAppService>();
            services.AddScoped<IDepartmentAppService, DepartmentAppService>();
            
            // 注册命令与查询总线
            services.AddSingleton<ICommandBus, CommandBus>();
            services.AddSingleton<IQueryBus, QueryBus>();
            
            // 注册命令处理器
            var commandHandlerType = typeof(ICommandHandler<>);
            var queryHandlerType = typeof(IQueryHandler<,>);
            
            // 查找并注册所有命令处理器
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    // 注册命令处理器
                    foreach (var @interface in type.GetInterfaces())
                    {
                        if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == commandHandlerType)
                        {
                            var commandType = @interface.GetGenericArguments()[0];
                            var handlerInterfaceType = commandHandlerType.MakeGenericType(commandType);
                            
                            services.AddTransient(handlerInterfaceType, type);
                        }
                        
                        // 注册查询处理器
                        if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == queryHandlerType)
                        {
                            var queryType = @interface.GetGenericArguments()[0];
                            var resultType = @interface.GetGenericArguments()[1];
                            var handlerInterfaceType = queryHandlerType.MakeGenericType(queryType, resultType);
                            
                            services.AddTransient(handlerInterfaceType, type);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 配置依赖注入服务，供 Program.cs 调用
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">配置</param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            AddDependencyInjectionServices(services, configuration);
        }
    }
}