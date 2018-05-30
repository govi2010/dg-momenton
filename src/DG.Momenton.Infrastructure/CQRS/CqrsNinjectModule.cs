using Ninject.Modules;

namespace DG.Momenton.Infrastructure.CQRS
{
    #region CqrsNinjectModule

    /// <summary>
    /// Ninject Module for CQRS
    /// </summary>
    public class CqrsNinjectModule : NinjectModule
    {
        #region Load

        /// <summary>
        /// Override to set up the binding
        /// </summary>
        public override void Load()
        {
            Bind<IQueryDispatcher>().To<QueryDispatcher>().InSingletonScope();
            Bind<ICommandDispatcher>().To<CommandDispatcher>().InSingletonScope();
        }

        #endregion
    }

    #endregion
}
