# Web App for Out of Office solution

### Technologies
- <img src='https://github.com/yaryna-bashchak/maths-course/assets/90560209/e3d0cafd-dcf6-4db2-9331-b8ca7b558d99' height='25'> **.NET** for the Back-end
- <img src='https://github.com/yaryna-bashchak/maths-course/assets/90560209/27b894e3-2717-4629-902d-3f46090a7502' height='25'> **React** for the Front-end

### Key points
- Developed REST Web API using .NET
- Followed N-Layered architecture to separate Controllers, Services, Repositories
- Created database on Microsoft SQL Server
- Built a front-end React app that provides user interface for convenient interaction with the API
- Used Contexts for data management on the client

## How to run

### Prerequisites
- .NET 8 SDK
- Node.js
- MS SQL Server

### Steps

1. **Clone the repository**
    ```sh
    git clone https://github.com/yaryna-bashchak/out-of-office.git
    cd out-of-office
    ```
2. **Set up the backend**
    - Navigate to the server directory:
        ```sh
        cd outofoffice.server
        ```
    - Install dependencies:
        ```sh
        dotnet restore
        ```
    - Configure the database connection string:
        - Open `appsettings.json`
        - Set your MS SQL Server connection string in the `ConnectionStrings` section.
    - Manually run the SQL scripts to set up the database:
        - Open SQL Server Management Studio (SSMS)
        - Create new database named `OutOfOfficeDB`
        - Run `TablesCreation.sql` and then `SeedInitialData.sql` scripts located in the `outofoffice.repositories/db_scripts` folder to create the tables and seed data
3. **Set up the frontend**
    - Navigate to the client directory:
        ```sh
        cd outofoffice.client
        ```
    - Install dependencies:
        ```sh
        npm install
        ```
4. **Run the App**
    - Navigate to the server directory:
        ```sh
        cd outofoffice.server
        ```
    - Run the App:
        ```sh
        dotnet run
        ```
