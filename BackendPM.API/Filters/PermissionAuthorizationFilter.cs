using BackendPM.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BackendPM.API.Filters
{
    /// <summary>
    /// 权限验证过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAuthorizationFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _permissionCode;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="permissionCode">权限代码</param>
        public PermissionAuthorizationFilter(string permissionCode)
        {
            _permissionCode = permissionCode;
        }
        
        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="context">授权过滤器上下文</param>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // 跳过匿名访问的接口
            if (context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute)))
            {
                return;
            }
            
            // 获取当前用户
            var user = context.HttpContext.User;
            
            // 检查用户是否已认证
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            // 获取用户ID
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            // 获取用户角色
            var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
            
            // 如果用户是超级管理员，直接放行
            if (roles.Contains("Administrator"))
            {
                return;
            }
            
            // 从DI容器中获取权限服务
            var permissionService = context.HttpContext.RequestServices.GetService(typeof(IPermissionAppService)) as IPermissionAppService;
            if (permissionService == null)
            {
                var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<PermissionAuthorizationFilter>)) as ILogger<PermissionAuthorizationFilter>;
                logger?.LogError("无法获取权限服务");
                context.Result = new StatusCodeResult(500);
                return;
            }
            
            // 验证用户是否有指定权限
            var userId = Guid.Parse(userIdClaim.Value);
            bool hasPermission = await permissionService.UserHasPermissionAsync(userId, _permissionCode);
            
            if (!hasPermission)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}