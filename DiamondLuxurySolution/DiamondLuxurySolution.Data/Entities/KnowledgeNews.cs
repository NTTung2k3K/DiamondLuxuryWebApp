using System;
using System.Collections.Generic;

namespace DiamondLuxurySolution.Data.Entities;

public partial class KnowledgeNews
{
    public int KnowledgeNewsId { get; set; }

    public string? KnowledgeNewsName { get; set; }

    public string? Thumnail { get; set; }

    public string? Description { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

    public Guid WriterId { get; set; }
    public int KnowledgeNewCatagoryId { get; set; }
    public virtual KnowledgeNewCatagory? KnowledgeNewCatagory { get; set; }

    public virtual AppUser? Writer { get; set; }
    public bool Active {  get; set; }
}
