using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("toofz Leaderboard Service")]
[assembly: AssemblyDescription("Provides Steam leaderboard data.")]
[assembly: AssemblyCompany("toofz")]
[assembly: AssemblyProduct("toofz Leaderboard Service")]
[assembly: AssemblyCopyright("Copyright © Leonard Thieu 2015")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.1.*")]

[assembly: InternalsVisibleTo("toofz.NecroDancer.Leaderboards.Services.LeaderboardUpdate.Tests")]

[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "leaderboard-log.config")]
