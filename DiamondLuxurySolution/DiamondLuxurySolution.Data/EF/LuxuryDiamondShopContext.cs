using System;
using System.Collections.Generic;
using DiamondLuxurySolution.Data.Configurations;
using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiamondLuxurySolution.Data.EF;

public partial class LuxuryDiamondShopContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public LuxuryDiamondShopContext()
    {
    }

    public LuxuryDiamondShopContext(DbContextOptions<LuxuryDiamondShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<About> Abouts { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }
    public virtual DbSet<AppRole> AppRoles { get; set; }

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

 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new AboutConfiguration());
        modelBuilder.ApplyConfiguration(new CampaignDetailConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new CollectionConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new GemConfiguration());
        modelBuilder.ApplyConfiguration(new GemPriceListConfiguration());
        modelBuilder.ApplyConfiguration(new GemPriceListDetailConfiguration());
        modelBuilder.ApplyConfiguration(new ImageConfiguration());
        modelBuilder.ApplyConfiguration(new InspectionCertificateConfiguration());
        modelBuilder.ApplyConfiguration(new KnowledgeNewCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new KnowledgeNewCategoryDetailConfiguration());
        modelBuilder.ApplyConfiguration(new KnowledgeNewsConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialPriceListConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialPriceListDetailConfiguration());
        modelBuilder.ApplyConfiguration(new NewsConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDiscountConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDetailsConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new PlatformConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCollectionConfiguration());
        modelBuilder.ApplyConfiguration(new ProductGemConfiguration());
        modelBuilder.ApplyConfiguration(new ProductMaterialConfiguration());
        modelBuilder.ApplyConfiguration(new ProductsWareHousesConfiguration());
        modelBuilder.ApplyConfiguration(new PromotionConfiguration());
        modelBuilder.ApplyConfiguration(new SlideConfiguration());
        modelBuilder.ApplyConfiguration(new WareHouseConfiguration());
        modelBuilder.ApplyConfiguration(new WarrantyConfiguration());



        modelBuilder.ApplyConfiguration(new AboutConfiguration());
        modelBuilder.ApplyConfiguration(new AboutConfiguration());
        modelBuilder.ApplyConfiguration(new AboutConfiguration());




        modelBuilder.ApplyConfiguration(new AppUserConfiguration());
        modelBuilder.ApplyConfiguration(new AppRoleConfiguration());


        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

        base.OnModelCreating(modelBuilder);
    }

}
