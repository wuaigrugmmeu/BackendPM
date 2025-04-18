using System.Threading;
using System.Threading.Tasks;

namespace BackendPM.Application.Commands
{
    /// <summary>
    /// 命令处理器接口
    /// </summary>
    /// <typeparam name="TCommand">命令类型</typeparam>
    /// <typeparam name="TResult">命令结果类型</typeparam>
    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
    
    /// <summary>
    /// 无返回值命令处理器接口
    /// </summary>
    /// <typeparam name="TCommand">命令类型</typeparam>
    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, bool> where TCommand : ICommand
    {
    }
}