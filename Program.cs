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
        static async Task Main(string[] args)
        {

            //await DataModel.pagelist.UpdateTite();
            //DataModel.pagelist.OutputSitemapMd();
            DataModel.pagelist.MarkdownToHTML();

//            DataModel.pagelist.DeleteInsert();
/*
            try
            {
                string url = "https://usefuledge.com/00001-easy-to-select-cooking-recipe.html";
                string title = await DataModel.pagelist.GetPageTitle(url);
            }
            catch (System.Exception)
            {
                Console.WriteLine("404");
            }
*/

        }

    }
}
