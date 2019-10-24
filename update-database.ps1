Param
(
	[String] $migrationName
)

Push-Location "$PSScriptRoot\src\Domain"

dotnet ef database update $migrationName --startup-project ..\Server\GasWeb.Server.csproj -c GasWebDbContext

Pop-Location