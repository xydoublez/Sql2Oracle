using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SfxOracle
{
    public class db
    {
        OracleHelper orcl;
        int failCount = 0;
        public  db(string connstr)
        {
            orcl = new OracleHelper(connstr);
        }
        public string GetDbVersion()
        {

            DataSet ds = orcl.Query(@"select product,version,status from product_component_version");
            var dt = ds.Tables[0];
            var result = "00.数据库版本信息：\r\n";
            foreach (DataRow row in dt.Rows)
            {
                result += "产品：" + row["product"].ToString() + "\r\n";
                result += "版本号：" + row["version"].ToString() + "\r\n";
                result += "状态：" + row["status"].ToString() + "\r\n";
            }
            return result;

        }
        public string GetVersion()
        {
            try
            {
                DataSet ds = orcl.Query(@"select version from product_component_version where rownum=1");
                var dt = ds.Tables[0];
                var result = "";
                foreach (DataRow row in dt.Rows)
                {
                    result += row["version"].ToString();

                }
                return result;
            }
            catch
            {
                return "";
            }

        }
        public string GetVerifyInfo(string ip, string port, string serviceName, string userId, string password, string hospitalName, string module = "")
        {
            //var connstr = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(Service_Name={2})));User Id={3};Password={4};Max Pool Size=512;Pooling=true;Connection Timeout=6;",
            //     ip, port, serviceName, userId, password);
            ////if (cb_sysdba.Checked)
            ////{
            ////    connstr += "DBA Privilege=SYSDBA;";
            ////}
            //orcl = new OracleHelper(connstr);
            StringBuilder res = new StringBuilder();
            failCount = 0;

            //0验证是否归档模式
            res.AppendLine("==================================================================");
            res.AppendLine(GetDbVersion());
            res.AppendLine("==================================================================");
            res.AppendLine("0.参数：");
            //if (IsArchive())
            //{
            //    res.AppendLine("归档模式：开启");
            //}
            //else
            //{
            //    res.AppendLine("归档模式：没有开启");
            //    failCount += 1;
            //}
            //if (isTraceOn())
            //{

            //    res.AppendLine("监听日志跟踪：开启（判定依据为跟踪日志有今天的数据）");
            //    failCount += 1;
            //}
            //else
            //{
            //    res.AppendLine("监听日志跟踪：关闭 （判定依据为跟踪日志有今天的数据）");
            //}
            //res.AppendLine(GetNls());
            //res.AppendLine("==================================================================");
            //res.AppendLine("==================================================================");
            //res.AppendLine("==================================================================");
            //res.AppendLine("==================================================================");
            ////1主要参数
            //res.AppendLine(queryParamter());
            //res.AppendLine("==================================================================");
            ////2RMAN参数
            //res.AppendLine(queryParamterRman());
            //res.AppendLine("==================================================================");
            ////3RMAN今天备份情况
            //res.AppendLine(queryParamterRmanStatus());
            //res.AppendLine("==================================================================");
            ////4 date file
            //res.AppendLine(queryRmanFile());
            //res.AppendLine("==================================================================");
            //res.AppendLine(queryDataFile());
            //res.AppendLine("==================================================================");
            if (failCount > 0)
            {
                res.AppendLine("SFXERROR:" + failCount);
            }
            return res.ToString();



        }
        public bool IsArchive()
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
        public string GetNls()
        {

            DataSet ds = orcl.Query(@"select parameter, value from nls_database_parameters where parameter = 'NLS_CHARACTERSET'");
            var dt = ds.Tables[0];
            var result = "字符集：" + dt.Rows[0][1].ToString();
            return result;

        }
        public string queryParamter()
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
        public string queryParamterRman()
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
        public string queryParamterRmanStatus()
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
        public string queryDataFile()
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
        public string queryRmanFile()
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

        public bool isTraceOn()
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
            }
            catch (Exception ex)
            {

                return true;
            }
        }
        public string getHostName()
        {

            DataSet ds = orcl.Query(@"select host_name from v$instance");
            return ds.Tables[0].Rows[0][0].ToString();
        }
        public string getBase()
        {

            DataSet ds = orcl.Query(@"select value from v$diag_info  where name='ADR Base'");
            return ds.Tables[0].Rows[0][0].ToString();
        }

      
        
    }
}
