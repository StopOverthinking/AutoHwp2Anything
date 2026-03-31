namespace AutoHwp2PdfSetup;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        var options = CommandLineOptions.Parse(args);
        if (options.UninstallMode)
        {
            UninstallRunner.Run(options);
            return;
        }

        Application.Run(new SetupWizardForm(options));
    }
}
