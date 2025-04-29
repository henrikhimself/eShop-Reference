#!/usr/bin/env pwsh
#requires -Version 7
Set-StrictMode -Version Latest
$InformationPreference = 'Continue'

<# Global #>
$UpdateTestReportScript = Join-Path $PSScriptRoot '..' 'scripts' 'Update-TestReport.ps1'
$SourceSolutionPath = Join-Path $PSScriptRoot '..' 'Common.sln'
$RunSettingsPath = Join-Path $PSScriptRoot '..' '.runsettings'
$TestReportPath = Join-Path $PSScriptRoot 'test-report'

<# Main #>
& $UpdateTestReportScript -TestPath $SourceSolutionPath -RunSettingsPath $RunSettingsPath -ReportDirectoryPath $TestReportPath
