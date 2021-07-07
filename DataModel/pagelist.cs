using System;
using System.Data.SQLite;
using System.Collections.Generic;

namespace GenPageList.DataModel
{
    public class pagelist
    {

        public static string dataSource = @"./Input/filelist.db";
        public static string fileListPath = @"./Input/filelist.txt";

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
                    data.ProcessFlag = 0;

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
                    record.Status404 = Convert.ToInt32((int)sdr["Status404"]);
                    record.PageView = Convert.ToInt32((int)sdr["PageView"]);
                    record.ProcessFlag = Convert.ToInt32((int)sdr["ProcessFlag"]);

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



    }
}