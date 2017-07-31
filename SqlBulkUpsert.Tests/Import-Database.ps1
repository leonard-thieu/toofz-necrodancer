$startPath = "$($env:appveyor_build_folder)\SqlBulkUpsert.Tests"
$sqlInstance = "(local)\SQL2014"
$dbName = "SqlBulkUpsertTestDb"

# attach mdf to local instance
$mdfFile = join-path $startPath $dbName + ".mdf"
$ldfFile = join-path $startPath $dbName + "_log.ldf"
sqlcmd -S "$sqlInstance" -Q "Use [master]; CREATE DATABASE [$dbName] ON (FILENAME = '$mdfFile'),(FILENAME = '$ldfFile') for ATTACH"