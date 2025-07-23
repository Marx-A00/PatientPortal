# Zelis Payments Practice Project

A bare minimum end‑to‑end demo showcasing how **Blazor Server**, **ASP.NET Core Web API**, **SQL Server**, and **Okta** interact—designed to mirror Zelis’s micro‑frontend architecture. Focus is on communication flow, not fancy UI or complex logic.

---

## 🎯 Project Goal

Build a simple **“Payment Viewer”** prototype where:

- **SQL Server**: Contains a `Payments` table with columns *(Id, CheckNumber, Amount, Status)* seeded with two rows.
- **Web API**: Minimal‑API endpoint `GET /api/payments` returns the seeded data using EF Core.
- **Blazor Server UI**: A Razor page `/payments` calls the Web API with `HttpClient`, displays the returned list, and is protected by Okta authentication.
- **Okta**: Handles login via OIDC and secures both the Blazor UI and Web API.

---

## 🏛 Architecture Diagram

```
Browser ──▶ Blazor Server (SignalR circuit)
                │
                ▼ calls API with HttpClient
          ASP.NET Core Minimal‑API
                │ calls EF Core
                ▼ connects to
             SQL Server
```

- **UI and API can reside in the same solution** for simplicity; perfect for a learning demo.
- **EF Core lives only in the API**, not in the Blazor UI, following micro‑frontend communication best-practices.
- **Okta OIDC** secures both UI (via auth cookies) and API (via JWT bearer tokens).

---

## 🛠 Step-by-Step Build Guide

### 1. Prerequisites
- **.NET 8 SDK**
- **SQL Server Developer Edition** (local instance)
- **Okta Developer Account**: Register a “Web” OIDC application (Authorization Code + PKCE)

### 2. Create the Solution

```bash
dotnet new sln -n ZelisPaymentsDemo

# Blazor Server project
dotnet new blazorserver -n Zelis.UI

# ASP.NET Core Web API
dotnet new webapi -n Zelis.Api --no-https

dotnet sln add Zelis.UI Zelis.Api
```

---

### 3. API Layer (`Zelis.Api`)

#### a. Add EF Core Packages
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
```

#### b. Create the Payment Model & DbContext
```csharp
public record Payment(int Id, string CheckNumber, decimal Amount, string Status);

public class PaymentsDb : DbContext
{
    public PaymentsDb(DbContextOptions<PaymentsDb> opts) : base(opts) {}
    public DbSet<Payment> Payments => Set<Payment>();
}
```

#### c. Add Minimal‑API Endpoint
```csharp
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<PaymentsDb>(
    builder.Configuration.GetConnectionString("Sql"));

builder.Services.AddAuthentication()
    .AddJwtBearer("Okta", options =>
    {
        // Okta JWT settings
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapGet("/api/payments", async (PaymentsDb db) =>
    await db.Payments.ToListAsync())
   .RequireAuthorization();

app.Run();
```

#### d. Migrations + Seed Data
```bash
dotnet ef migrations add Init
dotnet ef database update
```
Seed two rows either manually in SSMS or via EF seed logic.

---

### 4. Blazor Server UI (`Zelis.UI`)

#### a. Add Okta Package
```bash
dotnet add package Okta.AspNetCore
```

#### b. Configure Authentication (in `Program.cs`)
```csharp
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignInScheme       = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultChallengeScheme    = OktaDefaults.ApiAuthenticationScheme;
})
.AddOktaMvc(new OktaMvcOptions {
    OktaDomain   = "{yourOktaDomain}",
    ClientId     = "{clientId}",
    ClientSecret = "{clientSecret}"
});
```

#### c. Register `HttpClient`, Inject Token
```csharp
builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("https://localhost:5000/");
})
.AddHttpMessageHandler<OktaClientHandler>();
```

#### d. Create `Payments.razor`
```razor
@inject HttpClient Http

<table>
  <thead>
    <tr><th>Id</th><th>CheckNumber</th><th>Amount</th><th>Status</th></tr>
  </thead>
  <tbody>
  @if (data is not null)
  {
    foreach (var p in data)
    {
      <tr>
        <td>@p.Id</td><td>@p.CheckNumber</td><td>@p.Amount</td><td>@p.Status</td>
      </tr>
    }
  }
  </tbody>
</table>

@code {
  private List<Payment>? data;

  protected override async Task OnInitializedAsync()
  {
    data = await Http.GetFromJsonAsync<List<Payment>>("api/payments");
  }

  public record Payment(int Id, string CheckNumber, decimal Amount, string Status);
}
```

---

### 5. Run Both Projects Locally
```bash
dotnet run --project Zelis.Api     # http://localhost:5000
dotnet run --project Zelis.UI      # http://localhost:5001
```
- Ensure `appsettings.json` in UI has the correct API base-address (`https://localhost:5000/`).

---

## ✅ What to Observe

1. **Okta Flow**: Unauthenticated access to `/payments` redirects to Okta login, then returns to the UI.
2. **Data Flow**: Blazor UI → Web API → EF Core → SQL → back to UI.
3. **Layered Separation**: Changing the database or API endpoint requires only config changes in the UI.
4. **Error Handling**: Stop SQL Server → API returns 500 → UI should reflect data-loading failures.

---

## 🔧 Optional Enhancements

- **Docker Compose**: Bring up SQL Server, API, and UI containers together.
- **Swagger/Swashbuckle**: Add API documentation and an interactive tester.
- **Health Checks Middleware**: Demo readiness/liveness endpoints for container orchestration.
- **Feature Flags**: Integrate with `Microsoft.FeatureManagement` to toggle demo features.

---

## 📌 Architectural Notes
- **Payment** domain aligns with Zelis's micro‑frontend naming.
- **EF Core exclusively in API** reinforces separation and BFF pattern.
- **Okta secures across both layers**, with cookie-based UI auth and JWT-based API protection.

---

## 🧾 References
- Microsoft minimal‑API + EF Core guide  
- Okta docs/sample repos for Blazor Server integration  
- SQLPey tutorial for Okta + Blazor  
- StackOverflow + best‑practice citations on layering Blazor + EF Core  
