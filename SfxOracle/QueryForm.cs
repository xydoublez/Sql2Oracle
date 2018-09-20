﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

                orcl = new OracleHelper(string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Max Pool Size=512;Pooling=true;Connection Timeout=600;",
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
            verify();
        }
        private void verify()
        {
            try
            {
                orcl = new OracleHelper(string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Max Pool Size=512;Pooling=true;Connection Timeout=600;",
                 txtIp.Text, txtPort.Text, txtServiceName.Text, txtUserId.Text, txtPassword.Text));
                StringBuilder res = new StringBuilder();
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
                }
                res.AppendLine(GetNls());
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
                this.rbResult.Text = res.ToString();
                this.tabControl1.SelectedIndex = 1;

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }

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
    }
}