﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using log4net;
using SqlBulkUpsert;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsStoreClient : ILeaderboardsStoreClient
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(LeaderboardsStoreClient));

        #region Initialization

        public LeaderboardsStoreClient(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        #endregion

        #region Fields

        readonly string connectionString;

        #endregion

        #region Leaderboard

        const string LeaderboardsTableName = "Leaderboards";

        static readonly ColumnMappings<Leaderboard> LeaderboardMappings = new ColumnMappings<Leaderboard>
        {
            d => d.LeaderboardId,
            d => d.LastUpdate,
            d => d.CharacterId,
            d => d.RunId,
            d => d.Date,
        };

        public Task SaveChangesAsync(IEnumerable<Leaderboard> leaderboards, CancellationToken cancellationToken)
        {
            return UpsertAsync(LeaderboardsTableName, LeaderboardMappings, leaderboards, true, cancellationToken);
        }

        #endregion

        #region Entry

        const string EntriesTableName = "Entries";

        static readonly ColumnMappings<Entry> EntryMappings = new ColumnMappings<Entry>
        {
            d => d.LeaderboardId,
            d => d.Rank,
            d => d.SteamId,
            d => d.ReplayId,
            d => d.Score,
            d => d.Zone,
            d => d.Level,
        };

        public async Task SaveChangesAsync(IEnumerable<Entry> entries)
        {
            var cancellationToken = CancellationToken.None;

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                var tableName = GetEntriesTableName();
                var targetSchema = await SqlTableSchema.LoadFromDatabaseAsync(connection, tableName, cancellationToken).ConfigureAwait(false);
                var upserter = new TypedUpserter<Entry>(targetSchema, EntryMappings);

                var command = connection.CreateCommand();

                command.CommandText = $@"
DECLARE @sql AS VARCHAR(MAX)='';

SELECT @sql = @sql + 'ALTER INDEX ' + sys.indexes.name + ' ON ' + sys.objects.name + ' DISABLE;' + CHAR(13) + CHAR(10)
FROM sys.indexes
JOIN sys.objects ON sys.indexes.object_id = sys.objects.object_id
WHERE sys.indexes.type_desc = 'NONCLUSTERED'
  AND sys.objects.type_desc = 'USER_TABLE'
  AND sys.objects.name = '{tableName}';

EXEC(@sql);";
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                using (var notifier = new StoreNotifier(Log, EntriesTableName.ToLowerInvariant()))
                {
                    await upserter.InsertAsync(connection, entries, notifier.Progress).ConfigureAwait(false);
                }

                command.CommandText = $@"
DECLARE @sql AS VARCHAR(MAX)='';

SELECT @sql = @sql + 'ALTER INDEX ' + sys.indexes.name + ' ON ' + sys.objects.name + ' REBUILD;' + CHAR(13) + CHAR(10)
FROM sys.indexes
JOIN sys.objects ON sys.indexes.object_id = sys.objects.object_id
WHERE sys.indexes.type_desc = 'NONCLUSTERED'
  AND sys.objects.type_desc = 'USER_TABLE'
  AND sys.objects.name = '{tableName}';

EXEC(@sql);";
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                command.CommandText = $@"
ALTER VIEW [dbo].[{EntriesTableName}]
AS

SELECT
    {nameof(Entry.LeaderboardId)},
    {nameof(Entry.Rank)},
    {nameof(Entry.SteamId)},
    {nameof(Entry.Score)},
    {nameof(Entry.Zone)},
    {nameof(Entry.Level)},
    {nameof(Entry.ReplayId)}
FROM {tableName};";
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        string EntriesSuffix = "_A";

        string GetEntriesTableName()
        {
            var tableName = EntriesTableName + EntriesSuffix;
            EntriesSuffix = EntriesSuffix == "_A" ? "_B" : "_A";
            return tableName;
        }

        #endregion

        #region DailyLeaderboard

        const string DailyLeaderboardsTableName = "DailyLeaderboards";

        static readonly ColumnMappings<DailyLeaderboard> DailyLeaderboardMappings = new ColumnMappings<DailyLeaderboard>
        {
            d => d.LeaderboardId,
            d => d.LastUpdate,
            d => d.Date,
            d => d.ProductId,
            d => d.IsProduction
        };

        public Task SaveChangesAsync(IEnumerable<DailyLeaderboard> leaderboards, CancellationToken cancellationToken)
        {
            return UpsertAsync(DailyLeaderboardsTableName, DailyLeaderboardMappings, leaderboards, true, cancellationToken);
        }

        #endregion

        #region DailyEntry

        const string DailyEntriesTableName = "DailyEntries";

        static readonly ColumnMappings<DailyEntry> DailyEntryMappings = new ColumnMappings<DailyEntry>
        {
            d => d.LeaderboardId,
            d => d.Rank,
            d => d.SteamId,
            d => d.ReplayId,
            d => d.Score,
            d => d.Zone,
            d => d.Level,
        };

        public Task SaveChangesAsync(IEnumerable<DailyEntry> entries, CancellationToken cancellationToken)
        {
            return UpsertAsync(DailyEntriesTableName, DailyEntryMappings, entries, true, cancellationToken);
        }

        #endregion

        #region Player

        const string PlayersTableName = "Players";

        static readonly ColumnMappings<Player> PlayerMappings = new ColumnMappings<Player>
        {
            d => d.SteamId,
            d => d.Exists,
            d => d.Name,
            d => d.LastUpdate,
            d => d.Avatar,
        };

        public Task SaveChangesAsync(IEnumerable<Player> players, bool updateOnMatch, CancellationToken cancellationToken)
        {
            return UpsertAsync(PlayersTableName, PlayerMappings, players, updateOnMatch, cancellationToken);
        }

        #endregion

        #region Replay

        const string ReplaysTableName = "Replays";

        static readonly ColumnMappings<Replay> ReplayMappings = new ColumnMappings<Replay>
        {
            d => d.ReplayId,
            d => d.ErrorCode,
            d => d.Seed,
            d => d.KilledBy,
            d => d.Version,
        };

        public Task SaveChangesAsync(IEnumerable<Replay> replays, bool updateOnMatch, CancellationToken cancellationToken)
        {
            return UpsertAsync(ReplaysTableName, ReplayMappings, replays, updateOnMatch, cancellationToken);
        }

        #endregion

        async Task UpsertAsync<T>(string tableName, ColumnMappings<T> columnMappings, IEnumerable<T> items, bool updateOnMatch, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

                var targetSchema = await SqlTableSchema.LoadFromDatabaseAsync(connection, tableName, cancellationToken).ConfigureAwait(false);
                var upserter = new TypedUpserter<T>(targetSchema, columnMappings);

                using (var activity = new StoreNotifier(Log, tableName.Humanize().Transform(To.LowerCase)))
                {
                    await upserter.UpsertAsync(connection, items, activity.Progress, updateOnMatch, cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}