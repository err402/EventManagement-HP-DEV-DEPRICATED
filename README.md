# Event Management API (DEPRICATED ARCHITECTURE MOVED TO GO)

A microservices-based .NET Core Event Management API deployed on AWS, providing a scalable backend for event creation and management.

## Project Overview

This repository contains the codebase for the Event Management API, a microservices architecture built with .NET 8.0 and deployed on AWS. The system consists of an API Gateway, Event Service, and Notifications Service, with a PostgreSQL database for data persistence.

### Key Features

- **Microservices Architecture**: Separate services for API Gateway, Events, and Notifications
- **RESTful API Design**: Standard HTTP methods for intuitive event management
- **AWS Deployment**: Hosted on EC2 with RDS for database management
- **Comprehensive Documentation**: API documentation via Swagger
- **Security**: Implementation of best practices for API security
- **Scalability**: Designed for horizontal scaling of individual services

## Architecture

```
┌───────────────┐     ┌──────────────────┐     ┌──────────────────┐
│   Client      │────▶│   API Gateway    │────▶│   Event Service   │
│  Applications │     │  (Port 5000)     │     │   (Port 44386)    │
└───────────────┘     └──────────────────┘     └──────────────────┘
                               │                        │
                               │                        │
                               ▼                        ▼
                      ┌──────────────────┐     ┌──────────────────┐
                      │  Notifications   │     │   PostgreSQL     │
                      │   (Port 5002)    │     │   Database       │
                      └──────────────────┘     └──────────────────┘
```

## Infrastructure

- **EC2 Instance**: Ubuntu Server (Public IP: REDACT)
- **Security Groups**: Configured for ports 22, 80, 443, 44386
- **RDS**: PostgreSQL instance for data storage
- **Nginx**: Configured as a reverse proxy
- **Systemd**: Services for application management

## Database Schema

```sql
Schema: event_management
Table: events
Columns:
  - id (UUID, Primary Key)
  - name (varchar(200), Not Null)
  - created_at (timestamp with timezone)
  - is_draft (boolean, Default: true)
```

## API Endpoints

### Health Checks

```
GET /health - Check overall API health
GET /events-health - Check Events service health
GET /notifications-health - Check Notifications service health
```

### Event Management

```
GET /api/events - List all events
GET /api/events/{id} - Get specific event
POST /api/events - Create new event
PUT /api/events/{id} - Update event
DELETE /api/events/{id} - Delete event
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL
- Git
- AWS CLI (for deployment)

### Local Development Setup

1. Clone the repository:
   ```
   git clone https://github.com/PermafrostWolf/EventManagement-HP-DEV.git
   cd EventManagement-HP-DEV
   ```

2. Set up your database connection in each service's `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=event_management;Username=your_username;Password=your_password"
     }
   }
   ```

3. Navigate to each service directory and restore dependencies:
   ```
   cd src/EventManagement.Services.Events
   dotnet restore
   ```

4. Run the services:
   ```
   dotnet run
   ```

### Project Structure

```
EventManagement/
├── src/
│   ├── EventManagement.Core/            # Shared components
│   │   ├── Models/
│   │   └── DTOs/
│   │
│   ├── EventManagement.ApiGateway/      # API Gateway
│   │   └── Middleware/
│   │
│   ├── EventManagement.Services.Events/ # Main event service
│   │   ├── Controllers/
│   │   ├── Data/
│   │   └── Migrations/
│   │
│   └── EventManagement.Services.Notifications/
│
└── docker/
```

## Deployment

### AWS Deployment

1. Prepare your EC2 instance:
   - Ubuntu Server with .NET 8.0 SDK
   - Nginx configured as reverse proxy
   - PostgreSQL on RDS

2. Set up the directory structure:
   ```
   /var/www/eventapi/
   ├── ApiGateway/
   ├── Events/
   └── Notifications/
   ```

3. Use the deployment script:
   ```
   ./deploy.sh
   ```

### Deployment Script (deploy.sh)

The script handles:
- Pulling the latest code
- Publishing the application
- Restarting services
- Performing health checks

## Using the API

### PowerShell Examples

#### List All Events
```powershell
Invoke-RestMethod -Uri "http://IPADDR/api/events"
```

#### Create Event
```powershell
$newEvent = @{
    name = "New Test Event"
    isDraft = $true
} | ConvertTo-Json

Invoke-RestMethod -Uri "http:/IPADDR/api/events" -Method Post -Body $newEvent -ContentType "application/json"
```

## Security Considerations

Current implementation includes:
- Basic security groups configuration
- CORS enabled for development

Planned improvements:
- Authentication implementation
- HTTPS configuration
- Security group tightening

## Future Enhancements

1. **Authentication and Authorization**
   - JWT-based authentication
   - Role-based access control
   - Security headers implementation

2. **API Gateway Enhancements**
   - Caching layer
   - Rate limiting
   - Request validation

3. **Monitoring and Logging**
   - Centralized logging system
   - Performance metrics collection
   - Alert system for service issues

4. **CI/CD Pipeline**
   - Automated testing
   - Deployment automation
   - Environment-specific configurations

5. **Notifications Service**
   - Email notifications
   - Webhook integrations
   - Push notifications

## Contributing

1. Fork the repository
2. Create your feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add some amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
