namespace toofz.NecroDancer
{
    public static class GameContext
    {
        private const string NecroDancerInstall32 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 247080";
        private const string NecroDancerInstall64 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 247080";

        public static string GetFolderPath()
        {
            return Reg.HKLM(NecroDancerInstall32).GetValue<string>("InstallLocation") ??
                   Reg.HKLM(NecroDancerInstall64).GetValue<string>("InstallLocation");
        }
    }
}
