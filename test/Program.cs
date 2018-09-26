using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace test
{
     class Program
    {
    
        static void Main(string[] args)
        {
            run();
        }
        //static OracleConnection conn;
        private static void run()
        {

            System.Diagnostics.Trace.WriteLine("开始运行……。");
            Console.WriteLine("开始运行……。");
            ThreadPool.SetMaxThreads(2, 2);
            ThreadPool.SetMinThreads(2, 2);
            List<Item> list = new List<Item>();
            //var connstr = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Max Pool Size=10;Pooling=true;Connection Timeout=10;Enlist=false;",
            //   "10.68.4.31", "1521", "sfxorcl", "sfx", "371482");
            //conn = new OracleConnection(connstr);

            //conn.Open();
            for (int i = 0; i <= 100; i++)
            {
                list.Add(new Item() { sql = "select sysdate from dual" });
            }
            foreach (var item in list)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(verify2), item);
                //verify2(item);
            }
            
            Console.ReadLine();
        }
        private static void verify2(Object item)
        {
            Thread.Sleep(100);
            var connstr = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Min Pool Size=10;Max Pool Size=100;Pooling=true;Connection Timeout=100;Enlist=false;",
                   "10.68.4.31", "1521", "sfxorcl", "sfx", "371482");

            using (OracleConnection conn = new OracleConnection(connstr))
            {
                try
                {
                    //OracleConnection.ClearPool(conn);
                    conn.Open();
                    Console.WriteLine("打开连接成功...");
                    OracleDataAdapter adapter = new OracleDataAdapter("select sysdate from dual", conn);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    System.Diagnostics.Trace.WriteLine(ds.Tables[0].Rows[0][0].ToString());
                    Console.WriteLine(ds.Tables[0].Rows[0][0].ToString());
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message + ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private static void verify3(Object item)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder.DataSource = "10.68.4.31";
            sqlConnectionStringBuilder.UserID = "sa";
            sqlConnectionStringBuilder.Password = "Sfx371482";
            using (SqlConnection conn = new SqlConnection(sqlConnectionStringBuilder.ToString()))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("select getdate()", conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                System.Diagnostics.Trace.WriteLine(ds.Tables[0].Rows[0][0].ToString());
            }
        }
        private class Item
        {
            public string sql { get; set; }
        }
    }
}
