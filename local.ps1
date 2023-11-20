<#
.SYNOPSIS
Local dev commands for blazor-wasm-crud.

.DESCRIPTION
USAGE
    .\local.ps1 <command>

COMMANDS
    migrate ............ create database and run db migrations
    migration <name> ... create a new db migration
    run ................ run the server
    test ............... run tests
    compose ............ run docker-compose
    help, -? ........... show this help message
#>
param(
  [Parameter(Position=0)]
  [ValidateSet("migrate", "run", "test", "compose", "help", "migration")]
  [string]$Command,

  [Parameter(Position=1, ValueFromRemainingArguments=$true)]
  $Rest
)

function Invoke-Help { Get-Help $PSCommandPath }

if (!$Command) {
    Invoke-Help
    exit
}

function Invoke-Migrate {
    & dotnet ef database update `
        --project $PSScriptRoot\src\People.Infrastructure `
        --startup-project $PSScriptRoot\src\People.BlazorWasmServer
}
function Invoke-Run {
    if (!(Test-Path $PSScriptRoot\src\People.BlazorWasmServer\people.db)) {
        Write-Host "Database not found. Creating..." -F DarkGray
        Invoke-Migrate
    }

    & dotnet run `
        --project $PSScriptRoot\src\People.BlazorWasmServer `
        --launch-profile https
}
function Invoke-Test {
    & dotnet test $PSScriptRoot\PeopleBlazor.sln
}
function Invoke-Migration([string]$MigrationName) {
    & dotnet ef migrations add $MigrationName `
        --project $PSScriptRoot\src\People.Infrastructure `
        --startup-project $PSScriptRoot\src\People.BlazorWasmServer
}

function Invoke-Compose([string]$Rest) {
    Invoke-Expression "docker compose --file $PSScriptRoot\docker-compose.yaml --project-directory $PSScriptRoot up -d --build $Rest"
}

switch ($Command) {
    "migrate" { Invoke-Migrate }
    "run" { Invoke-Run }
    "test" { Invoke-Test }
    "compose" { Invoke-Expression "Invoke-Compose $Rest" }
    "help" { Invoke-Help }
    "migration" { Invoke-Expression "Invoke-Migration $Rest" }
}
