using DG.Momenton.Domain.Employee.Queries;
using DG.Momenton.Infrastructure.CQRS;
using DG.Momenton.Infrastructure.Repository;
using System.Collections.Generic;
using DG.Momenton.Domain.Employee.TransferObjects;
using System;
using System.Linq;
using NLog;

namespace DG.Momenton.Domain.Employee.QueryHandlers
{
    #region GetEmployeeHierarchyQueryHandler

    /// <summary>
    /// The handler implementation to get the employee hierarchy
    /// </summary>
    public class GetEmployeeHierarchyQueryHandler :
        IQueryHandler<GetEmployeeHierarchyQuery, GetEmployeeHierarchyQueryOutput>
    {
        #region Private instances

        /// <summary>
        /// The employee-repo where all the data will be gathered from
        /// </summary>
        private readonly IRepository<Infrastructure.Repository.Models.Employee> _employeeRepo;

        /// <summary>
        /// NLog logger
        /// </summary>
        private readonly ILogger _logger;

        #endregion
        #region ctor

        /// <summary>
        /// Default constructor entry for the class
        /// </summary>
        /// <param name="employeeRepo">The employee-repo that is to be populated by Ninject DI</param>
        /// <param name="logger">The NLog instance</param>
        public GetEmployeeHierarchyQueryHandler
            (IRepository<Infrastructure.Repository.Models.Employee> employeeRepo, ILogger logger)
        {
            this._employeeRepo = employeeRepo;
            this._logger = logger;
        }

        #endregion
        #region Handle

        /// <summary>
        /// The real handler's implementation for this query
        /// </summary>
        /// <param name="query">The input-query object</param>
        /// <returns>The output object that will contain the hierarchical employee data</returns>
        public GetEmployeeHierarchyQueryOutput Handle(GetEmployeeHierarchyQuery query)
        {
            #region Init

            var output = new GetEmployeeHierarchyQueryOutput();
            IQueryable<Infrastructure.Repository.Models.Employee> employees = null;
            var dictEmployees = new Dictionary<int, EmployeeTO>();
            var dictPendingManagerEmployees = new Dictionary<int, List<EmployeeTO>>();
            var rootEmployeeTOs = new List<EmployeeTO>();

            #endregion
            #region Retrieve all employees

            try
            {
                if (output.IsSuccess)
                {
                    employees = _employeeRepo.All();
                }
            }
            catch (Exception ex)
            {
                var message = $"Error occured when trying to gather all employees data : {ex.Message}";
                output.Fail(message);
                _logger.Error(ex, message);
            }

            #endregion
            #region Processing individual employee

            if (output.IsSuccess)
            {
                foreach (var employee in employees)
                {
                    if (output.IsSuccess)
                    {
                        try
                        {
                            // Check if this employee has already been processed
                            // Any duplicate ID will be skipped
                            if (!dictEmployees.ContainsKey(employee.Id))
                            {
                                // Populate the transfer-object from the retrieved-entity
                                var employeeTO = new EmployeeTO
                                {
                                    Id = employee.Id,
                                    Name = employee.Name,
                                    ManagerId = employee.ManagerId
                                };

                                // Add this new employee to the 'dictEmployees'
                                dictEmployees.Add(employee.Id, employeeTO);

                                // Check if whether the employee has manager-id
                                if (!employeeTO.ManagerId.HasValue)
                                {
                                    // This employee doesn't have any manager set against him/her
                                    // so, we will add it to the root-employee
                                    rootEmployeeTOs.Add(employeeTO);
                                }
                                else
                                {
                                    // This employee has any manager set against him/her

                                    // Check if this employee's manager has already been added to the employee-dictionary
                                    EmployeeTO managerTO = null;
                                    if (dictEmployees.TryGetValue(employee.ManagerId.Value, out managerTO))
                                    {
                                        // We find the manager of this employee, so we will establish the link between the two
                                        // i.e. adding this employee to the manager's subordinate list
                                        managerTO.AddSubordinate(employeeTO);
                                    }
                                    else
                                    {
                                        // We cannot find the manager for this employeee, 
                                        // therefore we will just add it into dictPendingManagerEmployees,
                                        // the dictionary that keep track of all 'pending' manager 
                                        // i.e. the manager-id that hasn't been found in the rettrieved-employee data
                                        // they 'key' will be the manager-id, 
                                        // and the 'value' will be the list of employee(subordinates)

                                        // This list will serve as "Temporary" list
                                        // to hold the list of employee(subordinates)
                                        // that will be used in the following block
                                        List<EmployeeTO> currentPendingManagerEmployees;

                                        // First, we'll check if this manager-id has been added to the dictionary
                                        // e.g. by other employees (subordinate) reporting to this manager
                                        if (dictPendingManagerEmployees.TryGetValue(employeeTO.ManagerId.Value, out currentPendingManagerEmployees))
                                        {
                                            // Yes, this manager-id has been added before
                                            // Therefore, we'll just add the currently-processed employee to the 'currentPendingManagerEmployees
                                            currentPendingManagerEmployees.Add(employeeTO);
                                        }
                                        else
                                        {
                                            // Nope, this manager-id has NOT been added before
                                            // Therefore we'll initialize the list of subordinate, and also adding the currently processed employee into it
                                            dictPendingManagerEmployees.Add(employeeTO.ManagerId.Value, new List<EmployeeTO> { employeeTO });
                                        }
                                    }
                                }

                                // Recall that previously we add the list of 'unknown-yet' manager-id to the 'dictPendingManagerEmployees'
                                // Here is now the chance for us to see if any of the entry matching the currently-processed employee-id
                                // or, in another word, is this employee the manager of any previously processed employees (whose emanager-id cannot be identified before) ?
                                List<EmployeeTO> pendingManagerEmployees;
                                if (dictPendingManagerEmployees.TryGetValue(employeeTO.Id, out pendingManagerEmployees))
                                {
                                    // Yeyy, we found a match here

                                    // Assign this employee as the manager of these associated pending-employee-with-no-manager list
                                    // which is stored in the 'value' part of the 'dictPendingManagerEmployees'
                                    pendingManagerEmployees.ForEach(x => employeeTO.AddSubordinate(x));

                                    // Once processed, we'll remove this employee from the dictPendingManagerEmployees 
                                    dictPendingManagerEmployees.Remove(employeeTO.Id);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var message = $"Error occured when trying to process employee data ({employee.Name} - ID {employee.Id}) : {ex.Message}";
                            output.Fail(message);
                            _logger.Error(ex,message);

                            // exit the loop once an error occured
                            break;
                        }
                    }
                }
            }

            #endregion
            #region Finalize checking on any "pending" employee whose manager-id hasn't been found yet

            if (output.IsSuccess)
            {
                var currentlyProcessedManagerId = -1;
                try
                {
                    // Final check if there is any remaining unknown-manager-id
                    // in which, at this stage, we will regard it as 'invalid'
                    // we will just create an "(Unknown)" entry

                    foreach (var pendingManagerEmployees in dictPendingManagerEmployees)
                    {
                        currentlyProcessedManagerId = pendingManagerEmployees.Key;
                        var unknownManagerTO = new EmployeeTO
                        {
                            Id = pendingManagerEmployees.Key,
                            Name = "(Unknown)",
                            Subordinates = pendingManagerEmployees.Value
                        };

                        rootEmployeeTOs.Add(unknownManagerTO);
                    }

                    // Clear the dictionary as we are no longer using it
                    dictPendingManagerEmployees.Clear();
                }
                catch (Exception ex)
                {
                    var message = $"Error occured when trying to finalize checking on any 'pending' employee whose manager-id hasn't been found yet : {ex.Message} // currenty-processed-manager-id : {currentlyProcessedManagerId}";
                    output.Fail(ex.Message);
                    _logger.Error(ex, message);
                }
            }
           
            #endregion
            #region Finalize the output object

            if (output.IsSuccess)
            {
                output.RootEmployees = rootEmployeeTOs;
            }

            return output;

            #endregion
        }

        #endregion
    }

    #endregion
}

