using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace BackendPM.API.Configurations
{
    /// <summary>
    /// CORS跨域配置
    /// </summary>
    public static class CorsConfiguration
    {
        /// <summary>
        /// 默认跨域策略名称
        /// </summary>
        public const string DefaultCorsPolicyName = "DefaultCorsPolicy";
        
        /// <summary>
        /// 添加跨域服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">配置</param>
        public static void AddCorsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    // 从配置文件中读取允许的来源
                    var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
                    if (allowedOrigins?.Length > 0)
                    {
                        builder.WithOrigins(allowedOrigins)
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    }
                    else
                    {
                        // 如果没有配置或配置为空，则允许所有来源（不推荐在生产环境中使用）
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    }
                });
            });
        }
        
        /// <summary>
        /// 使用跨域服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public static void UseCorsServices(this IApplicationBuilder app)
        {
            app.UseCors(DefaultCorsPolicyName);
        }

        /// <summary>
        /// 配置CORS服务，供 Program.cs 调用
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration">配置</param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration = null)
        {
            // 如果没有提供配置，则使用默认的 AllowAll 策略
            if (configuration == null)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
                });
            }
            else
            {
                // 使用配置文件中的 CORS 设置
                AddCorsServices(services, configuration);
            }
        }
    }
}