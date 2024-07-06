# Diamond Luxury Shop

Welcome to the Diamond Luxury Shop project! This application is designed to manage and showcase an extensive collection of diamonds and jewelry. The project includes both an admin web application for managing the products and a customer web application for browsing and purchasing.

## Project Structure

- **DiamondLuxurySolution.Utilities**: Contains constants and enums used across the application.
- **DiamondLuxurySolution.ViewModel**: Contains view model classes used across the application.
- **DiamondLuxurySolution.Application**: Handles business logic and data repositories.
- **DiamondLuxurySolution.Data**: Manages database connections and data entities.
- **DiamondLuxurySolution.BackendApi**: API backend providing data services. 
- **DiamondLuxurySolution.AdminCrewApp**: MVC application for administrators to manage products, orders, customers, etc.
- **DiamondLuxurySolution.WebApp**: MVC application for customers to browse and purchase products.
- **DiamondLuxurySolution.BackgroundServiceHost**: Worker services that handle automated tasks.


## Features

- **Product Management**: Add, edit, delete, and view detailed information about diamonds and jewelry.
- **Order Management**: Track and manage customer orders.
- **Customer Management**: Manage customer information and interactions.
- **Product Browsing**: Customers can browse products by categories, view detailed information, and see high-quality images.
- **Shopping Cart**: Customers can add products to their cart, adjust quantities, and proceed to checkout.

## Technologies Used

- **ASP.NET MVC & ASP.NET API**: To build the web applications and backend services.
- **Firebase**: For storing images and sending OTPs for authentication.
- **PayPal Sandbox**: For processing payments.
- **Facebook & Google OAuth**: For user login and authentication.
- **ASP.NET Identity**: For managing user accounts and authentication.
- **Azure DB**: To create a shared database for the team.
- **Automated Price Crawling**: To keep product prices updated automatically.

## About Our Team

Our team consists of dedicated students from FPT University, each contributing their unique skills and expertise to this project:

-  Nguyễn Thanh Tùng    (Student ID: SE171746)
-  Lâm Quang Hưng       (Student ID: SE171422)
-  Nguyễn Minh Quân     (Student ID: SE171433)
-  Lâm Thanh Quốc Thắng (Student ID: SE171445)
-  Võ Trọng Nhân        (Student ID: SE171411)


We have worked collaboratively to ensure that every aspect of this project meets the highest standards of quality and performance.

## Admin Web Application

### Admin Dashboard
![Admin Dashboard](path/to/admin-dashboard-image.jpg)

### Manager Dashboard


### Product Management
![Product Management](path/to/admin-product-management-image.jpg)

### Order Management
![Order Management](path/to/admin-order-management-image.jpg)

## Customer Web Application

### Home Page
![Home Page](path/to/customer-home-page-image.jpg)

### Product Listing
![Product Listing](path/to/customer-product-listing-image.jpg)

### Product Details
![Product Details](path/to/customer-product-details-image.jpg)

### Shopping Cart
![Shopping Cart](path/to/customer-shopping-cart-image.jpg)

## Installation

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

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
