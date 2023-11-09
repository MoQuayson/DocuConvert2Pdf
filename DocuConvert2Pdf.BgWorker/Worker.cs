using DocuConvert2Pdf.BgWorker.Services.Interfaces;
using DocuConvert2Pdf.BgWorker.Services.Providers;
using Microsoft.Extensions.Logging;

namespace DocuConvert2Pdf.BgWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IDocumentConverter _documentConverter;

        public Worker(ILogger<Worker> logger,IDocumentConverter documentConverter)
        {
            _logger = logger;
            _documentConverter = documentConverter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    _documentConverter.Convert();
                    await Task.Delay(3000, stoppingToken);
                }

                
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Cancelled");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}