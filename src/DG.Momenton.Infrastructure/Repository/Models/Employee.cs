using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DG.Momenton.Infrastructure.Repository.Models
{
    #region Employee

    /// <summary>
    /// Employee entity
    /// </summary>
    public class Employee : IEntity
    {
        /// <summary>
        /// Employee ID, which is also the primary key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Employee name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The employee's manager-id
        /// </summary>
        public int? ManagerId { get; set; }
    }

    #endregion
}
