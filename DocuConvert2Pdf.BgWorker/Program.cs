using DocuConvert2Pdf.BgWorker.Models;
using DocuConvert2Pdf.BgWorker.Services.Interfaces;
using DocuConvert2Pdf.BgWorker.Services.Providers;


namespace DocuConvert2Pdf.BgWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((ctx,services) =>
                {
                    /*
                     * Libre office configuration.
                     * Set the path in the appsettings.json to where libreoffice soffice.exe is installed 
                     * If it can be access everywhere via environment variable, set IsGlobal = true
                     */
                    services.Configure<LibreOfficeConfig>(ctx.Configuration.GetSection("LibreOfficeConfig"));
                    /*
                     * Document Conversion configuration.
                     *Set where both the word files and where the pdf files should be stored
                     * 
                     */
                    services.Configure<DocumentConversionConfig>(ctx.Configuration.GetSection("DocumentConversionConfig"));
                    

                    services.AddSingleton<ICmdPrompt, CmdPrompt>();
                    services.AddSingleton<IDocumentConverter, DocumentConverter>();


                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }
    }
}