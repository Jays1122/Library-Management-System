# Library Management System

A full-stack Library Management System built with **.NET 8 Core Web API (CQRS Pattern)** and **React.js (Tailwind CSS)**.

## 🚀 Features
- **Role-Based Authentication (JWT):** Admin and Member roles.
- **Manage Books:** Admin can Add, Edit, and Delete books.
- **Issue & Return:** Users can issue books. Admin can manage returns.
- **Bonus Tasks Completed:** 
  - Tracks total number of currently issued books.
  - Automatically manages "Available Copies" when a book is issued/returned.

## 🛠️ Tech Stack
- **Backend:** C#, .NET 8, Entity Framework Core, SQL Server, MediatR (CQRS), FluentValidation, AutoMapper.
- **Frontend:** React.js, Vite, Tailwind CSS, Axios.

## ⚙️ Setup Instructions

### 1. Database Setup
1. Open SQL Server Management Studio (SSMS).
2. The Database will be automatically created using EF Core Migrations.

### 2. Backend Setup (.NET Core)
1. Navigate to the `Backend` folder.
2. Open `appsettings.json` and update the `DefaultConnection` string with your SQL Server details.
3. Open Package Manager Console or Terminal and run:
   ```bash
   dotnet ef database update
