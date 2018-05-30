namespace DG.Momenton.Infrastructure.CQRS
{
    #region ICommandHandler

    /// <summary>
    /// The base interface for Command handlers
    /// </summary>
    /// <typeparam name="TCommandInput"></typeparam>
    public interface ICommandHandler<in TCommandInput> 
        where TCommandInput : ICommand
    {
        #region Handle

        /// <summary>
        /// Handle the Command request
        /// </summary>
        /// <param name="command">The command to be processed</param>
        void Handle(TCommandInput command);

        #endregion
    }

    #endregion
}
