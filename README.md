# toofz

## Project structure

### Web sites

#### [crypt.toofz.com](https://github.com/leonard-thieu/toofz-necrodancer-webclient) [![Build status](https://ci.appveyor.com/api/projects/status/83e8eikypiri2lhi/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-webclient/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/toofz-necrodancer-webclient/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/toofz-necrodancer-webclient?branch=master)

Source for http://crypt.toofz.com/.

#### [api.toofz.com](https://github.com/leonard-thieu/toofz-necrodancer-web-api) [![Build status](https://ci.appveyor.com/api/projects/status/2en9f6hcf72ujm9y/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-web-api/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/toofz-necrodancer-web-api/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/toofz-necrodancer-web-api?branch=master)

Source for [https://api.toofz.com/](https://api.toofz.com/help).

### Leaderboard Services

#### [Leaderboards Service](https://github.com/leonard-thieu/leaderboards-service) [![Build status](https://ci.appveyor.com/api/projects/status/77fd6okl8bc2ulkb/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/leaderboards-service/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/leaderboards-service/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/leaderboards-service?branch=master)

Service that updates leaderboard data.

#### [Players Service](https://github.com/leonard-thieu/players-service) [![Build status](https://ci.appveyor.com/api/projects/status/3udoy27b6tetostp/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/players-service/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/players-service/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/players-service?branch=master)

Service that updates player data.

#### [Replays Service](https://github.com/leonard-thieu/replays-service) [![Build status](https://ci.appveyor.com/api/projects/status/xeoko709p63qf3jb/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/replays-service/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/replays-service/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/replays-service?branch=master)

Service that updates replay data.

#### [Services Core](https://github.com/leonard-thieu/toofz-necrodancer-leaderboards-services-common)

Common code for services.

#### [Leaderboards](https://github.com/leonard-thieu/toofz-necrodancer-leaderboards) [![Build status](https://ci.appveyor.com/api/projects/status/fhfu870220jgfm3l/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-leaderboards/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/toofz-necrodancer-leaderboards/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/toofz-necrodancer-leaderboards?branch=master)

Provides types for working with Crypt of the NecroDancer leaderboards.

### Crypt of the NecroDancer

#### [NecroDancer](https://github.com/leonard-thieu/toofz-necrodancer-core)

Provides types for working with Crypt of the NecroDancer game data.

#### [NecroDancer (Entity Framework)](https://github.com/leonard-thieu/toofz-necrodancer-entityframework)

Provides a context for working with types from [toofz.NecroDancer](https://github.com/leonard-thieu/toofz-necrodancer-core) in a database.

### Utility and tools

#### [SqlBulkUpsert](https://github.com/leonard-thieu/SqlBulkUpsert) [![Build status](https://ci.appveyor.com/api/projects/status/q0r7259k9i1pky06/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/sqlbulkupsert/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/SqlBulkUpsert/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/SqlBulkUpsert?branch=master)

Provides bulk upserting functionality. Used by leaderboard services for performance. Forked from https://github.com/dezfowler/SqlBulkUpsert.

#### [Image Manager](https://github.com/leonard-thieu/toofz-necrodancer-imagemanager)

Prepares and uploads image data from Crypt of the NecroDancer for use on the toofz website.

#### [Data Loader](https://github.com/leonard-thieu/toofz-necrodancer-loaddata)

Parses and loads Crypt of the NecroDancer item and enemy data into a database.

#### [toofz](https://github.com/leonard-thieu/toofz)

Utility library.
