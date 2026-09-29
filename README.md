# VideoApp Ìæ¨

A production-ready video platform with premium content, Stripe payments, and MUX streaming.

## ‚ú® Features

- **Authentication:** JWT + Google OAuth
- **Video Management:** Upload, stream, and manage videos via MUX
- **Premium Content:** Locked videos with Stripe checkout
- **Dark Mode:** Persistent user preference
- **Responsive Design:** Mobile-first UI with Tailwind + DaisyUI
- **RESTful API:** Fully documented via Swagger
- **CQRS:** Clean separation of commands and queries
- **Validation:** FluentValidation with MediatR pipeline
- **Tests:** 21 unit and integration tests

## Ìª† Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Nuxt 3, TypeScript, Tailwind CSS, DaisyUI, Pinia |
| Backend | .NET 9, ASP.NET Core, MediatR (CQRS) |
| Data Access | Dapper, SQL Server 2022 |
| Authentication | JWT, Google OAuth 2.0, BCrypt |
| Video | MUX API |
| Payments | Stripe Checkout |
| Validation | FluentValidation |
| DevOps | Docker, Docker Compose, GitHub Actions |
| Testing | xUnit, Moq, WebApplicationFactory |

## Ì∫Ä Quick Start

### Prerequisites
- Docker Desktop
- Git

### Run with Docker

git clone https://github.com/1244Matt1244/videohub_app.git
cd videohub_app
cp .env.example .env
docker compose up --build

Wait 3-5 minutes for SQL Server to initialize.

Then open:
- Frontend: http://localhost:3000
- Backend Swagger: http://localhost:5000/swagger
- SQL Server: localhost:1433

### Run Locally

Backend:
cd backend
dotnet restore VideoApp.sln
dotnet run --project VideoApp

Frontend:
cd frontend
npm install
npm run dev

## Ì∑™ Testing

cd backend
dotnet test VideoApp.sln

Coverage: 21 tests (15 unit + 6 integration)

## Ìºê Deployment

### Frontend (Vercel)

1. Import repository at https://vercel.com
2. Set Root Directory to frontend
3. Set env variable NUXT_PUBLIC_API_BASE = your backend URL
4. Deploy

### Backend (Render / Azure)

1. Create Web Service, connect repo
2. Set Root Directory to backend/VideoApp
3. Runtime: Docker
4. Environment variables:

ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__Default=<your-connection-string>
Jwt__Key=<random-64-char-string>
Jwt__Issuer=VideoApp
Jwt__Audience=VideoAppClient
Frontend__Url=<frontend-url>
Mux__TokenId=<your-mux-token>
Mux__TokenSecret=<your-mux-secret>
Stripe__SecretKey=<your-stripe-key>
Stripe__WebhookSecret=<your-stripe-webhook>

### Database

This project uses SQL Server 2022. Free hosting options are limited:
- Azure SQL Database (free 12 months)
- Migration to PostgreSQL (requires code changes)

### Third-party Services

| Service | How to get keys |
|---------|-----------------|
| MUX | https://dashboard.mux.com/settings/access-tokens |
| Stripe | https://dashboard.stripe.com/test/apikeys |
| Google OAuth | https://console.cloud.google.com/apis/credentials |

## Ì¥í Security Notes

Before deploying to production:

1. Replace SQL Server password - YourStrong@Passw0rd is a placeholder
2. Generate strong JWT key - use openssl rand -base64 64
3. Move secrets to environment variables - never commit .env
4. Restrict CORS - change Frontend:Url to your actual domain
5. Enable HTTPS - use Let's Encrypt or reverse proxy
6. Remove SQL Server port exposure in production
7. Add rate limiting - consider AspNetCoreRateLimit
8. Keep packages updated - run dotnet list package --outdated

## Ì≥ù License

MIT License - see LICENSE for details.

## Ì±§ Author

**Matej Martinoviƒá**

- GitHub: https://github.com/1244Matt1244
- LinkedIn: https://www.linkedin.com/in/matej-martinovi%C4%87-140750265/
- Email: matt123.3a@gmail.com
