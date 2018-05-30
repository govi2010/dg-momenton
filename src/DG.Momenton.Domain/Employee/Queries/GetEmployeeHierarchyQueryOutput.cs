using DG.Momenton.Domain.Employee.TransferObjects;
using DG.Momenton.Infrastructure.CQRS;
using System.Collections.Generic;

namespace DG.Momenton.Domain.Employee.Queries
{
    #region GetEmployeeHierarchyQueryOutput

    /// <summary>
    /// The output-class of GetEmployeeHierarchyQuery
    /// </summary>
    public class GetEmployeeHierarchyQueryOutput : QueryResult
    {
        /// <summary>
        /// List of all root-employees
        /// </summary>
        public IEnumerable<EmployeeTO> RootEmployees { get; set; }
    }

    #endregion
}
