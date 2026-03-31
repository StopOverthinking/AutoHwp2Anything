param()

$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$setupProjectPath = Join-Path $PSScriptRoot "AutoHwp2PdfSetup\AutoHwp2PdfSetup.csproj"
$setupProjectDirectory = Split-Path -Parent $setupProjectPath
$payloadDirectory = Join-Path $PSScriptRoot "payload"
$publishDirectory = Join-Path $PSScriptRoot "publish"
$setupExePath = Join-Path $PSScriptRoot "AutoHwp2PdfSetup.exe"
$packageZipPath = Join-Path $PSScriptRoot "AutoHwp2PdfSetup-package.zip"

$payloadFiles = @(
    "AutoHwp2Pdf.deps.json",
    "AutoHwp2Pdf.dll",
    "AutoHwp2Pdf.exe",
    "AutoHwp2Pdf.pdb",
    "AutoHwp2Pdf.runtimeconfig.json",
    "FilePathCheckerModuleExample.dll",
    "Launch-AutoHwp2Pdf.cmd",
    "Launch-AutoHwp2Pdf.ps1"
)

if (Test-Path $payloadDirectory) {
    Remove-Item -LiteralPath $payloadDirectory -Recurse -Force
}

New-Item -ItemType Directory -Path $payloadDirectory -Force | Out-Null

foreach ($fileName in $payloadFiles) {
    $sourcePath = Join-Path $projectRoot $fileName
    if (-not (Test-Path $sourcePath)) {
        throw "Payload source file not found: $sourcePath"
    }

    Copy-Item -LiteralPath $sourcePath -Destination (Join-Path $payloadDirectory $fileName) -Force
}

if (Test-Path $publishDirectory) {
    Remove-Item -LiteralPath $publishDirectory -Recurse -Force
}

dotnet publish $setupProjectPath `
    -c Release `
    -r win-x64 `
    --self-contained true `
    /p:PublishSingleFile=true `
    /p:IncludeNativeLibrariesForSelfExtract=true `
    -o $publishDirectory

Copy-Item -LiteralPath (Join-Path $publishDirectory "AutoHwp2PdfSetup.exe") -Destination $setupExePath -Force

if (Test-Path $packageZipPath) {
    Remove-Item -LiteralPath $packageZipPath -Force
}

$itemsToPackage = @(
    $setupExePath,
    $payloadDirectory
)

Compress-Archive -Path $itemsToPackage -DestinationPath $packageZipPath -Force

Remove-Item -LiteralPath $publishDirectory -Recurse -Force

$transientDirectories = @(
    (Join-Path $setupProjectDirectory "bin"),
    (Join-Path $setupProjectDirectory "obj")
)

foreach ($directory in $transientDirectories) {
    if (Test-Path $directory) {
        Remove-Item -LiteralPath $directory -Recurse -Force
    }
}

Write-Host "Installer published:"
Write-Host "  $setupExePath"
Write-Host "Payload directory:"
Write-Host "  $payloadDirectory"
Write-Host "Package zip:"
Write-Host "  $packageZipPath"
