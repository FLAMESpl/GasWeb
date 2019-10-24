Push-Location "$PSScriptRoot\src\Domain"

dotnet ef database drop --startup-project ..\Server\GasWeb.Server.csproj -c GasWebDbContext

Pop-Location