using System;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

namespace toofz.NecroDancer.Leaderboards.Services.Common
{
    public abstract partial class WorkerRoleBase<TSettings> : ServiceBase
        where TSettings : Settings, new()
    {
        #region Static Members

        static readonly ILog Log = LogManager.GetLogger(typeof(WorkerRoleBase<TSettings>).GetSimpleFullName());

        static void LogError(string message, Exception ex)
        {
            var e = ex;
            if (ex is AggregateException)
            {
                var all = ((AggregateException)ex).Flatten();
                e = all.InnerExceptions.Count == 1 ? all.InnerException : all;
            }
            Log.Error(message, e);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerRoleBase{TSettings}"/> class.
        /// </summary>
        protected WorkerRoleBase(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
                throw new ArgumentException();
            if (serviceName.Length > MaxNameLength)
                throw new ArgumentException();

            InitializeComponent();
            ServiceName = serviceName;

            ServicePointManager.MaxServicePointIdleTime = (int)Constants.MaxServicePointIdleTime.TotalSeconds;
        }

        #region Fields

        readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        Thread thread;
        Idle idle;

        protected abstract string SettingsPath { get; }

        protected TSettings Settings { get; set; } = new TSettings();

        #endregion

        /// <summary>
        /// Starts the update process. This method is intended to be called from console applications.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        public void ConsoleStart(params string[] args)
        {
            OnStart(args);
            thread.Join(Timeout.InfiniteTimeSpan);
        }

        #region OnStart

        /// <summary>
        /// Executes when a Start command is sent to the service by the Service Control Manager (SCM) 
        /// or when the operating system starts (for a service that starts automatically).
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected sealed override void OnStart(string[] args)
        {
            OnStartOverride();
            thread = new Thread(Run);
            thread.Start();
        }

        /// <summary>
        /// When overridden in a derived class, performs initialization for the service.
        /// </summary>
        protected abstract void OnStartOverride();

        #endregion

        #region Run

        void Run()
        {
            var cancellationToken = cancellationTokenSource.Token;

            while (true)
            {
                try
                {
                    RunAsync(cancellationToken).Wait();
                }
                catch (Exception ex) when (!(ex.InnerException is TaskCanceledException))
                {
                    LogError("Failed to complete run due to an error.", ex);
                }

                try
                {
                    idle.Delay(TimeSpan.FromSeconds(Settings.UpdateInterval), cancellationToken);
                }
                catch (Exception ex)
                {
                    LogError("Failed to complete run due to a cancellation request.", ex);
                    break;
                }
            }
        }

        async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                idle = new Idle();

                using (var sr = File.OpenText(SettingsPath))
                {
                    var json = await sr.ReadToEndAsync().ConfigureAwait(false);
                    Settings = JsonConvert.DeserializeObject<TSettings>(json);
                }

                await RunAsyncOverride(cancellationToken).ConfigureAwait(false);

                idle.Delay(TimeSpan.FromSeconds(Settings.UpdateInterval), cancellationToken);
            }
        }

        /// <summary>
        /// When overridden in a derived class, provides the main logic to run the update.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token that will be checked prior to completing the returned task.
        /// </param>
        /// <returns>A task that represents the update process.</returns>
        protected abstract Task RunAsyncOverride(CancellationToken cancellationToken);

        #endregion

        #region OnStop

        /// <summary>
        /// Executes when a Stop command is sent to the service by the Service Control Manager (SCM).
        /// </summary>
        protected override void OnStop()
        {
            cancellationTokenSource.Cancel();
            thread.Join(TimeSpan.FromSeconds(10));
        }

        #endregion
    }
}
