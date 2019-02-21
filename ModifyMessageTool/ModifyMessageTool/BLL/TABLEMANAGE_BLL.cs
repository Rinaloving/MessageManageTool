using ModifyMessageTool.DAL;
using ModifyMessageTool.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModifyMessageTool.BLL
{
    /// <summary>
    /// BLL层从DAL层获取列表数据后，马上将表数据返回给UI层
    /// 会觉得BLL层的功能微乎其微。实际上在BLL层可以进行逻辑判断，
    /// 这也正是BLL层的好处所在。比如BLL层拿到DAL层返回过来的数据后，
    /// 可以进行相应的if判断，确保数据满足业务需求，再返回给UI层。
    /// </summary>
    public class TABLEMANAGE_BLL
    {

        private TABLEMANAGE_DAL tDal = new TABLEMANAGE_DAL();

        public List<TABLEMANAGE> Query()
        {
            return tDal.Query();
        }

        public TABLEMANAGE QueryByTableName(string tablename)
        {
            return tDal.QueryByTableName(tablename);
        }

        public bool UpdateByTableName(string tablename, string id)
        {
            return tDal.UpdateByTableName(tablename,id);
        }

        public bool Insert<T>(T t) where T : class
        {
           
            return tDal.Insert<T>(t);
        }

        public List<T> Select<T>( T t) where T:class
        {
            return tDal.Select<T>(t);
        }

    }
}
