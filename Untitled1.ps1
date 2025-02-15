# setup-microservices.ps1

# Ensure we're in the right directory
$baseDir = "C:\Users\error\RiderProjects\EventManagement"
Set-Location $baseDir

# Create src directory
New-Item -ItemType Directory -Path "src" -Force

# Create docker directory
New-Item -ItemType Directory -Path "docker" -Force
New-Item -ItemType File -Path "docker\docker-compose.yml" -Force

# Create and setup Core project
New-Item -ItemType Directory -Path "src\EventManagement.Core" -Force
New-Item -ItemType Directory -Path "src\EventManagement.Core\Models" -Force
New-Item -ItemType Directory -Path "src\EventManagement.Core\DTOs" -Force
dotnet new classlib -o "src\EventManagement.Core"

# Create and setup API Gateway project
New-Item -ItemType Directory -Path "src\EventManagement.ApiGateway" -Force
New-Item -ItemType Directory -Path "src\EventManagement.ApiGateway\Middleware" -Force
dotnet new webapi -o "src\EventManagement.ApiGateway"

# Create and setup Events Service project
New-Item -ItemType Directory -Path "src\EventManagement.Services.Events" -Force
New-Item -ItemType Directory -Path "src\EventManagement.Services.Events\Controllers" -Force
New-Item -ItemType Directory -Path "src\EventManagement.Services.Events\Data" -Force
New-Item -ItemType Directory -Path "src\EventManagement.Services.Events\Migrations" -Force
dotnet new webapi -o "src\EventManagement.Services.Events"

# Create and setup Notifications Service project
New-Item -ItemType Directory -Path "src\EventManagement.Services.Notifications" -Force
dotnet new webapi -o "src\EventManagement.Services.Notifications"

# Move existing files to new structure
Write-Host "Moving existing files to new structure..."

# Move Models and DTOs to Core
Copy-Item "EventManagement\Models\Event.cs" "src\EventManagement.Core\Models\" -Force
Copy-Item "EventManagement\DTOs\CreateEventDTO.cs" "src\EventManagement.Core\DTOs\" -Force

# Move Controller and DbContext
Copy-Item "EventManagement\Controllers\EventsController.cs" "src\EventManagement.Services.Events\Controllers\" -Force
Copy-Item "EventManagement\Data\ApplicationDbContext.cs" "src\EventManagement.Services.Events\Data\" -Force

# Move Migrations
Copy-Item "EventManagement\Migrations\*" "src\EventManagement.Services.Events\Migrations\" -Force

# Add projects to solution
Write-Host "Adding projects to solution..."
dotnet sln add (Get-Item "src\EventManagement.Core\EventManagement.Core.csproj")
dotnet sln add (Get-Item "src\EventManagement.ApiGateway\EventManagement.ApiGateway.csproj")
dotnet sln add (Get-Item "src\EventManagement.Services.Events\EventManagement.Services.Events.csproj")
dotnet sln add (Get-Item "src\EventManagement.Services.Notifications\EventManagement.Services.Notifications.csproj")

# Add project references
Write-Host "Adding project references..."
Set-Location "src\EventManagement.ApiGateway"
dotnet add reference "..\EventManagement.Core\EventManagement.Core.csproj"

Set-Location "..\EventManagement.Services.Events"
dotnet add reference "..\EventManagement.Core\EventManagement.Core.csproj"

Set-Location "..\EventManagement.Services.Notifications"
dotnet add reference "..\EventManagement.Core\EventManagement.Core.csproj"

# Add required NuGet packages
Write-Host "Adding NuGet packages..."

# API Gateway packages
Set-Location "..\EventManagement.ApiGateway"
dotnet add package Yarp.ReverseProxy
dotnet add package Microsoft.Extensions.Caching.Memory

# Events Service packages
Set-Location "..\EventManagement.Services.Events"
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design

# Return to base directory
Set-Location $baseDir

Write-Host "Setup complete! New microservices structure has been created."
Write-Host "Note: Please review the moved files and update namespaces as needed."
Write-Host "Remember to update connection strings and configuration in the new projects."