using Ninject;
using Ninject.Syntax;
using System;

namespace DG.Momenton.Infrastructure.CQRS
{
    #region CommandDispatcher

    /// <summary>
    /// The concrete implementation for dispatching Command
    /// </summary>
    public class CommandDispatcher : ICommandDispatcher
    {
        #region ResolutionRoot

        /// <summary>
        /// The Ninject Root to help us get the Kernel for injection
        /// </summary>
        [Inject]
        private IResolutionRoot ResolutionRoot { get; set; }

        #endregion
        #region Send

        /// <summary>
        /// Send the requested-Command
        /// </summary>
        /// <typeparam name="TCommandInput">The type of Command's input-param</typeparam>
        /// <param name="command">The Command that is to be executed</param>
        public void Send<TCommandInput>(TCommandInput command) where TCommandInput : ICommand
        {
            var handler = ResolutionRoot.TryGet<ICommandHandler<TCommandInput>>();
            if (handler == null)
            {
                throw new Exception($"Cannot find the command handler for {command.GetType()}");
            }
            handler.Handle(command);
        }

        #endregion
    }

    #endregion
}
