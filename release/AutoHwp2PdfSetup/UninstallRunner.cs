namespace AutoHwp2PdfSetup;

internal static class UninstallRunner
{
    public static void Run(CommandLineOptions options)
    {
        var installDirectory = options.InstallDirectory
            ?? Path.GetDirectoryName(Environment.ProcessPath)
            ?? InstallerOperations.DefaultInstallDirectory;

        var language = Directory.Exists(installDirectory)
            ? InstallerOperations.DetectInstalledLanguage(installDirectory)
            : Localization.DetectPreferredLanguage();

        if (!Directory.Exists(installDirectory))
        {
            MessageBox.Show(
                Localization.Get(language, "UninstallNotFound"),
                Localization.Get(language, "UninstallTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        if (InstallerOperations.IsAppRunning())
        {
            MessageBox.Show(
                Localization.Get(language, "AppRunning"),
                Localization.Get(language, "UninstallTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show(
            Localization.Get(language, "UninstallConfirm"),
            Localization.Get(language, "UninstallTitle"),
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
        {
            return;
        }

        try
        {
            InstallerOperations.Uninstall(installDirectory);
            MessageBox.Show(
                Localization.Get(language, "UninstallComplete"),
                Localization.Get(language, "UninstallTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                $"{Localization.Get(language, "UninstallFailed")}{Environment.NewLine}{Environment.NewLine}{exception.Message}",
                Localization.Get(language, "UninstallTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
