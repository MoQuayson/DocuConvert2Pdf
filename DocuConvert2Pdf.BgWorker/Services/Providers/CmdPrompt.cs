using DocuConvert2Pdf.BgWorker.Models;
using DocuConvert2Pdf.BgWorker.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuConvert2Pdf.BgWorker.Services.Providers
{
    public class CmdPrompt : ICmdPrompt
    {
        private readonly ILogger<CmdPrompt> _logger;
        private ProcessStartInfo cmdStartInfo;
        private Process cmdProcess;
        private readonly LibreOfficeConfig _libreOfficeConfig;

        public CmdPrompt(ILogger<CmdPrompt> logger,IOptions<LibreOfficeConfig> libreOfficeConfig)
        {
            _logger = logger;
            _libreOfficeConfig = libreOfficeConfig.Value;
            
        }

        /// <summary>
        /// Init cmd process used for conversion
        /// </summary>
        void Init()
        {
            cmdStartInfo = new ProcessStartInfo();
            cmdStartInfo.FileName = "cmd.exe";
            cmdStartInfo.RedirectStandardOutput = true;
            cmdStartInfo.RedirectStandardError = true;
            cmdStartInfo.RedirectStandardInput = true;
            cmdStartInfo.UseShellExecute = false;
            cmdStartInfo.CreateNoWindow = true;

            cmdProcess = new Process();
            cmdProcess.StartInfo = cmdStartInfo;
            cmdProcess.ErrorDataReceived += Cmd_Error;
            cmdProcess.OutputDataReceived += Cmd_DataReceived;
            cmdProcess.EnableRaisingEvents = true;
        }

        public async Task<bool> RunAsync(string originalFile,string outputDirectory)
        {
            try
            {
                Init();
                string sofficePath = "soffice";

                if (!_libreOfficeConfig.IsGlobal)
                {
                    sofficePath = _libreOfficeConfig.Path;
                }

                string cmd = $"\"{sofficePath}\" --headless --convert-to pdf " + $"\"{originalFile}\" --outdir " + $"\"{outputDirectory}\" && exit";

                cmdProcess.Start();
                cmdProcess.StandardInput.WriteLine(cmd);

                string output = cmdProcess.StandardOutput.ReadToEnd();
                _logger.LogInformation($"Output: {output}");
                cmdProcess.WaitForExit();

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "{Message}", ex.Message);
                return await Task.FromResult(false);
            }
        }

        void Cmd_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                _logger.LogInformation("Success: "+e.Data);
            }
        }

        void Cmd_Error(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {

                _logger.LogError("Error: "+e.Data);
            }
        }
    }
}
