using Aspose.Pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Drawing;
using System.Security.Policy;
using Tesseract;

namespace ocr.Controllers
{
    [Route("ocr/[controller]")]
    [ApiController]
    public class OcrController : ControllerBase
    {
        public const string folderName = "Imgaes/";
        public const string trainedDataFolderName = "tdata";
        [HttpPost]
        [Route("OcrGenerate")]
        public String DoOCR()
        {
            string result = "";


            using var wc = new System.Net.WebClient();

            var pdfUrl = "https://www.africau.edu/images/default/sample.pdf";
            wc.DownloadFile(pdfUrl, "output.pdf");
            wc.Dispose();

            var document = new Document("output.pdf");
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

            return String.IsNullOrWhiteSpace(result) ? "Ocr is finished. Return empty" : result;
        }
    }
}
