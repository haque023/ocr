using Aspose.Pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ocr.DTO;
using Syncfusion.Drawing;
using System;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using Tesseract;

namespace ocr.Controllers
{
    [Route("Ocr/[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {
        public const string folderName = "Imgaes/";
        public const string trainedDataFolderName = "tdata";
        [HttpPost]
        [Route("OcrGenerate")]
        public async Task<String> DoOCR(IFormFile? Cv, string? url)
        {
            string result = "";

            Document? document = new Document();
            using var wc = new System.Net.WebClient();
            if (Cv == null && url != null)
            {
                wc.DownloadFile(url, "output.pdf");
                wc.Dispose();
                document = await Task.FromResult(new Document("output.pdf"));

            }
            else if (Cv != null)
            {
                MemoryStream ms = new MemoryStream();
                using (var writer = new StreamWriter(ms))
                {
                    await Cv.OpenReadStream().CopyToAsync(ms);
                }
                document = await Task.FromResult(new Document(Cv.OpenReadStream()));
            }
            else
            {
                throw new Exception("hello world");
            }
            var renderer = new Aspose.Pdf.Devices.PngDevice();

            string tessPath = Path.Combine(trainedDataFolderName, "");
            using (var engine = new TesseractEngine(tessPath, "eng", EngineMode.Default))
            {

                foreach (var itm in document.Pages)
                {
                    renderer.Process(itm, "output.png");
                    var imageFileName = "output.png";
                    using (var img = Pix.LoadFromFile(imageFileName))
                    {
                        var page = engine.Process(img);
                        result += page.GetText();
                        page.Dispose();
                    }
                }

            }
            var obj = new
            {

                strEmail = "emdad@ibos.io",
                strCvData = result,
                intAutoId = 0
            };

            using (var client = new HttpClient())
            {
                var p = new
                {

                    strEmail = "emdad@ibos.io",
                    strCvData = result,
                    intAutoId = 0
                };
                client.BaseAddress = new Uri("https://localhost:44343/");
                var response = await client.PostAsJsonAsync("identity/Public/CvDataSavePost", p);
                if (response.IsSuccessStatusCode)
                {
                    Console.Write("Success");
                }
                else
                    Console.Write("Error");
            }


            return String.IsNullOrWhiteSpace(result) ? "Ocr is finished. Return empty" : result;
        }

        [HttpPost]
        [Route("DoOcrP")]
        public async Task<String> DoOcrP(IFormFile? Cv, string? url)
        {
            string result = "";

            Document? document = new Document();
            using var wc = new System.Net.WebClient();
            if (Cv == null && url != null)
            {
                wc.DownloadFile(url, "output.pdf");
                wc.Dispose();
                document = await Task.FromResult(new Document("output.pdf"));

            }
            else if (Cv != null)
            {
                MemoryStream ms = new MemoryStream();
                using (var writer = new StreamWriter(ms))
                {
                    await Cv.OpenReadStream().CopyToAsync(ms);
                }
                document = await Task.FromResult(new Document(Cv.OpenReadStream()));
            }
            else
            {
                throw new Exception("hello world");
            }
            var renderer = new Aspose.Pdf.Devices.JpegDevice();

            string tessPath = Path.Combine(trainedDataFolderName, "");
            using (var engine = new TesseractEngine(tessPath, "eng", EngineMode.Default))
            {

                foreach (var itm in document.Pages)
                {
                    renderer.Process(itm, "output.jpeg");
                    var imageFileName = "output.jpeg";
                    using (var img = Pix.LoadFromFile(imageFileName))
                    {
                        var page = engine.Process(img);
                        result += page.GetText();
                        page.Dispose();
                    }
                }

            }


            return String.IsNullOrWhiteSpace(result) ? "Ocr is finished. Return empty" : result;
        }
      
    }
}
