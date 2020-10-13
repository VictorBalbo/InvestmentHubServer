using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvestmentHub.ServerApplication.Services
{
    public abstract class BaseService : IService
    {
        private readonly SemaphoreSlim _semaphore;

        private CancellationTokenSource _cts;
        public Task Execution { get; private set; }

        protected BaseService()
        {
            _semaphore = new SemaphoreSlim(1);
        }

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                if (Execution != null)
                {
                    throw new InvalidOperationException($"The service '{GetType().Name}' is already started");
                }

                cancellationToken.ThrowIfCancellationRequested();

                _cts = new CancellationTokenSource();
                Execution = Task.Run(() =>
                {
                    ExecuteAsync(_cts.Token);
                }, _cts.Token);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Stop()
        {
            _semaphore.Wait();

            try
            {
                if (Execution == null)
                {
                    throw new InvalidOperationException($"The service '{GetType().Name}' is already stoped");
                }

                _cts?.Cancel();
                _cts?.Dispose();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}