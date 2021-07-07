using AngleSharp;
using AngleSharp.Html.Parser;
using System.Net;
using System.Threading.Tasks;
using System;

using System.Collections.Generic;

namespace GenPageList
{
    class Program
    {
        static void Main(string[] args)
        {

            DataModel.pagelist.DeleteInsert();

            /*
            List<DataModel.pagelist.Data> allData = new List<DataModel.pagelist.Data>();
            allData = DataModel.pagelist.GetAllData();

            foreach(DataModel.pagelist.Data record in allData)
            {
                Console.WriteLine(record.URL);
            }

            DataModel.pagelist.Data data = new DataModel.pagelist.Data();
            data.URL = "https://usefuledge.com/csharp-json.html";

            data.Title = await GetWebTitle(data.URL);

            Console.WriteLine(data.URL + "\t" + data.Title);
            */


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
