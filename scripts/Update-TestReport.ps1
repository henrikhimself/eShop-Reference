#requires -Version 7
param (
  [parameter(mandatory = $true)]
  [string]$TestPath,
  [parameter(mandatory = $true)]
  [string]$RunSettingsPath,
  [parameter(mandatory = $true)]
  [string]$ReportDirectoryPath
)
Set-StrictMode -Version Latest
$InformationPreference = 'Continue'

<# Global #>
# Check path of system under test and settings file.
if (-Not (Test-Path -Path $TestPath)) {
  throw 'Cannot find test path.'
}
if (-Not (Test-Path -Path $RunSettingsPath -PathType leaf)) {
  throw 'Cannot find run settings file.'
}

<# Main #>
# Set up report results path.
if (-Not (Test-Path -Path $ReportDirectoryPath)) {
  $null = New-Item -ItemType 'Directory' -Path $ReportDirectoryPath
}
[string]$ResultsDirectoryPath = Join-Path $ReportDirectoryPath 'Results'
if (-Not (Test-Path -Path $ResultsDirectoryPath)) {
  $null = New-Item -ItemType 'Directory' -Path $ResultsDirectoryPath
}
[string]$HistoryDirectoryPath = Join-Path $ReportDirectoryPath 'History'
if (-Not (Test-Path -Path $HistoryDirectoryPath)) {
  $null = New-Item -ItemType 'Directory' -Path $HistoryDirectoryPath
}

# Move past results to history directory.
Move-Item -Path (Join-Path $ResultsDirectoryPath '*') -Destination $HistoryDirectoryPath

# Execute unit tests.
$TestArgs = @(
  'test', $TestPath,
  '/property:GenerateFullPaths=true',
  '/consoleloggerparameters:NoSummary',
  "--results-directory:$ResultsDirectoryPath",
  "--settings:$RunSettingsPath"
)
& dotnet $TestArgs
if ($LastExitCode -ne 0) {
  exit 1
}

# Update test coverage report.
$ReportGeneratorArgs = @(
  'reportgenerator',
  "-title:SutFactory",
  "-reports:$ResultsDirectoryPath/**/coverage.cobertura.xml",
  "-historydir:$HistoryDirectoryPath"
  "-targetdir:$ReportDirectoryPath",
  '-reporttypes:HtmlInline_AzurePipelines'
)
& dotnet $ReportGeneratorArgs
