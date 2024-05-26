using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class Product
{
    public string ProductId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public string ProductThumbnail { get; set; } = null!;

    public bool IsHome { get; set; }

    public bool IsSale { get; set; }

    public DateTime DateCreate { get; set; }

    public DateTime DateModified { get; set; }


    public decimal OriginalPrice { get; set; }

    public decimal SellingPrice { get; set; }

    public int SellingCount { get; set; }

    public int PercentSale { get; set; }
    public int Quantity { get; set; }

    public int? CategoryId { get; set; }
    public string Status { get; set; }

    public string? InspectionCertificateId { get; set; }

    public virtual Category? Category { get; set; }
    public Guid? MaterialId { get; set; }

    public Guid GemId { get; set; }
    public Material Material { get; set; }

    public Gem Gem { get; set; } 

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public InspectionCertificate InspectionCertificate { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductsCollection> ProductsCollections { get; set; } = new List<ProductsCollection>();
<<<<<<< HEAD

=======
>>>>>>> 82106bed7b2dca6b6a9cc681d7adbedf5ad8dd6a
    public virtual ICollection<SubGemDetail> SubGemDetails { get; set; } = new List<SubGemDetail>();

    public int WarehouseId { get; set; }
    public WareHouse WareHouse { get; set; }
}
