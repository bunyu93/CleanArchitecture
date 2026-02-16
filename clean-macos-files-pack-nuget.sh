# Remove all macOS-specific files
find . -name ".DS_Store" -delete
find . -name "._*" -delete
find . -name ".AppleDouble" -type d -exec rm -rf {} +
find . -name "__MACOSX" -type d -exec rm -rf {} +
find . -name ".Spotlight-V100" -type d -exec rm -rf {} +
find . -name ".Trashes" -delete
find . -name ".fseventsd" -type d -exec rm -rf {} +
find . -name ".TemporaryItems" -delete
find . -name ".VolumeIcon.icns" -delete
find . -name ".LSOverride" -delete

dotnet pack -c Release