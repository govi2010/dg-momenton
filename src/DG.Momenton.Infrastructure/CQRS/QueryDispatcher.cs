using Ninject;
using System;

namespace DG.Momenton.Infrastructure.CQRS
{
    #region QueryDispatcher

    /// <summary>
    /// The concrete implementation for dispatching Query
    /// </summary>
    public class QueryDispatcher : IQueryDispatcher
    {
        #region private members

        /// <summary>
        /// Ninject's Kernel
        /// </summary>
        private readonly IKernel _kernel;

        #endregion
        #region ctor

        /// <summary>
        /// Constructor init
        /// </summary>
        /// <param name="kernel"></param>
        public QueryDispatcher(IKernel kernel)
        {
            _kernel = kernel ?? throw new ArgumentNullException("kernel");
        }

        #endregion
        #region Query

        /// <summary>
        /// Send the Query request
        /// </summary>
        /// <typeparam name="TQueryInput">The type of query's input-param</typeparam>
        /// <typeparam name="TQueryOutput">The type of query's result</typeparam>
        /// <param name="query">The query that is to be executed</param>
        /// <returns>The result of the query execution</returns>
        public TQueryOutput Query<TQueryInput, TQueryOutput>(TQueryInput query)
            where TQueryInput : IQuery<TQueryOutput>
            where TQueryOutput : QueryResult
        {
            var handler = _kernel.Get<IQueryHandler<TQueryInput, TQueryOutput>>();
            return handler.Handle(query);
        }

        #endregion
    }

    #endregion
}
