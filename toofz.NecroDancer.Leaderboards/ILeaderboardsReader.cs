﻿using System;
using System.Collections.Generic;
using System.IO;

namespace toofz.NecroDancer.Leaderboards
{
    public partial interface ILeaderboardsReader
    {
        IEnumerable<Player> ReadPlayers(Stream data);
        Uri ReadReplayUri(Stream data);
    }
}
