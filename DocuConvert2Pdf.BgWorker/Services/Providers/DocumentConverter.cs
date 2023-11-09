using DocuConvert2Pdf.BgWorker.Models;
using DocuConvert2Pdf.BgWorker.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuConvert2Pdf.BgWorker.Services.Providers
{
    public class DocumentConverter : IDocumentConverter,IDisposable
    {
        private readonly ILogger<DocumentConverter> _logger;
        private readonly DocumentConversionConfig _docConvConfig;
        private readonly ICmdPrompt _cmdPrompt;
        private bool _disposed = false;

        public DocumentConverter(ILogger<DocumentConverter> logger,IOptions<DocumentConversionConfig> docConvConfig, ICmdPrompt cmdPrompt)
        {
            _logger = logger;
            _docConvConfig = docConvConfig.Value;
            _cmdPrompt = cmdPrompt;
        }
        
        public async void Convert()
        {
            try
            {
                //get ms word files
                var wordFiles = Directory.EnumerateFiles(_docConvConfig.DocFilesPath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".doc") || s.EndsWith(".docx")).ToArray();
                //loop and convert each document in the specified folder
                for (int i = 0; i < wordFiles.Length; i++)
                {
                    //Convert File using cmd
                    _logger.LogInformation("File to be converted: "+ wordFiles[i]);
                    bool res = await _cmdPrompt.RunAsync(wordFiles[i], _docConvConfig.PdfFilesPath);

                    //delete file when conversion is successful
                    if (res)
                    {
                        _logger.LogInformation($"Deleted {wordFiles[i]}");
                        File.Delete(wordFiles[i]);
                    }
                }

                Dispose();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // dispose your stuff you created in this class
                // do the same for other classes

            }

            // dispose native events

            _disposed = true;
        }
    }
}
