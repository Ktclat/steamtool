param(
    [string]$version,
    [string]$configuration = 'Release',
    [bool]$isPublish = $false
)

$ErrorActionPreference = 'Stop'

Write-Host 'dotnet SDK info'
dotnet --info

$rootPath = Split-Path $PSScriptRoot -Parent
$outputDir = Join-Path $rootPath "src\ST.Client.Desktop.Avalonia.App\bin\$configuration\Publish"
$projectPath = Join-Path $rootPath 'src\ST.Client.Desktop.Avalonia.App\ST.Client.Avalonia.App.csproj'
$publishProfiles = @('fd-win-x64', 'win-x64')

if ($isPublish) {
    Write-Warning 'The legacy multi-platform publish/signing tool was removed. This script now creates Windows x64 builds only.'
}

foreach ($profile in $publishProfiles) {
    Write-Host "Building $version $profile"

    if ($profile.StartsWith('fd-')) {
        $publishDir = Join-Path $outputDir "FrameworkDependent\$profile"
    }
    else {
        $publishDir = Join-Path $outputDir $profile
    }

    Remove-Item -LiteralPath $publishDir -Recurse -Force -Confirm:$false -ErrorAction Ignore

    $effectiveProfile = if ($configuration -eq 'Debug') { "dev-$profile" } else { $profile }
    & dotnet publish $projectPath `
        --configuration $configuration `
        --runtime win7-x64 `
        -p:Platform=x64 `
        -p:PublishProfile=$effectiveProfile `
        -p:DeployOnBuild=true `
        -p:ExtraDefineConstants=$profile `
        --nologo

    if ($LASTEXITCODE) {
        exit $LASTEXITCODE
    }
}
