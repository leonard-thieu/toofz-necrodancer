# toofz

## Project structure

### Web

#### [toofz](https://github.com/leonard-thieu/toofz-necrodancer-webclient) [![Build status](https://ci.appveyor.com/api/projects/status/83e8eikypiri2lhi/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-webclient/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/toofz-necrodancer-webclient/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/toofz-necrodancer-webclient?branch=master)

Source for http://crypt.toofz.com/.

#### [toofz API](https://github.com/leonard-thieu/toofz-necrodancer-web-api) [![Build status](https://ci.appveyor.com/api/projects/status/2en9f6hcf72ujm9y/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-web-api/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/toofz-necrodancer-web-api/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/toofz-necrodancer-web-api?branch=master)

Source for [https://api.toofz.com/](https://api.toofz.com/help).

### Leaderboard Services

#### [Leaderboards Service](https://github.com/leonard-thieu/leaderboards-service) [![Build status](https://ci.appveyor.com/api/projects/status/77fd6okl8bc2ulkb/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/leaderboards-service/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/leaderboards-service/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/leaderboards-service?branch=master)

Service that updates leaderboard data.

#### [Players Service](https://github.com/leonard-thieu/players-service) [![Build status](https://ci.appveyor.com/api/projects/status/3udoy27b6tetostp/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/players-service/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/players-service/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/players-service?branch=master)

Service that updates player data.

#### [Replays Service](https://github.com/leonard-thieu/replays-service) [![Build status](https://ci.appveyor.com/api/projects/status/xeoko709p63qf3jb/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/replays-service/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/replays-service/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/replays-service?branch=master)

Service that updates replay data.

#### [Services Core](https://github.com/leonard-thieu/toofz-necrodancer-leaderboards-services-common) [![Build status](https://ci.appveyor.com/api/projects/status/ra5o1lcdc1hh3e29?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-leaderboards-services-common)

Common code for services.

#### [Leaderboards Core](https://github.com/leonard-thieu/toofz-necrodancer-leaderboards) [![Build status](https://ci.appveyor.com/api/projects/status/fhfu870220jgfm3l/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-leaderboards/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/toofz-necrodancer-leaderboards/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/toofz-necrodancer-leaderboards?branch=master)

Provides types for working with **Crypt of the NecroDancer** leaderboards.

### Crypt of the NecroDancer

#### [NecroDancer Core](https://github.com/leonard-thieu/toofz-necrodancer-core) [![Build status](https://ci.appveyor.com/api/projects/status/de1vj801al1krlfa/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-core/branch/master)

Provides types for working with **Crypt of the NecroDancer** game data.

#### [NecroDancer Core (Entity Framework)](https://github.com/leonard-thieu/toofz-necrodancer-entityframework) [![Build status](https://ci.appveyor.com/api/projects/status/cowbksjnikl2928m/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-entityframework/branch/master)

Provides a context for working with types from [NecroDancer Core](https://github.com/leonard-thieu/toofz-necrodancer-core) in a database.

### Utility and tools

#### [SqlBulkUpsert](https://github.com/leonard-thieu/SqlBulkUpsert) [![Build status](https://ci.appveyor.com/api/projects/status/q0r7259k9i1pky06/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/sqlbulkupsert/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/SqlBulkUpsert/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/SqlBulkUpsert?branch=master)

Provides bulk upserting functionality. Used by **Leaderboard Services** for performance. Forked from https://github.com/dezfowler/SqlBulkUpsert.

#### [Image Manager](https://github.com/leonard-thieu/toofz-necrodancer-imagemanager) [![Build status](https://ci.appveyor.com/api/projects/status/7o5ymk33junl322j/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-imagemanager/branch/master)

Prepares and uploads image data from **Crypt of the NecroDancer** for use on [toofz](https://github.com/leonard-thieu/toofz-necrodancer-webclient).

#### [Data Loader](https://github.com/leonard-thieu/toofz-necrodancer-loaddata) [![Build status](https://ci.appveyor.com/api/projects/status/gpnh3cbvi2224wyh/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-loaddata/branch/master)

Parses and loads **Crypt of the NecroDancer** item and enemy data into a database.

#### [toofz Utilities](https://github.com/leonard-thieu/toofz) [![Build status](https://ci.appveyor.com/api/projects/status/b2w3cuq05d3udp00/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/toofz/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/toofz?branch=master)

Utility library.

#### [toofz Utilities for Tests](https://github.com/leonard-thieu/toofz-testsshared) [![Build status](https://ci.appveyor.com/api/projects/status/5mrvq3c9shjkisgs/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-testsshared/branch/master) [![Coverage Status](https://coveralls.io/repos/github/leonard-thieu/toofz-testsshared/badge.svg?branch=master)](https://coveralls.io/github/leonard-thieu/toofz-testsshared?branch=master)

Utility library for test projects.
