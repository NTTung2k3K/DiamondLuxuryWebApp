using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class KnowledgeNews
{
    public int KnowledgeNewsId { get; set; }

    public string KnowledgeNewsName { get; set; } = null!;

    public string? Thumnail { get; set; }

    public string? Description { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public Guid? WriterId { get; set; }

    public virtual ICollection<KnowledgeNewCatagoriesDetail> KnowledgeNewCatagoriesDetails { get; set; } = new List<KnowledgeNewCatagoriesDetail>();

    public virtual AppUser? Writer { get; set; }
}
