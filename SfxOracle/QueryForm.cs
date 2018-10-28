using ExcelDataReader;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SfxOracle
{
    public partial class QueryForm : Form
    {
        public QueryForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            query();
        }
        private void query2()
        {
            try
            {

                var connstr = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Max Pool Size=512;Pooling=true;Connection Timeout=60;",
                    txtIp.Text, txtPort.Text, txtServiceName.Text, txtUserId.Text, txtPassword.Text);
                //Oracle.DataAccess.Client.OracleConnectionStringBuilder connectionStringBuilder = new Oracle.DataAccess.Client.OracleConnectionStringBuilder();
                //connectionStringBuilder.DataSource = this.txtIp.Text;
                //connectionStringBuilder.UserID = this.txtUserId.Text;
                //connectionStringBuilder.Password = this.txtPassword.Text;
                //var connstr = connectionStringBuilder.ToString();
                if (cb_sysdba.Checked)
                {
                    connstr += "DBA Privilege=SYSDBA;";
                }
                using (Oracle.DataAccess.Client.OracleConnection conn = new Oracle.DataAccess.Client.OracleConnection(connstr))
                {

                    DataSet ds = new DataSet();
                    Oracle.DataAccess.Client.OracleDataAdapter oracleDataAdapter = new Oracle.DataAccess.Client.OracleDataAdapter(this.sqlText.Text, conn);
                    oracleDataAdapter.Fill(ds);
                    this.dataGridView1.DataSource = ds.Tables[0];
                    this.dataGridView1.Refresh();
                }
                   


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        OracleHelper orcl;
        private void query()
        {
            try
            {

                var connstr = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Max Pool Size=512;Pooling=true;Connection Timeout=60;",
                    txtIp.Text, txtPort.Text, txtServiceName.Text, txtUserId.Text, txtPassword.Text);
                if (cb_sysdba.Checked)
                {
                    connstr += "DBA Privilege=SYSDBA;";
                }
                int type = 0;
                if (cb_client.Checked)
                {
                    type = 1;
                }
                db d = new db(connstr, type);
                DataSet ds = d.Query(this.sqlText.Text);
                this.dataGridView1.DataSource = ds.Tables[0];
                this.dataGridView1.Refresh();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void sqlText_Load(object sender, EventArgs e)
        {

        }

        private void 验证_Click(object sender, EventArgs e)
        {
            try
            {
                var connstr = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Max Pool Size=512;Pooling=true;Connection Timeout=6;",
                 txtIp.Text, txtPort.Text, txtServiceName.Text, txtUserId.Text, txtPassword.Text);
                int type = 0;
                if (cb_client.Checked)
                {
                    type = 1;
                }
                db d = new db(connstr, type);
                this.rbResult.Text = d.GetVerifyInfo(txtIp.Text, txtPort.Text, txtServiceName.Text, txtUserId.Text, txtPassword.Text, "");
                this.tabControl1.SelectedIndex = 1;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        //验证失败数
        int failCount = 0;
       

        private void button2_Click(object sender, EventArgs e)
        {

        }
        //批量数据表格路径
        string xlsFile = "";
        private void button3_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            this.Invoke(new Action(() => {
                this.openFileDialog1.Title = "请选择批量验证的EXCEL表格";
                if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    xlsFile = this.openFileDialog1.FileName;
                    lblxlsInfo.Text += xlsFile;
                    this.tabControl1.SelectedIndex = 2;
                    try
                    {
                        batchVerify();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.StackTrace);
                    }
                }
            }));
         
        }
        DataTable dt;
        int counter = 0;
        IntEx counter2 = new IntEx();
        private void batchVerify()
        {
            DataSet ds = getBatchInfo();
            AddBatchLog("开始验证");
            var table = ds.Tables[0];
            int i = 0;
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("医院名称"));
            dt.Columns.Add(new DataColumn("服务器"));
            dt.Columns.Add(new DataColumn("IP"));
            dt.Columns.Add(new DataColumn("服务名"));
            dt.Columns.Add(new DataColumn("验证内容"));
            dt.Columns.Add(new DataColumn("是否验证通过"));
            dt.Columns.Add(new DataColumn("数据库版本"));
            List<DataInfo> list = new List<DataInfo>();
            counter = table.Rows.Count;
            foreach (DataRow item in table.Rows)
            {
                i++;
                if (i == 1)
                {
                    continue;
                }
                
                var hospitalName = item[0].ToString();
                var module = item[1].ToString();
                var ip = item[2].ToString();
                var userId = item[3].ToString();
                var password = item[4].ToString();
                var serviceName = item[5].ToString();
                //string result = "";
                //try
                //{
                //    result = GetVerifyInfo(ip, "1521", serviceName, userId, password, hospitalName);
                //    AddBatchLog(hospitalName + "-" + module + "验证完成");
                //}
                //catch
                //{
                //    result = "SFXERROR:连接失败";

                //    AddBatchLog(result);
                //}
                //DataRow row = dt.NewRow();
                //row["医院名称"] = hospitalName;
                //row["服务器"] = module;
                //row["IP"] = ip;
                //row["服务名"] = serviceName;
                //row["验证内容"] = result;
                //row["是否验证通过"] = result.IndexOf("SFXERROR:")>-1 ? "否" : "是";
                //row["数据库版本"] = GetVersion();
                //dt.Rows.Add(row);
                DataInfo info = new DataInfo();
                info.hospitalname = hospitalName;
                info.ip = ip;
                info.IsPass = "否";
                info.result = "";
                info.serviceName = serviceName;
                info.userId = userId;
                info.password = password;
                info.module = module;
                list.Add(info);
                

            }
            ThreadPool.SetMaxThreads(4, 4);
            ThreadPool.SetMinThreads(2, 2);
            foreach (var item in list)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(verify2), item);
            }

           
            //AddBatchLog("验证结束");
            //MessageBox.Show("验证结束");
        }
        private void verify2(Object info)
        {
            DataInfo item = (DataInfo)info;
            var connstr = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Min Pool Size=1;Max Pool Size=100;Pooling=true;Connection Timeout=6;Enlist=false;",
             item.ip, "1521", item.serviceName, item.userId, item.password);
            int type = 0;
            if (cb_client.Checked)
            {
                type = 1;
            }
            db d = new db(connstr, type);
            //counter2.Incre();
            //AddBatchLog("第"+counter2.Val.ToString());

            try
            {
             
                item.result = d.GetVerifyInfo(item.ip, "1521", item.serviceName, item.userId, item.password, item.hospitalname, item.module);
                AddBatchLog(item.hospitalname + "-" + item.module + "验证完成");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                item.result = "SFXERROR:连接失败";
                AddBatchLog(item.result);
            }
            
            DataRow row = dt.NewRow();
            row["医院名称"] = item.hospitalname;
            row["服务器"] = item.module;
            row["IP"] = item.ip;
            row["服务名"] = item.serviceName;
            row["验证内容"] = item.result;
            row["是否验证通过"] = item.result.IndexOf("SFXERROR:") > -1 ? "否" : "是";
            row["数据库版本"] = d.GetVersion();
            dt.Rows.Add(row);
              counter2.Incre();
            AddBatchLog("第"+counter2.Val.ToString());
            this.BeginInvoke(new Action(() => {
                this.dataGridView2.DataSource = dt;
                this.dataGridView2.Refresh();
              

            }));

            
            if (counter2.Val == counter)
            {
                MessageBox.Show("验证完毕");
                AddBatchLog("验证完毕");
            }


        }
        private class DataInfo
        {
            public string hospitalname { get; set; }
            public string module { get; set; }
            public string ip { get; set; }
            public string serviceName { get; set; }

            public string result { get; set; }
            public string version { get; set; }
            public string IsPass { get; set; }
            public string userId { get; set; }
            public string password { get; set; }
        }
        private DataSet getBatchInfo()
        {
            using (var stream = File.Open(xlsFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    return  reader.AsDataSet();
                    
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                explort();
                MessageBox.Show("导出成功!");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        private void explort()
        {
            SaveFileDialog sflg = new SaveFileDialog();
            sflg.Filter = "Excel(*.xls)|*.xls|Excel(*.xlsx)|*.xlsx";
            if (sflg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            //this.gridView1.ExportToXls(sflg.FileName);
            //NPOI.xs book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.IWorkbook book = null;
            if (sflg.FilterIndex == 1)
            {
                book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            }
            else
            {
                book = new NPOI.XSSF.UserModel.XSSFWorkbook();
            }

            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("test_001");

            // 添加表头
            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);
            int index = 0;
            foreach (DataGridViewColumn item in this.dataGridView2.Columns)
            {
                if (item.Visible)
                {
                    NPOI.SS.UserModel.ICell cell = row.CreateCell(index);
                    cell.SetCellType(NPOI.SS.UserModel.CellType.String);
                    cell.SetCellValue(item.HeaderText);
                    index++;
                }
            }

            // 添加数据

            for (int i = 0; i < this.dataGridView2.Rows.Count; i++)
            {
                index = 0;
                row = sheet.CreateRow(i + 1);
                foreach (DataGridViewColumn item in this.dataGridView2.Columns)
                {
                    if (item.Visible)
                    {
                        if (dataGridView2.Rows[i].Cells[item.HeaderText].Value != null)
                        {
                            NPOI.SS.UserModel.ICell cell = row.CreateCell(index);
                            cell.SetCellType(NPOI.SS.UserModel.CellType.String);
                            cell.SetCellValue(dataGridView2.Rows[i].Cells[item.HeaderText].Value.ToString().Trim());
                        }
                        index++;
                    }
                }
            }
            // 写入 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            book = null;

            using (FileStream fs = new FileStream(sflg.FileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }

            ms.Close();
            ms.Dispose();
        }
        private void AddBatchLog(string msg)
        {  
            Application.DoEvents();
            this.rbBatchResult.BeginInvoke(new Action(() => {
                var info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n";
                info += msg + "\r\n";
                this.rbBatchResult.AppendText(info);
                File.AppendAllText("log.txt", info);
                this.rbBatchResult.ScrollToCaret();
            }));
        }
     

        private void cb_sysdba_CheckedChanged(object sender, EventArgs e)
        {

        }
        public class IntEx
        {
            int _val;
            public int Val
            {
                get { return _val; }
                set
                {
                    Interlocked.CompareExchange(ref _val, value, _val);
                }
            }

            public IntEx() { }
            public IntEx(int ival)
            {
                _val = ival;
            }

            public int Add(int ival)
            {
                return Interlocked.Add(ref _val, ival);
            }

            public int Incre()
            {
                return Interlocked.Increment(ref _val);
            }

            public int Decre()
            {
                return Interlocked.Decrement(ref _val);
            }
        }

        private void QueryForm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ViewText f = new ViewText(dataGridView2.CurrentRow.Cells[e.ColumnIndex].Value.ToString());
            f.ShowDialog();
        }
    }

}
