using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BackendPM.API.Configurations
{
    /// <summary>
    /// Swagger配置
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// 添加Swagger服务
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // 配置API文档信息
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BackendPM API",
                    Version = "v1",
                    Description = "后台权限管理系统API",
                    Contact = new OpenApiContact
                    {
                        Name = "BackendPM Team",
                        Email = "admin@backendpm.com",
                        Url = new Uri("https://www.backendpm.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                
                // 启用XML注释
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
                
                // 配置身份验证
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                
                // 启用请求校验
                options.EnableAnnotations();
                
                // 对Action的参数进行注释
                options.CustomSchemaIds(type => type.FullName);
            });
        }
        
        /// <summary>
        /// 使用Swagger服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public static void UseSwaggerServices(this IApplicationBuilder app)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });
            
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api-docs/v1/swagger.json", "BackendPM API V1");
                options.RoutePrefix = "api-docs";
                options.DocumentTitle = "BackendPM API 文档";
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                options.DefaultModelExpandDepth(2);
                options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                options.DefaultModelsExpandDepth(-1); // 隐藏Models
                options.DisplayRequestDuration();
                options.EnableDeepLinking();
                options.EnableFilter();
                options.ShowExtensions();
            });
        }

        /// <summary>
        /// 配置 Swagger 服务，供 Program.cs 调用
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void ConfigureServices(IServiceCollection services)
        {
            AddSwaggerServices(services);
        }
    }
}