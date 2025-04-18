using System.Threading.Tasks;

namespace BackendPM.Application.Commands
{
    /// <summary>
    /// 命令接口
    /// </summary>
    /// <typeparam name="TResult">命令执行结果类型</typeparam>
    public interface ICommand<TResult>
    {
    }
    
    /// <summary>
    /// 无返回值命令接口
    /// </summary>
    public interface ICommand : ICommand<bool>
    {
    }
}