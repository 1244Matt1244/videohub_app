---

```md
# 🎬 VideoHub API

## 📘 Opis projekta

VideoHub API je moderni backend sustav izrađen u .NET 9 (.NET 9 Preview), osmišljen za upravljanje video sadržajem putem integracije sa Mux-om, autentifikaciju korisnika (klasičnu i Google OAuth), naplatu putem Stripea i kontrolu pristupa premium sadržaju. Namijenjen je kao temelj za videoplatformu s premium modelom pristupa.

Projekt koristi JWT autentifikaciju, dokumentiran je putem Swagger UI (OpenAPI 3.0), a backend je organiziran po CQRS obrascu s Dapper ORM-om.

---

## 🚧 Arhitektura

Aplikacija koristi **CQRS** (Command Query Responsibility Segregation) uz **Dapper ORM**, odvajajući upite (queryje) od operacija koje mijenjaju stanje (commandi). Autentifikacija je temeljena na **JWT tokenima** i konfigurirana kroz ASP.NET middleware. Integracija sa **Mux** omogućuje streaming i hosting videa, dok se naplata premium sadržaja obavlja putem **Stripe Payment Intents** API-ja.

Slojevi aplikacije uključuju:

- `Controllers` – REST endpointi
- `Services` – logika poslovnih pravila
- `Interfaces` – apstrakcija servisa
- `Dto` i `Models` – prijenosni i bazni objekti
- `VideoApp.Tests` – osnovna testna pokrivenost (.NET xUnit)

---

## 🧰 Korištene tehnologije

- **.NET 9 (ASP.NET Core Web API)**
- **C# 10**
- **JWT** autentifikacija + ASP.NET Identity Middleware
- **Swagger / OpenAPI 3.0**
- **Stripe** (test okruženje)
- **Mux** za video upload i stream
- **Google OAuth** autentifikacija
- **Dapper ORM**
- **SQL Server** (lokalno / Docker)
- **Docker + Docker Compose**
- **xUnit** (testiranje)
- **GitHub Actions** (CI/CD)

---

## ✅ Funkcionalnosti

- 🔐 **Autentifikacija korisnika**: klasična (email + lozinka) + Google OAuth
- 🎞️ **Upload i prikaz videa**: pohrana preko Mux, pregled putem stream linka
- 💳 **Premium videi**: zaključani dok korisnik ne obavi Stripe testnu transakciju
- 🧾 **RESTful API**: pristup putem JWT tokena
- 📜 **Swagger dokumentacija**: `/swagger` endpoint
- 🧪 **Unit testovi**: pokrivenost testovima za ključne servise

---

## 🚀 Pokretanje projekta

### 🔑 Preduvjeti

- .NET 9 SDK
- Node.js (za frontend)
- Docker (za lokalnu SQL bazu)
- Stripe, Mux i Google API ključevi (testni mod)
- (Opcionalno) Vercel ili Azure račun za deploy

### ⚙️ Konfiguracija

1. Kopiraj `.env.example` u `.env` i postavi varijable:

```env
SA_PASSWORD=YourStrong@Passw0rd
JWT_KEY=secure_32+_char_key
MUX_TOKEN_ID=your_mux_token_id
MUX_TOKEN_SECRET=your_mux_token_secret
STRIPE_SECRET_KEY=sk_test_xxx
STRIPE_WEBHOOK_SECRET=whsec_xxx
GOOGLE_CLIENT_ID=google-client-id.apps.googleusercontent.com
GOOGLE_CLIENT_SECRET=google-client-secret
```

2. Pokreni SQL Server i backend putem Docker Compose:

```bash
docker-compose up -d
```

3. Otvori Swagger UI:

```
http://localhost:5000/swagger
```

---

## 📦 Pokretanje frontend-a (Nuxt 3)

1. Instaliraj ovisnosti:

```bash
cd frontend
npm install
```

2. Pokreni lokalni dev server:

```bash
npm run dev
```

---

## 🌍 Deploy (CI/CD)

Projekt koristi GitHub Actions za:

- Build i test .NET backend-a
- Build Nuxt frontend-a
- Deploy backend-a na **Azure App Service** (`videohub-app`)
- (Opcionalno) frontend deploy na Vercel

CI workflow nalazi se u: `.github/workflows/dotnet-nuxt.yml`

---

## 📈 Daljnji razvoj (roadmap)

- [ ] 💡 Rate Limiting i Anti-Spam mjere
- [ ] 💬 Video komentari i lajkovi
- [ ] 📊 Statistika gledanosti po korisniku
- [ ] 🧩 Admin dashboard
- [ ] 📽️ Video konverzija / thumbnails

---

## 🎯 Kanban board

Organizirani backlog nalazi se ovdje:  
🔗 https://github.com/users/1244Matt1244/projects/1

---

## 📝 Licenca

MIT License © [Matej](https://github.com/1244Matt1244)

```
