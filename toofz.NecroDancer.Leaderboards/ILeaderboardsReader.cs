using System;
using System.Collections.Generic;
using System.IO;

namespace toofz.NecroDancer.Leaderboards
{
    public interface ILeaderboardsReader
    {
        IEnumerable<Player> ReadPlayers(Stream data);
        Uri ReadReplayUri(Stream data);
    }
}
