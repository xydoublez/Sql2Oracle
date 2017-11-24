using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
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
            Stopwatch w = new Stopwatch();
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
                MessageBox.Show("导入成功！耗时" + w.ElapsedMilliseconds / 1000 + "秒");
            }
            catch(Exception ex)
            {
                MessageBox.Show("导入失败" + ex.Message);
            }
            
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
                sql += c.ColumnName + " " + DBTypeChange(c.DataType.Name) + ",";
            }
            sql = sql.TrimEnd(new char[] { ',' });
            sql += ") ';\r\n";
            sql += " end if;";
            sql += "end;";
            orcl.ExecuteSql(sql);
            StringBuilder sb = new StringBuilder();
            int count = 0;
            int sum = rows.Count;
            int number = 1000;
            foreach (DataRow r in rows)
            {
                count++;
                if (count % number == 0 )
                {

                    
                    {
                        sb.Append("execute immediate 'insert into ").Append(tmpName.ToUpper()).Append(" values(");
                        sb.Append(GetRowValueSql(r, true));
                        sb.Append(")';\r\n");
                    }
                    var s = "BEGIN \r\n"
                         + sb.ToString()
                        + "END;\r\n";
                    sb.Clear();
                    System.Diagnostics.Trace.WriteLine(s);
                    orcl.Query(s);


                }
                else
                {
                    if (count == sum)
                    {
                        {
                            sb.Append("execute immediate 'insert into ").Append(tmpName.ToUpper()).Append(" values(");
                            sb.Append(GetRowValueSql(r, true));
                            sb.Append(")';\r\n");
                        }
                        var s = "BEGIN \r\n"
                             + sb.ToString()
                            + "END;\r\n";
                        sb.Clear();
                        System.Diagnostics.Trace.WriteLine(s);
                        orcl.Query(s);

                    }
                }
                
                sb.Append("execute immediate 'insert into ").Append(tmpName.ToUpper()).Append(" values(");
                sb.Append(GetRowValueSql(r, true));
                sb.Append(")';\r\n");
                
            }
            
            
        }
        private string GetRowValueSql(DataRow row, bool doubleQuote = false)
        {
            StringBuilder result = new StringBuilder();
            var columns = row.Table.Columns;
            foreach (DataColumn c in columns)
            {
                string val = row[c].ToString();
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
                        result.Append(val).Append(",");
                        break;
                    case "decimal":
                        result.Append(val).Append(",");
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
        private string DBTypeChange(string str)
        {
            string outstr = "";
            switch (str.ToLower())
            {
                case "boolean":
                    outstr = "CHAR(1)";
                    break;
                case "string":
                    outstr = "VARCHAR2(4000 CHAR)";
                    break;
                case "int32":
                    outstr = "NUMBER(10)";
                    break;
                case "decimal":
                    outstr = "NUMBER(18)";
                    break;

                default:
                    outstr = "VARCHAR2(4000 CHAR)";
                    break;
            }

            return outstr;
        }
    }
}
