using Microsoft.AspNetCore.Mvc;
using System;

namespace BackendPM.API.Controllers
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        protected Guid CurrentUserId
        {
            get
            {
                var userIdClaim = User.FindFirst("sub");
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return userId;
                }
                return Guid.Empty;
            }
        }

        /// <summary>
        /// 返回成功结果
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>API响应</returns>
        protected IActionResult Success(object data = null)
        {
            return Ok(new
            {
                Success = true,
                Data = data
            });
        }

        /// <summary>
        /// 返回带分页信息的成功结果
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="total">总数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>API响应</returns>
        protected IActionResult SuccessWithPagination(object data, int total, int pageIndex, int pageSize)
        {
            return Ok(new
            {
                Success = true,
                Data = data,
                Pagination = new
                {
                    Total = total,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                }
            });
        }

        /// <summary>
        /// 返回失败结果
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="statusCode">HTTP状态码</param>
        /// <returns>API响应</returns>
        protected IActionResult Fail(string message, int statusCode = 400)
        {
            return StatusCode(statusCode, new
            {
                Success = false,
                Error = message
            });
        }
    }
}