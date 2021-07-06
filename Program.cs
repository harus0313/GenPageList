using AngleSharp;
using AngleSharp.Html.Parser;
using System.Net;
using System.Threading.Tasks;

using System;

namespace GenPageList
{
    class Program
    {
        static async Task Main(string[] args)
        {

            DataModel.pagelist.Data data = new DataModel.pagelist.Data();
            data.url = "https://usefuledge.com/csharp-json.html";

            Console.WriteLine(await GetWebTitle(data.url));

        }


        static async Task<string> GetWebTitle(string url)
        {
            WebClient wc = new WebClient();
            try
            {
                string htmldocs = wc.DownloadString(url);

                var config = Configuration.Default;
                var context = BrowsingContext.New(config);
                var document = await context.OpenAsync(req => req.Content(htmldocs));

                //Console.WriteLine(document.Title);

                return document.Title;

            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
