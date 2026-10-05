#!/usr/bin/env bash
# Removes macOS- and Windows-specific files, packs the template NuGet package and validates its contents.
# Bash equivalent of clean-windows-files-pack-nuget.ps1 (keep both scripts in sync).
#
# After packing, the .nupkg is inspected to make sure no solution files (.sln/.slnx)
# or macOS/Windows junk files ended up in the package. The script exits with a non-zero
# code if cleaning, packing or validation fails.
#
# Usage: ./clean-macos-files-pack-nuget.sh [configuration] [output-directory]
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT="$ROOT/Templates.csproj"
CONFIGURATION="${1:-Release}"
OUTPUT_DIRECTORY="${2:-$ROOT/bin/$CONFIGURATION}"

# Remove all macOS- and Windows-specific files and directories
JUNK_NAMES=(
    # macOS
    '.DS_Store' '._*' '.AppleDouble' '__MACOSX' '.Spotlight-V100' '.Trashes' '.fseventsd'
    '.TemporaryItems' '.VolumeIcon.icns' '.LSOverride'
    # Windows
    'Thumbs.db' 'ehthumbs.db' 'ehthumbs_vista.db' 'desktop.ini' '*.stackdump' '$RECYCLE.BIN'
)

echo "Removing macOS- and Windows-specific files..."

NAME_ARGS=()
for name in "${JUNK_NAMES[@]}"; do
    if [ "${#NAME_ARGS[@]}" -gt 0 ]; then
        NAME_ARGS+=(-o)
    fi
    NAME_ARGS+=(-iname "$name")
done

removed=0
while IFS= read -r -d '' path; do
    rm -rf "$path"
    echo "  removed ${path#"$ROOT"/}"
    removed=$((removed + 1))
done < <(find "$ROOT" -path "$ROOT/.git" -prune -o \( "${NAME_ARGS[@]}" \) -prune -print0)
echo "Removed $removed item(s)."

# Pack
echo "Packing ($CONFIGURATION)..."
if ! dotnet pack "$PROJECT" -c "$CONFIGURATION" -o "$OUTPUT_DIRECTORY"; then
    echo "dotnet pack failed." >&2
    exit 1
fi

# Validate package contents
PACKAGE_ID="$(dotnet msbuild "$PROJECT" -getProperty:PackageId | tr -d '[:space:]')"
PACKAGE_VERSION="$(dotnet msbuild "$PROJECT" -getProperty:PackageVersion | tr -d '[:space:]')"
PACKAGE="$OUTPUT_DIRECTORY/$PACKAGE_ID.$PACKAGE_VERSION.nupkg"

if [ ! -f "$PACKAGE" ]; then
    echo "Package not found: $PACKAGE" >&2
    exit 1
fi

echo "Validating $PACKAGE..."

# Matched case-insensitively (keep in sync with clean-windows-files-pack-nuget.ps1)
FORBIDDEN_PATTERNS=(
    '\.slnx?$'
    '(^|/)\.DS_Store$'
    '(^|/)\._[^/]*$'
    '(^|/)(\.AppleDouble|__MACOSX|\.Spotlight-V100|\.fseventsd|\.Trashes|\.TemporaryItems)(/|$)'
    '(^|/)(\.VolumeIcon\.icns|\.LSOverride)$'
    '(^|/)(Thumbs\.db|ehthumbs\.db|ehthumbs_vista\.db|desktop\.ini)$'
    '\.stackdump$'
    '(^|/)\$RECYCLE\.BIN/'
    '(^|/)(bin|obj|\.vs|\.idea|\.claude)/'
    '\.user$'
    '(^|/)\.env$'
)

GREP_ARGS=()
for pattern in "${FORBIDDEN_PATTERNS[@]}"; do
    GREP_ARGS+=(-e "$pattern")
done

ENTRIES="$(unzip -Z1 "$PACKAGE")"
VIOLATIONS="$(printf '%s\n' "$ENTRIES" | grep -Ei "${GREP_ARGS[@]}" || true)"

if [ -n "$VIOLATIONS" ]; then
    echo "Package contains files that should be excluded:" >&2
    printf '%s\n' "$VIOLATIONS" | sed 's/^/  /' >&2
    exit 1
fi

ENTRY_COUNT="$(printf '%s\n' "$ENTRIES" | wc -l | tr -d ' ')"
echo "Package is valid ($ENTRY_COUNT entries): $PACKAGE"

# command to run this script

# chmod +x clean-macos-files-pack-nuget.sh
# ./clean-macos-files-pack-nuget.sh
