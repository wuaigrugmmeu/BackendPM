using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BackendPM.Domain.Entities;

namespace BackendPM.Domain.Repositories
{
    /// <summary>
    /// 通用仓储接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>实体</returns>
        Task<T> GetByIdAsync(Guid id);
        
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        Task<IEnumerable<T>> GetAllAsync();
        
        /// <summary>
        /// 根据条件查询实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>满足条件的实体列表</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">实体</param>
        Task AddAsync(T entity);
        
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        Task UpdateAsync(T entity);
        
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体ID</param>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// 根据条件查询单个实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>满足条件的单个实体</returns>
        Task<T> FindOneAsync(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// 判断是否存在满足条件的实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        /// 获取满足条件的实体数量
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体数量</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">页码，从1开始</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="orderBy">排序表达式</param>
        /// <param name="isAscending">是否升序</param>
        /// <returns>分页结果</returns>
        Task<(IEnumerable<T> Data, int Total)> GetPagedAsync(
            int pageIndex, 
            int pageSize, 
            Expression<Func<T, bool>> predicate = null, 
            Expression<Func<T, object>> orderBy = null, 
            bool isAscending = true);
    }
}