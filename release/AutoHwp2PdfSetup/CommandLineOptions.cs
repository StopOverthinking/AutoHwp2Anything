namespace AutoHwp2PdfSetup;

internal sealed class CommandLineOptions
{
    public bool UninstallMode { get; private init; }

    public string? InstallDirectory { get; private init; }

    public static CommandLineOptions Parse(string[] args)
    {
        var uninstallMode = false;
        string? installDirectory = null;

        for (var index = 0; index < args.Length; index++)
        {
            var argument = args[index];
            if (argument.Equals("--uninstall", StringComparison.OrdinalIgnoreCase))
            {
                uninstallMode = true;
                continue;
            }

            if (argument.Equals("--install-dir", StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
            {
                installDirectory = args[++index];
            }
        }

        return new CommandLineOptions
        {
            UninstallMode = uninstallMode,
            InstallDirectory = string.IsNullOrWhiteSpace(installDirectory) ? null : installDirectory
        };
    }
}
