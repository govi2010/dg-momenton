using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Momenton.ConsoleApp.Models
{
    #region SimpleEmployeeTO

    /// <summary>
    /// MApping for Employee Transfer Object
    /// </summary>
    public class SimpleEmployeeTO
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
        /// Employee's subordinates
        /// </summary>
        public List<SimpleEmployeeTO> Subordinates { get; set; }
        
        #endregion
        #region ctor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SimpleEmployeeTO()
        {
            this.Subordinates = new List<SimpleEmployeeTO>();
        }

        #endregion
        #region ToString

        /// <summary>
        /// Print employee detail
        /// </summary>
        /// <param name="level">the hierachy level, with the root starting from 1</param>
        /// <param name="indentationSymbol">the symbol for the indentation displayed when hitting new level</param>
        /// <returns></returns>
        public string ToString(int level, string indentationSymbol)
        {
            var output = "\n ";

            // Add whitespace and indentation
            var totalPreWhitespace = indentationSymbol.Length * level;
            output += String.Join("", Enumerable.Range(1, totalPreWhitespace).Select(x => " "));

            // Print employee name & ID
            output += $"{indentationSymbol}{this.Name} - ID:{this.Id}";
            
            // Recursively going through all the employee's subordinates to get the total hierarchy-structure
            foreach (var subordinate in this.Subordinates)
            {
                output += $"{subordinate.ToString(level+1,indentationSymbol)}";
            }

            return output;
        }

        #endregion
    }

    #endregion
}
