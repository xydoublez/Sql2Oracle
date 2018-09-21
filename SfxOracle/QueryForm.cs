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
        OracleHelper orcl;
        private void query()
        {
            try
            {

                orcl = new OracleHelper(string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Max Pool Size=512;Pooling=true;",
                    txtIp.Text, txtPort.Text, txtServiceName.Text, txtUserId.Text, txtPassword.Text));
                DataSet ds = orcl.Query(this.sqlText.Text);
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
                this.rbResult.Text = GetVerifyInfo(txtIp.Text, txtPort.Text, txtServiceName.Text, txtUserId.Text, txtPassword.Text, "");
                this.tabControl1.SelectedIndex = 1;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        //验证失败数
        int failCount = 0;
        private string GetVerifyInfo(string ip,string port,string serviceName,string userId,string password,string hospitalName,string module="")
        {
            
                orcl = new OracleHelper(string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Max Pool Size=512;Pooling=true;",
                 ip,port,serviceName,userId,password));
                StringBuilder res = new StringBuilder();
                failCount = 0;

                //0验证是否归档模式
                res.AppendLine("==================================================================");
                res.AppendLine("0.参数：");
                if (IsArchive())
                {
                    res.AppendLine("归档模式：开启");
                }
                else
                {
                    res.AppendLine("归档模式：没有开启");
                    failCount += 1;
                }
                if (isTraceOn())
                {

                    res.AppendLine("监听日志跟踪：开启（判定依据为跟踪日志有今天的数据）");
                    failCount += 1;
                }
                else
                {
                    res.AppendLine("监听日志跟踪：关闭 （判定依据为跟踪日志有今天的数据）");
                }
                res.AppendLine(GetNls());
                res.AppendLine("==================================================================");
                res.AppendLine("==================================================================");
                res.AppendLine("==================================================================");
                res.AppendLine("==================================================================");
                //1主要参数
                res.AppendLine(queryParamter());
                res.AppendLine("==================================================================");
                //2RMAN参数
                res.AppendLine(queryParamterRman());
                res.AppendLine("==================================================================");
                //3RMAN今天备份情况
                res.AppendLine(queryParamterRmanStatus());
                res.AppendLine("==================================================================");
                //4 date file
                res.AppendLine(queryRmanFile());
                res.AppendLine("==================================================================");
                res.AppendLine(queryDataFile());
                res.AppendLine("==================================================================");
                if (failCount > 0)
                {
                    res.AppendLine("SFXERROR:" + failCount);
                }
                return res.ToString();

           

        }
        private bool IsArchive()
        {
            DataSet ds = orcl.Query("select log_mode from v$database");
            if (ds.Tables[0].Rows[0][0].ToString() == "ARCHIVELOG")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string GetNls()
        {

            DataSet ds = orcl.Query(@"select parameter, value from nls_database_parameters where parameter = 'NLS_CHARACTERSET'");
            var dt = ds.Tables[0];
            var result = "字符集：" + dt.Rows[0][1].ToString();
            return result;

        }
        private string queryParamter()
        {
            DataSet ds = orcl.Query(@"select name,display_value from v$parameter
where name in ('processes','sga_max_size','spfile','memory_target','memory_max_target',
'control_files','control_file_record_keep_time','log_archive_dest_1','db_recovery_file_dest_size'
,'undo_retention','service_names','audit_trail','db_name','optimizer_mode')");
            var dt = ds.Tables[0];
            var result = "1.系统主要参数：\r\n";
            foreach (DataRow row in dt.Rows)
            {
                result += "参数名：" + row["name"].ToString() + "\r\n";
                result += "值：" + row["display_value"].ToString() + "\r\n";
            }
            return result;
        }
        private string queryParamterRman()
        {
            DataSet ds = orcl.Query(@"SELECT NAME, VALUE FROM V$RMAN_CONFIGURATION");
            var dt = ds.Tables[0];
            var result = "2.RMAN配置参数：\r\n";
            if (dt.Rows.Count == 0)
            {
                failCount += 1;
            }
            foreach (DataRow row in dt.Rows)
            {
                result += "参数名：" + row["NAME"].ToString() + "\r\n";
                result += "值：" + row["VALUE"].ToString() + "\r\n";
            }
            return result;
        }
        private string queryParamterRmanStatus()
        {
            DataSet ds = orcl.Query(@"SELECT operation,status,object_type,start_time,end_time FROM V$RMAN_STATUS 
WHERE START_TIME >= sysdate-7
  AND END_TIME   <= sysdate
  AND OPERATION ='BACKUP'
  order by start_time desc
");
            var dt = ds.Tables[0];
            var result = "3.RMAN一周内备份情况：\r\n";
            foreach (DataRow row in dt.Rows)
            {
                result += "操作：" + row["operation"].ToString() + "\r\n";
                result += "状态：" + row["status"].ToString() + "\r\n";
                result += "类型：" + row["object_type"].ToString() + "\r\n";
                result += "开始时间：" + row["start_time"].ToString() + "\r\n";
                result += "结束时间：" + row["end_time"].ToString() + "\r\n";
            }
            return result;
        }
        private string queryDataFile()
        {
            DataSet ds = orcl.Query(@"Select file_name,status,file_id,tablespace_name,round(user_bytes/1024/1024/1024,2) GB FROM DBA_DATA_FILES
ORDER BY FILE_ID ");
            var dt = ds.Tables[0];
            var result = "5.数据文件情况：\r\n";
            foreach (DataRow row in dt.Rows)
            {
                result += "文件名称：" + row["file_name"].ToString() + "\r\n";
                result += "状态：" + row["status"].ToString() + "\r\n";
                result += "文件ID：" + row["file_id"].ToString() + "\r\n";
                result += "表空间名称：" + row["tablespace_name"].ToString() + "\r\n";
                result += "文件使用大小(GB)：" + row["GB"].ToString() + "\r\n";
            }
            return result;
        }
        private string queryRmanFile()
        {
            DataSet ds = orcl.Query(@"SELECT
	A .RECID 备份集ID,
	A .SET_STAMP,
	DECODE (
		B.INCREMENTAL_LEVEL,
		'',
		DECODE (
			BACKUP_TYPE,
			'L',
			'Archivelog',
			'Full'
		),
		1,
		'Incr-1级',
		0,
		'Incr-0级',
		B.INCREMENTAL_LEVEL
	) 备份类型,
	B.CONTROLFILE_INCLUDED 包含CTL,
	DECODE (
		A .STATUS,
		'A',
		'AVAILABLE',
		'D',
		'DELETED',
		'X',
		'EXPIRED',
		'ERROR'
	) 备份状态,
	A .DEVICE_TYPE 磁盘类型,
	A .START_TIME  开始时间,
	A .COMPLETION_TIME 完成时间,
	round(A .ELAPSED_SECONDS,2)  耗时秒,
	round(a.BYTES/1024/1024/1024,2)大小G,
	a.COMPRESSED 是否压缩,
	A .TAG Tag,
	A .HANDLE 路径
FROM
	GV$BACKUP_PIECE A,
	GV$BACKUP_SET B
WHERE
	A .SET_STAMP = B.SET_STAMP
AND A .DELETED = 'NO'
AND A .set_count = b.set_count
and A.START_TIME >= sysdate-7 and A.start_time <= sysdate 
ORDER BY
	A .COMPLETION_TIME DESC");
            var dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                failCount += 1;
            }
            var result = "4.RMAN一周内备份集相关信息：\r\n";
            foreach (DataRow row in dt.Rows)
            {
                result += "备份集ID：" + row["备份集ID"].ToString() + "\r\n";
                result += "备份类型：" + row["备份类型"].ToString() + "\r\n";
                result += "开始时间：" + row["开始时间"].ToString() + "\r\n";
                result += "完成时间：" + row["完成时间"].ToString() + "\r\n";
                result += "耗时秒：" + row["耗时秒"].ToString() + "\r\n";
                result += "大小G：" + row["大小G"].ToString() + "\r\n";
                result += "是否压缩：" + row["是否压缩"].ToString() + "\r\n";
                result += "备份路径：" + row["路径"].ToString() + "\r\n";
            }
            return result;
        }

        private bool isTraceOn()
        {
            bool suc = false;
            try
            {
                //0创建表
                string traceDir = getBase() + "\\diag\\tnslsnr\\" + getHostName() + "\\listener\\trace";
                orcl.ExecuteSql(@"BEGIN

    EXECUTE IMMEDIATE 'create or replace directory sfxdirtrace as ''" + traceDir + @"''' ;
            EXECUTE IMMEDIATE 'create  table sfxdirtrace
           (log varchar2(2000))
 organization external
 (type oracle_loader
  default directory sfxdirtrace
  access parameters

 (records delimited by newline  CHARACTERSET ZHS16GBK)
 location(''listener.log''))
 reject limit unlimited
' ; 

    END; ");
                //1查询数据

                DataSet ds = orcl.Query(@"select count(*) from sfxdirtrace where log like '' ||  to_char(sysdate,'DD-MON-RRRR') || '%'");
                if (ds.Tables[0].Rows[0][0].ToString() != "0")
                {
                    suc = true;
                }
                else
                {
                    suc = false;
                }
                //2最后删除表
                orcl.ExecuteSql("drop table sfxdirtrace");
                return suc;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
                return true;
            }
        }
        private string getHostName()
        {

            DataSet ds = orcl.Query(@"select host_name from v$instance");
            return ds.Tables[0].Rows[0][0].ToString();
        }
        private string getBase()
        {

            DataSet ds = orcl.Query(@"select value from v$diag_info  where name='ADR Base'");
            return ds.Tables[0].Rows[0][0].ToString();
        }

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
        private void batchVerify()
        {
            DataSet ds = getBatchInfo();
            AddBatchLog("开始验证");
            var table = ds.Tables[0];
            int i = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("医院名称"));
            dt.Columns.Add(new DataColumn("服务器"));
            dt.Columns.Add(new DataColumn("IP"));
            dt.Columns.Add(new DataColumn("服务名"));
            dt.Columns.Add(new DataColumn("验证内容"));
            dt.Columns.Add(new DataColumn("是否验证通过"));
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
                string result = "";
                try
                {
                    result = GetVerifyInfo(ip, "1521", serviceName, userId, password, hospitalName);
                    AddBatchLog(hospitalName + "-" + module + "验证完成");
                }
                catch(Exception ex)
                {
                    result = "SFXERROR:"+ ex.Message + ex.StackTrace;
                    AddBatchLog(result);
                }
                DataRow row = dt.NewRow();
                row["医院名称"] = hospitalName;
                row["服务器"] = module;
                row["IP"] = ip;
                row["服务名"] = serviceName;
                row["验证内容"] = result;
                row["是否验证通过"] = result.IndexOf("SFXERROR:")>-1 ? "否" : "是";
                dt.Rows.Add(row);

            }
            this.dataGridView2.DataSource = dt;
            this.dataGridView2.Refresh();
            AddBatchLog("验证结束");
            MessageBox.Show("验证结束");
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
                var info = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n";
                info += msg + "\r\n";
                this.rbBatchResult.AppendText(info);
                File.AppendAllText("log.txt", info);
                this.rbBatchResult.ScrollToCaret();
            }));
        }
    }

}
