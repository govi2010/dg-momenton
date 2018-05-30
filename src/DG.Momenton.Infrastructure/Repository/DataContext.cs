using DG.Momenton.Infrastructure.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DG.Momenton.Infrastructure.Repository
{
    #region DataContext

    /// <summary>
    /// The generic data-context for our repository
    /// </summary>
    public class DataContext : DbContext
    {
        #region ctor

        /// <summary>
        /// The default constructor
        /// </summary>
        /// <param name="options"></param>
        public DataContext(DbContextOptions<DataContext> options) : base(options) {

        }

        #endregion
        #region Employees

        /// <summary>
        /// Employee data
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        #endregion
    }

    #endregion
}
