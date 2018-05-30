namespace DG.Momenton.Infrastructure.Repository
{
    #region IEntity

    /// <summary>
    /// The base interface for all of our entities
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The compulsory ID field
        /// </summary>
        int Id { get; set; }
    }

    #endregion
}
