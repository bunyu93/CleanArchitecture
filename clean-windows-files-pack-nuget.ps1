<#
.SYNOPSIS
    Removes macOS- and Windows-specific files, packs the template NuGet package and validates its contents.

.DESCRIPTION
    PowerShell equivalent of clean-macos-files-pack-nuget.sh, extended with Windows junk files.
    After packing, the .nupkg is inspected to make sure no solution files (.sln/.slnx)
    or macOS/Windows junk files ended up in the package. The script exits with a non-zero code
    if cleaning, packing or validation fails.

.EXAMPLE
    ./clean-windows-files-pack-nuget.ps1

.EXAMPLE
    ./clean-windows-files-pack-nuget.ps1 -Configuration Release -OutputDirectory ./artifacts
#>
[CmdletBinding()]
param(
    [string]$Configuration = 'Release',
    [string]$OutputDirectory
)

$ErrorActionPreference = 'Stop'

$root = $PSScriptRoot
$project = Join-Path $root 'Templates.csproj'

# Remove all macOS- and Windows-specific files and directories
# (keep in sync with clean-macos-files-pack-nuget.sh)
$junkNames = @(
    # macOS
    '.DS_Store', '._*', '.AppleDouble', '__MACOSX', '.Spotlight-V100', '.Trashes', '.fseventsd',
    '.TemporaryItems', '.VolumeIcon.icns', '.LSOverride',
    # Windows
    'Thumbs.db', 'ehthumbs.db', 'ehthumbs_vista.db', 'desktop.ini', '*.stackdump', '$RECYCLE.BIN'
)

Write-Host 'Removing macOS- and Windows-specific files...' -ForegroundColor Cyan

$items = Get-ChildItem -Path $root -Recurse -Force -ErrorAction SilentlyContinue |
    Where-Object { $_.FullName -notmatch '[\\/]\.git([\\/]|$)' }

$removed = 0
foreach ($item in $items) {
    $isMatch = @($junkNames | Where-Object { $item.Name -like $_ }).Count -gt 0

    if ($isMatch -and (Test-Path -LiteralPath $item.FullName)) {
        Remove-Item -LiteralPath $item.FullName -Recurse -Force
        Write-Host "  removed $($item.FullName.Substring($root.Length + 1))"
        $removed++
    }
}
Write-Host "Removed $removed item(s)." -ForegroundColor Green

# Pack
if (-not $OutputDirectory) {
    $OutputDirectory = Join-Path $root "bin\$Configuration"
}

Write-Host "Packing ($Configuration)..." -ForegroundColor Cyan
dotnet pack $project -c $Configuration -o $OutputDirectory
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet pack failed with exit code $LASTEXITCODE."
    exit $LASTEXITCODE
}

# Validate package contents
$packageId = (dotnet msbuild $project -getProperty:PackageId).Trim()
$packageVersion = (dotnet msbuild $project -getProperty:PackageVersion).Trim()
$package = Join-Path $OutputDirectory "$packageId.$packageVersion.nupkg"

if (-not (Test-Path -LiteralPath $package)) {
    Write-Error "Package not found: $package"
    exit 1
}

Write-Host "Validating $package..." -ForegroundColor Cyan

# Matched case-insensitively (keep in sync with clean-macos-files-pack-nuget.sh)
$forbiddenPatterns = @(
    '\.slnx?$',
    '(^|/)\.DS_Store$',
    '(^|/)\._[^/]*$',
    '(^|/)(\.AppleDouble|__MACOSX|\.Spotlight-V100|\.fseventsd|\.Trashes|\.TemporaryItems)(/|$)',
    '(^|/)(\.VolumeIcon\.icns|\.LSOverride)$',
    '(^|/)(Thumbs\.db|ehthumbs\.db|ehthumbs_vista\.db|desktop\.ini)$',
    '\.stackdump$',
    '(^|/)\$RECYCLE\.BIN/',
    '(^|/)(bin|obj|\.vs|\.idea|\.claude)/',
    '\.user$',
    '(^|/)\.env$'
)

Add-Type -AssemblyName System.IO.Compression.FileSystem
$zip = [System.IO.Compression.ZipFile]::OpenRead((Resolve-Path -LiteralPath $package))
try {
    $entries = $zip.Entries | ForEach-Object { $_.FullName }
} finally {
    $zip.Dispose()
}

$violations = $entries | Where-Object {
    $entry = $_
    @($forbiddenPatterns | Where-Object { $entry -match $_ }).Count -gt 0
}

if ($violations) {
    Write-Host 'Package contains files that should be excluded:' -ForegroundColor Red
    $violations | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
    exit 1
}

Write-Host "Package is valid ($($entries.Count) entries): $package" -ForegroundColor Green

# command to run this script

# ./clean-windows-files-pack-nuget.ps1
# If script execution is blocked:
# powershell -ExecutionPolicy Bypass -File ./clean-windows-files-pack-nuget.ps1
