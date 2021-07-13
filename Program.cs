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

            //filelist.dbから処理対象URL一覧を取得する
            //処理フラグが1のURLのページタイトルを取得する（404チェック含む）
            //ページが存在するもののサイトマップ(markdown)を出力する
            //markdownからhtmlに変換する
            //htmlをsitemap.cshtmlへ出力する
            //usefuledge にてコンパイル


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
