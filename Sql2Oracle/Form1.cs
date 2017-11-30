using ICSharpCode.TextEditor.Document;
    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Sql2Oracle
{
    public partial class Form1 : Form
    {
        OracleHelper orcl;
        public Form1()
        {
            InitializeComponent();
            orcl = new OracleHelper(oracleStr.Text);

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(() => {
                disableAll();
                if (chkClob.Checked)
                {
                    import();

                }
                else
                {
                    import2();
                }

                enableAll();

            }));
        }
        private void disableAll()
        {
            foreach (Control item in this.Controls)
            {
                item.Enabled = false;
            }
        }
        private void enableAll()
        {
            foreach (Control item in this.Controls)
            {
                item.Enabled = true;
            }
        }
        int totalCount = 0;
        private void import()
        {
            Stopwatch w = new Stopwatch();
            totalCount = 0;
            try
            {

                w.Start();
                StringBuilder sb = new StringBuilder();
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(sqlConnStr.Text))
                {
                    conn.Open();
                    SqlDataAdapter ad = new SqlDataAdapter(sqlText.Text, conn);
                    ad.Fill(ds);
                }

                CreateOraTmpSql(ds, tableName.Text);
                //orcl.Query(sql);
                w.Stop();
                MessageBox.Show("导入"+ totalCount + "条成功！耗时" + w.ElapsedMilliseconds / 1000 + "秒");
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败" + ex.Message);
            }

        }
        private void import2()
        {
            Stopwatch w = new Stopwatch();
            totalCount = 0;
            try
            {

                w.Start();
                StringBuilder sb = new StringBuilder();
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(sqlConnStr.Text))
                {
                    conn.Open();
                    SqlDataAdapter ad = new SqlDataAdapter(sqlText.Text, conn);
                    ad.Fill(ds);
                }

                CreateOraTmpSql2(ds, tableName.Text);
                //orcl.Query(sql);
                w.Stop();
                MessageBox.Show("导入" + totalCount + "条成功！耗时" + w.ElapsedMilliseconds / 1000 + "秒");
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败" + ex.Message);
            }

        }
        public void CreateOraTmpSql2(DataSet his, string tmpName)
        {

            DataRowCollection rows = his.Tables[0].Rows;
            var columns = his.Tables[0].Columns;




            string sql = "declare v_cnt Number; ";
            sql += " BEGIN ";
            sql += " select count(*) into v_cnt from user_tables where table_name = '" + tmpName.ToUpper() + "'; ";
            sql += " if v_cnt=0 then ";
            sql += "execute immediate 'CREATE  TABLE " + tmpName.ToUpper() + "(";

            foreach (DataColumn c in columns)
            {
                sql += c.ColumnName + " " + DBTypeChange(c) + ",";
            }
            sql = sql.TrimEnd(new char[] { ',' });
            sql += ") ';\r\n";
            sql += " end if;";
            sql += "end;";
            orcl.ExecuteSql(sql);
            StringBuilder sb = new StringBuilder();
            int count = 1;
            int sum = rows.Count;
            totalCount = sum;
            System.Diagnostics.Trace.WriteLine("total:" + sum);
            int number = int.Parse(batchNumber.Text);
            foreach (DataRow r in rows)
            {

                if (count % number == 0)
                {


                    sb.Append("execute immediate 'insert into ").Append(tmpName.ToUpper()).Append(" values(");
                    sb.Append(GetRowValueSql2(r, true));
                    sb.Append(")';\r\n");
                    var s = "BEGIN \r\n"
                         + sb.ToString()
                        + "END;\r\n";
                    sb.Clear();
                    System.Diagnostics.Trace.WriteLine(s);
                    System.Diagnostics.Trace.WriteLine("批处理执行");
                    orcl.Query(s);


                }
                else
                {
                    if (count == sum)
                    {

                        sb.Append("execute immediate 'insert into ").Append(tmpName.ToUpper()).Append(" values(");
                        sb.Append(GetRowValueSql2(r, true));
                        sb.Append(")';\r\n");
                        var s = "BEGIN \r\n"
                             + sb.ToString()
                            + "END;\r\n";
                        sb.Clear();
                        System.Diagnostics.Trace.WriteLine(s);
                        System.Diagnostics.Trace.WriteLine("批处理执行");
                        orcl.Query(s);

                    }
                    sb.Append("execute immediate 'insert into ").Append(tmpName.ToUpper()).Append(" values(");
                    sb.Append(GetRowValueSql2(r, true));
                    sb.Append(")';\r\n");
                }
                count++;



            }


        }
        private string GetRowValueSql2(DataRow row, bool doubleQuote = false)
        {
            StringBuilder result = new StringBuilder();
            var columns = row.Table.Columns;
            foreach (DataColumn c in columns)
            {
                string val = row[c].ToString().Replace("\0", "");
                if (val.Length > 2000)
                {
                    int byteLength = Encoding.GetEncoding("gb18030").GetBytes(val).Length;
                    if (byteLength > 4000)
                    {
                        val = val.Substring(0, val.Length - (byteLength - 4000) * 2);
                    }
                }

                switch (c.DataType.Name.ToLower())
                {

                    case "boolean":
                        if (doubleQuote)
                        {
                            result.Append(val == "False" ? "''0''" : "''1''").Append(",");
                        }
                        else
                        {
                            result.Append((val == "False" ? "'0'" : "'1'")).Append(",");
                        }
                        break;
                    case "string":
                        if (doubleQuote)
                        {
                            result.Append("''").Append(val.Replace("'", "''''")).Append("'',");
                        }
                        else
                        {
                            result.Append("'").Append(val).Append("',");
                        }
                        break;
                    case "int32":
                        if (val == "")
                        {
                            result.Append("-1,");
                        }
                        else
                        {
                            result.Append(val).Append(",");
                        }

                        break;
                    case "decimal":
                        if (val == "")
                        {
                            result.Append("-1,");
                        }
                        else
                        {
                            result.Append(val).Append(",");
                        }
                        break;

                    default:
                        if (doubleQuote)
                        {
                            result.Append("''").Append(val.Replace("'", "''''")).Append("'',");
                        }
                        else
                        {
                            result.Append("'").Append(val).Append("',");
                        }
                        break;
                }
            }
            return result.ToString().TrimEnd(new char[] { ',' });
        }
        public void CreateOraTmpSql(DataSet his, string tmpName)
        {

            DataRowCollection rows = his.Tables[0].Rows;
            var columns = his.Tables[0].Columns;


            
            
            string sql = "declare v_cnt Number; ";
            sql += " BEGIN ";
            sql += " select count(*) into v_cnt from user_tables where table_name = '" + tmpName.ToUpper() + "'; ";
            sql += " if v_cnt=0 then ";
            sql += "execute immediate 'CREATE  TABLE " + tmpName.ToUpper() + "(";

            foreach (DataColumn c in columns)
            {
                sql += c.ColumnName + " " + DBTypeChange(c) + ",";
            }
            sql = sql.TrimEnd(new char[] { ',' });
            sql += ") ';\r\n";
            sql += " end if;";
            sql += "end;";
            orcl.ExecuteSql(sql);
            StringBuilder sb = new StringBuilder();
            int count = 1;
            int sum = rows.Count;
            totalCount = sum;
            System.Diagnostics.Trace.WriteLine("total:" + sum);
            int number = int.Parse(batchNumber.Text);
            foreach (DataRow r in rows)
            {
                
                if (count % number == 0 )
                {


                    //sb.Append("execute immediate 'insert into ").Append(tmpName.ToUpper()).Append(" values(");
                    
                    sb.Append(GetRowValueSql(r, true,tmpName));
                    //sb.Append(")';\r\n");
                    var s = getDeclarHeader(columns);
                     s += " BEGIN \r\n"
                         + sb.ToString()
                        + " END;\r\n";
                    sb.Clear();
                    System.Diagnostics.Trace.WriteLine(s);
                    System.Diagnostics.Trace.WriteLine("批处理执行");
                    orcl.ExecuteSql(s);


                }
                else
                {
                    if (count == sum)
                    {

                        sb.Append(GetRowValueSql(r, true, tmpName));
                        //sb.Append(")';\r\n");
                        var s = getDeclarHeader(columns);
                        s += " BEGIN \r\n"
                            + sb.ToString()
                           + " END;\r\n";
                        sb.Clear();
                        System.Diagnostics.Trace.WriteLine(s);
                        System.Diagnostics.Trace.WriteLine("批处理执行");
                        orcl.Query(s);

                    }
                
                    sb.Append(GetRowValueSql(r, true, tmpName));
                }
                count++;



            }
            
            
        }
        private string getDeclarHeader(DataColumnCollection  columns)
        {
            string header = "declare \r\n";
            int count = 1;
            foreach (DataColumn c in columns)
            {
                header += "v_" + c.ColumnName + " " + DBTypeChange(c) +";\r\n";
                count++;
            }
            return header;

        }
        private string GetRowValueSql(DataRow row, bool doubleQuote = false,string tmpName="")
        {
            StringBuilder result = new StringBuilder();
            var columns = row.Table.Columns;
            foreach (DataColumn c in columns)
            {
                
                string val = row[c].ToString().Replace("\0", "");
                switch (c.DataType.Name.ToLower())
                {

                    case "boolean":
                        result.Append("v_").Append(c.ColumnName).Append(":=").Append((val == "False" ? "'0'" : "'1'")).Append(";\r\n");
                        break;
                    case "string":
                        result.Append("v_").Append(c.ColumnName).Append(":='").Append(val.Replace("'", "''")).Append("';\r\n");
                        break;
                    case "int32":
                        if (val == "")
                            result.Append("v_").Append(c.ColumnName).Append(":=-1;\r\n");
                        else
                            result.Append("v_").Append(c.ColumnName).Append(":=").Append(val).Append(";\r\n");

                        break;
                    case "decimal":
                        if (val == "")
                            result.Append("v_").Append(c.ColumnName).Append(":=-1;\r\n");
                        else
                            result.Append("v_").Append(c.ColumnName).Append(":=").Append(val).Append(";\r\n");
                        break;

                    default:
                        result.Append("v_").Append(c.ColumnName).Append(":='").Append(val).Append("';\r\n");
                        break;
                }
            }
            result.Append("execute immediate 'insert into ").Append(tmpName.ToUpper()).Append(" values(");
            foreach(DataColumn c in columns)
            {
                result.Append(":").Append("v_").Append(c.ColumnName).Append(",");
            }
            var tmp = result.ToString().TrimEnd(new char[] { ',' });
            result.Clear();
            result.Append(tmp);
            result.Append(")'");
            result.Append(" using ");
            foreach (DataColumn c in columns)
            {
                result.Append("v_").Append(c.ColumnName).Append(",");
            }


            return result.ToString().TrimEnd(new char[] { ',' })+" ; \r\n";
            
        }
        private string DBTypeChange(DataColumn column)

        {
            var str = column.DataType.Name;
            string outstr = "";
            switch (str.ToLower())
            {
                case "boolean":
                    outstr = "CHAR(1)";
                    break;
                case "string":
                    if (column.ColumnName.ToLower().IndexOf("_clob") > -1)
                    {
                        outstr = " clob";
                    }
                    else
                    {
                        outstr = " varchar2(4000) ";
                    }
                    break;
                case "int32":
                    outstr = "NUMBER";
                    break;
                case "decimal":
                    outstr = "NUMBER";
                    break;
                case "double":
                    outstr = "NUMBER";
                    break;
                default:
                    outstr = "varchar2(4000)";
                    break;
            }

            return outstr;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

          

            //sqlText.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("SQL");
        }
    }
}
