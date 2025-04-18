using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BackendPM.Application.Queries
{
    /// <summary>
    /// 查询总线实现
    /// </summary>
    public class QueryBus : IQueryBus
    {
        private readonly IServiceProvider _serviceProvider;
        
        public QueryBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            var queryType = query.GetType();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));
            
            var handler = _serviceProvider.GetService(handlerType);
            if (handler == null)
            {
                throw new InvalidOperationException($"没有找到查询 {queryType.Name} 的处理器");
            }
            
            // 通过反射调用HandleAsync方法
            var method = handlerType.GetMethod("HandleAsync");
            var task = (Task<TResult>)method.Invoke(handler, new object[] { query, cancellationToken });
            
            return await task;
        }
    }
}