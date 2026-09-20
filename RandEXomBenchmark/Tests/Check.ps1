param([string]$Runner = (Join-Path $PSScriptRoot '../bin/portable/RandEXomBenchmark.exe'))
$ErrorActionPreference = 'Stop'
$Runner = (Resolve-Path $Runner).Path
dotnet build (Join-Path $PSScriptRoot 'Fixture.csproj') -c Release --ignore-failed-sources
if ($LASTEXITCODE -ne 0) { throw 'Fixture build failed.' }
$fixture = Join-Path $PSScriptRoot 'bin/Release/net8.0/Fixture.dll'
$output = Join-Path $PSScriptRoot 'bin/reports'
New-Item -ItemType Directory -Force -Path $output | Out-Null
$config = Join-Path $output 'arguments.json'
'{ "customValue": 7 }' | Set-Content $config
& $Runner smoke --dll $fixture --random NewRandom --config $config --items 16 --output $output
if ($LASTEXITCODE -ne 0) { throw 'Fixture smoke failed.' }
$report = Get-Content (Join-Path $output 'smoke.json') -Raw | ConvertFrom-Json
if ($report.Results.Count -ne 12) { throw "Expected 12 discovered workloads, got $($report.Results.Count)." }
if ($report.Results.Error | Where-Object { $_ }) { throw 'Workload failure.' }
if ($report.Skipped.Count -ne 0) { throw 'Unexpected skipped fixture class/method.' }
& $Runner quality --dll $fixture --bytes 4096 --output $output
if ($LASTEXITCODE -ne 0) { throw 'Fixture quality failed.' }
$quality = Get-Content (Join-Path $output 'quality.json') -Raw | ConvertFrom-Json
if ($quality.Results.Count -ne 3) { throw 'Expected three seed results for the discovered random class.' }
Write-Output 'DLL discovery, injection, removal sampling, configuration and quality checks passed.'

