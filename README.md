# 💎 Diamond Luxury Shop 💍

👋 Welcome to the Diamond Luxury Shop project! This application is designed to manage and showcase an extensive collection of diamonds and jewelry. The project includes both an admin web application for managing the products and a customer web application for browsing and purchasing.

## Project Structure 📂

- **DiamondLuxurySolution.Utilities**: Contains constants and enums used across the application.
- **DiamondLuxurySolution.ViewModel**: Contains view model classes used across the application.
- **DiamondLuxurySolution.Application**: Handles business logic and data repositories.
- **DiamondLuxurySolution.Data**: Manages database connections and data entities.
- **DiamondLuxurySolution.BackendApi**: API backend providing data services. 
- **DiamondLuxurySolution.AdminCrewApp**: MVC application for administrators to manage products, orders, customers, etc.
- **DiamondLuxurySolution.WebApp**: MVC application for customers to browse and purchase products.
- **DiamondLuxurySolution.BackgroundServiceHost**: Worker services that handle automated tasks.


## Features 🔑

- **Product Management**: Add, edit, delete, and view detailed information about diamonds and jewelry.
- **Order Management**: Track and manage customer orders.
- **Customer Management**: Manage customer information and interactions.
- **Product Browsing**: Customers can browse products by categories, view detailed information, and see high-quality images.
- **Shopping Cart**: Customers can add products to their cart, adjust quantities, and proceed to checkout.

## Technologies Used 🔎

- **ASP.NET MVC & ASP.NET API**: To build the web applications and backend services.
- **Firebase**: For storing images and sending OTPs for authentication.
- **VNPAY Sandbox**: For processing payments.
- **PayPal Sandbox**: For processing payments.
- **Facebook & Google OAuth**: For user login and authentication.
- **ASP.NET Identity**: For managing user accounts and authentication.
- **Azure DB**: To create a shared database for the team.
- **Automated Price Crawling**: To keep product prices updated automatically.

## About Our Team 👥

Our team consists of dedicated students from FPT University, each contributing their unique skills and expertise to this project:

-  Nguyễn Thanh Tùng    (Student ID: SE171746)
-  Lâm Quang Hưng       (Student ID: SE171422)
-  Nguyễn Minh Quân     (Student ID: SE171433)
-  Lâm Thanh Quốc Thắng (Student ID: SE171445)
-  Võ Trọng Nhân        (Student ID: SE171411)


We have worked collaboratively to ensure that every aspect of this project meets the highest standards of quality and performance.

## Admin Web Application 👮

### Admin Dashboard
![Screenshot 2024-07-06 213318](https://github.com/NTTung2k3K/DiamondLuxuryWebApp/assets/143085090/805aec1c-7f9a-4603-9976-dec068414e2a)

### Manager Dashboard
![Screenshot 2024-07-06 213458](https://github.com/NTTung2k3K/DiamondLuxuryWebApp/assets/143085090/19b9c104-26e5-4b17-be5e-bd0d4f38b375)
![Screenshot 2024-07-06 213521](https://github.com/NTTung2k3K/DiamondLuxuryWebApp/assets/143085090/a9805fdd-5b4c-4fba-bca4-8fea965f9387)

### Product Management
![Screenshot 2024-07-06 213543](https://github.com/NTTung2k3K/DiamondLuxuryWebApp/assets/143085090/8ad06f4e-247c-42e4-bca6-a4016a84e5e1)

### Order Management
![Screenshot 2024-07-06 213602](https://github.com/NTTung2k3K/DiamondLuxuryWebApp/assets/143085090/a6254a08-a27f-438b-a8f4-6d97693ab401)

## Customer Web Application 🤵

### Home Page
![Screenshot 2024-07-06 213620](https://github.com/NTTung2k3K/DiamondLuxuryWebApp/assets/143085090/8ce6fc09-fd44-4c3d-8474-d60cd50d827e)

### Product Listing
![Screenshot 2024-07-06 213656](https://github.com/NTTung2k3K/DiamondLuxuryWebApp/assets/143085090/525ee071-7627-4f5c-9164-75ebd88281d5)

### Product Details
![Screenshot 2024-07-06 213746](https://github.com/NTTung2k3K/DiamondLuxuryWebApp/assets/143085090/9b3fba58-e34d-46fa-89e4-8ed1cca8102e)

### Shopping Cart
![Screenshot 2024-07-06 213821](https://github.com/NTTung2k3K/DiamondLuxuryWebApp/assets/143085090/5948e4de-3cd0-4fc2-b1d7-f461273f8e3a)

## Installation 🔨

1. Clone the repository:
    ```bash
    git clone https://github.com/NTTung2k3K/DiamondLuxuryWebApp
    ```
2. Navigate to the project directory:
    ```bash
    cd DiamondLuxurySolution
    ```
3. Update the connection string in `appsettings.json` to point to your Azure DB or your local DB.
4. Restore dependencies:
    ```bash
    dotnet restore
    ```
5. Add and apply migrations in `DiamondLuxurySolution.Data`:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```
6. Run the application:
    ```bash
    dotnet run
    ```

## Contributing 👐

Contributions are welcome! Please open an issue or submit a pull request.
