Tour Management System - ASP.NET Core MVC Migration
---------------------------------------------------

Original Source: https://github.com/jaygajera17/Tour_Management_Asp.Net  
Migrated and modernized by: https://github.com/brcodeworks  
Migration Type: ASP.NET Web Forms to ASP.NET Core MVC 8.0  
Database: SQL Server LocalDB to SQL Server 2022  
Status: Completed

---------------------------------------------------

Project Overview
----------------
This project is a migration of a legacy ASP.NET Web Forms based Tour Management System to a modern ASP.NET Core MVC architecture.

The system includes:
- Admin Panel: add and manage tours, view bookings
- User Panel: register, login, book tours, and view personal bookings

The goal of this migration was to improve maintainability, structure, and performance while keeping the same core features.

---------------------------------------------------

Migration Approach
------------------
This migration focused on upgrading the framework and structure while keeping the existing database logic intact.

- Replaced old ASPX code-behind with MVC controllers and Razor views.
- Retained direct ADO.NET (SqlConnection) queries, rewritten as parameterized and async.
- Added dependency injection for configuration and services.
- Added session-based login for user authentication.
- Moved configuration from Web.config to appsettings.json.
- Updated the user interface with Bootstrap 5 for modern design.

This approach demonstrates how a Web Forms application can be modernized quickly without rewriting the entire data layer to Entity Framework.

---------------------------------------------------

Key Migration Highlights
------------------------
Legacy Framework: ASP.NET Web Forms (.NET Framework 4.x)
Modern Framework: ASP.NET Core MVC 8.0

Data Access: ADO.NET queries (legacy) to async parameterized ADO.NET (modern)
Database: LocalDB (.mdf) to SQL Server 2022
Authentication: Forms Authentication to Session-based Login
Configuration: Web.config to appsettings.json
Hosting: IIS only to Cross-platform (IIS, Linux, or Cloud)

---------------------------------------------------

Tech Stack
----------
Backend: ASP.NET Core 8.0 (MVC)
Frontend: Razor Views with Bootstrap 5
Database: SQL Server 2022
IDE: Visual Studio 2022
Language: C#
Version Control: Git + GitHub

---------------------------------------------------

Setup Instructions
------------------
1. Clone the repository
   git clone https://github.com/brcodeworks/TourManagement_AspNetCore_Migrated.git
   cd TourManagement_AspNetCore_Migrated

2. Update the database connection string inside appsettings.json

3. Run the project
   dotnet run

4. Open the browser at
   https://localhost:5001

---------------------------------------------------

Screenshots
-----------
Before Migration:
<img width="1911" height="995" alt="image" src="https://github.com/user-attachments/assets/2c697580-78e5-47ba-ab16-9f27f9553ac3" />
<img width="1920" height="1004" alt="image" src="https://github.com/user-attachments/assets/f13af03f-97df-4333-bd2a-383589bd537e" />
<img width="1920" height="998" alt="image" src="https://github.com/user-attachments/assets/1bda4e5f-1095-4abb-85a4-2dbfffb69386" />
<img width="1920" height="638" alt="image" src="https://github.com/user-attachments/assets/cde3a0db-88e1-4c39-8d0c-0a28bfbe5918" />
<img width="1920" height="1015" alt="image" src="https://github.com/user-attachments/assets/39037ce8-254b-4da5-a027-179d693422ee" />
<img width="1920" height="971" alt="image" src="https://github.com/user-attachments/assets/d9d756fd-74c6-425e-849e-96da90f77d2a" />
<img width="1916" height="932" alt="image" src="https://github.com/user-attachments/assets/d56ff0d7-ce70-4fa5-918d-28f6081e6180" />
<img width="1919" height="1024" alt="image" src="https://github.com/user-attachments/assets/ad24d3da-0eaa-4997-a0b1-c4d2aefe7b45" />
<img width="1920" height="1036" alt="image" src="https://github.com/user-attachments/assets/876c16cc-23e7-4b8a-8e1b-b518c9e1dad8" />
<img width="1917" height="951" alt="image" src="https://github.com/user-attachments/assets/d39538cb-d6bf-45b8-8b64-28cc468cbcf1" />
<img width="1920" height="982" alt="image" src="https://github.com/user-attachments/assets/90dbd1e5-7d19-4fda-9bb4-8808e4055f83" />
<img width="1920" height="636" alt="image" src="https://github.com/user-attachments/assets/631de381-55ca-496f-9934-ff246d6306ba" />

After Migration:
<img width="1919" height="924" alt="image" src="https://github.com/user-attachments/assets/94715eb7-de1b-450c-8998-df152809829e" />
<img width="1919" height="978" alt="image" src="https://github.com/user-attachments/assets/e7a82373-170c-4a5c-825d-b0fd3f30e2f6" />
<img width="1911" height="923" alt="image" src="https://github.com/user-attachments/assets/643dcbc2-1546-416c-a62e-0ec1e9ead818" />
<img width="1912" height="921" alt="image" src="https://github.com/user-attachments/assets/1ece7f90-9416-4c3f-a2fc-f556fd429652" />
<img width="1909" height="926" alt="image" src="https://github.com/user-attachments/assets/c18aba38-d7e6-4534-a088-224bbfe3a28d" />
<img width="1920" height="860" alt="image" src="https://github.com/user-attachments/assets/63ed82b5-1579-4a1d-a729-08bb406c7c63" />
<img width="1920" height="897" alt="image" src="https://github.com/user-attachments/assets/a145bdd0-2103-4ab5-b86c-0aca7f9d15f3" />
<img width="1920" height="839" alt="image" src="https://github.com/user-attachments/assets/e10f328e-fa42-4f67-9d4f-f75fcb779b33" />
<img width="1918" height="965" alt="image" src="https://github.com/user-attachments/assets/0258034a-07c4-419c-aac1-2d5df85bb04c" />
<img width="1916" height="931" alt="image" src="https://github.com/user-attachments/assets/0c9c59fc-0d1b-4753-ae6c-18e58734b5ab" />
<img width="1914" height="812" alt="image" src="https://github.com/user-attachments/assets/ef51181e-26e6-4148-b14e-3bba1e90c8c1" />

---------------------------------------------------

Learnings and Improvements
--------------------------
- Converted Web Forms structure to MVC pattern
- Applied async database operations using SqlConnection
- Used dependency injection for configuration
- Simplified routing and folder organization
- Improved the UI using Bootstrap 5 for responsive layout

---------------------------------------------------

About BRCodeWorks
-----------------
BRCodeWorks is a small technical brand focused on upgrading and modernizing legacy applications.

We help businesses move their old ASP.NET, AngularJS, and SQL Server projects to modern, secure, and efficient versions.

Contact:
-----------------
Website: https://brcodeworks.com/
Email: hello@brcodeworks.com

---------------------------------------------------
Tags
-----
aspnet-core, migration, ado-net, webforms, mvc, brcodeworks, modernization
