using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiamondLuxurySolution.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    AboutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AboutImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Abouts__717FC93C4893C36F", x => x.AboutId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CategoryType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CategoryImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__19093A0B0B6F138B", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    CollectionId = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    CollectionName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Collecti__7DE6BC04F05F28C3", x => x.CollectionId);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DiscountImage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PercentSale = table.Column<double>(type: "float", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Discount__E43F6D964D50BE3B", x => x.DiscountId);
                });

            migrationBuilder.CreateTable(
                name: "GemPriceLists",
                columns: table => new
                {
                    GemPriceListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cut = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Clarity = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    CaratWeight = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Color = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GemPrice__EFF6BEE083204377", x => x.GemPriceListId);
                });

            migrationBuilder.CreateTable(
                name: "Gems",
                columns: table => new
                {
                    GemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Proportion = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Symetry = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Polish = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsOrigin = table.Column<bool>(type: "bit", nullable: false),
                    _4C = table.Column<bool>(name: "4C", type: "bit", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    Fluoresence = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Gems__F101D5804FDDCABA", x => x.GemId);
                });

            migrationBuilder.CreateTable(
                name: "InspectionCertificates",
                columns: table => new
                {
                    InspectionCertificateId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    InspectionCertificateName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DateGrading = table.Column<DateTime>(type: "datetime", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inspecti__23BDD56CAB56582C", x => x.InspectionCertificateId);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeNewCatagories",
                columns: table => new
                {
                    KnowledgeNewCatagoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnowledgeNewCatagoriesName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Knowledg__69B8D362765095E8", x => x.KnowledgeNewCatagoryID);
                });

            migrationBuilder.CreateTable(
                name: "MaterialPriceLists",
                columns: table => new
                {
                    MaterialPriceListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__2B355356E5A81769", x => x.MaterialPriceListId);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SubMaterial = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__C50610F7A4B8B8A5", x => x.MaterialId);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    PlatformId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PlatformURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlatformLogo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Platform__F559F6FABA63124D", x => x.PlatformId);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PromotionImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Promotio__52C42FCF3F8664C3", x => x.PromotionId);
                });

            migrationBuilder.CreateTable(
                name: "Slides",
                columns: table => new
                {
                    SlideId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlideName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SlideURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SlideImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Slides__9E7CB650F4EBAD0A", x => x.SlideId);
                });

            migrationBuilder.CreateTable(
                name: "WareHouses",
                columns: table => new
                {
                    WareHouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WareHouseName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WareHous__69FF807867312D67", x => x.WareHouseId);
                });

            migrationBuilder.CreateTable(
                name: "Warrantys",
                columns: table => new
                {
                    WarrantyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarrantyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DateActive = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateExpired = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Warranty__2ED31813DFECD7DE", x => x.WarrantyId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeNews",
                columns: table => new
                {
                    KnowledgeNewsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnowledgeNewsName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Thumnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    WriterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Knowledg__F4C49FE2953AEFFC", x => x.KnowledgeNewsID);
                    table.ForeignKey(
                        name: "FK__Knowledge__Write__40058253",
                        column: x => x.WriterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsOutstanding = table.Column<bool>(type: "bit", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__News__954EBDF39002B82C", x => x.NewsId);
                    table.ForeignKey(
                        name: "FK__News__Id__3B40CD36",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    ShipName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShipperId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShipPhoneNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShipEmail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShipAdress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Total_Amout = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ShipPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__C3905BCF2AC66985", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK__Orders__Customer__236943A5",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Orders__ShipperI__22751F6C",
                        column: x => x.ShipperId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GemPriceListDetails",
                columns: table => new
                {
                    GemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GemPriceListId = table.Column<int>(type: "int", nullable: false),
                    EffectDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GemPrice__EFFEBE6E32CA28A7", x => new { x.GemId, x.GemPriceListId });
                    table.ForeignKey(
                        name: "FK__GemPriceL__GemId__797309D9",
                        column: x => x.GemId,
                        principalTable: "Gems",
                        principalColumn: "GemId");
                    table.ForeignKey(
                        name: "FK__GemPriceL__GemPr__7A672E12",
                        column: x => x.GemPriceListId,
                        principalTable: "GemPriceLists",
                        principalColumn: "GemPriceListId");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ProductThumbnail = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IsHome = table.Column<bool>(type: "bit", nullable: false),
                    IsSale = table.Column<bool>(type: "bit", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProcessingPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SellingCount = table.Column<int>(type: "int", nullable: false),
                    PercentSale = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    InspectionCertificateId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__B40CC6CDA1869B6D", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK__Products__Catego__01142BA1",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK__Products__Inspec__02084FDA",
                        column: x => x.InspectionCertificateId,
                        principalTable: "InspectionCertificates",
                        principalColumn: "InspectionCertificateId");
                });

            migrationBuilder.CreateTable(
                name: "MaterialPriceListDetails",
                columns: table => new
                {
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaterialPriceListId = table.Column<int>(type: "int", nullable: false),
                    EffectDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__B7B545C2C6B38165", x => new { x.MaterialId, x.MaterialPriceListId });
                    table.ForeignKey(
                        name: "FK__MaterialP__Mater__1AD3FDA4",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK__MaterialP__Mater__1BC821DD",
                        column: x => x.MaterialPriceListId,
                        principalTable: "MaterialPriceLists",
                        principalColumn: "MaterialPriceListId");
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeNewCatagoriesDetail",
                columns: table => new
                {
                    KnowledgeNewsID = table.Column<int>(type: "int", nullable: false),
                    KnowledgeNewCatagoryID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Knowledg__C25F12D4CF23B2A7", x => new { x.KnowledgeNewsID, x.KnowledgeNewCatagoryID });
                    table.ForeignKey(
                        name: "FK__Knowledge__Knowl__42E1EEFE",
                        column: x => x.KnowledgeNewsID,
                        principalTable: "KnowledgeNews",
                        principalColumn: "KnowledgeNewsID");
                    table.ForeignKey(
                        name: "FK__Knowledge__Knowl__43D61337",
                        column: x => x.KnowledgeNewCatagoryID,
                        principalTable: "KnowledgeNewCatagories",
                        principalColumn: "KnowledgeNewCatagoryID");
                });

            migrationBuilder.CreateTable(
                name: "CampaignDetail",
                columns: table => new
                {
                    PromotionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: false),
                    FromAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ToAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    MaxDiscount = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Campaign__26BC19331CE2CCBC", x => new { x.OrderId, x.PromotionId });
                    table.ForeignKey(
                        name: "FK__CampaignD__Order__3864608B",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK__CampaignD__Promo__37703C52",
                        column: x => x.PromotionId,
                        principalTable: "Promotions",
                        principalColumn: "PromotionId");
                });

            migrationBuilder.CreateTable(
                name: "Orders_Discounts",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders_D__BDD3AD1612975D9D", x => new { x.OrderId, x.DiscountId });
                    table.ForeignKey(
                        name: "FK__Orders_Di__Disco__31B762FC",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "DiscountId");
                    table.ForeignKey(
                        name: "FK__Orders_Di__Order__32AB8735",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PaymentTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payments__9B556A38A04DF308", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK__Payments__OrderI__2CF2ADDF",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Images__7516F70C3E996361", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK__Images__ProductI__04E4BC85",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    OrderId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    WarrantyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Total_Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__1AFE44BB57371514", x => new { x.OrderId, x.ProductId, x.WarrantyId });
                    table.ForeignKey(
                        name: "FK__OrderDeta__Order__29221CFB",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Produ__282DF8C2",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Warra__2A164134",
                        column: x => x.WarrantyId,
                        principalTable: "Warrantys",
                        principalColumn: "WarrantyId");
                });

            migrationBuilder.CreateTable(
                name: "Products_Collections",
                columns: table => new
                {
                    CollectionId = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    ProductId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__B6A6706821D3B801", x => new { x.CollectionId, x.ProductId });
                    table.ForeignKey(
                        name: "FK__Products___Colle__0D7A0286",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "CollectionId");
                    table.ForeignKey(
                        name: "FK__Products___Produ__0E6E26BF",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "Products_Gems",
                columns: table => new
                {
                    GemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    MainGemPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SubGemPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__3A4119EC56C37C09", x => new { x.GemId, x.ProductId });
                    table.ForeignKey(
                        name: "FK__Products___GemId__07C12930",
                        column: x => x.GemId,
                        principalTable: "Gems",
                        principalColumn: "GemId");
                    table.ForeignKey(
                        name: "FK__Products___Produ__08B54D69",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "Products_Materials",
                columns: table => new
                {
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__0E46DC9BC0B1F4E3", x => new { x.MaterialId, x.ProductId });
                    table.ForeignKey(
                        name: "FK__Products___Mater__1EA48E88",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK__Products___Produ__1F98B2C1",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "Products_WareHouses",
                columns: table => new
                {
                    WareHouseId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    Quantity_in_Stocks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__A2BF4C1407265BF9", x => new { x.WareHouseId, x.ProductId });
                    table.ForeignKey(
                        name: "FK__Products___Produ__14270015",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK__Products___WareH__1332DBDC",
                        column: x => x.WareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "WareHouseId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "([NormalizedName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDetail_PromotionId",
                table: "CampaignDetail",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_GemPriceListDetails_GemPriceListId",
                table: "GemPriceListDetails",
                column: "GemPriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ProductId",
                table: "Images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeNewCatagoriesDetail_KnowledgeNewCatagoryID",
                table: "KnowledgeNewCatagoriesDetail",
                column: "KnowledgeNewCatagoryID");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeNews_WriterId",
                table: "KnowledgeNews",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialPriceListDetails_MaterialPriceListId",
                table: "MaterialPriceListDetails",
                column: "MaterialPriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_News_Id",
                table: "News",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_WarrantyId",
                table: "OrderDetails",
                column: "WarrantyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShipperId",
                table: "Orders",
                column: "ShipperId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Discounts_DiscountId",
                table: "Orders_Discounts",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_InspectionCertificateId",
                table: "Products",
                column: "InspectionCertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Collections_ProductId",
                table: "Products_Collections",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Gems_ProductId",
                table: "Products_Gems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Materials_ProductId",
                table: "Products_Materials",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_WareHouses_ProductId",
                table: "Products_WareHouses",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abouts");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CampaignDetail");

            migrationBuilder.DropTable(
                name: "GemPriceListDetails");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "KnowledgeNewCatagoriesDetail");

            migrationBuilder.DropTable(
                name: "MaterialPriceListDetails");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders_Discounts");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.DropTable(
                name: "Products_Collections");

            migrationBuilder.DropTable(
                name: "Products_Gems");

            migrationBuilder.DropTable(
                name: "Products_Materials");

            migrationBuilder.DropTable(
                name: "Products_WareHouses");

            migrationBuilder.DropTable(
                name: "Slides");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "GemPriceLists");

            migrationBuilder.DropTable(
                name: "KnowledgeNews");

            migrationBuilder.DropTable(
                name: "KnowledgeNewCatagories");

            migrationBuilder.DropTable(
                name: "MaterialPriceLists");

            migrationBuilder.DropTable(
                name: "Warrantys");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "Gems");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "WareHouses");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "InspectionCertificates");
        }
    }
}
