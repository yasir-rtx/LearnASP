# Obscura API

**Obscura** adalah RESTful API berbasis **ASP.NET Core** yang dirancang untuk **manajemen buku oleh pembaca**, dengan pendekatan **Clean Architecture** agar sistem tetap **scalable, testable, dan siap berevolusi**.

Obscura tidak hanya bertujuan menyimpan data buku, tetapi juga menjadi fondasi untuk **penemuan (discovery)** dan **rekomendasi bacaan** yang relevan bagi pembaca.

---

## 🕯️ Makna Nama *Obscura*

Kata **Obscura** berasal dari bahasa Latin *obscurus*, yang berarti:

> *gelap, tersembunyi, atau belum terlihat jelas.*

Istilah ini dikenal luas melalui konsep **Camera Obscura**, sebuah ruang gelap yang memungkinkan dunia luar terlihat melalui cahaya kecil yang masuk.

### Filosofi Obscura
Dalam konteks proyek ini:
- **Buku** adalah pengetahuan
- **Pembaca** adalah penjelajah
- **Obscura** adalah ruang tempat minat membaca terbentuk
- **API** adalah alat untuk menyingkap hal-hal yang tersembunyi

> **Obscura adalah sistem yang membantu pembaca menemukan buku yang sebelumnya tersembunyi, jarang diperhatikan, atau belum terpikirkan.**

Nama ini dipilih agar:
- Tidak terlalu teknis
- Tidak membatasi fitur
- Relevan untuk pengembangan jangka panjang (rekomendasi, insight, discovery)

---

## ✨ Fitur Utama

- CRUD **Author**, **Book**, dan **Category**
- Clean Architecture (Presentation, Application, Domain, Infrastructure)
- Entity Framework Core (SQL Server)
- Health Check endpoint
- Swagger / OpenAPI
- Cross-platform (Windows & Linux)

---

## 🏗️ Arsitektur Proyek

Obscura
├── Application
│ ├── DTOs
│ ├── Interfaces
│ ├── Services
│ └── Mappings
├── Domain
│ └── Entities
├── Infrastructure
│ └── Data
├── Presentation
│ └── Controllers
├── Migrations
└── Program.cs


### Penjelasan Layer
| Layer | Tanggung Jawab |
|---|---|
| Presentation | HTTP, routing, response |
| Application | Business logic, service, DTO |
| Domain | Entity & domain model |
| Infrastructure | Database & EF Core |

---

## 🚀 Tech Stack

- **.NET 8 / .NET 9**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **AutoMapper**
- **NSwag (Swagger UI)**
- **DotNetEnv**

---

## ⚙️ Konfigurasi Environment

Gunakan file `.env` (tidak di-commit ke repository).

### Contoh `.env`
```env
ASPNETCORE_ENVIRONMENT=Development

DB_HOST=localhost
DB_PORT=1433
DB_NAME=ObscureDB
DB_USER=sa
DB_PASSWORD=StrongPassword123!
```
    ⚠️ Untuk development lokal, koneksi SQL Server menggunakan:

    Encrypt=False;TrustServerCertificate=True;

### 🗄️ Database Setup
1. Pastikan SQL Server

    TCP/IP aktif

    SQL Authentication aktif

    Port 1433 terbuka (jika digunakan)

2. Test koneksi manual
```
sqlcmd -S localhost,1433 -U sa -P StrongPassword123! -C
```
Jika perintah ini gagal, perbaiki konfigurasi SQL Server terlebih dahulu.
🔧 Menjalankan Aplikasi
```
dotnet restore
dotnet ef database update
dotnet run
```
Aplikasi akan berjalan di: http://localhost:5000

### 📚 Swagger / OpenAPI

Swagger tersedia di: http://localhost:5000/swagger

Digunakan untuk:
1. Dokumentasi API
2. Testing endpoint
3. Validasi request/response

### ❤️ Health Check

Endpoint health check:

GET /health

Contoh response:
```
{
  "status": "Healthy",
  "database": "ObscureDB",
  "checks": [
    {
      "name": "sqlserver",
      "status": "Healthy"
    }
  ]
}
```
### 📦 Contoh Endpoint
```
Category
Method	Endpoint
GET	/api/categories
GET	/api/categories/{id}
POST	/api/categories
PUT	/api/categories/{id}
DELETE	/api/categories/{id}
```

### 🧠 Business Rules (Category)
```
    slug dibuat otomatis dari name

    Slug:
        lowercase
        URL-safe
        unik

    Slug dikelola di Service Layer

    Database memiliki unique index pada Slug
```
### 🔮 Roadmap: Sistem Rekomendasi Obscura

Obscura dirancang sejak awal untuk berkembang menjadi platform discovery buku, bukan sekadar CRUD API.
Tahap 1 — Content-Based Recommendation

    Berdasarkan:
        Category
        Genre
        Author
        Tag

    Rekomendasi buku dengan metadata serupa

Tahap 2 — Behavior-Based Recommendation

    Berdasarkan aktivitas pembaca:

        Buku yang dibaca

        Buku yang disimpan

        Rating

    Rekomendasi personal per pengguna

Tahap 3 — Collaborative Filtering

    “Pembaca dengan minat serupa membaca buku yang sama”

    Rekomendasi berdasarkan pola kolektif

Tahap 4 — Hidden Gems (Visi Obscura)

    Buku dengan popularitas rendah tapi relevansi tinggi

    Discovery terhadap bacaan yang jarang ditemukan

    Tujuan akhir Obscura adalah menyingkap bacaan yang tersembunyi, bukan hanya yang populer.

## 🧪 Testing (Planned)

    Unit Test: Service Layer

    Integration Test: Controller + Database

    Mocking via Interface (IAuthorService, ICategoryService, dll)

## 📌 Best Practices yang Digunakan

    Controller tipis (thin controller)

    Business logic di Service

    DTO sebagai boundary

    Dependency Inversion (SOLID)

    Environment-based configuration

    CancellationToken di semua async method

## 🔐 Security Notes

    Jangan commit .env

    Jangan gunakan Encrypt=False di production

    Gunakan TLS valid di production

    Gunakan secrets manager untuk deployment cloud

## 📈 Future Improvements

    Authentication & Authorization (JWT)

    Role-based access control

    Pagination & filtering

    Caching

    Soft delete

    Audit log

    CI/CD pipeline

## 👤 Author

Developed by Yasir Chaniago
Built as a foundation for a reader-centric book discovery platform.
📄 License

This project is licensed under the MIT License.