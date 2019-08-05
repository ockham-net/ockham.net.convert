function Join-FSPath([string[]]$Parts) {
  [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($Parts))
}

function Get-RefAssemblyPath {
  $output = &dotnet --info
  $basePathLine = $output | ? { $_ -match 'Base Path' }
  $sdkPath = [regex]::Replace($basePathLine, '\s*Base\s*Path\s*:\s*', '')
  Join-FSPath $sdkPath, 'ref'
}
 
function  Get-PackagesPath {
  $output = &dotnet nuget locals global-packages --list
  return [regex]::Replace($output, 'info\s*:\s*global-packages\s*:\s*', '')
}

$refAsmPath  =  Get-RefAssemblyPath

$projectRoot = $PSScriptRoot

$toolsPath   = Join-FSPath $projectRoot, 'ref', 'ockham.net', 'tools'
$genAPI      = Join-FSPath $toolsPath, 'bin', 'GenAPI', 'GenAPI.exe'

$testDir     = Join-FSPath $projectRoot, 'tests'
$resultsPath = Join-FSPath $testDir, 'results'
$apiDir      = Join-FSPath $resultsPath, 'api'

if(!(Test-Path $apiDir)) { mkdir $apiDir | Out-Null }


$apiAPI  = (Join-FSPath $apiDir, 'api.cs')
$srcAPI  = (Join-FSPath $apiDir, 'src.netstandard2.0.cs')
$srcDiff = (Join-FSPath $apiDir, 'src.netstandard2.0.diff')

Write-Host "Comparing APIs" -f  Cyan
Write-Host "  Building api project" -f Cyan

Push-Location (Join-FSPath $projectRoot, 'api', 'lib')
dotnet publish (Resolve-Path .) -c Debug

Write-Host "    Generating API file" -f Cyan

$publishDir = Join-FSPath $projectRoot, 'api', 'lib', 'bin', 'Debug', 'netstandard2.0', 'publish'
&$genAPI (Join-FSPath $publishDir, 'Ockham.Convert.API.dll') `
  -libPath:"$publishDir;$refAsmPath" `
  -xal:(Join-FSPath $projectRoot, 'tests', 'results', 'api', '_excludeMembers.txt') `
  -apiOnly | Out-File $apiAPI -Encoding utf8

Write-Host "  Building src project" -f Cyan

Set-Location (Join-FSPath $projectRoot, 'src', 'lib')
dotnet publish (Resolve-Path .) -c Debug

Write-Host "    Generating API file" -f Cyan

$publishDir = Join-FSPath $projectRoot, 'src', 'lib', 'bin', 'Debug', 'netstandard2.0', 'publish'
&$genAPI (Join-FSPath $publishDir, 'Ockham.Convert.dll') `
  -libPath:"$publishDir;$refAsmPath" `
  -apiOnly | Out-File $srcAPI -Encoding utf8

Write-Host "  Comparing API files" -f Cyan

git diff --no-index $apiAPI $srcAPI | Out-File $srcDiff -Encoding utf8

if((Get-Item $srcDiff).Length -gt 0) {
  Write-Host ''
  Write-Warning "Found differences between api and src:`r`n"
  gc (Join-FSPath $apiDir, 'src.netstandard2.0.diff')
} else {
  Write-Host '    ✓ APIs are identical' -ForegroundColor Green
}

Pop-Location

<#
$publishDir = Join-FSPath $projectRoot, 'src', 'lib', 'bin', 'Debug', 'net472', 'publish'
&$genAPI (Join-FSPath $publishDir, 'Ockham.Convert.dll') -libPath:"$publishDir;$refAsmPath" -apiOnly > (Join-FSPath $apiDir, 'src.net472.cs')

$publishDir = Join-FSPath $projectRoot, 'src', 'lib', 'bin', 'Debug', 'netcoreapp2.2', 'publish'
&$genAPI (Join-FSPath $publishDir, 'Ockham.Convert.dll') -libPath:"$publishDir;$refAsmPath" -apiOnly > (Join-FSPath $apiDir, 'src.netcoreapp2.2.cs')
#>

<#
$cciHost = [Microsoft.Cci.Extensions.HostEnvironment]::new()
$cciHost.AddLibPaths([string[]]@($publishDir, $refAsmPath))
#>
