namespace AutoHwp2PdfSetup;

internal enum InstallerLanguage
{
    Korean,
    English
}

internal readonly record struct Translation(string Korean, string English);

internal static class Localization
{
    private static readonly Dictionary<string, Translation> Strings = new(StringComparer.Ordinal)
    {
        ["WizardTitle"] = new("AutoHwp2Pdf 설치 마법사", "AutoHwp2Pdf Setup Wizard"),
        ["WelcomeTitle"] = new("설치를 시작합니다", "Let's install AutoHwp2Pdf"),
        ["WelcomeBody"] = new(
            "설치에 사용할 언어를 선택한 뒤 계속 진행하세요. 여기서 선택한 언어는 프로그램 설치 후 기본 언어로 적용됩니다.",
            "Choose the language for setup and continue. The selected language will become the app's default language after installation."),
        ["LanguageGroup"] = new("설치 언어", "Setup language"),
        ["LanguageHint"] = new("이 선택은 설치 마법사와 설치 후 기본 앱 언어에 모두 반영됩니다.", "This choice applies to both the installer and the app's default language."),
        ["InstallTitle"] = new("설치 위치를 확인하세요", "Choose the installation location"),
        ["InstallBody"] = new(
            "사용자별 프로그램 폴더에 설치하면 설정 파일을 안전하게 저장할 수 있습니다.",
            "Installing to a per-user program folder lets the app save its settings safely."),
        ["InstallPath"] = new("설치 폴더", "Install folder"),
        ["Browse"] = new("찾아보기...", "Browse..."),
        ["DesktopShortcut"] = new("바탕 화면 바로가기 만들기", "Create a desktop shortcut"),
        ["StartMenuShortcut"] = new("시작 메뉴 바로가기는 자동으로 생성됩니다.", "A Start menu shortcut will be created automatically."),
        ["ProgressTitle"] = new("설치 중입니다", "Installing"),
        ["ProgressBody"] = new("파일을 복사하고 설정을 준비하고 있습니다.", "Copying files and preparing settings."),
        ["FinishTitle"] = new("설치가 완료되었습니다", "Installation is complete"),
        ["FinishBody"] = new("AutoHwp2Pdf를 바로 실행할 수 있습니다.", "AutoHwp2Pdf is ready to launch."),
        ["RunNow"] = new("설치가 끝나면 AutoHwp2Pdf 실행", "Launch AutoHwp2Pdf when setup closes"),
        ["Back"] = new("이전", "Back"),
        ["Next"] = new("다음", "Next"),
        ["Install"] = new("설치", "Install"),
        ["Installing"] = new("설치 중...", "Installing..."),
        ["Finish"] = new("마침", "Finish"),
        ["Cancel"] = new("취소", "Cancel"),
        ["Close"] = new("닫기", "Close"),
        ["SelectFolder"] = new("설치 폴더 선택", "Select installation folder"),
        ["PayloadMissing"] = new("설치에 필요한 payload 폴더를 찾을 수 없습니다.", "The payload folder required for installation could not be found."),
        ["InstallFailed"] = new("설치 중 오류가 발생했습니다.", "Setup encountered an error."),
        ["InstallCompletedStatus"] = new("설치가 완료되었습니다.", "Installation completed successfully."),
        ["PreparingFiles"] = new("배포 파일을 확인하는 중...", "Checking payload files..."),
        ["CopyingFiles"] = new("프로그램 파일을 복사하는 중...", "Copying application files..."),
        ["WritingSettings"] = new("설정 파일을 작성하는 중...", "Writing the settings file..."),
        ["CreatingShortcuts"] = new("바로가기를 만드는 중...", "Creating shortcuts..."),
        ["RegisteringUninstall"] = new("제거 항목을 등록하는 중...", "Registering uninstall information..."),
        ["Finalizing"] = new("설치를 마무리하는 중...", "Finalizing setup..."),
        ["InvalidInstallPath"] = new("설치 폴더를 확인해 주세요.", "Please choose a valid installation folder."),
        ["AppRunning"] = new("AutoHwp2Pdf가 실행 중입니다. 설치를 계속하려면 먼저 프로그램을 종료해 주세요.", "AutoHwp2Pdf is currently running. Please close it before continuing."),
        ["UninstallConfirm"] = new("AutoHwp2Pdf를 제거할까요?", "Do you want to uninstall AutoHwp2Pdf?"),
        ["UninstallTitle"] = new("AutoHwp2Pdf 제거", "Uninstall AutoHwp2Pdf"),
        ["UninstallComplete"] = new("제거가 시작되었습니다. 잠시 후 프로그램 폴더가 정리됩니다.", "Uninstallation has started. The program folder will be cleaned up shortly."),
        ["UninstallNotFound"] = new("설치 폴더를 찾을 수 없습니다.", "The installation folder could not be found."),
        ["UninstallFailed"] = new("제거 중 오류가 발생했습니다.", "Uninstallation failed."),
        ["AlreadyInstalledPrompt"] = new("같은 위치에 설치된 파일이 있으면 덮어씁니다.", "Existing files in the same location will be overwritten."),
        ["UninstallShortcutName"] = new("AutoHwp2Pdf 제거", "Uninstall AutoHwp2Pdf")
    };

    public static InstallerLanguage DetectPreferredLanguage()
    {
        return System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("ko", StringComparison.OrdinalIgnoreCase)
            ? InstallerLanguage.Korean
            : InstallerLanguage.English;
    }

    public static string Get(InstallerLanguage language, string key)
    {
        if (!Strings.TryGetValue(key, out var translation))
        {
            return key;
        }

        return language == InstallerLanguage.Korean ? translation.Korean : translation.English;
    }
}
