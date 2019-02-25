using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ModifyMessageTool.Model
{
    /// <summary>
    /// 实体类
    /// </summary>
    [Serializable]
    [DataContract]
    public class TABLEMANAGE
    {
        [DataMember]
        public string TABLENAME { get; set; }

        [DataMember]
        public decimal TABLECURENTID { get; set; }

        [DataMember]
        public string TABLEMANAGEDDSC { get; set; }
   
        public TABLEMANAGE() { }

        public TABLEMANAGE(string TABLENAME, decimal TABLECURENTID, string TABLEMANAGEDDSC)
        {
            this.TABLENAME = TABLENAME;
            this.TABLECURENTID = TABLECURENTID;
            this.TABLEMANAGEDDSC = TABLEMANAGEDDSC;

        }
    }
}
