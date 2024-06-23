using DiamondLuxurySolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.About
{
    public class AboutInitialize : IAboutInitialize
    {
        private readonly LuxuryDiamondShopContext _context;
        public AboutInitialize(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task CreateDefaultAbout()
        {
            var about = new DiamondLuxurySolution.Data.Entities.About {
                AboutName = "Diamond Luxury",
                AboutAddress = "Vinhomes Grand Park, Quận 9",
                AboutEmail = "DiamondLuxuryService@gmail.com",
                AboutPhoneNumber = "0987654321",
                Status = true
            };
            var checkExist = await _context.Abouts.Where(x => x.AboutName == about.AboutName).ToListAsync();
            if (!checkExist.Any())
            {
                _context.Abouts.Add(about);
                await _context.SaveChangesAsync();
            }
        }
    }
}
