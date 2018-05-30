using Newtonsoft.Json;
using System.Collections.Generic;

namespace DG.Momenton.Domain.Employee.TransferObjects
{
    #region EmployeeTO

    /// <summary>
    /// The Transfer-Object for Employee
    /// </summary>
    public class EmployeeTO
    {
        #region Members

        /// <summary>
        /// Employee ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of this employee's subordinates
        /// </summary>
        public List<EmployeeTO> Subordinates { get; set; }

        /// <summary>
        /// The ID of the this employee's Manager
        /// </summary>
        [JsonIgnore]
        public int? ManagerId { get; set; }

        #endregion
        #region ctor

        /// <summary>
        /// The constructor
        /// </summary>
        public EmployeeTO()
        {
            this.Subordinates = new List<EmployeeTO>();
        }

        #endregion
        #region AddSubordinate

        /// <summary>
        /// Register a subordinate
        /// </summary>
        /// <param name="employeeTO"></param>
        public void AddSubordinate(EmployeeTO employeeTO)
        {
            this.Subordinates.Add(employeeTO);
        }

        #endregion
    }

    #endregion
}
