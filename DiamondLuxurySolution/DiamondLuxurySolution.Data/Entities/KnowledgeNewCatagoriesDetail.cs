using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class KnowledgeNewCatagoriesDetail
{
    public int KnowledgeNewsId { get; set; }

    public int KnowledgeNewCatagoryId { get; set; }

    public string? Description { get; set; }

    public virtual KnowledgeNewCatagory KnowledgeNewCatagory { get; set; } = null!;

    public virtual KnowledgeNews KnowledgeNews { get; set; } = null!;
}
