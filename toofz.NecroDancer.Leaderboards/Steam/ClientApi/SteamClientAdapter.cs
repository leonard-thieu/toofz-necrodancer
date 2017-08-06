using System;
using System.Net;
using System.Threading.Tasks;
using log4net;
using SteamKit2;

namespace toofz.NecroDancer.Leaderboards.Steam.ClientApi
{
    /// <summary>
    /// Wraps an instance of <see cref="SteamClient"/> and presents a testable interface through <see cref="ISteamClient"/>.
    /// </summary>
    sealed class SteamClientAdapter : ISteamClient
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(SteamClientAdapter));

        /// <summary>
        /// Initializes a new instance of the <see cref="SteamClientAdapter"/> class.
        /// </summary>
        /// <param name="steamClient">
        /// The instance of <see cref="SteamClient"/> to wrap.
        /// </param>
        /// <param name="manager">
        /// The callback manager associated with <paramref name="steamClient"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="steamClient"/> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="manager"/> is null.
        /// </exception>
        public SteamClientAdapter(SteamClient steamClient, ICallbackManager manager)
        {
            SteamClient = steamClient ?? throw new ArgumentNullException(nameof(steamClient), $"{nameof(steamClient)} is null.");
            this.manager = manager ?? throw new ArgumentNullException(nameof(manager), $"{nameof(manager)} is null.");
        }

        readonly ICallbackManager manager;

        /// <summary>
        /// The instance of <see cref="SteamClient"/> wrapped by the adapter.
        /// </summary>
        public SteamClient SteamClient { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is connected to the remote CM server.
        /// </summary>
        public bool IsConnected => SteamClient.IsConnected;

        /// <summary>
        /// Gets a value indicating whether this instance is logged on to the remote CM server.
        /// </summary>
        public bool IsLoggedOn => SteamClient.SessionID != null;

        /// <summary>
        /// Gets or sets the network listening interface. Use this for debugging only. For
        /// your convenience, you can use <see cref="NetHookNetworkListener"/> class.
        /// </summary>
        public ProgressDebugNetworkListener ProgressDebugNetworkListener
        {
            get => SteamClient.DebugNetworkListener as ProgressDebugNetworkListener;
            set => SteamClient.DebugNetworkListener = value;
        }

        /// <summary>
        /// Returns a registered handler.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the handler to cast to. Must derive from <see cref="ClientMsgHandler"/>.
        /// </typeparam>
        /// <returns>
        /// A registered handler on success, or null if the handler could not be found.
        /// </returns>
        public T GetHandler<T>() where T : ClientMsgHandler
        {
            return SteamClient.GetHandler<T>();
        }

        /// <summary>
        /// Returns a wrapped registered handler for <see cref="SteamUserStats"/>.
        /// </summary>
        public ISteamUserStats GetSteamUserStats()
        {
            var steamUserStats = SteamClient.GetHandler<SteamUserStats>();

            return new SteamUserStatsAdapter(steamUserStats);
        }

        /// <summary>
        /// Connects this client to a Steam3 server. This begins the process of connecting
        /// and encrypting the data channel between the client and the server. Results are
        /// returned asynchronously in a <see cref="SteamClient.ConnectedCallback"/>. If the
        /// server that SteamKit attempts to connect to is down, a <see cref="SteamClient.DisconnectedCallback"/>
        /// will be posted instead. SteamKit will not attempt to reconnect to Steam, you
        /// must handle this callback and call Connect again preferrably after a short delay.
        /// </summary>
        /// <param name="cmServer">
        /// The <see cref="IPEndPoint"/> of the CM server to connect to. If null, SteamKit will
        /// randomly select a CM server from its internal list.
        /// </param>
        public Task<SteamClient.ConnectedCallback> ConnectAsync(IPEndPoint cmServer = null)
        {
            var tcs = new TaskCompletionSource<SteamClient.ConnectedCallback>();

            IDisposable onConnected = null;
            IDisposable onDisconnected = null;
            onConnected = manager.Subscribe<SteamClient.ConnectedCallback>(response =>
            {
                switch (response.Result)
                {
                    case EResult.OK:
                        Log.Info("Connected to Steam.");
                        tcs.TrySetResult(response);
                        break;
                    default:
                        tcs.TrySetException(new SteamClientApiException($"Unable to connect to Steam.") { Result = response.Result });
                        break;
                }

                onConnected.Dispose();
                onDisconnected.Dispose();

                onDisconnected = manager.Subscribe<SteamClient.DisconnectedCallback>(_ =>
                {
                    Log.Info("Disconnected from Steam.");
                    onDisconnected.Dispose();
                });
            });
            onDisconnected = manager.Subscribe<SteamClient.DisconnectedCallback>(response =>
            {
                tcs.TrySetException(new SteamClientApiException("Unable to connect to Steam."));
                onConnected.Dispose();
                onDisconnected.Dispose();
            });

            SteamClient.Connect(cmServer);
            manager.RunWaitAllCallbacks(TimeSpan.FromSeconds(1));

            return tcs.Task;
        }

        /// <summary>
        /// Logs the client into the Steam3 network. The client should already have been
        /// connected at this point. Results are returned in a <see cref="SteamUser.LoggedOnCallback"/>.
        /// </summary>
        /// <param name="details">The details to use for logging on.</param>
        /// <exception cref="System.ArgumentNullException">
        /// No logon details were provided.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Username or password are not set within details.
        /// </exception>
        public Task<SteamUser.LoggedOnCallback> LogOnAsync(SteamUser.LogOnDetails details)
        {
            var tcs = new TaskCompletionSource<SteamUser.LoggedOnCallback>();

            IDisposable onLoggedOn = null;
            IDisposable onDisconnected = null;
            onLoggedOn = manager.Subscribe<SteamUser.LoggedOnCallback>(response =>
            {
                switch (response.Result)
                {
                    case EResult.OK:
                        Log.Info("Logged on to Steam.");
                        tcs.TrySetResult(response);
                        break;
                    case EResult.AccountLogonDenied:
                        {
                            var ex = new SteamClientApiException("Unable to logon to Steam: This account is SteamGuard protected.") { Result = response.Result };
                            tcs.TrySetException(ex);
                            break;
                        }
                    default:
                        {
                            var ex = new SteamClientApiException("Unable to logon to Steam.") { Result = response.Result };
                            tcs.TrySetException(ex);
                            break;
                        }
                }

                onLoggedOn.Dispose();
                onDisconnected.Dispose();
            });
            onDisconnected = manager.Subscribe<SteamClient.DisconnectedCallback>(response =>
            {
                tcs.TrySetException(new SteamClientApiException("Unable to connect to Steam."));
                onLoggedOn.Dispose();
                onDisconnected.Dispose();
            });

            GetHandler<SteamUser>().LogOn(details);
            manager.RunWaitAllCallbacks(TimeSpan.FromSeconds(1));

            return tcs.Task;
        }

        /// <summary>
        /// Disconnects this client.
        /// </summary>
        public void Disconnect()
        {
            SteamClient.Disconnect();
            manager.RunWaitAllCallbacks(TimeSpan.FromSeconds(1));
        }
    }
}
