using System;
using System.Collections.Generic;
using DiamondLuxurySolution.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiamondLuxurySolution.Data.EF;

public partial class LuxuryDiamondShopContext : DbContext
{
    public LuxuryDiamondShopContext()
    {
    }

    public LuxuryDiamondShopContext(DbContextOptions<LuxuryDiamondShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<About> Abouts { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<CampaignDetail> CampaignDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Gem> Gems { get; set; }

    public virtual DbSet<GemPriceList> GemPriceLists { get; set; }

    public virtual DbSet<GemPriceListDetail> GemPriceListDetails { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<InspectionCertificate> InspectionCertificates { get; set; }

    public virtual DbSet<KnowledgeNewCatagoriesDetail> KnowledgeNewCatagoriesDetails { get; set; }

    public virtual DbSet<KnowledgeNewCatagory> KnowledgeNewCatagories { get; set; }

    public virtual DbSet<KnowledgeNews> KnowledgeNews { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialPriceList> MaterialPriceLists { get; set; }

    public virtual DbSet<MaterialPriceListDetail> MaterialPriceListDetails { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Platform> Platforms { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsCollection> ProductsCollections { get; set; }

    public virtual DbSet<ProductsGem> ProductsGems { get; set; }

    public virtual DbSet<ProductsMaterial> ProductsMaterials { get; set; }

    public virtual DbSet<ProductsWareHouse> ProductsWareHouses { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Slide> Slides { get; set; }

    public virtual DbSet<WareHouse> WareHouses { get; set; }

    public virtual DbSet<Warranty> Warrantys { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-AEVH89JM\\TUNGK;Initial Catalog=LuxuryDiamondShop;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<About>(entity =>
        {
            entity.HasKey(e => e.AboutId).HasName("PK__Abouts__717FC93C4893C36F");

            entity.Property(e => e.AboutName).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(250);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<CampaignDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.PromotionId }).HasName("PK__Campaign__26BC19331CE2CCBC");

            entity.ToTable("CampaignDetail");

            entity.Property(e => e.OrderId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FromAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaxDiscount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ToAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.CampaignDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CampaignD__Order__3864608B");

            entity.HasOne(d => d.Promotion).WithMany(p => p.CampaignDetails)
                .HasForeignKey(d => d.PromotionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CampaignD__Promo__37703C52");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B0B6F138B");

            entity.Property(e => e.CategoryName).HasMaxLength(250);
            entity.Property(e => e.CategoryType).HasMaxLength(250);
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.CollectionId).HasName("PK__Collecti__7DE6BC04F05F28C3");

            entity.Property(e => e.CollectionId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CollectionName).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(250);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__Discount__E43F6D964D50BE3B");

            entity.Property(e => e.DiscountId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.DiscountImage).HasMaxLength(250);
            entity.Property(e => e.DiscountName).HasMaxLength(250);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Gem>(entity =>
        {
            entity.HasKey(e => e.GemId).HasName("PK__Gems__F101D5804FDDCABA");

            entity.Property(e => e.GemId).ValueGeneratedNever();
            entity.Property(e => e.Polish).HasMaxLength(250);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Proportion).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Symetry).HasMaxLength(250);
            entity.Property(e => e._4c).HasColumnName("4C");
        });

        modelBuilder.Entity<GemPriceList>(entity =>
        {
            entity.HasKey(e => e.GemPriceListId).HasName("PK__GemPrice__EFF6BEE083204377");

            entity.Property(e => e.CaratWeight)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Clarity)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Cut)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<GemPriceListDetail>(entity =>
        {
            entity.HasKey(e => new { e.GemId, e.GemPriceListId }).HasName("PK__GemPrice__EFFEBE6E32CA28A7");

            entity.Property(e => e.EffectDate).HasColumnType("datetime");

            entity.HasOne(d => d.Gem).WithMany(p => p.GemPriceListDetails)
                .HasForeignKey(d => d.GemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GemPriceL__GemId__797309D9");

            entity.HasOne(d => d.GemPriceList).WithMany(p => p.GemPriceListDetails)
                .HasForeignKey(d => d.GemPriceListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GemPriceL__GemPr__7A672E12");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Images__7516F70C3E996361");

            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.ProductId)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.Images)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Images__ProductI__04E4BC85");
        });

        modelBuilder.Entity<InspectionCertificate>(entity =>
        {
            entity.HasKey(e => e.InspectionCertificateId).HasName("PK__Inspecti__23BDD56CAB56582C");

            entity.Property(e => e.InspectionCertificateId)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.DateGrading).HasColumnType("datetime");
            entity.Property(e => e.InspectionCertificateName).HasMaxLength(250);
        });

        modelBuilder.Entity<KnowledgeNewCatagoriesDetail>(entity =>
        {
            entity.HasKey(e => new { e.KnowledgeNewsId, e.KnowledgeNewCatagoryId }).HasName("PK__Knowledg__C25F12D4CF23B2A7");

            entity.ToTable("KnowledgeNewCatagoriesDetail");

            entity.Property(e => e.KnowledgeNewsId).HasColumnName("KnowledgeNewsID");
            entity.Property(e => e.KnowledgeNewCatagoryId).HasColumnName("KnowledgeNewCatagoryID");

            entity.HasOne(d => d.KnowledgeNewCatagory).WithMany(p => p.KnowledgeNewCatagoriesDetails)
                .HasForeignKey(d => d.KnowledgeNewCatagoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Knowledge__Knowl__43D61337");

            entity.HasOne(d => d.KnowledgeNews).WithMany(p => p.KnowledgeNewCatagoriesDetails)
                .HasForeignKey(d => d.KnowledgeNewsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Knowledge__Knowl__42E1EEFE");
        });

        modelBuilder.Entity<KnowledgeNewCatagory>(entity =>
        {
            entity.HasKey(e => e.KnowledgeNewCatagoryId).HasName("PK__Knowledg__69B8D362765095E8");

            entity.Property(e => e.KnowledgeNewCatagoryId).HasColumnName("KnowledgeNewCatagoryID");
            entity.Property(e => e.KnowledgeNewCatagoriesName).HasMaxLength(250);
        });

        modelBuilder.Entity<KnowledgeNews>(entity =>
        {
            entity.HasKey(e => e.KnowledgeNewsId).HasName("PK__Knowledg__F4C49FE2953AEFFC");

            entity.Property(e => e.KnowledgeNewsId).HasColumnName("KnowledgeNewsID");
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.KnowledgeNewsName).HasMaxLength(250);

            entity.HasOne(d => d.Writer).WithMany(p => p.KnowledgeNews)
                .HasForeignKey(d => d.WriterId)
                .HasConstraintName("FK__Knowledge__Write__40058253");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Material__C50610F7A4B8B8A5");

            entity.Property(e => e.MaterialId).ValueGeneratedNever();
            entity.Property(e => e.Color).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.MaterialName).HasMaxLength(250);
            entity.Property(e => e.SubMaterial).HasMaxLength(250);
        });

        modelBuilder.Entity<MaterialPriceList>(entity =>
        {
            entity.HasKey(e => e.MaterialPriceListId).HasName("PK__Material__2B355356E5A81769");

            entity.Property(e => e.BuyPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SellPrice).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<MaterialPriceListDetail>(entity =>
        {
            entity.HasKey(e => new { e.MaterialId, e.MaterialPriceListId }).HasName("PK__Material__B7B545C2C6B38165");

            entity.Property(e => e.EffectDate).HasColumnType("datetime");

            entity.HasOne(d => d.Material).WithMany(p => p.MaterialPriceListDetails)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MaterialP__Mater__1AD3FDA4");

            entity.HasOne(d => d.MaterialPriceList).WithMany(p => p.MaterialPriceListDetails)
                .HasForeignKey(d => d.MaterialPriceListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MaterialP__Mater__1BC821DD");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF39002B82C");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.NewName).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.News)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK__News__Id__3B40CD36");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF2AC66985");

            entity.Property(e => e.OrderId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.ShipAdress).HasMaxLength(250);
            entity.Property(e => e.ShipEmail).HasMaxLength(250);
            entity.Property(e => e.ShipName).HasMaxLength(250);
            entity.Property(e => e.ShipPhoneNumber).HasMaxLength(250);
            entity.Property(e => e.ShipPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalAmout)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Amout");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderCustomers)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__Customer__236943A5");

            entity.HasOne(d => d.Shipper).WithMany(p => p.OrderShippers)
                .HasForeignKey(d => d.ShipperId)
                .HasConstraintName("FK__Orders__ShipperI__22751F6C");

            entity.HasMany(d => d.Discounts).WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "OrdersDiscount",
                    r => r.HasOne<Discount>().WithMany()
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Orders_Di__Disco__31B762FC"),
                    l => l.HasOne<Order>().WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Orders_Di__Order__32AB8735"),
                    j =>
                    {
                        j.HasKey("OrderId", "DiscountId").HasName("PK__Orders_D__BDD3AD1612975D9D");
                        j.ToTable("Orders_Discounts");
                        j.IndexerProperty<string>("OrderId")
                            .HasMaxLength(20)
                            .IsUnicode(false);
                    });
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId, e.WarrantyId }).HasName("PK__OrderDet__1AFE44BB57371514");

            entity.Property(e => e.OrderId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Price");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__29221CFB");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Produ__282DF8C2");

            entity.HasOne(d => d.Warranty).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.WarrantyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Warra__2A164134");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38A04DF308");

