using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BackendPM.API.Filters
{
    /// <summary>
    /// API性能监控过滤器
    /// </summary>
    public class ApiPerformanceFilter : IAsyncActionFilter
    {
        private readonly ILogger<ApiPerformanceFilter> _logger;
        private readonly Stopwatch _stopwatch;
        
        /// <summary>
        /// 性能警告阈值(毫秒)
        /// </summary>
        private const long PerformanceWarningThreshold = 500;
        
        /// <summary>
        /// 性能严重警告阈值(毫秒)
        /// </summary>
        private const long PerformanceCriticalThreshold = 1000;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public ApiPerformanceFilter(ILogger<ApiPerformanceFilter> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }
        
        /// <summary>
        /// 性能监控
        /// </summary>
        /// <param name="context">动作执行上下文</param>
        /// <param name="next">动作执行委托</param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 开始计时
            _stopwatch.Start();
            
            // 获取控制器名称和操作名称
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            
            // 执行后续管道
            var resultContext = await next();
            
            // 停止计时
            _stopwatch.Stop();
            
            // 获取执行时间
            var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
            
            // 记录性能日志
            if (elapsedMilliseconds > PerformanceCriticalThreshold)
            {
                _logger.LogError(
                    "API性能严重警告 - 控制器: {Controller}, 操作: {Action}, 耗时: {ElapsedMilliseconds}ms",
                    controllerName,
                    actionName,
                    elapsedMilliseconds);
            }
            else if (elapsedMilliseconds > PerformanceWarningThreshold)
            {
                _logger.LogWarning(
                    "API性能警告 - 控制器: {Controller}, 操作: {Action}, 耗时: {ElapsedMilliseconds}ms",
                    controllerName,
                    actionName,
                    elapsedMilliseconds);
            }
            else
            {
                _logger.LogDebug(
                    "API性能正常 - 控制器: {Controller}, 操作: {Action}, 耗时: {ElapsedMilliseconds}ms",
                    controllerName,
                    actionName,
                    elapsedMilliseconds);
            }
        }
    }
}