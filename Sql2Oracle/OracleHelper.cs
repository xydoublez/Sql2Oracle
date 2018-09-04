using System;

using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;
//引用Oracle odp.net 纯托管类，不依赖于oracle客户端
using Oracle.ManagedDataAccess.Client;
using System.Text;
using Oracle.ManagedDataAccess.Types;
namespace Sql2Oracle
{
	/// <summary>
    /// 数据访问基础类(基于Oracle) 
    /// 引用Oracle odp.net 纯托管类，不依赖于oracle客户端
	/// </summary>
	public class OracleHelper
	{
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.		
        private string _connectionString = "";
        /// <summary>
        ///  //标准连接-SSPI
        ///  private static readonly string defaultConnectString = "Data Source=ORCL;Integrated Security=SSPI;";
        ///  标准连接
        ///  private static readonly string defaultConnectString = "Data Source=ORCL;User Id=UPDM;Password=1234;";
        ///  PACS使用此连接字符串格式 
        /// private static readonly string defaultConnectString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.67.89.9)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ZYRISDB)));User Id=power;Password=Sfx371482;";
        /// </summary>
        public string connectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public OracleHelper(string connectionString)
            
		{
            this.connectionString = connectionString;
        }

        #region 公用方法
        
        public T getInstance<T>() where T:new()
        {
            return new T();
        }
        public  bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public bool Exists(string strSql, List<DbParameter> paramList)
        {
            Type t = paramList.GetType();
            if (t.IsClass == true) 
            {
                string a = t.FullName;
            }
            object obj = GetSingle(strSql, paramList);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

       
        #endregion

		
		#region  执行简单SQL语句

		/// <summary>
		/// 执行SQL语句，返回影响的记录数
		/// </summary>
		/// <param name="SQLString">SQL语句</param>
		/// <returns>影响的记录数</returns>
		public  int ExecuteSql(string SQLString)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{				
				using (OracleCommand cmd = new OracleCommand(SQLString,connection))
				{
						connection.Open();
						cmd.ExecuteNonQuery();
						return 0;
					 
				}				
			}
		}
		
