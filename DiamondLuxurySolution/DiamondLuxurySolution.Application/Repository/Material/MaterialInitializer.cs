using DiamondLuxurySolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Material
{
    public class MaterialInitializer : IMaterialInitializer
    {
        private readonly LuxuryDiamondShopContext _context;
        public MaterialInitializer(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task CreateDefaultMaterial()
        {
            var Gold10K = DiamondLuxurySolution.Utilities.Constants.Systemconstant.MaterialNameDefault.Gold10K.ToString();
            var Gold14K = DiamondLuxurySolution.Utilities.Constants.Systemconstant.MaterialNameDefault.Gold14K.ToString();
            var Gold18K = DiamondLuxurySolution.Utilities.Constants.Systemconstant.MaterialNameDefault.Gold18K.ToString();
            var Platium850 = DiamondLuxurySolution.Utilities.Constants.Systemconstant.MaterialNameDefault.Platium850.ToString();
            var Platium950 = DiamondLuxurySolution.Utilities.Constants.Systemconstant.MaterialNameDefault.Platium950.ToString();

            var materialCount = await _context.Materials.ToListAsync();
            if (materialCount.Count == 0)
            {
                var AddGold10K = new Data.Entities.Material()
                {
                    MaterialName = Gold10K,
                    EffectDate = DateTime.Now,
                    Price = 3090000,
                    Color = "Vàng",
                    Status = true,
                    MaterialImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fvang-10k.png?alt=media&token=daca48a1-0f9a-4a36-8afe-894f8c153824"
                };

                var status = await _context.Materials.AddAsync(AddGold10K);

                var AddGold14K = new Data.Entities.Material()
                {
                    MaterialName = Gold14K,
                    EffectDate = DateTime.Now,
                    Price = 4340000,
                    Color = "Vàng Tây",
                    Status = true,
                    MaterialImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fvang-14K.png?alt=media&token=95aa8a00-3fc8-4e0d-b5d1-6ee46d110b11",
                };

                status = await _context.Materials.AddAsync(AddGold14K);

                var AddGold18K = new Data.Entities.Material()
                {
                    MaterialName = Gold18K,
                    EffectDate = DateTime.Now,
                    Price = 5240000,
                    Color = "Vàng",
                    Status = true,
                    MaterialImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fvang-18K.png?alt=media&token=acba8254-1253-46d6-a492-8a62eef08d51",
                };

                status = await _context.Materials.AddAsync(AddGold18K);

                var AddPlatium850 = new Data.Entities.Material()
                {
                    MaterialName = Platium850,
                    EffectDate = DateTime.Now,
                    Price = 3654000,
                    Color = "Vàng Trắng",
                    Status = true,
                    MaterialImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fplatium850.png?alt=media&token=d0e3ee94-e530-4620-abc5-5add97692be4",
                };

                status = await _context.Materials.AddAsync(AddPlatium850);

                var AddPlatium950 = new Data.Entities.Material()
                {
                    MaterialName = Platium950,
                    EffectDate = DateTime.Now,
                    Price = 5410000,
                    Color = "Vàng Trắng",
                    Status = true,
                    MaterialImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fplatium950.png?alt=media&token=96344b31-ab1a-4487-88e5-b8b7c3ecb2db",
                };

                status = await _context.Materials.AddAsync(AddPlatium950);


                _context.SaveChanges();
            }
        }
    }
}
