# PowerShell script to update NuGet packages in all project files
# This script finds all .csproj and .fsproj files and updates stable (non-prerelease) packages

$regex = 'PackageReference Include="([^"]*)" Version="([^"]*)"'

Get-ChildItem -Path . -Recurse -Include "*.csproj", "*.fsproj" | ForEach-Object {
    $projFile = $_.FullName
    Write-Host "Processing project: $projFile"
    $content = Get-Content $projFile
    foreach ($line in $content) {
        if ($line -match $regex) {
            $packageName = $matches[1]
            $version = $matches[2]
            Write-Host "Found package: $packageName, version: $version"

            # Only update stable versions (not prerelease versions containing -)
            if ($version -notmatch '-') {
                Write-Host "Updating package: $packageName"
                dotnet add "$projFile" package "$packageName"
            }
        }
    }
}