using Ninject.Modules;

namespace DG.Momenton.Infrastructure.Repository
{
    #region RepositoryNinjectModule

    /// <summary>
    /// Ninject Module for our Repository
    /// </summary>
    public class RepositoryNinjectModule : NinjectModule
    {
        #region Load

        /// <summary>
        /// Override to set up the binding
        /// </summary>
        public override void Load()
        {
            Bind<IRepository<Models.Employee>>().To<Repository<Models.Employee>>().InSingletonScope();
        }

        #endregion
    }

    #endregion
}
