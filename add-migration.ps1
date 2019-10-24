Param
(
	[String][Parameter(Mandatory = $true)] $migrationName
)

Push-Location "$PSScriptRoot\src\Domain"

dotnet ef migrations add $migrationName --startup-project ..\Server\GasWeb.Server.csproj -c GasWebDbContext

Pop-Location