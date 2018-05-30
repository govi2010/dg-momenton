namespace DG.Momenton.Infrastructure.CQRS
{
    #region IQueryDispatcher

    /// <summary>
    /// Send and process the requested-query
    /// </summary>
    public interface IQueryDispatcher
    {
        #region Query

        /// <summary>
        /// Dispatches the query
        /// </summary>
        /// <typeparam name="TQueryInput">The type of query's input-param</typeparam>
        /// <typeparam name="TQueryOutput">The type of query's result</typeparam>
        /// <param name="query">The query that is to be executed</param>
        /// <returns>The result of the query execution</returns>
        TQueryOutput Query<TQueryInput, TQueryOutput>(TQueryInput query)
            where TQueryInput : IQuery<TQueryOutput>
            where TQueryOutput : QueryResult;

        #endregion
    }

    #endregion
}
