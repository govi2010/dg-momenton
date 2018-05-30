using System.Linq;
using System.Threading.Tasks;

namespace DG.Momenton.Infrastructure.Repository
{
    #region IRepository

    /// <summary>
    /// The base interface for our Repository
    /// </summary>
    /// <typeparam name="TEntity">The type of entity managed by this Repository</typeparam>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        #region All

        /// <summary>
        /// Retrieve ALL records
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> All();

        #endregion
        #region Get

        /// <summary>
        /// Retrieve by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> Get(int id);

        #endregion
        #region Create

        /// <summary>
        /// Create record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Create(TEntity entity);

        #endregion
        #region Update

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(int id, TEntity entity);

        #endregion
        #region Delete

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        #endregion
    }

    #endregion
}
