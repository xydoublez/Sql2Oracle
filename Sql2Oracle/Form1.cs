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
        public Form1()
        {
            InitializeComponent();
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
                OracleHelper orcl = new OracleHelper(oracleStr.Text);
                var sql = CreateOraTmpSql(ds, tableName.Text);
                orcl.Query(sql);
                w.Stop();
            }catch(Exception ex)
            {
                MessageBox.Show("导入失败" + ex.Message);
            }
            MessageBox.Show("导入成功！耗时"+w.ElapsedMilliseconds/1000+"秒");
        }
        public string CreateOraTmpSql(DataSet his, string tmpName)
        {
            string sql = "declare v_cnt Number; ";
            sql += " BEGIN ";
            sql += " select count(*) into v_cnt from user_tables where table_name = '" + tmpName.ToUpper() + "'; ";
            sql += " if v_cnt=0 then ";
            sql += "execute immediate 'CREATE  TABLE " + tmpName.ToUpper() + "(";
            var columns = his.Tables[0].Columns;
            foreach (DataColumn c in columns)
            {
                sql += c.ColumnName + " " + DBTypeChange(c.DataType.Name) + ",";
            }
            sql = sql.TrimEnd(new char[] { ',' });
            sql += ") ';\r\n";
            sql += " end if;";

            DataRowCollection rows = his.Tables[0].Rows;
            foreach (DataRow r in rows)
            {
                sql += "execute immediate 'insert into " + tmpName.ToUpper() + " values(";
                sql += GetRowValueSql(r, true);
                sql += ")';\r\n";
            }
            sql += "END;";
            return sql;
        }
        private string GetRowValueSql(DataRow row, bool doubleQuote = false)
        {
            string result = "";
            var columns = row.Table.Columns;
            foreach (DataColumn c in columns)
            {
                switch (c.DataType.Name.ToLower())
                {
                    case "boolean":
                        if (doubleQuote)
                        {
                            result += (row[c].ToString() == "False" ? "''0''" : "''1''") + ",";
                        }
                        else
                        {
                            result += (row[c].ToString() == "False" ? "'0'" : "'1'") + ",";
                        }
                        break;
                    case "string":
                        if (doubleQuote)
                        {
                            result += "''" + row[c].ToString() + "''" + ",";
                        }
                        else
                        {
                            result += "'" + row[c].ToString() + "'" + ",";
                        }
                        break;
                    case "int32":
                        result += row[c].ToString() + ",";
                        break;
                    case "decimal":
                        result += row[c].ToString() + ",";
                        break;

                    default:
                        if (doubleQuote)
                        {
                            result += "''" + row[c].ToString() + "'',";
                        }
                        else
                        {
                            result += "'" + row[c].ToString() + "',";
                        }
                        break;
                }
            }
            result = result.TrimEnd(new char[] { ',' });
            return result;
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
                    outstr = "VARCHAR2(2000)";
                    break;
                case "int32":
                    outstr = "NUMBER(10)";
                    break;
                case "decimal":
                    outstr = "NUMBER(18)";
                    break;

                default:
                    outstr = "VARCHAR2(4000)";
                    break;
            }

            return outstr;
        }
    }
}
