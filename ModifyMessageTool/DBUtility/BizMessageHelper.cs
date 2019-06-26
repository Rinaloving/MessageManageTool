using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModifyMessageTool.DBUtility
{
    public class BizMessageHelper
    {
        public static string GetBizDjsjElementByRecType(string rectypecode)
        {
            switch (rectypecode)
            {
                case "1000101": return "QLF_QL_TDSYQ";
                case "1000301": return "QLF_QL_JSYDSYQ";
                case "1000401": return "QLF_FW_FDCQ_DZ";
                case "1000402": return "QLT_FW_FDCQ_YZ";
                case "1000403": return "QLF_FW_FDCQ_QFSYQ";
                case "1001501": return "QLF_QL_HYSYQ";
                case "1001601": return "QLT_QL_GJZWSYQ";
                case "1000901": return "QLF_QL_NYDSYQ";
                case "1001201": return "QLT_QL_LQ";
                case "2000101": return "QLF_QL_TDSYQ";
                case "2000301": return "QLF_QL_JSYDSYQ";
                case "2000401": return "QLF_FW_FDCQ_DZ";
                case "2000402": return "QLT_FW_FDCQ_YZ";
                case "2000403": return "QLF_FW_FDCQ_QFSYQ";
                case "2001501": return "QLF_QL_HYSYQ";
                case "2001601": return "QLT_QL_GJZWSYQ";
                case "2000901": return "QLF_QL_NYDSYQ";
                case "2001201": return "QLT_QL_LQ";
                case "3000101": return "QLF_QL_TDSYQ";
                case "3000301": return "QLF_QL_JSYDSYQ";
                case "3000401": return "QLT_FW_FDCQ_DZ";
                case "3000402": return "QLT_FW_FDCQ_YZ";
                case "3000403": return "QLF_FW_FDCQ_QFSYQ";
                case "3001501": return "QLF_QL_HYSYQ";
                case "3001601": return "QLT_QL_GJZWSYQ";
                case "3000901": return "QLF_QL_NYDSYQ";
                case "3001201": return "QLT_QL_LQ";
                case "4000101": return "QLF_QL_ZXDJ";
                case "5000101": return "QLF_QL_TDSYQ";
                case "5000301": return "QLF_QL_JSYDSYQ";
                case "5000401": return "QLT_FW_FDCQ_DZ";
                case "5000402": return "QLT_FW_FDCQ_YZ";
                case "5000403": return "QLF_FW_FDCQ_QFSYQ";
                case "5001501": return "QLF_QL_HYSYQ";
                case "5001601": return "QLT_QL_GJZWSYQ";
                case "5000901": return "QLF_QL_NYDSYQ";
                case "5001201": return "QLT_QL_LQ";
                case "6000101": return "QLF_QL_YYDJ";
                case "7000101": return "QLF_QL_DYAQ";
                case "8000101": return "QLF_QL_CFDJ";
                case "9000101": return "QLF_QL_DYAQ";
                case "9000102": return "QLF_QL_DYIQ";

                default:
                    return null;
            }
        }


        public static string GetBizRecTypeByTypeCode(string rectypecode)
        {
            switch (rectypecode)
            {
                case "1000101": return "首次登记";
                case "1000301": return "首次登记";
                case "1000401": return "首次登记";
                case "1000402": return "首次登记";
                case "1000403": return "首次登记";
                case "1001501": return "首次登记";
                case "1001601": return "首次登记";
                case "1000901": return "首次登记";
                case "1001201": return "首次登记";
                case "2000101": return "转移登记";
                case "2000301": return "转移登记";
                case "2000401": return "转移登记";
                case "2000402": return "转移登记";
                case "2000403": return "转移登记";
                case "2001501": return "转移登记";
                case "2001601": return "转移登记";
                case "2000901": return "转移登记";
                case "2001201": return "转移登记";
                case "3000101": return "变更登记";
                case "3000301": return "变更登记";
                case "3000401": return "变更登记";
                case "3000402": return "变更登记";
                case "3000403": return "变更登记";
                case "3001501": return "变更登记";
                case "3001601": return "变更登记";
                case "3000901": return "变更登记";
                case "3001201": return "变更登记";
                case "4000101": return "注销登记";
                case "5000101": return "更正登记";
                case "5000301": return "更正登记";
                case "5000401": return "更正登记";
                case "5000402": return "更正登记";
                case "5000403": return "更正登记";
                case "5001501": return "更正登记";
                case "5001601": return "更正登记";
                case "5000901": return "更正登记";
                case "5001201": return "更正登记";
                case "6000101": return "异议登记";
                case "7000101": return "预告登记";
                case "8000101": return "查封登记";
                case "9000101": return "抵押权登记";
                case "9000102": return "地役权登记";

                default:
                    return null;
            }
        }


    }
}
