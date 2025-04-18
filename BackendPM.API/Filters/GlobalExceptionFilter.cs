using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace BackendPM.API.Filters
{
    /// <summary>
    /// 全局异常处理过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="context">异常上下文</param>
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "服务器内部错误，请稍后重试";
            
            var exception = context.Exception;
            
            // 根据不同类型的异常返回不同的状态码和消息
            if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = "未授权的请求";
            }
            else if (exception is ArgumentException || exception is FormatException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = "请求参数错误";
            }
            else if (exception is TimeoutException)
            {
                statusCode = HttpStatusCode.RequestTimeout;
                message = "请求超时";
            }
            else if (exception is NotImplementedException)
            {
                statusCode = HttpStatusCode.NotImplemented;
                message = "请求的功能尚未实现";
            }
            
            // 记录异常日志
            _logger.LogError(exception, $"API异常: {exception.Message}");
            
            // 创建错误响应
            var result = new ObjectResult(new 
            {
                code = (int)statusCode,
                message = message,
                detailMessage = context.HttpContext.Request.Headers["X-Show-Error-Details"] == "true" 
                    ? exception.ToString() 
                    : null
            })
            {
                StatusCode = (int)statusCode
            };
            
            // 设置结果并标记为已处理
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}