using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DG.Momenton.Infrastructure.Repository
{
    /// <summary>
    /// The concrete Repository implementation
    /// </summary>
    /// <typeparam name="TEntity">The type of entity managed by this Repository</typeparam>
    public class Repository<TEntity> : IRepository<TEntity>
      where TEntity : class, IEntity
    {
        #region Private members

        /// <summary>
        /// The data-context this repository based on
        /// </summary>
        private readonly DataContext _dbContext;
        
        /// <summary>
        /// The data-context this repository based on
        /// </summary>
        private readonly DbSet<TEntity> _dbSetEntity;
        
        #endregion
        #region ctor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(DataContext dbContext)
        {
            this._dbContext = dbContext;
            this._dbSetEntity = this._dbContext.Set<TEntity>();
        }

        #endregion
        #region All

        /// <summary>
        /// Retrieve ALL records
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> All()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        #endregion
        #region Get

        /// <summary>
        /// Retrieve by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> Get(int id)
        {
            return await _dbSetEntity
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);
        }

        #endregion
        #region Create

        /// <summary>
        /// Create record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Create(TEntity entity)
        {
            await _dbSetEntity.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        #endregion
        #region Update

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Update(int id, TEntity entity)
        {
            _dbSetEntity.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        #endregion
        #region Delete

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            var entity = await Get(id);
            _dbSetEntity.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        
        #endregion
    }
}
