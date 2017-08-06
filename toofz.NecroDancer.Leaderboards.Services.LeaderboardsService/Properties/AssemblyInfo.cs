using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("toofz Leaderboards Service")]
[assembly: AssemblyDescription("Provides Steam leaderboard data.")]
[assembly: AssemblyCompany("toofz")]
[assembly: AssemblyProduct("toofz Leaderboards Service")]
[assembly: AssemblyCopyright("Copyright © Leonard Thieu 2015")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.1.*")]

[assembly: InternalsVisibleTo("toofz.NecroDancer.Leaderboards.Services.LeaderboardsService.Tests")]

[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "log.config")]
