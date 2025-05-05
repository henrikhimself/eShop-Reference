#requires -Version 7
param (
  [ValidateNotNullOrEmpty()]
  [string]$Model = $Env:LLM1_MODEL,
  [ValidateNotNullOrEmpty()]
  [string]$ApiKey = $Env:LLM1_APIKEY,
  [ValidateNotNullOrEmpty()]
  [string]$Url = $Env:LLM1_URL,
  [switch]$Staged
)
Set-StrictMode -Version Latest

Write-Host "Reviewing..." -ForegroundColor Green
$ReviewFilesArgs = @('diff', '--name-only', '--relative')
$ReviewDiffArgs = @('diff', '--unified=3')
if ($Staged) {
  $ReviewFilesArgs += '--cached'
  $ReviewDiffArgs += '--cached'
}

$ReviewFiles = & git @ReviewFilesArgs
if (-not $ReviewFiles) {
  Write-Host 'No files found.'
  exit 0
}

$ReviewInput = @("Code changes to review (unified git diff format):")
foreach ($reviewFile in @($ReviewFiles)) {
  Write-Host "- $reviewFile"
  $fileDiffArgs = $ReviewDiffArgs + @('--', $reviewFile)
  $fileDiff = & git @fileDiffArgs
  if ($fileDiff) {
      $ReviewInput += "`n---`nFile: $reviewFile`n$fileDiff"
  }
}

$Messages = @(
    @{
      role = 'system'
      content = @'
You are an expert .NET and PowerShell code reviewer. Review only the following code changes (diffs) for issues, bugs, improvements, and best practices. Ignore code indentation and other code style related issues. Respond with a bulleted list of comments. Each comment should specify the file name and line number(s) it concerns and a diff showing the old and new line of code.

Example:
- [MyFile.cs:98] Consider using interpolated strings for logging to improve readability and avoid potential null reference exceptions.
old: _logger.LogError($"My name is {firstName} {lastName}");
new: _logger.LogError("My name is {0} {1}", firstName, lastName);
- [OtherFile.cs:27] Possible null reference exception.
old: (no previous line exists)
new: return myObject.Name;
'@
    },
    @{
      role = 'user'
      content = $ReviewInput -join "`n"
    }
)

$Body = @{
  messages = $Messages
  model = $Model
  stream = $false
  temperature = 0.4
} | ConvertTo-Json -Depth 4

try {
  $apiKeySecureString = ConvertTo-SecureString $ApiKey -AsPlainText -Force
  $requestArgs = @{
    Uri = "$Url/v1/openai/chat/completions"
    Method = 'Post'
    ContentType = 'application/json'
    Authentication = 'Bearer'
    Token = $apiKeySecureString
    Body = $Body
    ErrorAction = 'Stop'
  }
  $Response = Invoke-RestMethod @requestArgs
} catch {
  Write-Error "API call failed: $_"
  exit 1
}

Write-Host 'Comments:' -ForegroundColor Green
foreach ($choice in $Response.choices) {
  $choice.message.content
}
