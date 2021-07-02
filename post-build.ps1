# Project file
$currentDirProj = (Get-Item .).FullName + '\BiDegree.csproj'
$currentDate = get-date -format yyyy.MM.dd.HHmm;
$find = "<Version>(.|\n)*?</Version>";
$replace = "<Version>" + $currentDate + "</Version>";
$csproj = Get-Content $currentDirProj
$csprojUpdated = $csproj -replace $find, $replace

Set-Content -Path $currentDirProj -Value $csprojUpdated

# Service Worker file
$currentDirSW = (Get-Item .).FullName + '\wwwroot\service-worker.published.js'
$find = "-- Version update (.|\n)*? --";
$replace = "-- Version update " + $currentDate + " --";
$swjs = Get-Content $currentDirSW
$csprojUpdated = $swjs -replace $find, $replace

Set-Content -Path $currentDirSW -Value $csprojUpdated