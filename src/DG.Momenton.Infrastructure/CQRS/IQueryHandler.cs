namespace DG.Momenton.Infrastructure.CQRS
{
    #region IQueryHandler

    /// <summary>
    /// The base interface for any Query handlers
    /// </summary>
    /// <typeparam name="TQueryInput">The type of Query's input</typeparam>
    /// <typeparam name="TResuTQueryOutput">The type of Query's output</typeparam>
    public interface IQueryHandler<in TQueryInput, out TQueryOutput> 
        where TQueryOutput : QueryResult 
        where TQueryInput : IQuery<TQueryOutput>
    {
        #region Handle

        /// <summary>
        /// Handle the Query request
        /// </summary>
        /// <param name="query">The Query</param>
        /// <returns>The result of the Query</returns>
        TQueryOutput Handle(TQueryInput query);

        #endregion
    }

    #endregion
}
