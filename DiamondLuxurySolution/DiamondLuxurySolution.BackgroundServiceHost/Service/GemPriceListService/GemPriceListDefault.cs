using DiamondLuxurySolution.Data.EF;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.BackgroundServiceHost.Service.GemPriceListService
{
    public class GemPriceListDefault : IGemPriceListDefault
    {
        private readonly LuxuryDiamondShopContext _context;
        public GemPriceListDefault(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task CreateDefaultGemPriceList(CancellationToken cancellationToken)
        {
            string url = "https://caohungdiamond.com/bang-gia-kim-cuong/";

            // Khởi tạo HttpClient
            HttpClient client = new HttpClient();

            // Gửi yêu cầu GET đến URL
            HttpResponseMessage response = await client.GetAsync(url);

            // Đọc nội dung của phản hồi
            string htmlContent = await response.Content.ReadAsStringAsync();

            // Load HTML vào HtmlDocument
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            // List để lưu trữ giá kim cương
            List<Data.Entities.GemPriceList> diamondPrices = new List<Data.Entities.GemPriceList>();

            // Các giá trị mặc định của carat
            string[] caratValues = new string[]
            {
                "3.6 MM", "3.9 MM", "4.1 MM", "4.5 MM", "5.0 MM", "5.2 MM", "5.3 MM",
                "5.4 MM", "6.0 MM", "6.2 MM", "6.3 MM(<1CT)", "6.3 MM(>1CT)", "6.8 MM",
                "7.2 MM", "8.1 MM", "9.0 MM(<3CT)"
            };

            // Phân tích và trích xuất dữ liệu từ HTML
            HtmlNodeCollection tables = htmlDocument.DocumentNode.SelectNodes("//table");

            int caratIndex = 0;
            foreach (var table in tables)
            {
                var rows = table.SelectNodes(".//tbody//tr");

                if (rows == null)
                {
                    continue;
                }

                foreach (var row in rows)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var cells = row.SelectNodes("td");

                    if (cells != null && cells.Count == 6)
                    {
                        string carat = caratValues[caratIndex];
                        string color = cells[0].InnerText.Trim();
                        string[] prices = new string[5];
                        for (int i = 1; i < cells.Count; i++)
                        {
                            prices[i - 1] = cells[i].InnerText.Trim();
                        }

                        string[] clarities = { "IF", "VVS1", "VVS2", "VS1", "VS2" };
                        for (int i = 0; i < clarities.Length; i++)
                        {
                            // Chuyển đổi giá trị Price từ chuỗi sang decimal
                            if (decimal.TryParse(prices[i].Replace(",", "").Replace(".", ""), out decimal price))
                            {
                                Data.Entities.GemPriceList diamond = new Data.Entities.GemPriceList
                                {
                                    CaratWeight = carat,
                                    Color = color,
                                    Clarity = clarities[i],
                                    Cut = "Good",
                                    Price = price,
                                    Active = true,
                                    effectDate = DateTime.Now,
                                };

                                diamondPrices.Add(diamond);
                            }
                        }
                    }
                }

                caratIndex++;
                if (caratIndex >= caratValues.Length)
                    break;
            }


            foreach (var diamond in diamondPrices)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var existingDiamond = _context.GemPriceLists
                    .SingleOrDefault(d => d.CaratWeight == diamond.CaratWeight &&
                                          d.Color == diamond.Color &&
                                          d.Clarity == diamond.Clarity &&
                                          d.Cut == diamond.Cut);

                if (existingDiamond != null)
                {
                    // Cập nhật giá trị Price nếu tồn tại và effectDate không phải là ngày hiện tại
                    if (existingDiamond.effectDate.Date != DateTime.Now.Date)
                    {
                        existingDiamond.Price = diamond.Price;
                        existingDiamond.effectDate = diamond.effectDate;
                    }
                }
                else
                {
                    // Thêm mới nếu không tồn tại
                    _context.GemPriceLists.Add(diamond);
                }
            }

            _context.SaveChanges();

            await Console.Out.WriteLineAsync("Done Update GemPriceList");
        }

        public async Task CreateDefaultGemPriceListNatural(CancellationToken cancellationToken)
        {
            string url = "https://trangsucdaquy.vn/bang-gia-kim-cuong-nhan-tao-moissanite-chinh-hang-gra/";

            // Khởi tạo HttpClient
            HttpClient client = new HttpClient();

            // Gửi yêu cầu GET đến URL
            HttpResponseMessage response = await client.GetAsync(url);

            // Đọc nội dung của phản hồi
            string htmlContent = await response.Content.ReadAsStringAsync();

            // Load HTML vào HtmlDocument của HtmlAgilityPack
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            // List để lưu trữ giá kim cương
            List<Data.Entities.GemPriceList> diamondPrices = new List<Data.Entities.GemPriceList>();

            // Các giá trị caratWeight cần lấy
            string[] caratValues = new string[]
            {
            "6.3", "6.0", "5.5", "4.0", "8.0", "8.5"
            };

            // Các màu sắc cần lấy
            string[] colors = { "Trắng", "Xanh Lá", "Xanh Dương", "Vàng" };

            // Phân tích và trích xuất dữ liệu từ HTML
            HtmlNodeCollection tables = htmlDocument.DocumentNode.SelectNodes("//table");

            if (tables != null && tables.Count > 0)
            {
                var rows = tables[0].SelectNodes(".//tbody//tr");

                if (rows != null)
                {

                    foreach (var row in rows)
                    {
                        var cells = row.SelectNodes("td");

                        if (cells != null) // Kiểm tra có đủ cột (kích thước và giá)
                        {
                            string size = cells[0].InnerText.Trim();

                            // Chỉ xử lý nếu kích thước nằm trong các giá trị cần lấy
                            if (caratValues.Contains(size))
                            {
                                for (int i = 0; i < colors.Length; i++)
                                {
                                    string color = colors[i];
                                    string priceString = cells[i + 2].InnerText.Trim(); // Chỉnh sửa để khớp với chỉ số cột của price

                                    decimal price;
                                    if (decimal.TryParse(priceString.Replace(" đ", "").Replace(",", "").Trim(), out price))
                                    {

                                        Data.Entities.GemPriceList diamond = new Data.Entities.GemPriceList
                                        {
                                            CaratWeight = size + " MM", // Thêm đơn vị MM
                                            Color = color,
                                            Clarity = "VVS1",
                                            Cut = "Round brilliant",
                                            Price = price * 1000,
                                            Active = true,
                                            effectDate = DateTime.Now,
                                        };

                                        diamondPrices.Add(diamond);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Sử dụng DbContext để lưu dữ liệu vào cơ sở dữ liệu
            foreach (var diamond in diamondPrices)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var existingDiamond = _context.GemPriceLists
                    .SingleOrDefault(d => d.CaratWeight == diamond.CaratWeight &&
                                          d.Color == diamond.Color &&
                                          d.Clarity == diamond.Clarity &&
                                          d.Cut == diamond.Cut);

                if (existingDiamond != null)
                {
                    // Cập nhật giá trị Price nếu tồn tại và effectDate không phải là ngày hiện tại
                    if (existingDiamond.effectDate.Date != DateTime.Now.Date)
                    {
                        existingDiamond.Price = diamond.Price;
                        existingDiamond.effectDate = diamond.effectDate;
                    }
                }
                else
                {
                    // Thêm mới nếu không tồn tại
                    _context.GemPriceLists.Add(diamond);
                }
            }

            _context.SaveChanges();

            await Console.Out.WriteLineAsync("Done Update GemPriceListNatural");
        }
    }
}
