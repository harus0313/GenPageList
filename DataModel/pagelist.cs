using System;
using System.Data.SQLite;

namespace GenPageList.DataModel
{
    public class pagelist
    {
        public class Data
        {
            public string URL {get; set;}
            public string Title {get; set;}
            public int Status404 {get; set;}
            public int PageView {get; set;}
        }

        public static void Insert()
        {
            string dataSource = @"./Input/filelist.db";

            var sqlConnectionSB = new SQLiteConnectionStringBuilder{DataSource = dataSource};

            using(var cn = new SQLiteConnection(sqlConnectionSB.ToString()))
            {
                cn.Open();


                string fileName = @"./Input/filelist.txt";
                System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    //Console.WriteLine(line);

                    DataModel.pagelist.Data data = new Data();
                    data.URL = line;
                    data.Title = "";
                    data.Status404 = 0;
                    data.PageView = 0;

                    string sql = $"INSERT  INTO  FileList values ('{data.URL}', '{data.Title}', {data.Status404}, {data.PageView})";
                    SQLiteCommand cmd = new SQLiteCommand(sql, cn);
                    cmd.ExecuteNonQuery();

                }

            }
            

        }




    }
}