using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class KnowledgeNewCatagory
{
    public int KnowledgeNewCatagoryId { get; set; }

    public string KnowledgeNewCatagoriesName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<KnowledgeNews> KnowledgeNews{ get; set; } = new List<KnowledgeNews>();
}
