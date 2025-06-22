---

# VideoHub API

## Opis projekta

VideoHub API je backend aplikacija razvijena u .NET 8, koja omogućava upravljanje video sadržajem uz integriranu autentifikaciju, naplatu putem Stripe-a, upload videa putem Mux platforme i pregled video sadržaja. API je dokumentiran koristeći OpenAPI 3.0 (Swagger) te podržava JWT autentifikaciju za sigurnu komunikaciju.

---

## Tehnologije

* .NET 8
* ASP.NET Core Web API
* JWT autentifikacija
* Swagger (OpenAPI 3.0)
* Stripe za naplatu
* Mux za video upload i streaming
* C# 10
* Dependency Injection

---

## Funkcionalnosti

* **Autentifikacija**:

  * Klasična prijava (`POST /api/auth/login`)
  * Google OAuth prijava (`POST /api/auth/google`)
* **Video management**:

  * Upload videa (`POST /api/videos`)
  * Dohvat svih videa (`GET /api/videos`)
  * Kupnja videa (`POST /api/videos/{id}/purchase`)
* **Naplatni sustav**:

  * Stripe Payment Intents (`POST /api/stripe/create-payment-intent`)
* **Mux integracija**:

  * Upload videa na Mux (`POST /api/mux/upload`)
* **Swagger UI** s podrškom za autorizaciju JWT tokenom
* Root endpoint (`GET /`) preusmjerava na Swagger UI

---

## Pokretanje

1. Postavi JWT konfiguraciju u `appsettings.json`:

```json
"Jwt": {
  "Key": "tvoja-tajna-key",
  "Issuer": "tvoj-api",
  "Audience": "tvoj-api-korisnik"
}
```

2. Pokreni aplikaciju:

```bash
dotnet run --launch-profile backend
```

3. Otvori Swagger UI na adresi:

```
https://localhost:7290/swagger
```

---

## Daljnji razvoj i mogućnosti

### ➕ Dodaci koje možeš implementirati

* **Rate Limiting**
  Ograniči broj zahtjeva korisnika u određenom vremenskom periodu za bolju zaštitu i stabilnost API-ja.

* **Logging**
  Implementiraj detaljno logiranje zahtjeva, grešaka i događaja radi lakšeg praćenja rada aplikacije.

* **Testovi (Unit / Integration)**
  Generiraj testove za ključne dijelove sustava kao što su:

  * Autentifikacija (Auth)
  * Stripe integracija
  * Mux upload

* **Docker**
  Pripremi Dockerfile za jednostavan deployment i skalabilnost.

---