using DiamondLuxurySolution.BackgroundServiceHost.Service.GemPriceListService;
using DiamondLuxurySolution.BackgroundServiceHost.Service.MaterialService;
using DiamondLuxurySolution.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace DiamondLuxurySolution.BackgroundServiceHost
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = Host.CreateApplicationBuilder(args);
			builder.Services.AddHostedService<Worker>();
			builder.Services.AddScoped<IGemPriceListDefault, GemPriceListDefault>();
            builder.Services.AddScoped<IMaterialService, MaterialService>();

            builder.Services.AddDbContext<LuxuryDiamondShopContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("LuxuryDiamondDb"));
			});

			var host = builder.Build();
			host.Run();
		}
	}
}