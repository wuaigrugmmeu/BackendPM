using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BackendPM.API.Filters
{
    /// <summary>
    /// 请求/响应日志过滤器
    /// </summary>
    public class RequestResponseLogFilter : IAsyncActionFilter
    {
        private readonly ILogger<RequestResponseLogFilter> _logger;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public RequestResponseLogFilter(ILogger<RequestResponseLogFilter> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// 请求/响应处理
        /// </summary>
        /// <param name="context">动作执行上下文</param>
        /// <param name="next">动作执行委托</param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 开始计时
            var stopwatch = Stopwatch.StartNew();
            
            // 获取请求信息
            var request = context.HttpContext.Request;
            var requestTime = DateTime.Now;
            var requestId = Guid.NewGuid().ToString();
            
            // 记录请求信息
            _logger.LogInformation(
                "API请求 - ID: {RequestId}, 时间: {RequestTime}, 方法: {Method}, 路径: {Path}, 查询: {Query}, IP: {IP}",
                requestId,
                requestTime,
                request.Method,
                request.Path,
                request.QueryString,
                context.HttpContext.Connection.RemoteIpAddress);
            
            // 记录请求体 (如果有)
            if (request.ContentLength > 0 && request.Body.CanRead)
            {
                // 备份原始请求体流
                var originalRequestBody = request.Body;
                
                try
                {
                    // 读取请求体
                    request.EnableBuffering();
                    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                    await request.Body.ReadAsync(buffer, 0, buffer.Length);
                    var requestBodyContent = Encoding.UTF8.GetString(buffer).Trim();
                    
                    // 记录请求体 (过滤敏感信息)
                    var sanitizedRequestBody = FilterSensitiveInfo(requestBodyContent);
                    _logger.LogDebug("请求体: {RequestBody}", sanitizedRequestBody);
                    
                    // 重置请求体位置，以便后续中间件可以读取
                    request.Body.Position = 0;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "读取请求体时发生错误");
                }
            }
            
            // 捕获原始响应体
            var originalBodyStream = context.HttpContext.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.HttpContext.Response.Body = responseBodyStream;
            
            // 执行后续管道
            var resultContext = await next();
            stopwatch.Stop();
            
            // 记录响应信息
            var response = context.HttpContext.Response;
            var responseTime = DateTime.Now;
            var elapsedTime = stopwatch.ElapsedMilliseconds;
            
            _logger.LogInformation(
                "API响应 - ID: {RequestId}, 时间: {ResponseTime}, 状态码: {StatusCode}, 耗时: {ElapsedTime}ms",
                requestId,
                responseTime,
                response.StatusCode,
                elapsedTime);
            
            // 记录响应体 (如果有)
            try
            {
                responseBodyStream.Position = 0;
                var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
                
                // 记录响应体 (过滤敏感信息)
                var sanitizedResponseBody = FilterSensitiveInfo(responseBody);
                _logger.LogDebug("响应体: {ResponseBody}", sanitizedResponseBody);
                
                // 复制响应体到原始流，以便返回给客户端
                responseBodyStream.Position = 0;
                await responseBodyStream.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "读取响应体时发生错误");
            }
            finally
            {
                // 恢复原始响应体流
                context.HttpContext.Response.Body = originalBodyStream;
            }
        }
        
        /// <summary>
        /// 过滤敏感信息 (如密码等)
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns>过滤后的内容</returns>
        private string FilterSensitiveInfo(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }
            
            // 替换密码字段
            content = System.Text.RegularExpressions.Regex.Replace(
                content,
                @"""(password|pwd|secret|token|apiKey)"":\s*""([^""]*)""",
                @"""$1"":""***""",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                
            return content;
        }
    }
}