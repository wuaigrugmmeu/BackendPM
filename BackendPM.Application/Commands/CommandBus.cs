using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BackendPM.Application.Commands
{
    /// <summary>
    /// 命令总线实现
    /// </summary>
    public class CommandBus : ICommandBus
    {
        private readonly IServiceProvider _serviceProvider;
        
        public CommandBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        {
            var commandType = command.GetType();
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));
            
            var handler = _serviceProvider.GetService(handlerType);
            if (handler == null)
            {
                throw new InvalidOperationException($"没有找到命令 {commandType.Name} 的处理器");
            }
            
            // 通过反射调用HandleAsync方法
            var method = handlerType.GetMethod("HandleAsync");
            var task = (Task<TResult>)method.Invoke(handler, new object[] { command, cancellationToken });
            
            return await task;
        }
        
        public async Task<bool> SendAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            return await SendAsync<bool>(command, cancellationToken);
        }
    }
}