Push-Location "$PSScriptRoot\src\Domain"

dotnet ef migrations remove --startup-project ..\Server\GasWeb.Server.csproj -c GasWebDbContext

Pop-Location