using Ninject.Modules;
using NLog;

namespace DG.Momenton.Infrastructure.NLog
{
    #region NLogNinjectModule

    /// <summary>
    /// Ninject Module for NLog
    /// </summary>
    public class NLogNinjectModule : NinjectModule
    {
        #region Load

        /// <summary>
        /// Override to set up the binding
        /// </summary>
        public override void Load()
        {
             Bind<ILogger>().ToMethod(p => LogManager.GetLogger(
                 p.Request.Target.Member.DeclaringType.ToString(), 
                 p.Request.Target.Member.DeclaringType));
        }

        #endregion
    }

    #endregion
}
