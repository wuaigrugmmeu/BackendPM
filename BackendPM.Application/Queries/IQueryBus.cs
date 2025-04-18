using System.Threading;
using System.Threading.Tasks;

namespace BackendPM.Application.Queries
{
    /// <summary>
    /// 查询总线接口
    /// </summary>
    public interface IQueryBus
    {
        /// <summary>
        /// 发送查询
        /// </summary>
        /// <typeparam name="TResult">查询结果类型</typeparam>
        /// <param name="query">查询</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>查询结果</returns>
        Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}