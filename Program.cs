using AngleSharp;
using AngleSharp.Html.Parser;
using System.Net;
using System.Threading.Tasks;

using System.Data.SQLite;

using System;

namespace GenPageList
{
    class Program
    {
        static async Task Main(string[] args)
        {

            DataModel.pagelist.DeleteInsert();


            DataModel.pagelist.Data data = new DataModel.pagelist.Data();
            data.URL = "https://usefuledge.com/csharp-json.html";

            data.Title = await GetWebTitle(data.URL);

            Console.WriteLine(data.URL + "\t" + data.Title);

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
            catch (System.Exception e)
            {
                return e.Message;
            }
        }
    }
}
