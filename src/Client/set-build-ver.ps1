# Project file
$currentDirProj = (Get-Item .).FullName + '\BiDegree.csproj'
$currentDate = get-date -format yyyyMMdd.HHmm;
$find = "<Version>(.|\n)*?</Version>";
$replace = "<Version>" + $currentDate + "</Version>";
$csproj = Get-Content $currentDirProj
$csprojUpdated = $csproj -replace $find, $replace

Set-Content -Path $currentDirProj -Value $csprojUpdated

# Service Worker file
$currentDirSW = (Get-Item .).FullName + '\wwwroot\service-worker.published.js'
$now = Get-Date -format 'dddd, MMMM dd, yyyy HH:mm:ss'
$find = ">>> Build updated(.)*? <<<";
$replace = ">>> Build updated " + $now + " <<<";
$swjs = Get-Content $currentDirSW
$csprojUpdated = $swjs -replace $find, $replace

Set-Content -Path $currentDirSW -Value $csprojUpdated

