[![Build status](https://ci.appveyor.com/api/projects/status/kl4iyoueism9ojdo?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer)

# toofz

## Project structure

#### [toofz](https://github.com/leonard-thieu/toofz)

Utility library.

#### [toofz.NecroDancer](https://github.com/leonard-thieu/toofz-necrodancer-core)

Provides types for working with Crypt of the NecroDancer game data.

#### [toofz.NecroDancer.EntityFramework](toofz.NecroDancer.EntityFramework)

Provides a context for working with types from [toofz.NecroDancer](toofz.NecroDancer) in a database.

#### [toofz.NecroDancer.Leaderboards](toofz.NecroDancer.Leaderboards)

Provides types for working with Crypt of the NecroDancer leaderboards.

#### [toofz.NecroDancer.Leaderboards.Tests](toofz.NecroDancer.Leaderboards.Tests)

Tests for [toofz.NecroDancer.Leaderboards](toofz.NecroDancer.Leaderboards).

#### [toofz.NecroDancer.Leaderboards.Services.Common](toofz.NecroDancer.Leaderboards.Services.Common)

Common code for services.

#### [toofz.NecroDancer.Leaderboards.Services.LeaderboardUpdate](toofz.NecroDancer.Leaderboards.Services.LeaderboardUpdate)

Service that updates leaderboard data.

#### [toofz.NecroDancer.Leaderboards.Services.PlayerUpdate](toofz.NecroDancer.Leaderboards.Services.PlayerUpdate)

Service that updates player data.

#### [toofz.NecroDancer.Leaderboards.Services.ReplayUpdate](toofz.NecroDancer.Leaderboards.Services.ReplayUpdate)

Service that updates replay data.

#### [toofz.NecroDancer.Leaderboards.Services.Installer](toofz.NecroDancer.Leaderboards.Services.Installer)

Installer for leaderboard services.

#### [SqlBulkUpsert](https://github.com/leonard-thieu/SqlBulkUpsert)

Provides bulk upserting functionality. Used by leaderboard services for performance. Forked from https://github.com/dezfowler/SqlBulkUpsert.

#### [toofz.NecroDancer.WebClient](toofz.NecroDancer.WebClient)

Source for http://crypt.toofz.com/.

#### [toofz.NecroDancer.Web.Api](toofz.NecroDancer.Web.Api)

Source for https://api.toofz.com/help.

#### [toofz.NecroDancer.Web.Api.Tests](toofz.NecroDancer.Web.Api.Tests)

Tests for [toofz.NecroDancer.Web.Api](toofz.NecroDancer.Web.Api).

#### [toofz.NecroDancer.Web.ImageManager](toofz.NecroDancer.Web.ImageManager)

Prepares and uploads image data from Crypt of the NecroDancer for use on the toofz website.

#### [toofz.NecroDancer.Web.LoadData](toofz.NecroDancer.Web.LoadData)

Parses and loads Crypt of the NecroDancer item and enemy data into a database.
