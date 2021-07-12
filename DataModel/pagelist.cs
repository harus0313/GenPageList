using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

using AngleSharp;
using AngleSharp.Html.Parser;
using System.Net;

using System.IO;

using Markdig;


namespace GenPageList.DataModel
{
    public class pagelist
    {

        public static string dataSource = @"./Input/filelist.db";
        public static string fileListPath = @"./Input/filelist.txt";
        public static string markdownFilePath = @"./Output/sitemap.md";
        public static string sitemapHTMLFilePath = @"./Output/sitemap.html";

        public class Data
        {
            public string URL {get; set;}
            public string Title {get; set;}
            public int Status404 {get; set;}
            public int PageView {get; set;}
            public int ProcessFlag {get; set;}
        }

        /// <summary>
        /// FileListテーブルのデータを全件削除する
        /// </summary>
        public static void DeleteAllRecordFileList()
        {

            var sqlConnectionSB = new SQLiteConnectionStringBuilder{DataSource = dataSource};

            using(var cn = new SQLiteConnection(sqlConnectionSB.ToString()))
            {
                cn.Open();

                string sql = $"DELETE  FROM  FileList";
                SQLiteCommand cmd = new SQLiteCommand(sql, cn);
                cmd.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// レコードを１件インサートする
        /// </summary>
        /// <param name="data">インサートデータ</param>
        public static void AddRecord(DataModel.pagelist.Data data)
        {
            var sqlConnectionSB = new SQLiteConnectionStringBuilder{DataSource = dataSource};

            using(var cn = new SQLiteConnection(sqlConnectionSB.ToString()))
            {
                cn.Open();

                string sql = $"INSERT  INTO  FileList values ('{data.URL}', '{data.Title}', {data.Status404}, {data.PageView}, {data.ProcessFlag})";
                SQLiteCommand cmd = new SQLiteCommand(sql, cn);
                cmd.ExecuteNonQuery();

            }
        }


        /// <summary>
        /// レコードを１件更新する
        /// </summary>
        /// <param name="data">更新データ</param>
        public static void UpdateRecord(DataModel.pagelist.Data data)
        {
            var sqlConnectionSB = new SQLiteConnectionStringBuilder{DataSource = dataSource};

            using(var cn = new SQLiteConnection(sqlConnectionSB.ToString()))
            {
                cn.Open();

                string sql = $"Update  FileList set Title = '{data.Title}' , Status404 = {data.Status404}, PageView = {data.PageView}, ProcessFlag = {data.ProcessFlag}  where  URL = '{data.URL}'";
                SQLiteCommand cmd = new SQLiteCommand(sql, cn);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// FileListテーブルを全件削除して全件インサートしなおす
        /// </summary>
        public static void DeleteInsert()
        {

                DeleteAllRecordFileList();

                System.IO.StreamReader sr = new System.IO.StreamReader(fileListPath);
                string line;

                Data data = new Data();

                while((line = sr.ReadLine()) != null)
                {
                    data.URL = line;
                    data.Title = "";
                    data.Status404 = 0;
                    data.PageView = 0;
                    data.ProcessFlag = 1;

                    AddRecord(data);
                }
        }

        /// <summary>
        /// FileListテーブルのレコードを取得する
        /// </summary>
        /// <param name="sql">SELECT SQL文字列</param>
        /// <returns></returns>
        public static List<Data> GetData(string sql)
        {
            List<Data> allData = new List<Data>();
            Data record = new Data();

             var sqlConnectionSB = new SQLiteConnectionStringBuilder{DataSource = dataSource};

            using(var cn = new SQLiteConnection(sqlConnectionSB.ToString()))
            {
                cn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql, cn);

                SQLiteDataReader sdr = cmd.ExecuteReader();

                while(sdr.Read() == true)
                {
                    record = new Data();

                    record.URL = (string)sdr["URL"];
                    record.Title = (string)sdr["Title"];
                    record.Status404 = Convert.ToInt32((decimal)sdr["Status404"]);
                    record.PageView = Convert.ToInt32((decimal)sdr["PageView"]);
                    record.ProcessFlag = Convert.ToInt32((decimal)sdr["ProcessFlag"]);

                    allData.Add(record);
                }

            }

            return allData;
        }


        public static List<Data> GetAllData()
        {
            string sql = "SELECT  *  FROM  FileList";
            List<Data> allData = new List<Data>();

            allData = GetData(sql);

            return allData;
        }


        public static List<Data> GetProcessData()
        {
            string sql = "SELECT  *  FROM  FileList where  ProcessFlag = 1";
            List<Data> allData = new List<Data>();

            allData = GetData(sql);

            return allData;
        }



        public static async Task<string> GetPageTitle(string url)
        {
            using(WebClient wc = new WebClient())
            {
                string htmldocs = wc.DownloadString(url);

                var config = Configuration.Default;
                var context = BrowsingContext.New(config);
                var document = await context.OpenAsync(req => req.Content(htmldocs));

                return document.Title;
            }
        }

        public static async Task UpdateTite()
        {
            List<Data> alldata = new List<Data>();
            alldata = GetProcessData();

            foreach(Data data in alldata)
            {

                try
                {
                    string title = await GetPageTitle(data.URL);
                    title = title.Replace("'", "");
                    data.Title = title;
                    data.Status404 = 0;
                    data.ProcessFlag = 0;
                }
                catch (System.Exception)
                {
                    data.Status404 = 1;
                    data.ProcessFlag = 1;
                }


                UpdateRecord(data);
            }


        }

        public static void OutputSitemapMd()
        {
            string sql = "select  *  from  FileList  where  Status404 = '0'";
            List<Data> alldata = new List<Data>();
            alldata = GetData(sql);
 
            using(StreamWriter sw = new StreamWriter(markdownFilePath))
            {
                foreach(Data data in alldata)
                {
                    string line = $"- [{data.Title}]({data.URL})";
                    sw.WriteLine(line);
                }



            }
        }

        public static void MarkdownToHTML()
        {
            Markdig.MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            string fileContents = System.IO.File.ReadAllText(markdownFilePath);

            string mdContents = Markdown.ToHtml(fileContents,markdownPipeline);

            using(StreamWriter sw = new StreamWriter(sitemapHTMLFilePath))
            {
                sw.WriteLine(mdContents);
            }
        }


    }
}