            entity.Property(e => e.PaymentId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Message).HasMaxLength(250);
            entity.Property(e => e.OrderId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMethod).HasMaxLength(250);
            entity.Property(e => e.PaymentTime).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Payments__OrderI__2CF2ADDF");
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.PlatformId).HasName("PK__Platform__F559F6FABA63124D");

            entity.Property(e => e.PlatformName).HasMaxLength(250);
            entity.Property(e => e.PlatformUrl).HasColumnName("PlatformURL");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CDA1869B6D");

            entity.Property(e => e.ProductId)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.DateCreate).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.InspectionCertificateId)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProcessingPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(250);
            entity.Property(e => e.ProductThumbnail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SellingPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__01142BA1");

            entity.HasOne(d => d.InspectionCertificate).WithMany(p => p.Products)
                .HasForeignKey(d => d.InspectionCertificateId)
                .HasConstraintName("FK__Products__Inspec__02084FDA");
        });

        modelBuilder.Entity<ProductsCollection>(entity =>
        {
            entity.HasKey(e => new { e.CollectionId, e.ProductId }).HasName("PK__Products__B6A6706821D3B801");

            entity.ToTable("Products_Collections");

            entity.Property(e => e.CollectionId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(250);

            entity.HasOne(d => d.Collection).WithMany(p => p.ProductsCollections)
                .HasForeignKey(d => d.CollectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products___Colle__0D7A0286");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsCollections)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products___Produ__0E6E26BF");
        });

        modelBuilder.Entity<ProductsGem>(entity =>
        {
            entity.HasKey(e => new { e.GemId, e.ProductId }).HasName("PK__Products__3A4119EC56C37C09");

            entity.ToTable("Products_Gems");

            entity.Property(e => e.ProductId)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.MainGemPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubGemPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Gem).WithMany(p => p.ProductsGems)
                .HasForeignKey(d => d.GemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products___GemId__07C12930");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsGems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products___Produ__08B54D69");
        });

        modelBuilder.Entity<ProductsMaterial>(entity =>
        {
            entity.HasKey(e => new { e.MaterialId, e.ProductId }).HasName("PK__Products__0E46DC9BC0B1F4E3");

            entity.ToTable("Products_Materials");

            entity.Property(e => e.ProductId)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Material).WithMany(p => p.ProductsMaterials)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products___Mater__1EA48E88");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsMaterials)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products___Produ__1F98B2C1");
        });

        modelBuilder.Entity<ProductsWareHouse>(entity =>
        {
            entity.HasKey(e => new { e.WareHouseId, e.ProductId }).HasName("PK__Products__A2BF4C1407265BF9");

            entity.ToTable("Products_WareHouses");

            entity.Property(e => e.ProductId)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.QuantityInStocks).HasColumnName("Quantity_in_Stocks");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsWareHouses)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products___Produ__14270015");

            entity.HasOne(d => d.WareHouse).WithMany(p => p.ProductsWareHouses)
                .HasForeignKey(d => d.WareHouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products___WareH__1332DBDC");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__52C42FCF3F8664C3");

            entity.Property(e => e.PromotionId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.PromotionName).HasMaxLength(250);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Slide>(entity =>
        {
            entity.HasKey(e => e.SlideId).HasName("PK__Slides__9E7CB650F4EBAD0A");

            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.SlideName).HasMaxLength(250);
            entity.Property(e => e.SlideUrl).HasColumnName("SlideURL");
        });

        modelBuilder.Entity<WareHouse>(entity =>
        {
            entity.HasKey(e => e.WareHouseId).HasName("PK__WareHous__69FF807867312D67");

            entity.Property(e => e.AcquisitionDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Location).HasMaxLength(250);
            entity.Property(e => e.WareHouseName).HasMaxLength(250);
        });

        modelBuilder.Entity<Warranty>(entity =>
        {
            entity.HasKey(e => e.WarrantyId).HasName("PK__Warranty__2ED31813DFECD7DE");

            entity.Property(e => e.WarrantyId).ValueGeneratedNever();
            entity.Property(e => e.DateActive).HasColumnType("datetime");
            entity.Property(e => e.DateExpired).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.WarrantyName).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