		/// <summary>
		/// 执行多条SQL语句，实现数据库事务。
		/// </summary>
		/// <param name="SQLStringList">多条SQL语句</param>		
		public  int ExecuteSqlTran(ArrayList SQLStringList)
		{
			using (OracleConnection conn = new OracleConnection(connectionString))
			{
				conn.Open();
				OracleCommand cmd = new OracleCommand();
				cmd.Connection=conn;				
				OracleTransaction tx=conn.BeginTransaction();			
				cmd.Transaction=tx;				
				try
				{
                    int count = 0;
					for(int n=0;n<SQLStringList.Count;n++)
					{
						string strsql=SQLStringList[n].ToString();
						if (strsql.Trim().Length>1)
						{
							cmd.CommandText=strsql;
							count += cmd.ExecuteNonQuery();
						}
					}										
					tx.Commit();
                    return count;
				}
				catch(OracleException E)
				{		
					tx.Rollback();
                    return 0;
				}
			}
		}
		/// <summary>
		/// 执行带一个存储过程参数的的SQL语句。
		/// </summary>
		/// <param name="SQLString">SQL语句</param>
		/// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
		/// <returns>影响的记录数</returns>
		public  int ExecuteSql(string SQLString,string content)
		{				
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				OracleCommand cmd = new OracleCommand(SQLString,connection);
                OracleParameter myParameter = new OracleParameter("@content", OracleDbType.NVarchar2);
				myParameter.Value = content ;
				cmd.Parameters.Add(myParameter);
				try
				{
					connection.Open();
					int rows=cmd.ExecuteNonQuery();
					return rows;
				}
				catch(OracleException E)
				{				
					throw new Exception(E.Message);
				}
				finally
				{
					cmd.Dispose();
					connection.Close();
				}	
			}
		}		
		/// <summary>
		/// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
		/// </summary>
		/// <param name="strSQL">SQL语句</param>
		/// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
		/// <returns>影响的记录数</returns>
		public  int ExecuteSqlInsertImg(string strSQL,byte[] fs)
		{		
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				OracleCommand cmd = new OracleCommand(strSQL,connection);
                OracleParameter myParameter = new OracleParameter("@fs", OracleDbType.LongRaw);
				myParameter.Value = fs ;
				cmd.Parameters.Add(myParameter);
				try
				{
					connection.Open();
					int rows=cmd.ExecuteNonQuery();
					return rows;
				}
				catch(OracleException E)
				{				
					throw new Exception(E.Message);
				}
				finally
				{
					cmd.Dispose();
					connection.Close();
				}				
			}
		}
		
		/// <summary>
		/// 执行一条计算查询结果语句，返回查询结果（object）。
		/// </summary>
		/// <param name="SQLString">计算查询结果语句</param>
		/// <returns>查询结果（object）</returns>
		public object GetSingle(string SQLString)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				using(OracleCommand cmd = new OracleCommand(SQLString,connection))
				{
					try
					{
						connection.Open();
						object obj = cmd.ExecuteScalar();
						if((Object.Equals(obj,null))||(Object.Equals(obj,System.DBNull.Value)))
						{					
							return null;
						}
						else
						{
							return obj;
						}				
					}
					catch(OracleException e)
					{						
						connection.Close();
						throw new Exception(e.Message);
					}	
				}
			}
		}
		/// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
		/// </summary>
		/// <param name="strSQL">查询语句</param>
		/// <returns>OracleDataReader</returns>
		public  OracleDataReader ExecuteReader(string strSQL)
		{
			OracleConnection connection = new OracleConnection(connectionString);			
			OracleCommand cmd = new OracleCommand(strSQL,connection);				
			try
			{
				connection.Open();
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				return myReader;
			}
			catch(OracleException e)
			{								
				throw new Exception(e.Message);
			}			
			
		}		
		/// <summary>
		/// 执行查询语句，返回DataSet
		/// </summary>
		/// <param name="SQLString">查询语句</param>
		/// <returns>DataSet</returns>
		public  DataSet Query(string SQLString)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				DataSet ds = new DataSet();
				try
				{
                    
					connection.Open();
					OracleDataAdapter command = new OracleDataAdapter(SQLString,connection);	
					command.Fill(ds,"ds");
				}
				catch(OracleException ex)
				{
                    
					throw new Exception(ex.Message);
				}			
				return ds;
			}			
		}

        /// <summary>
        /// 执行多条查询语句，返回多个表的DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet Query2(string SQLString, List<DbParameter> cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    
                    connection.Open();
                    OracleCommand cmd = new OracleCommand();
                     cmd.BindByName = true;
                     PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                     cmd.ExecuteNonQuery();
                    int count = SQLString.Split(new char[] { ';' }).Length-2;
                    for (int i = 0; i < count; i++)
                    {

                        OracleDataReader dr = ((OracleRefCursor)cmd.Parameters[i].Value).GetDataReader();
                        DataTable dt = new DataTable();
                        dt.Load(dr);
                        ds.Tables.Add(dt);
                        dr.Close();
                    }
                  
                    
                  
                }
                catch (OracleException ex)
                {
                    System.Diagnostics.Trace.WriteLine("出错SQL:" + SQLString);
                    throw new Exception(ex.Message + "SQL:" + SQLString);
                }
                return ds;
            }
        }
		#endregion

		 #region 执行带参数的SQL语句

		/// <summary>
		/// 执行SQL语句，返回影响的记录数
		/// </summary>
		/// <param name="SQLString">SQL语句</param>
		/// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, List<DbParameter> cmdParms)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{				
				using (OracleCommand cmd = new OracleCommand())
				{
					try
					{
                        cmd.BindByName = true;
						PrepareCommand(cmd, connection, null,SQLString, cmdParms);
						int rows=cmd.ExecuteNonQuery();
						cmd.Parameters.Clear();
						return rows;
					}
					catch(OracleException E)
					{				
						throw new Exception(E.Message);
					}
				}				
			}
		}
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的DbParameter[]）</param>
        /// <returns>不回滚则返回影响的行数，回滚则返回0</returns>
      
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的DbParameter[]）</param>
        /// <returns>不回滚则返回影响的行数，回滚则返回0</returns>
      
      
		/// <summary>
		/// 执行多条SQL语句，实现数据库事务。
		/// </summary>
		/// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的DbParameter[]）</param>
		public  void ExecuteSqlTran(Hashtable SQLStringList)
		{			
			using (OracleConnection conn = new OracleConnection(connectionString))
			{
				conn.Open();
				using (OracleTransaction trans = conn.BeginTransaction()) 
				{
					OracleCommand cmd = new OracleCommand();
					try 
					{
						//循环
						foreach (DictionaryEntry myDE in SQLStringList)
						{	
							string 	cmdText=myDE.Key.ToString();
                            List<DbParameter> cmdParms = (List<DbParameter>)myDE.Value;
							PrepareCommand(cmd,conn,trans,cmdText, cmdParms);
							int val = cmd.ExecuteNonQuery();
							cmd.Parameters.Clear();

							trans.Commit();
						}					
					}
					catch 
					{
						trans.Rollback();
						throw;
					}
				}				
			}
		}
	
				
		/// <summary>
		/// 执行一条计算查询结果语句，返回查询结果（object）。
		/// </summary>
		/// <param name="SQLString">计算查询结果语句</param>
		/// <returns>查询结果（object）</returns>
        public object GetSingle(string SQLString, List<DbParameter> cmdParms)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				using (OracleCommand cmd = new OracleCommand())
				{
					try
					{
						PrepareCommand(cmd, connection, null,SQLString, cmdParms);
						object obj = cmd.ExecuteScalar();
						cmd.Parameters.Clear();
						if((Object.Equals(obj,null))||(Object.Equals(obj,System.DBNull.Value)))
						{					
							return null;
						}
						else
						{
							return obj;
						}				
					}
					catch(OracleException e)
					{				
						throw new Exception(e.Message);
					}					
				}
			}
		}
		
		/// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
		/// </summary>
		/// <param name="strSQL">查询语句</param>
		/// <returns>OracleDataReader</returns>
        public OracleDataReader ExecuteReader(string SQLString, List<DbParameter> cmdParms)
		{		
			OracleConnection connection = new OracleConnection(connectionString);
			OracleCommand cmd = new OracleCommand();				
			try
			{
				PrepareCommand(cmd, connection, null,SQLString, cmdParms);
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				cmd.Parameters.Clear();
				return myReader;
			}
			catch(OracleException e)
			{								
				throw new Exception(e.Message);
			}					
			
		}		
		
		/// <summary>
		/// 执行查询语句，返回DataSet
		/// </summary>
		/// <param name="SQLString">查询语句</param>
		/// <returns>DataSet</returns>
        public DataSet Query(string SQLString, List<DbParameter> cmdParms)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				OracleCommand cmd = new OracleCommand();
                cmd.BindByName = true;
				PrepareCommand(cmd, connection, null,SQLString, cmdParms);
				using( OracleDataAdapter da = new OracleDataAdapter() )
				{
                    da.SelectCommand = cmd;
					DataSet ds = new DataSet();	
					try
					{												
						da.Fill(ds,"ds");
						cmd.Parameters.Clear();
					}
					catch(OracleException ex)
					{				
						throw new Exception(ex.Message);
					}			
					return ds;
				}				
			}			
		}


        private void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, List<DbParameter> cmdParms) 
		{
			if (conn.State != ConnectionState.Open)
				conn.Open();
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			if (trans != null)
				cmd.Transaction = trans;
			cmd.CommandType = CommandType.Text;//cmdType;
			if (cmdParms != null) 
			{
				foreach (DbParameter parm in cmdParms)
					cmd.Parameters.Add(parm);
			}
		}

		#endregion

		#region 存储过程操作

		/// <summary>
        /// 执行存储过程 返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>OracleDataReader</returns>
        public OracleDataReader RunProcedure(string ConnStr, string storedProcName, List<DbParameter> parameters)
		{
            
			OracleConnection connection = new OracleConnection(ConnStr);
			OracleDataReader returnReader;
			connection.Open();
			OracleCommand command = BuildQueryCommand( connection,storedProcName, parameters );
			command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);				
			return returnReader;			
		}
		
		
		/// <summary>
		/// 执行存储过程
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <param name="tableName">DataSet结果中的表名</param>
		/// <returns>DataSet</returns>
        public DataSet RunProcedure(string ConnStr, string storedProcName, List<DbParameter> parameters, string tableName)
		{
            
			using (OracleConnection connection = new OracleConnection(ConnStr))
			{
				DataSet dataSet = new DataSet();
				connection.Open();
				OracleDataAdapter sqlDA = new OracleDataAdapter();
				sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters );
				sqlDA.Fill( dataSet, tableName );
				connection.Close();
				return dataSet;
			}
		}

		
		/// <summary>
		/// 构建 OracleCommand 对象(用来返回一个结果集，而不是一个整数值)
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>OracleCommand</returns>
        private OracleCommand BuildQueryCommand(OracleConnection connection, string storedProcName, List<DbParameter> parameters)
		{			
			OracleCommand command = new OracleCommand( storedProcName, connection );
			command.CommandType = CommandType.StoredProcedure;
			foreach (DbParameter parameter in parameters)
			{
				command.Parameters.Add( parameter );
			}
			return command;			
		}

        /// <summary>
        /// !!!注意!!!不要用影响行数做逻辑判定，因为影响行数不准确
        /// 执行存储过程，返回影响的行数	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public int RunProcedure(string ConnStr, string storedProcName, List<DbParameter> parameters, out int rowsAffected)
		{
            
			using (OracleConnection connection = new OracleConnection(ConnStr))
			{
				connection.Open();
                OracleCommand command = BuildIntCommand(connection,storedProcName, parameters );
                rowsAffected = command.ExecuteNonQuery();
				return rowsAffected;
			}
		}
		
		/// <summary>
		/// 创建 OracleCommand 对象实例(用来返回一个整数值)	
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>OracleCommand 对象实例</returns>
        private OracleCommand BuildIntCommand(OracleConnection connection, string storedProcName, List<DbParameter> parameters)
		{
			OracleCommand command = BuildQueryCommand(connection,storedProcName, parameters );
            //command.Parameters.Add(new OracleParameter("ReturnValue",
            //    OracleDbType.Int32, 4, ParameterDirection.ReturnValue,
            //    false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
		}
        /// <summary>
        /// 返回对应表的模拟自增字段值
        /// </summary>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="procname">存储过程名</param>
        /// <returns>表的模拟自增字段值</returns>
        public decimal RunProcedure(string ConnStr, List<DbParameter> parameters, string procname)
        {
            DataSet ds = new DataSet();
            try
            {

                using (OracleConnection connection = new OracleConnection(ConnStr))
                {
                    decimal PK_CURNUM;
                    connection.Open();
                    OracleDataAdapter adp = new OracleDataAdapter();
                    OracleCommand command = BuildIntCommand(connection, procname, parameters);
                    adp.SelectCommand = command;
                    adp.Fill(ds, "ds");
                    decimal.TryParse(command.Parameters[":key_value"].Value.ToString(), out PK_CURNUM);
                    return PK_CURNUM;
                }

            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
        /// <summary>
        /// 返回复查登记的返回值
        /// </summary>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="procname">存储过程名</param>
        /// <returns>返回值</returns>
        public decimal RunProcedure1(string ConnStr, List<DbParameter> parameters, string procname)
        {
            DataSet ds = new DataSet();
            try
            {

                using (OracleConnection connection = new OracleConnection(ConnStr))
                {
                    decimal PK_CURNUM;
                    connection.Open();
                    OracleDataAdapter adp = new OracleDataAdapter();
                    OracleCommand command = BuildIntCommand(connection, procname, parameters);
                    adp.SelectCommand = command;
                    adp.Fill(ds, "ds");
                    decimal.TryParse(command.Parameters[":STATUS"].Value.ToString(), out PK_CURNUM);
                    return PK_CURNUM;
                }

            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
		#endregion	

	

    

        public int RunProcedureTrans(string ConnStr, string storedProcName, List<DbParameter> parameters, out int rowsAffected)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据指定格式拼接where后条件转换成DbParameter,参数化查询使用 复杂where条件尽可能不要调用
        /// !!!新添加的DAL方法自己写参数化查询，不允许调用此方法
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<DbParameter> WhereParsePara(string where, out string reWhere)
        {
            throw new NotImplementedException();
        }


        public DataSet DataPageAntiSqlInject(string strTableName, string strFieldsName, string strKey, int intPageSize, int intCurPageIndex, string strOrderBy, string strConditions, bool bitReturnCounts, bool bitDistinct, bool bitOrder)
        {
            throw new NotImplementedException();
        }
        private  DataSet ConvertDataReaderToDataSet(OracleDataReader reader)
        {
            DataSet dataSet = new DataSet();
            do
            {
                // Create new data table 

                DataTable schemaTable = reader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    // A query returning records was executed 

                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        // Create a column name that is unique in the data table 
                        string columnName = (string)dataRow["ColumnName"]; //+ " // Add the column definition to the data table 
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);

                    // Fill the data table we just created 

                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                            dataRow[i] = reader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    // No records were returned 

                    DataColumn column = new DataColumn("RowsAffected");
                    dataTable.Columns.Add(column);
                    dataSet.Tables.Add(dataTable);
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = reader.RecordsAffected;
                    dataTable.Rows.Add(dataRow);
                }
            }
            while (reader.NextResult());
            return dataSet;
        }
        /// <summary>
        ///查询pl/sql块即多条语句使用且用临时表
        ///因为临时表类型，需要开启事务，否则报错
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public DataSet QuerySql(string SQLString, List<DbParameter> cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleTransaction tran = connection.BeginTransaction())
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.BindByName = true;
                    PrepareCommand(cmd, connection, tran, SQLString, cmdParms);
                    cmd.AddToStatementCache = true;
                    using (OracleDataAdapter da = new OracleDataAdapter())
                    {
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "ds");
                            cmd.Parameters.Clear();
                        }
                        catch (OracleException ex)
                        {
                            tran.Rollback();
                            throw new Exception(ex.Message);
                        }
                        tran.Commit();
                        return ds;
                    }
                    
                }
            }			
        }
    }
}
