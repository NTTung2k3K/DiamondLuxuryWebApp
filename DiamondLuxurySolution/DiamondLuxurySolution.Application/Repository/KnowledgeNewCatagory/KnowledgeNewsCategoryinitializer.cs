using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.KnowledgeNewCatagory
{
    public class KnowledgeNewsCategoryinitializer : IKnowledgeNewsCategoryinitializer
    {
        private readonly LuxuryDiamondShopContext _context;
        public KnowledgeNewsCategoryinitializer(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task CreateDefaultKnowledgeNewsCategory()
        {
            var categoryFirst = await _context.KnowledgeNewCatagories.Where(x => x.KnowledgeNewCatagoriesName == DiamondLuxurySolution.Utilities.Constants.Systemconstant.KnowledgeNewsCategoryNameDefault.JewelryKnowledgeNewsCategoryName.ToString()).ToListAsync();
            if(categoryFirst.Count() <= 0 )
            {
                var knowledgeNewsCategoryJewelry = new DiamondLuxurySolution.Data.Entities.KnowledgeNewCatagory()
                {
                    KnowledgeNewCatagoriesName = DiamondLuxurySolution.Utilities.Constants.Systemconstant.KnowledgeNewsCategoryNameDefault.JewelryKnowledgeNewsCategoryName.ToString(),
                    Description = "Thông tin về kiến thức trang sức",
                };
                await _context.KnowledgeNewCatagories.AddAsync(knowledgeNewsCategoryJewelry);
            }
            var categorySecond = await _context.KnowledgeNewCatagories.Where(x => x.KnowledgeNewCatagoriesName == DiamondLuxurySolution.Utilities.Constants.Systemconstant.KnowledgeNewsCategoryNameDefault.DiamondKnowledgeNewsCategoryName.ToString()).ToListAsync();
            if (categorySecond.Count() <= 0)
            {
                var knowledgeNewsCategoryDiamond = new DiamondLuxurySolution.Data.Entities.KnowledgeNewCatagory()
                {
                    KnowledgeNewCatagoriesName = DiamondLuxurySolution.Utilities.Constants.Systemconstant.KnowledgeNewsCategoryNameDefault.DiamondKnowledgeNewsCategoryName.ToString(),
                    Description = "Thông tin về kiến thức kim cương",
                };
                await _context.KnowledgeNewCatagories.AddAsync(knowledgeNewsCategoryDiamond);
            }
            await _context.SaveChangesAsync();
        }
    }
}
