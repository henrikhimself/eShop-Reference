#!/usr/bin/env pwsh
#requires -Version 7
Set-StrictMode -Version Latest
$InformationPreference = 'Continue'

<# Global #>
$TestReportPath = Join-Path $PSScriptRoot 'test-report' 'index.html'

<# Main #>
if (-Not (Test-Path -Path $TestReportPath -PathType leaf)) {
  throw 'Cannot find test report file.'
}

Write-Information "Test report path: $TestReportPath"
if ($IsWindows) {
  Start-Process $TestReportPath
}
