# SahibindenAl 

SahibindenAl is a web application that models the core functionalities of the popular second-hand classifieds platform, Sahibinden.com. It allows users to post listings in various categories (e.g., real estate, vehicles), search for listings with filters, and view their details. The project was developed using modern .NET technologies following the MVC (Model-View-Controller) architecture.

## Features

- **User Management:** Registration and login functionalities.
- **Ad Creation:** Allows users to post listings in different categories with dynamic properties (e.g., "Model Year," "Mileage" for a car).
- **Ad Viewing:** Detailed pages for listings, including information and images.
- **Advanced Search and Filtering:** Search for ads on the homepage by category, city, district, and price.
- **Dynamic Forms:** Ad posting forms that change based on the selected category.
- **Database Management:** Code-first database management and migrations with Entity Framework Core.
- **Repository Pattern:** Abstraction of the data access layer to increase manageability.

## Technology Stack

- **Backend:** C#, ASP.NET Core MVC (.NET 9)
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core
- **User Management:** ASP.NET Core Identity
- **Frontend:** HTML, CSS, JavaScript, Bootstrap
- **Other Libraries:**
  - `Npgsql.EntityFrameworkCore.PostgreSQL`: PostgreSQL database provider.
  - Library Manager (LibMan): For client-side library management.

## Project Structure

The main directories and files of the project are explained below:

```
SahibindenAl/
├── Controllers/      # Controllers that handle incoming requests and return responses.
│   ├── AccountController.cs  # User login/registration operations.
│   ├── AdvertController.cs   # Ad creation and viewing operations.
│   └── HomeController.cs     # Homepage and ad listing/filtering.
├── Data/               # Database context and migrations.
│   └── AppDbContext.cs
├── DTOs/               # Data Transfer Objects.
├── Models/             # Classes representing database entities.
├── Views/              # Razor (.cshtml) files that create the user interface.
├── Repository/         # Data access layer (Repository Pattern).
├── Service/            # Business logic layer.
├── wwwroot/            # Static files (CSS, JS, images).
├── appsettings.json    # Application configuration settings (e.g., database connection string).
├── libman.json         # Client-side library definitions for LibMan.
└── Program.cs          # The application's entry point and service configuration.
```

## Installation

Follow the steps below to run the project on your local machine:

1.  **Clone the Project:**
    ```bash
    git clone https://github.com/kilic-mustafa/SahibindenAl.git
    cd SahibindenAl
    ```

2.  **Configure the Database Connection:**
    Open the `SahibindenAl/appsettings.json` file and update the `DefaultConnection` field with your PostgreSQL connection details.
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Database=sahibindenal_db;Username=postgres;Password=your_password"
    }
    ```

3.  **Install .NET Dependencies:**
    Open a terminal in the project root directory (where the .sln file is) and run the command:
    ```bash
    dotnet restore
    ```

4.  **Restore Client-Side Libraries (LibMan):**
  This project uses LibMan to manage static assets. To restore them:
    
    ```bash
    # Install LibMan CLI if you haven't already
    dotnet tool install -g Microsoft.Web.LibraryManager.Cli

    # Restore libraries
    cd SahibindenAl
    libman restore
    ```

5.  **Create the Database (Migrations):**
    Navigate to the `SahibindenAl` project folder and apply the Entity Framework Core migrations to create the database.
    ```bash
    cd SahibindenAl
    dotnet ef database update
    ```

## Usage

To start the project, run the following command in the `SahibindenAl` folder:

```bash
dotnet run
```

The application will start running on `https://localhost:7209` and `http://localhost:5209` by default. You can start using the application by navigating to one of these addresses in your browser.

- **Post a New Ad:** Create an account using the "Login" or "Register" links in the top right corner. After logging in, you can create your new ad with the "Post Ad" button.
- **Search for Ads:** You can search among existing ads using the filtering options (category, city, district, etc.) on the homepage.

## Screenshots

<img width="1919" height="1020" alt="Ekran görüntüsü 2025-12-27 094634" src="https://github.com/user-attachments/assets/8ed9ea81-6853-478d-a84c-b3f8c4b3396a" />
<br /><br />
<img width="1919" height="1019" alt="Ekran görüntüsü 2025-12-27 095814" src="https://github.com/user-attachments/assets/c8476842-5b13-420c-b35e-55b077aaf24b" />
<br /><br />
<img width="1919" height="1020" alt="Ekran görüntüsü 2025-12-27 094736" src="https://github.com/user-attachments/assets/8016fd3d-b721-4a5e-8dc7-b3b9d9dcb3b9" />


## Contributing

Your contributions will make the project better! If you want to contribute:

1.  Fork the project.
2.  Create your own branch for a new feature or fix (`git checkout -b feature/new-feature`).
3.  Commit your changes (`git commit -m 'Added new feature'`).
4.  Push your branch to the main repository (`git push origin feature/new-feature`).
5.  Open a Pull Request (PR).

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
