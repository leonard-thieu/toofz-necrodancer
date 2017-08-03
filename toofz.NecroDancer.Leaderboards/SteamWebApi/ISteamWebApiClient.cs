using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using toofz.NecroDancer.Leaderboards.SteamWebApi.ISteamUser;

namespace toofz.NecroDancer.Leaderboards.SteamWebApi
{
    public interface ISteamWebApiClient : IDisposable
    {
        /// <summary>
        /// A Steam Web API key. This is required by some API endpoints.
        /// </summary>
        string SteamWebApiKey { get; set; }

        /// <summary>
        /// Returns basic profile information for a list of 64-bit Steam IDs.
        /// </summary>
        /// <param name="steamIds">
        /// List of 64 bit Steam IDs to return profile information for. Up to 100 Steam IDs can be requested.
        /// </param>
        /// <param name="progress">
        /// A progress provider that will be called with total bytes requested. <paramref name="progress"/> may be null.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        /// <see cref="GetPlayerSummariesAsync"/> requires <see cref="SteamWebApiKey"/> to be set to a valid Steam Web API Key.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="steamIds"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// Unable to request more than 100 player summaries.
        /// </exception>
        Task<PlayerSummaries> GetPlayerSummariesAsync(IEnumerable<long> steamIds, IProgress<long> progress = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<ReplayContext>> GetReplaysAsync(IEnumerable<long> ugcIds, CancellationToken cancellationToken = default(CancellationToken));
    }
}