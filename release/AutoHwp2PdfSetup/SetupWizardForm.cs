using System.Diagnostics;

namespace AutoHwp2PdfSetup;

internal sealed class SetupWizardForm : Form
{
    private readonly Label _titleLabel;
    private readonly Label _bodyLabel;
    private readonly Label _pageTitleLabel;
    private readonly Label _pageBodyLabel;
    private GroupBox _languageGroup = null!;
    private RadioButton _koreanRadioButton = null!;
    private RadioButton _englishRadioButton = null!;
    private Label _languageHintLabel = null!;
    private Label _installPathLabel = null!;
    private TextBox _installPathTextBox = null!;
    private Button _browseButton = null!;
    private CheckBox _desktopShortcutCheckBox = null!;
    private Label _startMenuHintLabel = null!;
    private ProgressBar _progressBar = null!;
    private Label _progressStatusLabel = null!;
    private CheckBox _runNowCheckBox = null!;
    private readonly Button _backButton;
    private readonly Button _nextButton;
    private readonly Button _cancelButton;
    private readonly Panel _welcomePanel;
    private readonly Panel _optionsPanel;
    private readonly Panel _progressPanel;
    private readonly Panel _finishPanel;
    private readonly IReadOnlyList<Panel> _pages;

    private InstallerLanguage _language;
    private int _currentPageIndex;
    private InstallResult? _installResult;

