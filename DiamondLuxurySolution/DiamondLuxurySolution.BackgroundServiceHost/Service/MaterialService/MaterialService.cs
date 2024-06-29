using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DiamondLuxurySolution.BackgroundServiceHost.Service.MaterialService
{
    public class MaterialService : IMaterialService
    {
        private readonly LuxuryDiamondShopContext _context;
        public MaterialService(LuxuryDiamondShopContext context)
        {
            _context = context;
        }

        public async Task CreateDefaultMaterial(CancellationToken cancellationToken)
        {
            string url = "https://www.pnj.com.vn/blog/gia-vang/?r=1719498356146";
            List<Material> materialPrices = new List<Material>();

            // Initialize WebDriver
            var options = new ChromeOptions();
            options.AddArgument("headless");
            using (var driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(url);
                await Task.Delay(5000); // Wait for the page to fully load

                // Tìm các hàng trong bảng
                var rows = driver.FindElements(By.XPath("//tbody//tr"));

                // Các loại vàng cần cào giá
                string[] goldTypes = { "18K", "14K", "10K", "22K", "9K" };

                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));

                    if (cells.Count > 0)
                    {
                        string goldType = cells[0].Text.Trim();
                        foreach (var type in goldTypes)
                        {
                            if (goldType.Contains(type))
                            {
                                if (type.Equals("18K"))
                                {
                                    if (decimal.TryParse(cells[2].Text.Trim().Replace(",", ""), out decimal sellPrice))
                                    {
                                        Material materialPrice = new Material
                                        {
                                            MaterialId = Guid.NewGuid(),
                                            MaterialName = "Vàng 18K",
                                            Description = "",
                                            Color = "Vàng",
                                            MaterialImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fvang-18K.png?alt=media&token=acba8254-1253-46d6-a492-8a62eef08d51",
                                            Price = sellPrice * 1000,
                                            Status = true,
                                            EffectDate = DateTime.Now
                                        };

                                        materialPrices.Add(materialPrice);
                                    }
                                    break; // Exit the inner loop once a match is found
                                } 
                                else if (type.Equals("14K"))
                                {
                                    if (decimal.TryParse(cells[2].Text.Trim().Replace(",", ""), out decimal sellPrice))
                                    {
                                        Material materialPrice = new Material
                                        {
                                            MaterialId = Guid.NewGuid(),
                                            MaterialName = "Vàng 14K",
                                            Description = "",
                                            Color = "Vàng",
                                            MaterialImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fvang-14K.png?alt=media&token=95aa8a00-3fc8-4e0d-b5d1-6ee46d110b11",
                                            Price = sellPrice * 1000,
                                            Status = true,
                                            EffectDate = DateTime.Now
                                        };

                                        materialPrices.Add(materialPrice);
                                    }
                                    break; // Exit the inner loop once a match is found
                                }
                                else if (type.Equals("10K"))
                                {
                                    if (decimal.TryParse(cells[2].Text.Trim().Replace(",", ""), out decimal sellPrice))
                                    {
                                        Material materialPrice = new Material
                                        {
                                            MaterialId = Guid.NewGuid(),
                                            MaterialName = "Vàng 10K",
                                            Description = "",
                                            Color = "Vàng",
                                            MaterialImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fvang-10k.png?alt=media&token=daca48a1-0f9a-4a36-8afe-894f8c153824",
                                            Price = sellPrice * 1000,
                                            Status = true,
                                            EffectDate = DateTime.Now
                                        };

                                        materialPrices.Add(materialPrice);
                                    }
                                    break; // Exit the inner loop once a match is found
                                }
                                else if (type.Equals("22K"))
                                {
                                    if (decimal.TryParse(cells[2].Text.Trim().Replace(",", ""), out decimal sellPrice))
                                    {
                                        Material materialPrice = new Material
                                        {
                                            MaterialId = Guid.NewGuid(),
                                            MaterialName = "Vàng 22K",
                                            Description = "",
                                            Color = "Vàng",
                                            MaterialImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fvang-22K.png?alt=media&token=84cd5bdc-a514-4a9f-ae30-c1a3f517f504",
                                            Price = sellPrice * 1000,
                                            Status = true,
                                            EffectDate = DateTime.Now
                                        };

                                        materialPrices.Add(materialPrice);
                                    }
                                    break; // Exit the inner loop once a match is found
                                }
                                else if (type.Equals("9K"))
                                {
                                    if (decimal.TryParse(cells[2].Text.Trim().Replace(",", ""), out decimal sellPrice))
                                    {
                                        Material materialPrice = new Material
                                        {
                                            MaterialId = Guid.NewGuid(),
                                            MaterialName = "Vàng 9K",
                                            Description = "",
                                            Color = "Vàng",
                                            MaterialImage = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2Fvang-9K.png?alt=media&token=d6b3698f-d8b6-4679-946a-17b10a8d2f8b",
                                            Price = sellPrice * 1000,
                                            Status = true,
                                            EffectDate = DateTime.Now
                                        };

                                        materialPrices.Add(materialPrice);
                                    }
                                    break; // Exit the inner loop once a match is found
                                }
                            }
                        }
                    }
                }
            }

            // Lưu dữ liệu vào database
            foreach (var material in materialPrices)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var existingMaterial = _context.Materials
                    .SingleOrDefault(m => m.MaterialName == material.MaterialName
                                          );

                if (existingMaterial != null)
                {
                    // Cập nhật giá trị nếu tồn tại
                    existingMaterial.Price = material.Price;
                    existingMaterial.EffectDate = DateTime.Now;
                }
                else
                {
                    // Thêm mới nếu không tồn tại
                    _context.Materials.Add(material);
                }
            }

            _context.SaveChanges();

            Console.WriteLine("Update Material Complete");
        }
    }
}

