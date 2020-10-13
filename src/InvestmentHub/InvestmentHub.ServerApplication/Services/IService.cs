using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.ServerApplication.Services
{
    /// <summary>
    /// Defines a service basic operarions.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Gets the Task that represents the execution of the service.
        /// </summary>
        Task Execution { get; }

        /// <summary>
        /// Starts the execution of the service.
        /// </summary>
        /// <param name="cancellationToken">Token to allow the abortion of the service start</param>
        /// <returns></returns>
        Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Signals to stop the execution of the service.
        /// The loop is stopped when the Execution task is completed.
        /// </summary>
        void Stop();
    }
}