    public SetupWizardForm(CommandLineOptions options)
    {
        _language = Localization.DetectPreferredLanguage();

        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(241, 244, 248);
        ClientSize = new Size(760, 560);
        Font = new Font("Segoe UI", 9.5f);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Padding = new Padding(0);
        StartPosition = FormStartPosition.CenterScreen;

        var rootPanel = new Panel
        {
            BackColor = BackColor,
            Dock = DockStyle.Fill
        };

        var headerPanel = new Panel
        {
            BackColor = Color.FromArgb(24, 66, 109),
            Dock = DockStyle.Top,
            Height = 126,
            Padding = new Padding(30, 22, 30, 18)
        };

        _titleLabel = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 18f, FontStyle.Bold),
            ForeColor = Color.White,
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _bodyLabel = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10f),
            ForeColor = Color.FromArgb(226, 235, 245),
            Padding = new Padding(0, 8, 0, 0)
        };

        headerPanel.Controls.Add(_bodyLabel);
        headerPanel.Controls.Add(_titleLabel);

        var footerPanel = new Panel
        {
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Bottom,
            Height = 76,
            Padding = new Padding(24, 16, 24, 16)
        };

        _backButton = CreateFooterButton(94, 36);
        _backButton.Click += (_, _) => ShowPage(_currentPageIndex - 1);

        _nextButton = CreateFooterButton(116, 36);
        _nextButton.BackColor = Color.FromArgb(24, 66, 109);
        _nextButton.FlatAppearance.BorderColor = Color.FromArgb(24, 66, 109);
        _nextButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 50, 84);
        _nextButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 82, 130);
        _nextButton.ForeColor = Color.White;
        _nextButton.Click += NextButton_Click;

        _cancelButton = CreateFooterButton(94, 36);
        _cancelButton.Click += (_, _) => Close();

        footerPanel.Controls.AddRange([_cancelButton, _nextButton, _backButton]);
        footerPanel.Resize += (_, _) => LayoutFooterButtons(footerPanel);

        var shellPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(26, 24, 26, 20)
        };

        var cardPanel = new Panel
        {
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Fill,
            Padding = new Padding(28, 24, 28, 24)
        };

        _pageTitleLabel = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 14f, FontStyle.Bold),
            Height = 36,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _pageBodyLabel = new Label
        {
            AutoSize = false,
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 9.75f),
            ForeColor = Color.FromArgb(77, 77, 77),
            Height = 56,
            Padding = new Padding(0, 4, 0, 4)
        };

        var divider = new Panel
        {
            BackColor = Color.FromArgb(230, 233, 238),
            Dock = DockStyle.Top,
            Height = 1,
            Margin = new Padding(0, 0, 0, 18)
        };

        var pageHostPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(0, 20, 0, 0)
        };

        _welcomePanel = new Panel { Dock = DockStyle.Fill };
        _optionsPanel = new Panel { Dock = DockStyle.Fill };
        _progressPanel = new Panel { Dock = DockStyle.Fill };
        _finishPanel = new Panel { Dock = DockStyle.Fill };
        _pages = [_welcomePanel, _optionsPanel, _progressPanel, _finishPanel];

        ConfigureWelcomePage();
        ConfigureOptionsPage();
        ConfigureProgressPage();
        ConfigureFinishPage();

        pageHostPanel.Controls.Add(_finishPanel);
        pageHostPanel.Controls.Add(_progressPanel);
        pageHostPanel.Controls.Add(_optionsPanel);
        pageHostPanel.Controls.Add(_welcomePanel);

        cardPanel.Controls.Add(pageHostPanel);
        cardPanel.Controls.Add(divider);
        cardPanel.Controls.Add(_pageBodyLabel);
        cardPanel.Controls.Add(_pageTitleLabel);

        shellPanel.Controls.Add(cardPanel);

        rootPanel.Controls.Add(shellPanel);
        rootPanel.Controls.Add(footerPanel);
        rootPanel.Controls.Add(headerPanel);
        Controls.Add(rootPanel);

        AcceptButton = _nextButton;
        CancelButton = _cancelButton;

        _installPathTextBox.Text = options.InstallDirectory ?? InstallerOperations.DefaultInstallDirectory;
        _desktopShortcutCheckBox.Checked = true;
        _koreanRadioButton.Checked = _language == InstallerLanguage.Korean;
        _englishRadioButton.Checked = _language == InstallerLanguage.English;

        ApplyLanguage();
        ShowPage(0);
        LayoutFooterButtons(footerPanel);
    }

    private static Button CreateFooterButton(int width, int height)
    {
        var button = new Button
        {
            BackColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(37, 49, 63),
            Size = new Size(width, height),
            TabStop = true,
            UseVisualStyleBackColor = false
        };
        button.FlatAppearance.BorderColor = Color.FromArgb(201, 208, 217);
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(235, 239, 244);
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(246, 248, 251);
        return button;
    }

    private void ConfigureWelcomePage()
    {
        var pageLayout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 2
        };
        pageLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        pageLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        _languageGroup = new GroupBox
        {
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            Height = 176,
            Padding = new Padding(18, 16, 18, 16)
        };

        var radioLayout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 3
        };
        radioLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42f));
        radioLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42f));
        radioLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        _koreanRadioButton = new RadioButton
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10f, FontStyle.Regular),
            Margin = new Padding(8, 0, 0, 0)
        };
        _koreanRadioButton.CheckedChanged += (_, _) => UpdateLanguageFromSelection();

        _englishRadioButton = new RadioButton
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10f, FontStyle.Regular),
            Margin = new Padding(8, 0, 0, 0)
        };
        _englishRadioButton.CheckedChanged += (_, _) => UpdateLanguageFromSelection();

        _languageHintLabel = new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.FromArgb(88, 96, 107),
            Padding = new Padding(4, 10, 0, 0)
        };

        radioLayout.Controls.Add(_koreanRadioButton, 0, 0);
        radioLayout.Controls.Add(_englishRadioButton, 0, 1);
        radioLayout.Controls.Add(_languageHintLabel, 0, 2);
        _languageGroup.Controls.Add(radioLayout);

        var supportPanel = new Panel
        {
            BackColor = Color.FromArgb(246, 248, 251),
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Top,
            Height = 94,
            Margin = new Padding(0, 16, 0, 0),
            Padding = new Padding(18, 16, 18, 16)
        };

        var supportLabel = new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.FromArgb(77, 77, 77),
            Text = "Setup language and the installed app language will stay in sync."
        };
        supportPanel.Controls.Add(supportLabel);

        pageLayout.Controls.Add(_languageGroup, 0, 0);
        pageLayout.Controls.Add(supportPanel, 0, 1);
        _welcomePanel.Controls.Add(pageLayout);
    }

    private void ConfigureOptionsPage()
    {
        var pageLayout = new TableLayoutPanel
        {
            ColumnCount = 1,
            Dock = DockStyle.Fill,
            RowCount = 2
        };
        pageLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        pageLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        var installCard = new Panel
        {
            BackColor = Color.FromArgb(248, 250, 252),
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Top,
            Height = 134,
            Padding = new Padding(18, 16, 18, 16)
        };

        _installPathLabel = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9.75f, FontStyle.Bold),
            Location = new Point(18, 16)
        };

        _installPathTextBox = new TextBox
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            Location = new Point(18, 46),
            Width = 520
        };

        _browseButton = CreateFooterButton(116, 32);
        _browseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _browseButton.Location = new Point(560, 42);
        _browseButton.Click += BrowseButton_Click;

        _desktopShortcutCheckBox = new CheckBox
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5f),
            Location = new Point(18, 88)
        };

        installCard.Controls.AddRange([_installPathLabel, _installPathTextBox, _browseButton, _desktopShortcutCheckBox]);
        installCard.Resize += (_, _) =>
        {
            _browseButton.Location = new Point(installCard.ClientSize.Width - _browseButton.Width - 18, 42);
            _installPathTextBox.Width = _browseButton.Left - 16 - _installPathTextBox.Left;
        };

        var infoCard = new Panel
        {
            BackColor = Color.FromArgb(246, 248, 251),
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Top,
            Height = 110,
            Margin = new Padding(0, 16, 0, 0),
            Padding = new Padding(18, 16, 18, 16)
        };

        _startMenuHintLabel = new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.FromArgb(77, 77, 77)
        };
        infoCard.Controls.Add(_startMenuHintLabel);

        pageLayout.Controls.Add(installCard, 0, 0);
        pageLayout.Controls.Add(infoCard, 0, 1);
        _optionsPanel.Controls.Add(pageLayout);
    }

    private void ConfigureProgressPage()
    {
        var wrapper = new Panel
        {
            BackColor = Color.FromArgb(248, 250, 252),
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Top,
            Height = 170,
            Padding = new Padding(24, 26, 24, 20)
        };

        _progressBar = new ProgressBar
        {
            Dock = DockStyle.Top,
            Height = 22,
            MarqueeAnimationSpeed = 28,
            Style = ProgressBarStyle.Marquee
        };

        _progressStatusLabel = new Label
        {
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 10f),
            Height = 60,
            Padding = new Padding(0, 18, 0, 0)
        };

        var noteLabel = new Label
        {
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 9f),
            ForeColor = Color.FromArgb(93, 101, 111),
            Height = 46,
            Padding = new Padding(0, 12, 0, 0),
            Text = "Please keep this window open until the installation completes."
        };

        wrapper.Controls.Add(noteLabel);
        wrapper.Controls.Add(_progressStatusLabel);
        wrapper.Controls.Add(_progressBar);
        _progressPanel.Controls.Add(wrapper);
    }

    private void ConfigureFinishPage()
    {
        var wrapper = new Panel
        {
            BackColor = Color.FromArgb(248, 250, 252),
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Top,
            Height = 130,
            Padding = new Padding(20, 20, 20, 20)
        };

        _runNowCheckBox = new CheckBox
        {
            AutoSize = true,
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 9.75f),
            Checked = true
        };

        var doneLabel = new Label
        {
            Dock = DockStyle.Top,
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.FromArgb(77, 77, 77),
            Height = 54,
            Padding = new Padding(0, 12, 0, 0),
            Text = "Shortcuts and uninstall information have been created."
        };

        wrapper.Controls.Add(doneLabel);
        wrapper.Controls.Add(_runNowCheckBox);
        _finishPanel.Controls.Add(wrapper);
    }

    private void LayoutFooterButtons(Control footerPanel)
    {
        var centerY = (footerPanel.ClientSize.Height - _nextButton.Height) / 2;
        _cancelButton.Location = new Point(footerPanel.ClientSize.Width - _cancelButton.Width - 24, centerY);
        _nextButton.Location = new Point(_cancelButton.Left - _nextButton.Width - 12, centerY);
        _backButton.Location = new Point(_nextButton.Left - _backButton.Width - 12, centerY);
    }

    private void BrowseButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = Localization.Get(_language, "SelectFolder"),
            UseDescriptionForTitle = true,
            InitialDirectory = _installPathTextBox.Text
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            _installPathTextBox.Text = dialog.SelectedPath;
        }
    }

    private async void NextButton_Click(object? sender, EventArgs e)
    {
        if (_currentPageIndex == 0)
        {
            ShowPage(1);
            return;
        }

        if (_currentPageIndex == 1)
        {
            if (!ValidateInstallDirectory())
            {
                return;
            }

            if (InstallerOperations.IsAppRunning())
            {
                MessageBox.Show(
                    Localization.Get(_language, "AppRunning"),
                    Localization.Get(_language, "WizardTitle"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await StartInstallAsync();
            return;
        }

        if (_currentPageIndex == 3)
        {
            if (_runNowCheckBox.Checked && _installResult is not null)
            {
                LaunchInstalledApp(_installResult);
            }

            Close();
        }
    }

    private void UpdateLanguageFromSelection()
    {
        var newLanguage = _englishRadioButton.Checked ? InstallerLanguage.English : InstallerLanguage.Korean;
        if (newLanguage == _language)
        {
            return;
        }

        _language = newLanguage;
        ApplyLanguage();
    }

    private void ApplyLanguage()
    {
        Text = Localization.Get(_language, "WizardTitle");
        _titleLabel.Text = Localization.Get(_language, "WizardTitle");
        _bodyLabel.Text = Localization.Get(_language, "WelcomeBody");
        _pageBodyLabel.MaximumSize = new Size(_pageBodyLabel.Width, 0);
        _languageGroup.Text = Localization.Get(_language, "LanguageGroup");
        _koreanRadioButton.Text = "\uD55C\uAD6D\uC5B4";
        _englishRadioButton.Text = "English";
        _languageHintLabel.Text = Localization.Get(_language, "LanguageHint");
        _installPathLabel.Text = Localization.Get(_language, "InstallPath");
        _browseButton.Text = Localization.Get(_language, "Browse");
        _desktopShortcutCheckBox.Text = Localization.Get(_language, "DesktopShortcut");
        _startMenuHintLabel.Text = Localization.Get(_language, "StartMenuShortcut");
        _runNowCheckBox.Text = Localization.Get(_language, "RunNow");
        _backButton.Text = Localization.Get(_language, "Back");
        _cancelButton.Text = _currentPageIndex == 3 ? Localization.Get(_language, "Close") : Localization.Get(_language, "Cancel");
        UpdatePageCopy();
    }

    private void ShowPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= _pages.Count)
        {
            return;
        }

        _currentPageIndex = pageIndex;
        for (var index = 0; index < _pages.Count; index++)
        {
            _pages[index].Visible = index == pageIndex;
        }

        _backButton.Enabled = pageIndex > 0 && pageIndex < 2;
        _cancelButton.Enabled = pageIndex != 2;
        UpdatePageCopy();
    }

    private void UpdatePageCopy()
    {
        switch (_currentPageIndex)
        {
            case 0:
                _pageTitleLabel.Text = Localization.Get(_language, "WelcomeTitle");
                _pageBodyLabel.Text = Localization.Get(_language, "WelcomeBody");
                _nextButton.Text = Localization.Get(_language, "Next");
                break;
            case 1:
                _pageTitleLabel.Text = Localization.Get(_language, "InstallTitle");
                _pageBodyLabel.Text = string.Join(Environment.NewLine,
                    Localization.Get(_language, "InstallBody"),
                    Localization.Get(_language, "AlreadyInstalledPrompt"));
                _nextButton.Text = Localization.Get(_language, "Install");
                break;
            case 2:
                _pageTitleLabel.Text = Localization.Get(_language, "ProgressTitle");
                _pageBodyLabel.Text = Localization.Get(_language, "ProgressBody");
                _nextButton.Text = Localization.Get(_language, "Installing");
                break;
            case 3:
                _pageTitleLabel.Text = Localization.Get(_language, "FinishTitle");
                _pageBodyLabel.Text = Localization.Get(_language, "FinishBody");
                _nextButton.Text = Localization.Get(_language, "Finish");
                break;
        }

        _nextButton.Enabled = _currentPageIndex != 2;
        _cancelButton.Text = _currentPageIndex == 3 ? Localization.Get(_language, "Close") : Localization.Get(_language, "Cancel");
    }

    private bool ValidateInstallDirectory()
    {
        var installDirectory = _installPathTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(installDirectory))
        {
            MessageBox.Show(
                Localization.Get(_language, "InvalidInstallPath"),
                Localization.Get(_language, "WizardTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        try
        {
            _installPathTextBox.Text = Path.GetFullPath(installDirectory);
            return true;
        }
        catch
        {
            MessageBox.Show(
                Localization.Get(_language, "InvalidInstallPath"),
                Localization.Get(_language, "WizardTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }
    }

    private async Task StartInstallAsync()
    {
        ShowPage(2);
        _progressStatusLabel.Text = Localization.Get(_language, "PreparingFiles");

        var request = new InstallRequest(
            AppContext.BaseDirectory,
            _installPathTextBox.Text.Trim(),
            _language,
            _desktopShortcutCheckBox.Checked);

        var progress = new Progress<string>(message => _progressStatusLabel.Text = message);

        try
        {
            _installResult = await InstallerOperations.InstallAsync(request, progress, CancellationToken.None);
            _progressStatusLabel.Text = Localization.Get(_language, "InstallCompletedStatus");
            ShowPage(3);
        }
        catch (DirectoryNotFoundException)
        {
            MessageBox.Show(
                Localization.Get(_language, "PayloadMissing"),
                Localization.Get(_language, "WizardTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            ShowPage(1);
        }
        catch (Exception exception)
        {
            MessageBox.Show(
                $"{Localization.Get(_language, "InstallFailed")}{Environment.NewLine}{Environment.NewLine}{exception.Message}",
                Localization.Get(_language, "WizardTitle"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            ShowPage(1);
        }
    }

    private static void LaunchInstalledApp(InstallResult installResult)
    {
        var startInfo = new ProcessStartInfo(
            Path.Combine(Environment.SystemDirectory, "wscript.exe"),
            $"\"{installResult.LauncherScriptPath}\"")
        {
            UseShellExecute = true,
            WorkingDirectory = installResult.InstallDirectory
        };

        Process.Start(startInfo);
    }
}
