namespace DG.Momenton.Infrastructure.CQRS
{
    #region IQuery

    /// <summary>
    /// The base-interface for any Query
    /// </summary>
    /// <typeparam name="TQueryOutput">The type of query's output</typeparam>
    public interface IQuery<out TQueryOutput> where TQueryOutput : QueryResult
    {}

    #endregion
}
