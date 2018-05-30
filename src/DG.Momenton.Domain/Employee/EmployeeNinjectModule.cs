using DG.Momenton.Domain.Employee.Queries;
using DG.Momenton.Domain.Employee.QueryHandlers;
using DG.Momenton.Infrastructure.CQRS;
using Ninject.Modules;

namespace DG.Momenton.Domain.Employee
{
    #region EmployeeNinjectModule

    /// <summary>
    /// Ninject Module for Employee Domain
    /// </summary>
    public class EmployeeNinjectModule : NinjectModule
    {
        #region Load

        /// <summary>
        /// Override to set up the binding
        /// </summary>
        public override void Load()
        {
            Bind<IQueryHandler<GetEmployeeHierarchyQuery, GetEmployeeHierarchyQueryOutput>>()
                .To<GetEmployeeHierarchyQueryHandler>().InSingletonScope();
        }

        #endregion
    }

    #endregion
}
