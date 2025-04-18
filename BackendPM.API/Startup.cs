using BackendPM.API.Configurations;
using BackendPM.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json.Serialization;

namespace BackendPM.API
{
    /// <summary>
    /// 应用程序启动配置
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 环境
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <param name="environment">环境</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services">服务集合</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加跨域服务
            services.AddCorsServices(Configuration);
            
            // 添加身份验证服务
            services.AddAuthenticationServices(Configuration);
            
            // 添加授权服务
            services.AddAuthorizationServices();
            
            // 添加依赖注入服务
            services.AddDependencyInjectionServices(Configuration);
            
            // 添加控制器
            services.AddControllers(options =>
            {
                // 注册全局过滤器
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<RequestResponseLogFilter>();
                options.Filters.Add<ApiPerformanceFilter>();
            })
            .AddJsonOptions(options =>
            {
                // 处理枚举类型
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            
            // 添加API版本控制
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });
            
            // 添加健康检查
            services.AddHealthChecks();
            
            // 添加HttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            // 添加内存缓存
            services.AddMemoryCache();
            
            // 添加响应压缩
            services.AddResponseCompression();
            
            // 添加Swagger服务
            services.AddSwaggerServices();
        }

        /// <summary>
        /// 配置应用程序
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        /// <param name="env">环境</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }
            
            // 使用响应压缩
            app.UseResponseCompression();
            
            // 使用HTTPS重定向
            app.UseHttpsRedirection();
            
            // 使用静态文件
            app.UseStaticFiles();
            
            // 使用路由
            app.UseRouting();
            
            // 使用跨域
            app.UseCorsServices();
            
            // 使用身份验证
            app.UseAuthentication();
            
            // 使用授权
            app.UseAuthorization();
            
            // 使用Swagger
            app.UseSwaggerServices();
            
            // 使用终结点
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}