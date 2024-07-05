using DiamondLuxurySolution.BackgroundServiceHost.Service.GemPriceListService;
using DiamondLuxurySolution.BackgroundServiceHost.Service.MaterialService;
using PdfSharp.Charting;

namespace DiamondLuxurySolution.BackgroundServiceHost
{
    public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;

		public Worker(IServiceProvider services,
			ILogger<Worker> logger)
		{
			Services = services;
			_logger = logger;
		}

		public IServiceProvider Services { get; }

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Consume Scoped Service Hosted Service running.");
			await DoWork(stoppingToken);
		}

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consume Scoped Service Hosted Service is working.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = Services.CreateScope())
                {
                    var myService = scope.ServiceProvider.GetRequiredService<IGemPriceListDefault>();

                    // Execute your function
                    await myService.CreateDefaultGemPriceList(stoppingToken);

                    await myService.CreateDefaultGemPriceListNatural(stoppingToken);

                    var myService2 = scope.ServiceProvider.GetRequiredService<IMaterialService>();

                    await myService2.CreateDefaultMaterial(stoppingToken);
                }

                // Calculate the time until 00:00 next day
                var now = DateTime.Now;
                var nextRunTime = now.Date.AddDays(1); // 00:00 next day

                var timeToWait = nextRunTime - now;

                // Wait until 00:00 next day or handle task cancellation
                try
                {
                    await Task.Delay(timeToWait, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // Handle the task cancellation if stoppingToken is canceled
                    break;
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");
			await base.StopAsync(stoppingToken);
		}
	}
}
