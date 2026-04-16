<a id="readme-top"></a>

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <h3 align="center">BSR — Blue Sky Realty</h3>

  <p align="center">
    A UK property listing platform with role-based access control, advanced filtering, and Azure cloud support.
    <br />
    <a href="https://github.com/Dylan-JM/BSR"><strong>Explore the repo »</strong></a>
    <br />
    <br />
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#option-a-local-sql-server">Option A — Local SQL Server</a></li>
        <li><a href="#option-b-azure-sql-server">Option B — Azure SQL Server</a></li>
        <li><a href="#finishing-setup">Finishing Setup</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

---

<!-- ABOUT THE PROJECT -->

## About The Project

BSR is a full-stack ASP.NET Core MVC web application for browsing and managing UK property listings. It features role-based access control, advanced property filtering by price, size, location, and more, and supports both local and Azure-hosted SQL Server databases.

**Default test accounts (seeded on first run):**

| Role  | Email         | Password  |
| ----- | ------------- | --------- |
| Admin | admin@bsr.com | Admin123! |
| Sales | sales@bsr.com | Sales123! |
| User  | user@bsr.com  | User123!  |

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

- [![.NET][dotnet-shield]][dotnet-url]
- [![C#][csharp-shield]][csharp-url]
- [![SQL Server][sqlserver-shield]][sqlserver-url]
- [![Azure][azure-shield]][azure-url]
- [![Bootstrap][Bootstrap.com]][Bootstrap-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- GETTING STARTED -->

## Getting Started

BSR supports two database configurations: a **local SQL Server** instance for development and an **Azure SQL Server** instance for production/cloud deployments. Follow the option that suits your environment.

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (local) **or** an [Azure subscription](https://azure.microsoft.com/free/) with an Azure SQL Server instance
- [EF Core CLI tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
  ```sh
  dotnet tool install --global dotnet-ef
  ```
- _(Optional)_ [Seq](https://datalust.co/seq) for structured log viewing (runs on `localhost:5341`)
- A [GeoNames](https://www.geonames.org/login) account for UK city lookups
- A [Microsoft Entra (Azure AD) app registration](https://portal.azure.com/) for Microsoft OAuth sign-in

---

### Clone the repo

```sh
git clone https://github.com/Dylan-JM/BSR.git
cd BSR
```

---

### Option A — Local SQL Server

Use this option when you have SQL Server installed on your machine.

1. Open **SQL Server Management Studio** (or any SQL client) and create a new empty database, e.g. `bsr`.

2. Create `BSR/appsettings.local.json` (this file is git-ignored) and add your local connection string:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=bsr;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "GeoNames": {
       "Username": "YOUR_GEONAMES_USERNAME"
     },
     "MicrosoftAuth": {
       "ClientId": "YOUR_MICROSOFT_CLIENT_ID",
       "ClientSecret": "YOUR_MICROSOFT_CLIENT_SECRET"
     }
   }
   ```

3. Apply EF Core migrations to create the schema:

   ```sh
   cd BSR
   dotnet ef database update
   ```

4. Run the application — seed data (users, roles, and sample properties) is inserted automatically on first launch:

   ```sh
   dotnet run
   ```

---

### Option B — Azure SQL Server

Use this option to connect to an Azure-hosted SQL database.

1. In the [Azure Portal](https://portal.azure.com/), create an **Azure SQL Server** and a **SQL Database** (e.g. `bsr`).

2. Under **Networking**, add your local IP address to the server firewall rules so your machine can connect.

3. Copy the **ADO.NET connection string** from the Azure Portal (found under the database → Connection strings).

4. Create `BSR/appsettings.local.json` and paste the Azure connection string:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=tcp:<your-server>.database.windows.net,1433;Initial Catalog=bsr;Persist Security Info=False;User ID=<your-username>;Password=<your-password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
     },
     "GeoNames": {
       "Username": "YOUR_GEONAMES_USERNAME"
     },
     "MicrosoftAuth": {
       "ClientId": "YOUR_MICROSOFT_CLIENT_ID",
       "ClientSecret": "YOUR_MICROSOFT_CLIENT_SECRET"
     }
   }
   ```

5. Apply EF Core migrations against the Azure database:

   ```sh
   cd BSR
   dotnet ef database update
   ```

6. Run the application:

   ```sh
   dotnet run
   ```

---

### Finishing Setup

**GeoNames API**
Register for a free account at [geonames.org](https://www.geonames.org/login) and enable the free web services. Set the `GeoNames.Username` value in `appsettings.local.json`.

**Microsoft OAuth (optional)**
Create an app registration in [Microsoft Entra / Azure AD](https://portal.azure.com/). Add `https://localhost:<port>/signin-microsoft` as a redirect URI. Copy the **Application (client) ID** and a generated **client secret** into `appsettings.local.json`.

**Seq logging (optional)**
Download and install [Seq](https://datalust.co/seq). It runs on `http://localhost:5341` by default. The `appsettings.json` is pre-configured to send logs there — no extra steps needed.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- USAGE -->

## Usage

| Role          | Capabilities                                        |
| ------------- | --------------------------------------------------- |
| **Anonymous** | Browse all property listings                        |
| **User**      | Browse listings, view property details              |
| **Sales**     | All of the above + add, edit, and delete properties |
| **Admin**     | All of the above + manage users and assign roles    |

**Filtering** — the listing page supports filtering by price range, area (sq ft), bedrooms, bathrooms, garage spots, county, and city.

**City lookup** — city dropdowns are populated dynamically via the GeoNames API based on the selected county.

**Data seeding** — on first run the app seeds one property per UK county using the [Bogus](https://github.com/bchavez/Bogus) library, so you have real-looking data immediately.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- ROADMAP -->

## Roadmap

- [x] Role-based access control (Admin / Sales / User)
- [x] Advanced property filtering
- [x] Pagination
- [x] Microsoft OAuth sign-in
- [x] Azure SQL Server support
- [ ] Property image upload
- [ ] Saved / favourited listings per user
- [ ] Map view integration

See the [open issues](https://github.com/Dylan-JM/BSR/issues) for a full list of proposed features and known issues.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- ACKNOWLEDGMENTS -->

## Acknowledgments

- [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Bogus — fake data generator](https://github.com/bchavez/Bogus)
- [GeoNames API](https://www.geonames.org/)
- [Serilog](https://serilog.net/)
- [Seq](https://datalust.co/seq)
- [Img Shields](https://shields.io)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

<!-- MARKDOWN LINKS & IMAGES -->

[contributors-shield]: https://img.shields.io/github/contributors/Dylan-JM/BSR.svg?style=for-the-badge
[contributors-url]: https://github.com/Dylan-JM/BSR/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Dylan-JM/BSR.svg?style=for-the-badge
[forks-url]: https://github.com/Dylan-JM/BSR/network/members
[stars-shield]: https://img.shields.io/github/stars/Dylan-JM/BSR.svg?style=for-the-badge
[stars-url]: https://github.com/Dylan-JM/BSR/stargazers
[issues-shield]: https://img.shields.io/github/issues/Dylan-JM/BSR.svg?style=for-the-badge
[issues-url]: https://github.com/Dylan-JM/BSR/issues
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/dylan-jm
[dotnet-shield]: https://img.shields.io/badge/.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[dotnet-url]: https://dotnet.microsoft.com/
[csharp-shield]: https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white
[csharp-url]: https://learn.microsoft.com/en-us/dotnet/csharp/
[sqlserver-shield]: https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white
[sqlserver-url]: https://www.microsoft.com/en-us/sql-server
[azure-shield]: https://img.shields.io/badge/Azure-0078D4?style=for-the-badge&logo=microsoftazure&logoColor=white
[azure-url]: https://azure.microsoft.com/
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
