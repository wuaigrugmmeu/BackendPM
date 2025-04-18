using System.Threading;
using System.Threading.Tasks;

namespace BackendPM.Application.Commands
{
    /// <summary>
    /// 命令总线接口
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <typeparam name="TResult">命令结果类型</typeparam>
        /// <param name="command">命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// 发送无返回值命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功</returns>
        Task<bool> SendAsync(ICommand command, CancellationToken cancellationToken = default);
    }
}