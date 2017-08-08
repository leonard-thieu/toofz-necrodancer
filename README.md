[![Build status](https://ci.appveyor.com/api/projects/status/kl4iyoueism9ojdo?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer)

# toofz

## Project structure

#### [toofz](https://github.com/leonard-thieu/toofz)

Utility library.

#### [toofz.NecroDancer](https://github.com/leonard-thieu/toofz-necrodancer-core)

Provides types for working with Crypt of the NecroDancer game data.

#### [toofz.NecroDancer.EntityFramework](https://github.com/leonard-thieu/toofz-necrodancer-entityframework)

Provides a context for working with types from [toofz.NecroDancer](https://github.com/leonard-thieu/toofz-necrodancer-core) in a database.

#### [toofz.NecroDancer.Leaderboards](https://github.com/leonard-thieu/toofz-necrodancer-leaderboards)

Provides types for working with Crypt of the NecroDancer leaderboards.

#### [toofz.NecroDancer.Leaderboards.Services.Common](toofz.NecroDancer.Leaderboards.Services.Common)

Common code for services.

#### [toofz.NecroDancer.Leaderboards.Services.LeaderboardsService](toofz.NecroDancer.Leaderboards.Services.LeaderboardsService)

Service that updates leaderboard data.

#### [toofz.NecroDancer.Leaderboards.Services.PlayersService](toofz.NecroDancer.Leaderboards.Services.PlayersService)

Service that updates player data.

#### [toofz.NecroDancer.Leaderboards.Services.ReplaysService](toofz.NecroDancer.Leaderboards.Services.ReplaysService)

Service that updates replay data.

#### [SqlBulkUpsert](https://github.com/leonard-thieu/SqlBulkUpsert)

Provides bulk upserting functionality. Used by leaderboard services for performance. Forked from https://github.com/dezfowler/SqlBulkUpsert.

#### [toofz.NecroDancer.WebClient](https://github.com/leonard-thieu/toofz-necrodancer-webclient)

Source for http://crypt.toofz.com/.

#### [toofz.NecroDancer.Web.Api](toofz.NecroDancer.Web.Api)

Source for https://api.toofz.com/help.

#### [toofz.NecroDancer.Web.Api.Tests](toofz.NecroDancer.Web.Api.Tests)

Tests for [toofz.NecroDancer.Web.Api](toofz.NecroDancer.Web.Api).

#### [toofz.NecroDancer.ImageManager](https://github.com/leonard-thieu/toofz-necrodancer-imagemanager)

Prepares and uploads image data from Crypt of the NecroDancer for use on the toofz website.

#### [toofz.NecroDancer.LoadData](https://github.com/leonard-thieu/toofz-necrodancer-loaddata)

Parses and loads Crypt of the NecroDancer item and enemy data into a database.
