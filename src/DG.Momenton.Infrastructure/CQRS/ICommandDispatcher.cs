namespace DG.Momenton.Infrastructure.CQRS
{
    #region ICommandDispatcher

    /// <summary>
    /// The base interface for dispatching Command
    /// </summary>
    public interface ICommandDispatcher
    {
        #region Send

        /// <summary>
        /// Send the requested-Command
        /// </summary>
        /// <typeparam name="TCommandInput">The type of Command's input-param</typeparam>
        /// <param name="command">The Command that is to be executed</param>
        void Send<TCommandInput>(TCommandInput command) where TCommandInput : ICommand;

        #endregion
    }

    #endregion
}
