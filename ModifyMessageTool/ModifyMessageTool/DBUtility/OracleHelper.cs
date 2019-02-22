using ModifyMessageTool.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModifyMessageTool.DBUtility
{
    public class OracleHelper
    {
        public static readonly string oracleconnectionstring = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).AppSettings.Settings["pfuserconnectionstring"].Value;

        private static OracleConnection Instance = null;

        public static OracleConnection getInstance(OracleConnection Instance)
        {
            if (Instance == null)
            {
                Instance = new OracleConnection(oracleconnectionstring);
            }
            return Instance;
        }



        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static DataSet Query<T>(string connectionString, string SQLString, T t)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                    Type type = t.GetType();
                    command.Fill(ds, type.Name);
                }
                catch (OracleException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="SQLString"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Update<T>(string connectionString, string SQLString, T t)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                    Type type = t.GetType();
                    command.Fill(ds, type.Name);

                }
                catch (OracleException ex)
                {
                    return false;
                    throw new Exception(ex.Message);

                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                return true;
            }
        }


        public static bool Insert<T>(string connectionString, string SQLString, T t) where T : class
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                    Type type = t.GetType();
                    command.Fill(ds, type.Name);

                }
                catch (OracleException ex)
                {
                    return false;
                    throw new Exception(ex.Message);

                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                return true;
            }
        }

        public static bool Delete<T>(string connectionString, string SQLString, T t) where T : class
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                    Type type = t.GetType();
                    command.Fill(ds, type.Name);

                }
                catch (OracleException ex)
                {
                    return false;
                    throw new Exception(ex.Message);

                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 执行sql查询，返回List集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IList<T> Query<T>(string sql,T t) where T :class
        {

            DataSet ds = OracleHelper.Query<T>(OracleHelper.oracleconnectionstring, sql, t);
            Type type = t.GetType();
            DataTable dt = ds.Tables[type.Name];
            List<T> list = ConvertToObject<T>(dt);

            return list;
        }
        /// <summary>
        /// 查询返回实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T Select<T>(string sql, T t) where T : new()
        {

            DataSet ds = OracleHelper.Query<T>(OracleHelper.oracleconnectionstring, sql, t);
            Type type = t.GetType();
            DataTable dt = ds.Tables[type.Name];
            IDataReader dr = dt.CreateDataReader();
            
            return DataReaderToModel<T>(dr);
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Update<T>(string sql, T t) where T : class
        {

            return OracleHelper.Update<T>(OracleHelper.oracleconnectionstring, sql, t);

        }
        /// <summary>
        /// 插入操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Insert<T>(string sql, T t) where T : class
        {

            return OracleHelper.Insert<T>(OracleHelper.oracleconnectionstring, sql, t);

        }

        public static bool Delete<T>(string sql,T t) where T: class
        {
            return OracleHelper.Delete<T>(OracleHelper.oracleconnectionstring, sql, t);
        }




        public static DataSet Query(string connectionString, string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                    return ds;
                }
            }
        }

        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
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
                foreach (OracleParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string connectionString, string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (OracleException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }



        /// <summary>
        ///     从 DataTale 对象中逐行读取记录并将记录转化为 T 类型的集合
        /// </summary>
        /// <typeparam name="T">目标类型参数</typeparam>
        /// <param name="reader">DataTale 对象。</param>
        /// <returns>指定类型的对象集合。</returns>
        public static List<T> ConvertToObject<T>(DataTable table)
            where T : class
        {
            return table == null
                ? new List<T>()
                : ConvertToObject<T>(table.CreateDataReader() as IDataReader);
        }
        /// <summary>
        ///     从 reader 对象中逐行读取记录并将记录转化为 T 类型的集合
        /// </summary>
        /// <typeparam name="T">目标类型参数</typeparam>
        /// <param name="reader">实现 IDataReader 接口的对象。</param>
        /// <returns>指定类型的对象集合。</returns>
        public static List<T> ConvertToObject<T>(IDataReader reader)
            where T : class
        {
            List<T> list = new List<T>();
            T obj = default(T);
            Type t = typeof(T);
            Assembly ass = t.Assembly;

            Dictionary<string, PropertyInfo> propertys = GetFields<T>(reader);
            PropertyInfo p = null;
            if (reader != null)
            {
                while (reader.Read())
                {
                    obj = ass.CreateInstance(t.FullName) as T;
                    foreach (string key in propertys.Keys)
                    {
                        p = propertys[key];
                        p.SetValue(obj, ChangeType(reader[key], p.PropertyType));
                    }
                    list.Add(obj);
                }
            }

            return list;
        }

        /// <summary>
        ///     将数据转化为 type 类型
        /// </summary>
        /// <param name="value">要转化的值</param>
        /// <param name="type">目标类型</param>
        /// <returns>转化为目标类型的 Object 对象</returns>
        private static object ChangeType(object value, Type type)
        {
            if (type.FullName == typeof(string).FullName)
            {
                return Convert.ChangeType(Convert.IsDBNull(value) ? null : value, type);
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                NullableConverter convertor = new NullableConverter(type);
                return Convert.IsDBNull(value) ? null : convertor.ConvertFrom(value);
            }
            return value;
        }


        /// <summary>
        ///     获取reader存在并且在 T 类中包含同名可写属性的集合
        /// </summary>
        /// <param name="reader">
        ///     可写域的集合
        /// </param>
        /// <returns>
        ///     以属性名为键，PropertyInfo 为值得字典对象
        /// </returns>
        private static Dictionary<string, PropertyInfo> GetFields<T>(IDataReader reader)
        {
            Dictionary<string, PropertyInfo> result = new Dictionary<string, PropertyInfo>();
            int columnCount = reader.FieldCount;
            Type t = typeof(T);

            PropertyInfo[] properties = t.GetProperties();
            if (properties != null)
            {
                List<string> readerFields = new List<string>();
                for (int i = 0; i < columnCount; i++)
                {
                    readerFields.Add(reader.GetName(i));
                }
                IEnumerable<PropertyInfo> resList =
                    (from PropertyInfo prop in properties
                     where prop.CanWrite && readerFields.Contains(prop.Name)
                     select prop);

                foreach (PropertyInfo p in resList)
                {
                    result.Add(p.Name, p);
                }
            }
            return result;
        }


        /// <summary>
        /// DataReader转实体
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="dr">DataReader</param>
        /// <returns>实体</returns>
        public static T DataReaderToModel<T>(IDataReader dr) where T : new()
        {
            T t = new T();
            if (dr == null) return default(T);
            using (dr)
            {
                if (dr.Read())
                {
                    // 获得此模型的公共属性
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    List<string> listFieldName = new List<string>(dr.FieldCount);
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        listFieldName.Add(dr.GetName(i).ToLower());
                    }

                    foreach (PropertyInfo p in propertys)
                    {
                        string columnName = p.Name;
                        if (listFieldName.Contains(columnName.ToLower()))
                        {
                            // 判断此属性是否有Setter或columnName值是否为空
                            object value = dr[columnName];
                            if (!p.CanWrite || value is DBNull || value == DBNull.Value) continue;
                            try
                            {
                                #region SetValue
                                switch (p.PropertyType.ToString())
                                {
                                    case "System.String":
                                        p.SetValue(t, Convert.ToString(value), null);
                                        break;
                                    case "System.Int32":
                                        p.SetValue(t, Convert.ToInt32(value), null);
                                        break;
                                    case "System.DateTime":
                                        p.SetValue(t, Convert.ToDateTime(value), null);
                                        break;
                                    case "System.Boolean":
                                        p.SetValue(t, Convert.ToBoolean(value), null);
                                        break;
                                    case "System.Double":
                                        p.SetValue(t, Convert.ToDouble(value), null);
                                        break;
                                    case "System.Decimal":
                                        p.SetValue(t, Convert.ToDecimal(value), null);
                                        break;
                                    default:
                                        p.SetValue(t, value, null);
                                        break;
                                }
                                #endregion
                            }
                            catch
                            {
                                //throw (new Exception(ex.Message));
                            }
                        }
                    }
                }
            }
            return t;
        }

    }
}
