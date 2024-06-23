using DiamondLuxurySolution.BackgroundServiceHost.Service;
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

					var myService =
						scope.ServiceProvider
							.GetRequiredService<IGemPriceListDefault>();

					// Execute your function
					await myService.CreateDefaultGemPriceList(stoppingToken);
				}

				// Wait for 24 hours
				try
				{
					await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
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
