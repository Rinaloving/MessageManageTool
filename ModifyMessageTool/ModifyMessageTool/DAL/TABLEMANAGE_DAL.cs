using ModifyMessageTool.DBUtility;
using ModifyMessageTool.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ModifyMessageTool.DAL
{
    /// <summary>
    /// DAL层调用方法，将结果返回给BLL层
    /// </summary>
    public class TABLEMANAGE_DAL
    {

        /// <summary>
        /// 查询TABLEMANAGE表集合
        /// </summary>
        /// <returns></returns>
        public List<TABLEMANAGE> Query()
        {
            TABLEMANAGE t = new TABLEMANAGE();
            string sql = @"select * from TABLEMANAGE";

            List<TABLEMANAGE> list = OracleHelper.Query<TABLEMANAGE>(sql, t).ToList();

            return list;
        }

        public TABLEMANAGE QueryByTableName(string tablename)
        {
            TABLEMANAGE t = new TABLEMANAGE();
            string sql = @"SELECT * FROM TABLEMANAGE WHERE TABLENAME = '" + tablename + "'";
            TABLEMANAGE tABLEMANAGE = OracleHelper.Select<TABLEMANAGE>(sql, t);

            return tABLEMANAGE;
        }

        public bool UpdateByTableName(string tablename, string id)
        {
            TABLEMANAGE t = new TABLEMANAGE();
            string sql = @"UPDATE TABLEMANAGE SET TABLECURENTID = '" + id + "'  WHERE TABLENAME = '" + tablename + "'";
            return OracleHelper.Update<TABLEMANAGE>(sql, t);

        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<T> Select<T>(T t) where T : class
        {

            Type type = t.GetType();
            string sql = SelectSql(type.Name);
            List<T> list = OracleHelper.Query<T>(sql, t).ToList();

            return list;
        }


        Func<string, string> SelectSql = (x) => @"select * from " + x;


        public bool Delete<T>(T t)where T : class
        {
            string sql = GetInsertSql(t);
            return OracleHelper.Delete(sql, t);
        }

        Func<string, string> DeleteSql = (x) => @"DELETE FROM TABLEMANAGE  WHERE TABLENAME = '"+ x +"'";

        /// <summary>
        /// 插入方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Insert<T>(T t) where T : class
        {
            string sql = GetInsertSql(t);
            return OracleHelper.Insert(sql, t);
        }

        Func<StringBuilder, StringBuilder, JObject string,string> MakeInsertSql = (x, y, obj ,t ) => 
        {
            int num = obj.Count;
            string fieldnames = "";
            string fieldvalues = "";
            foreach (var item in obj)
            {
                if (num == 1)
                {
                    fieldnames = x.Append(item.Key).ToString();
                    fieldvalues = y.Append("'").Append(item.Value).Append("'").ToString();
                }
                else if (num == 0)
                {
                    //不用操作，我们已经得到所要的结果了
                }
                else
                {
                    fieldnames = x.Append(item.Key).Append(",").ToString();
                    fieldvalues = y.Append("'").Append(item.Value).Append("'").Append(",").ToString();
                }

                num--;
            }
            return InsertSql(t,fieldnames, fieldvalues);
        };

        public String GetInsertSql<T>(T t)
        {
            //通过反射获取表的所有字段
            // FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance); //获取私有字段
            // StringBuilder sb = new StringBuilder();

            string jsonstr = DataContractJsonSerialize<T>(t);
            JObject obj = JObject.Parse(jsonstr);
            StringBuilder sk = new StringBuilder();
            StringBuilder sv = new StringBuilder();
            Type type = t.GetType();
            return MakeInsertSql(sk,sv,obj,type.Name);
        }

        static Func<string, string, string,string> InsertSql = (t, x, y) => @"INSERT INTO "+ t +"(" + x + ") VALUES(" + y + ")";

        // <summary>

        /// 对象转换成json

        /// </summary>

        /// <typeparam name="T"></typeparam>

        /// <param name="jsonObject">需要格式化的对象</param>

        /// <returns>Json字符串</returns>

        public static string DataContractJsonSerialize<T>(T jsonObject)

        {

            var serializer = new DataContractJsonSerializer(typeof(T));

            string json = null;

            using (var ms = new MemoryStream()) //定义一个stream用来存发序列化之后的内容

            {

                serializer.WriteObject(ms, jsonObject);

                var dataBytes = new byte[ms.Length];

                ms.Position = 0;

                ms.Read(dataBytes, 0, (int)ms.Length);

                json = Encoding.UTF8.GetString(dataBytes);

                ms.Close();

            }

            return json;

        }

        /// <summary>

        /// json字符串转换成对象

        /// </summary>

        /// <typeparam name="T"></typeparam>

        /// <param name="json">要转换成对象的json字符串</param>

        /// <returns></returns>

        public static T DataContractJsonDeserialize<T>(string json)

        {

            var serializer = new DataContractJsonSerializer(typeof(T));

            var obj = default(T);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))

            {

                obj = (T)serializer.ReadObject(ms);

                ms.Close();

            }

            return obj;

        }
    }
}
