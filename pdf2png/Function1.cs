using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Ghostscript.NET.Rasterizer;
using System.Drawing;

using System.IO;
using System.Drawing.Imaging;
using System;

namespace pdf2png
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            var options = new Options()
            {
                Input = DownloadFile(GetInputFileFromUrl(req))
            };

            using (GhostscriptRasterizer _rasterizer = new GhostscriptRasterizer())
            {
                _rasterizer.Open(options.Input, options.Gvi, true);

                for (int pageNr = 1; pageNr <= _rasterizer.PageCount; pageNr++)
                {
                    string dest = Path.Combine(options.OutputPath, $"{options.DestFileNameWithoutExtension}-{pageNr}.png");
                    Image img = _rasterizer.GetPage(options.Desired_X_dpi, options.Desired_Y_dpi, pageNr);
                    img.Save(dest, ImageFormat.Png);

                    //upload to azure
                    //http://lightswitchhelpwebsite.com/Blog/tabid/61/EntryId/3303/Convert-PDF-files-to-PNG-Images-using-Azure-Functions.aspx
                    //file.delete
                }
            }

            File.Delete(options.Input);

            return req.CreateResponse(HttpStatusCode.OK, "Hello");
        }

        private static string GetInputFileFromUrl(HttpRequestMessage req)
        {
            //TODO: invalid/missing input
            return req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "input", true) == 0)
                .Value;
        }

        private static string DownloadFile(string input)
        {
            string res = $"c:\\temp\\{Guid.NewGuid()}.pdf"; //TODO temp

            using (var wc = new WebClient())
            {                
                wc.DownloadFile(input, res);
            }

            return res;
        }
    }
}
