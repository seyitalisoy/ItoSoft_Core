# ShopITo - E-Commerce Project

ShopITo is a modern e-commerce platform designed with a modular architecture using **ASP.NET Core**, **MVC**, and **Redis** for real-time session management. It features **user authentication**, **role-based authorization**, a **shopping cart** system, and an **admin panel** for managing products, users, and orders.

🚀 **Current Development:**  
This project is continuously evolving. Recent updates include an **API layer** that will handle more complex queries and interactions, and future plans involve integrating the API with a modern frontend framework for a complete user experience.

## Features

- 🛒 **Shopping Cart System**
- 🔒 **Secure User Authentication**
- 🔑 **Role-based Authorization** (Admin, User)
- 📦 **Product Catalog and Categories**
- 🧑‍💻 **Admin Panel** for managing users, roles, and products
- 🔌 **API Integration** (In Progress)
- 🔄 **Redis Integration** for session management
- 📊 **User Role Management**

## Tech Stack

- **Backend:** ASP.NET Core 8
- **Frontend:** HTML, CSS, JavaScript (Frontend framework integration upcoming)
- **Database:** SQL Server (MSSQL)
- **Caching:** Redis for Session Management
- **Authentication:** ASP.NET Core Identity
- **API:** RESTful API Layer (Under Development)

## Project Setup

1. **Clone the Repository**  
    ```bash
    git clone https://github.com/seyitalisoy/ItoSoft_Core.git
    ```

2. **Install Dependencies**  
   Make sure you have **.NET Core SDK** installed. Run the following commands to restore and build the project.
    ```bash
    dotnet restore
    dotnet build
    ```

3. **Configure Database Connection**  
   Set your database connection string in the **appsettings.json** file:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=your_server;Database=ShopIToDB;User Id=your_user;Password=your_password;"
    }
    ```

4. **Run Migrations**  
    Apply database migrations by running:
    ```bash
    dotnet ef database update
    ```

5. **Run the Application**  
    Start the application with:
    ```bash
    dotnet run
    ```

## API Layer (In Progress)

The **API layer** is currently under development and will expose endpoints for product management, user roles, and more. This will allow easier integration with external applications and frontend frameworks.

- 🎯 **Future Enhancements:** API will support complex queries, interactions, and third-party integrations.
- 🔄 **Frontend Framework Integration:** Upcoming integration with popular frontend frameworks like React or Angular for a modern, dynamic user interface.

## Admin Panel

The **Admin Panel** allows administrators to manage:

- **Users:** Create, update, delete users, assign roles.
- **Products:** Add, update, delete products, manage categories.
- **Orders:** View and manage customer orders.

For a better understanding of the **Admin Panel**, we have prepared 3 videos that show the differences between users who make orders without logging in and those who log in as users:

1. **[Admin Panel Overview - User Without Login](https://youtu.be/2LzqXYJbeOg)**
   This video demonstrates the process of managing orders for users who are not logged in.

2. **[Admin Panel Overview - User With Login](https://youtu.be/VMC9eWBMLlo)**
   This video shows how to handle orders placed by users who are logged into their accounts.

3. **[Admin Panel Product Management](https://youtu.be/zK0LM8bThhU)**
   This video demonstrates how to manage products in the admin panel.

🎉 **Upcoming Features:**  
- More complex queries and data visualization for admin analytics.
- Extended role management and admin-specific permissions.

## Contact

For more information, follow the repository on GitHub or contact me at [soyseyitali23@gmail.com].

---

## 🛠️ Current and Upcoming Developments

- API layer development for improved data handling and querying.
- Integration of frontend frameworks like React or Angular.
- Ongoing security improvements and session management via Redis.
- Complex admin functionalities such as advanced filtering and reporting.

📍 **Follow the developments on GitHub:** [GitHub Repository](https://github.com/seyitalisoy/ItoSoft_Core)
