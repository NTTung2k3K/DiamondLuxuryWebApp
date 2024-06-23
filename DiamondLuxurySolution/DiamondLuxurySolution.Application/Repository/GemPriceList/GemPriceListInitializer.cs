using DiamondLuxurySolution.Data.EF;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.GemPriceList
{
	public class GemPriceListInitializer : IGemPriceListInitializer
	{
		private readonly LuxuryDiamondShopContext _context;
		public GemPriceListInitializer(LuxuryDiamondShopContext context)
		{
			_context = context;
		}

		public async Task CreateDefaultGemPriceList()
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
				"3.6MM", "3.9MM", "4.1MM", "4.5MM", "5.0MM", "5.2MM",
				"5.3MM", "5.4MM", "6.0MM", "6.2MM", "6.3MM(<1CT)",
				"6.3MM(>1CT)", "6.8MM", "7.2MM", "8.1MM", "9.0MM(<3CT)"
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
		}
	}
}
