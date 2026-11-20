# Government Website

A group project for a government website that handles government forms.

## Current Progress
- **Backend (In Progress):** Added initial ASP.NET Core Web API code for form submissions and reference code lookups using an in-memory service while the database and frontend are still being built.
- **Frontend (In Progress):** Conceptualizing design for frontend.
- **Database (In Progress)**

### In Database
Currently, the website has no front-end(UI), therefore the database handling is splitted into two access:
1. Client
2. Administrative

For the Client Side, the website could do these four functionalities: 
- Accept a submission of a request
- Track a request
- Update a request
- Delete a request

While for the Administrative Side:
- There is still no functionalities here as it is currently in progress.

**Known Issue:**
  - Submitted data is not being saved in the database on the client side.

---

## How to Run

### Prerequisites
* [.NET SDK 10.0](https://dotnet.microsoft.com/download) (or .NET SDK 9.0.200+)
* Any modern web browser (Edge, Chrome, Firefox, etc.)

---

### Option 1: Running via Terminal / Command Prompt (Recommended)

1. Open **Terminal**, **PowerShell**, or **Command Prompt** in the project root folder.
2. Navigate into the web project directory:
   ```bash
   cd [the location of the GovernmentWebsite folder]
   ```
3. Run the application:
   ```bash
   dotnet run
   ```
4. Once the terminal displays `Now listening on: http://localhost:5137`, open your browser and visit:
   * **Citizen Portal (Client):** [http://localhost:5137](http://localhost:5137)
   * **Staff & Admin Dashboard:** [http://localhost:5137/Admin](http://localhost:5137/Admin)

> 💡 *To stop the application at any time, press `Ctrl + C` in the terminal.*

---

### Option 2: Running via Visual Studio 2022

1. Open **`GovernmentWebsite.sln`** (or `GovernmentWebsite.slnx`) in Visual Studio.
2. Select the **`http`** or **`https`** profile from the run dropdown at the top toolbar.
3. Press **`Ctrl + F5`** (Start Without Debugging) or **`F5`** (Start With Debugging).
4. Visual Studio will build the project and automatically open your default browser.

---

### 💾 Database Setup
* The project uses **SQLite** (`GovernmentWebsite.db`), which runs locally without requiring an external database server (no SQL Server setup needed).
* All tables and migrations are pre-configured and ready to use.
