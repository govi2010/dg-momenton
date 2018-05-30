using DG.Momenton.ConsoleApp.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DG.Momenton.ConsoleApp
{
    #region EmployeeHierarchyManager

    /// <summary>
    /// Manager to get the
    /// </summary>
    public class EmployeeHierarchyManager
    {
        #region Private members

        /// <summary>
        /// NLog Logger instance
        /// </summary>
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion
        #region GetEmployeeHierarchyAsync

        /// <summary>
        /// Get the employee hierarchy
        /// </summary>
        /// <param name="apiEndpoint">the API endpoint to retrieve the data</param>
        /// <returns></returns>
        public async Task<string> GetEmployeeHierarchyAsync(string apiEndpoint)
        {
            var output = "";
            var isSuccess = true;
            _logger.Error("GetEmployeeHierarchyAsync");
            #region Retrieving employees data from the API

            var employeeDataJsonStr = "";
            if (isSuccess)
            {
                try
                {
                    var client = new HttpClient();
                    var stringTask = client.GetStringAsync(apiEndpoint);
                    employeeDataJsonStr = await stringTask;
                }
                catch (Exception ex)
                {
                    var message = $"Error occured when trying to retrieve employees data from the API // Error : {ex.Message}";
                    Console.WriteLine(message);
                    _logger.Error(ex, message);
                }
            }
            
            #endregion
            #region Deserialize JSON data

            IEnumerable<SimpleEmployeeTO> employees = null;
            if (isSuccess)
            {
                try
                {
                    employees = JsonConvert.DeserializeObject<IEnumerable<SimpleEmployeeTO>>(employeeDataJsonStr);
                }
                catch (Exception ex)
                {
                    var message = $"Error occured when trying to deserialize JSON data // Error : {ex.Message}";
                    Console.WriteLine(message);
                    _logger.Error(ex, message);
                }
            }

            #endregion
            #region Printing the employee hierarchy

            if (isSuccess)
            {
                try
                {
                    foreach (var employee in employees)
                    {
                        output += $"\n{employee.ToString(0, "+ ")}";
                    }
                }
                catch (Exception ex)
                {
                    var message = $"Error occured when trying to print the employee hierarchy // Error : {ex.Message}";
                    Console.WriteLine(message);
                    _logger.Error(ex, message);
                }
            }

            #endregion

            return output;
        }

        #endregion
    }

    #endregion
}
