using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("toofz Replays Service")]
[assembly: AssemblyDescription("Provides Steam replay data.")]
[assembly: AssemblyCompany("toofz")]
[assembly: AssemblyProduct("toofz Replays Service")]
[assembly: AssemblyCopyright("Copyright © Leonard Thieu 2015")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.*")]

[assembly: InternalsVisibleTo("toofz.NecroDancer.Leaderboards.Services.ReplaysService.Tests")]

[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "log.config")]
