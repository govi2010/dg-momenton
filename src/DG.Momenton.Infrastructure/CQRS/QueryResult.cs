using System;
using System.Collections.Generic;
using System.Text;

namespace DG.Momenton.Infrastructure.CQRS
{
    #region QueryResult

    /// <summary>
    /// The base interface for Query's result
    /// </summary>
    public abstract class QueryResult {

        #region ctor

        /// <summary>
        /// Default constructor
        /// </summary>
        public QueryResult() => this.IsSuccess = true;

        #endregion
        #region Members

        /// <summary>
        /// Boolean value to indicate success/fail result
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Info about the error (if any)
        /// </summary>
        public string ErrorInfo { get; set; }

        #endregion
        #region Fail

        /// <summary>
        /// Helper method to fail the result while also attaching the error-info
        /// </summary>
        /// <param name="errorInfo">Info about the error</param>
        public void Fail(string errorInfo) => this.ErrorInfo = errorInfo;

        #endregion
    }

    #endregion
}
