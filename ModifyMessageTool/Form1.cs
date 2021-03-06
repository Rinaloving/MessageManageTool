﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CCWin;
using Microsoft.Office.Interop.Excel;
using System.Threading;
using Oracle.ManagedDataAccess.Client;
using DataTable = System.Data.DataTable;
using System.Configuration;
using ModifyMessageTool.DBUtility;
using ModifyMessageTool.Model;
using System.Reflection;
using ModifyMessageTool.BLL;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Data.SQLite;
using System.Data.Common;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace ModifyMessageTool
{
    public partial class Form1 : CCSkinMain
    {
        public delegate void dUploadProgress(long total, long current);
        public event dUploadProgress onUpLoadProgress;
        ProgressBar pbar = new ProgressBar();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 多个选取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

                if (this.folderBrowserDialog1.SelectedPath.Trim() != "")
                {
                    skinTextBox1.Text = this.folderBrowserDialog1.SelectedPath.Trim(); //D:\dzxml\UnUpload
                }


            }
        }
        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(skinTextBox1.Text.Trim()))
            {
                MessageBox.Show("请先选取文件");
                return;
            }
            else
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(StartModify)).Start(); //启动线程，导入数据
                pbar.ShowDialog(); //显示进度条 modify by cfl 2018年2月1日11:18:28
                pbar.Close();
            }


            //获取所选报文路径
            // string xmlPath = textBox1.Text.Trim();

            //if(GetYWHFromXMLFile(xmlPath))
            //{
            //    MessageBox.Show("修改完成！");
            //}
            //else
            //{
            //    MessageBox.Show("修改失败！");
            //}
            #region 废弃
            if (string.IsNullOrEmpty(skinTextBox1.Text.Trim()))
            {
                MessageBox.Show("请选择文件夹！");
            }
            else
            {
                string filePath = new StringBuilder(skinTextBox1.Text.Trim()).Append("\\").ToString();
                List<string> filelist = Directory.GetFiles(filePath, "*.xml").ToList();
                string newfilePath = filePath + "NewFile\\";

                try
                {
                    filelist.ForEach(item => {
                        ModifyXmlFileName(item); //修改文件名移动到NewFile文件夹下      
                    });

                    Directory.GetFiles(newfilePath, "*.xml").ToList().ForEach(item => {
                        //修改报文内容(修改报文中BizMsgID字段值、CreateDate字段值、保存文件)
                        ModifyXMLFile(item);

                    });

                    MessageBox.Show("修改完成！");
                }
                catch (Exception ex)
                {

                    MessageBox.Show("修改失败！原因：" + ex.ToString());
                }
            }
            #endregion

        }
        /// <summary>
        /// 获取响应报文名称
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetRepXmlFileName(string xmlPath)
        {
            try
            {
                XElement root = XElement.Load(xmlPath);
                //获取报文名
                string filename = root.Element("BizMsgID").Value;

                return filename;
            }
            catch (Exception ex)
            {
                return "无响应报文名称";
                throw ex;
            }

        }
        /// <summary>
        /// 获取响应报文结果
        /// </summary>
        /// <returns></returns>
        public static string GetRepXmlFileResponseInfo(string xmlPath)
        {
            try
            {
                XElement root = XElement.Load(xmlPath);
                //获取响应报文生成时间
                string responseinfo = root.Element("ResponseInfo").Value;

                return responseinfo;
            }
            catch (Exception ex)
            {
                return "无响应结果";
                throw ex;
            }
        }
        /// <summary>
        /// 获取响应报文生成时间
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetRepXmlFileCreateDate(string xmlPath)
        {
            try
            {
                XElement root = XElement.Load(xmlPath);
                //获取响应报文生成时间
                string createdate = root.Element("AdditionalData2").Value;

                return createdate;
            }
            catch (Exception ex)
            {
                return "无信息";
                throw ex;
            }

        }
        /// <summary>
        /// 获取报文数字签名
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetBizXmlFileDigitalSign(string xmlPath)
        {
            try
            {
                XElement root = XElement.Load(xmlPath);
                //获取报文数字签名
                string digitalSign = root.Element("Head").Element("DigitalSign").Value;

                return digitalSign;
            }
            catch (Exception ex)
            {
                return "无数字签名";
                throw ex;
            }
        }
        /// <summary>
        /// 获取报文名称
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetBizXmlFileName(string xmlPath)
        {
            try
            {
                XElement root = XElement.Load(xmlPath);
                //获取报文名
                string filename = root.Element("Head").Element("BizMsgID").Value;

                return filename;
            }
            catch (Exception ex)
            {
                return "无报文名称";
                throw ex;
            }
        }
        /// <summary>
        /// 获取报文创建时间
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetBizXmlCreateDate(string xmlPath)
        {
            try
            {
                XElement root = XElement.Load(xmlPath);
                //获取报文创建时间
                string createdate = root.Element("Head").Element("CreateDate").Value;

                return createdate;
            }
            catch (Exception ex)
            {
                return "无报文创建时间";
                throw ex;
            }
        }
        /// <summary>
        /// 获取报文案件编号 RecFlowID
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetBizXmlCasenum(string xmlPath)
        {
            try
            {
                XElement root = XElement.Load(xmlPath);
                //获取报文创建时间
                string createdate = root.Element("Head").Element("RecFlowID").Value;

                return createdate;
            }
            catch (Exception ex)
            {
                return "无案件编号";
                throw ex;
            }
        }
        /// <summary>
        /// 获取报文登记时间
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetBizXmlDJSJ(string xmlPath)
        {
            string element = BizMessageHelper.GetBizDjsjElementByRecType(GetBizXmlRecType(xmlPath));
            try
            {
                XElement root = XElement.Load(xmlPath);
                string djsj = root.Element("Data").Element(element).Attribute("DJSJ").Value;
                return djsj;

            }
            catch (Exception ex)
            {
                return "无登记时间";
                throw ex;
            }
        }
        /// <summary>
        ///获取报文业务类型名称  GetBizRecTypeByTypeCode
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetBizXmlYWLXMC(string xmlPath)
        {
            string casetypename = string.Empty;
            try
            {
                casetypename = BizMessageHelper.GetBizRecTypeByTypeCode(GetBizXmlRecType(xmlPath));
                return casetypename;

            }
            catch (Exception ex)
            {
                return "登记类型未知";
                throw ex;
            }
        }
        /// <summary>
        /// 获取报文登记类型码
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetBizXmlRecType(string xmlPath)
        {
            try
            {
                XElement root = XElement.Load(xmlPath);
                //获取报文创建时间
                string createdate = root.Element("Head").Element("RecType").Value;

                return createdate;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        /// <summary>
        /// 获取报文受理时间
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetBizXmlSLSJ(string xmlPath)
        {
            try
            {
                XElement root = XElement.Load(xmlPath);
                string slsj = root.Element("Data").Element("DJT_DJ_SLSQ").Attribute("SLSJ").Value;
                return slsj;

            }
            catch (Exception ex)
            {
                return "无受理时间";
                throw ex;
            }
        }
        /// <summary>
        /// 获取报文受理人员 SLRY
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static string GetBizXmlSLRY(string xmlPath)
        {
            try
            {
                XElement root = XElement.Load(xmlPath);
                string slry = root.Element("Data").Element("DJT_DJ_SLSQ").Attribute("SLRY").Value;
                return slry;
            }
            catch (Exception ex)
            {
                return "无受理人员";
                throw ex;
            }
        }

        /// <summary>
        /// 生成新报文名称（弃用）
        /// </summary>
        /// <param name="citycode"></param>
        /// <returns></returns>
        public static string CreateXmlFileName(string citycode)
        {

            string filename = new StringBuilder(citycode).Append(System.DateTime.Now.ToString("yyMMddmmsfff").ToString()).ToString();
            Thread.Sleep(100); //以防产生重复名称
            return filename;

        }
        /// <summary>
        /// 修改文件名
        /// </summary>
        /// <param name="xmlPath"></param>
        public static void ModifyXmlFileName(string xmlPath)
        {

            string realPath = Path.GetDirectoryName(xmlPath);
            string filename = Path.GetFileName(xmlPath); //"Biz371428181224008555.xml"
            string check = System.DateTime.Now.ToString("yyMMdd");
            string newfilename = "";
            if ((!filename.Contains(check)) && filename.Contains("371602")) //如果是是滨州过去报文
            {
                //滨州文件名修改（滨州的规则不一样，从pfuser库中的tablemanage表中的bw3716_2019字段获取报文后六位顺序号）
                newfilename = filename.Substring(0, 9) + System.DateTime.Now.ToString("yyMMdd") + GetAndUpadateMaxXmlFileNameNum() + ".xml";
            }
            else  // 否则一律使用下面规则重新命名
            {
                newfilename = filename.Substring(0, 9) + System.DateTime.Now.ToString("yyMMdd") + filename.Substring(15, 6) + ".xml";
            }

            if (!Directory.Exists(realPath + "\\NewFile\\"))
                Directory.CreateDirectory(realPath + "\\NewFile\\");
            File.Copy(xmlPath, realPath + "\\NewFile\\" + newfilename, true);


        }
        /// <summary>
        /// 修改旧报文文件名
        /// </summary>
        /// <param name="xmlPath"></param>
        public static void ModifyOldXmlFileName(string xmlPath)
        {
            string realPath = Path.GetDirectoryName(xmlPath);
            string filename = Path.GetFileName(xmlPath); //"Biz371428181224008555.xml"
            string newfilename = filename.Substring(0, 9) + System.DateTime.Now.ToString("yyMMdd") + filename.Substring(15, 6) + ".xml";
            if (!Directory.Exists(realPath + "\\NewFile\\"))
                Directory.CreateDirectory(realPath + "\\NewFile\\");
            File.Copy(xmlPath, realPath + "\\NewFile\\" + newfilename, true);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="xmlPath"></param>

        public static void testOracleDate()
        {
            TABLEMANAGE_BLL tBll = new TABLEMANAGE_BLL();
            //List<TABLEMANAGE> list = tBll.Query();
            //list.ForEach(p=>{
            //    Console.WriteLine(p.TABLENAME + " " + p.TABLEMANAGEDDSC + " " + p.TABLECURENTID);
            //});

            //TABLEMANAGE ta = tBll.QueryByTableName("bw3716_2019");
            //bool b = tBll.UpdateByTableName("bw3716_2019", "2858");
            //获取所有字段 
            //FieldInfo[] fields = typeof(TABLEMANAGE).GetFields(BindingFlags.NonPublic | BindingFlags.Instance); //获取私有字段

            TABLEMANAGE tab = new TABLEMANAGE();
            //方法插入数据库
            //1.创建有参构造方法
            //2.调用Insert方法
            

            //foreach (var item in fields)
            //{
            //    Console.WriteLine(item.FieldType.Name+" "+item.Name);
            //}


            TABLEMANAGE t = new TABLEMANAGE("Rina",111,"测试");

            bool b = tBll.Insert(t);

            Console.WriteLine(b);
            //Console.WriteLine(ta.TABLECURENTID);

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
    /// 获取数据库中报文序列最大数，使用完并更新最大数+1
    /// </summary>
    /// <returns></returns>
    public static string GetAndUpadateMaxXmlFileNameNum()
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string strCon = config.AppSettings.Settings["pfuserconnectionstring"].Value;

            //string strCon = @"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=pfuser;Password=123;";
            //定义连接
            OracleConnection MyCon = new OracleConnection(strCon);
            try
            {

                // 打开连接  
                MyCon.Open();
                string strSQL = @"SELECT * FROM TABLEMANAGE WHERE TABLENAME = 'bw3716_2019'"; // bw3716_2019 BW3716_2019
                OracleDataAdapter MyDataAdapter = new OracleDataAdapter();
                MyDataAdapter.SelectCommand = new OracleCommand(strSQL, MyCon);
                // 将数据填充到DataSet中
                DataSet MyDataSet = new DataSet();

                MyDataAdapter.Fill(MyDataSet, "TABLEMANAGE");
                // 从DataSet中获取DataTable 

                DataTable MyDataTable = MyDataSet.Tables["TABLEMANAGE"];

                //第一步：获取数据库pfuser中的tablemanage表中的bw3716_2019字段获取报文后六位顺序号
                string num = MyDataTable.Rows[0]["TABLECURENTID"].ToString();

                string newnum = Convert.ToString(1000000 + Convert.ToInt32(num)).Substring(1, 6);


                //第五步：更新数据库pfuser中的tablemanage表中的bw3716_2019字段的值（+1）

                MyDataTable.Rows[0]["TABLECURENTID"] = Convert.ToInt32(num) + 1;


                // 将DataSet中的数据更新到数据库中 

                OracleCommandBuilder MyCommandBuilder = new OracleCommandBuilder(MyDataAdapter);

                MyDataAdapter.Update(MyDataSet, "TABLEMANAGE");


                return newnum;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.ToString());
                throw ex;
            }
            finally
            {
                MyCon.Close();
            }
        }

        /// <summary>
        /// 修改报文内容
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static bool ModifyXMLFile(string xmlPath)
        {
            try
            {
               

                XElement root = XElement.Load(xmlPath);
                //修改CreateDate节点的值 
                root.Element("Head").SetElementValue("CreateDate", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));  // 2019-01-08T14:56:59
                root.Save(xmlPath);
                //获取BizMsgID节点的值
                string filename = Path.GetFileNameWithoutExtension(xmlPath); //"Biz371428181224008555"
                
                //修改BizMsgID节点的值
                root.Element("Head").SetElementValue("BizMsgID", filename.Substring(3,18));

                //删除报文中原有<DigitalSign>节点
                root.Element("Head").Element("DigitalSign").Remove();
                root.Save(xmlPath);
                //在报文中添加<DigitalSign>节点
                HeadHandler.Instance.CreateMsgWithSignature(xmlPath);  //每次修改完，这要重新保存一下
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //int dogs = 0; //计数器
            //progressBar1.Visible = true; //显示进度条
            if (string.IsNullOrEmpty(skinTextBox1.Text.Trim()))
            {
                MessageBox.Show("请先选取文件");
                return;
            }
            else
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(StartUpload)).Start(); //启动线程，导入数据
                if (pbar.IsDisposed)
                {
                    pbar = new ProgressBar();
                }
                pbar.ShowDialog(); //显示进度条 modify by cfl 2018年2月1日11:18:28
                pbar.Close();
            }
           

            #region 废弃
            /*
            string filePath = new StringBuilder(skinTextBox1.Text.Trim()).Append("\\").ToString();
            List<string> filelist = Directory.GetFiles(filePath, "*.xml").ToList();
            string logPath = System.AppDomain.CurrentDomain.BaseDirectory;
            //实例化一个Excel.Application对象    
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook;

            Microsoft.Office.Interop.Excel.Worksheet worksheet;



            excel.Visible = false;                            //是Excel不可见，如果为true 导出数据的时候会弹出来excel显示数据插入的过程



            workbook = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

            worksheet = (Worksheet)workbook.Worksheets[1];
            //excel.Visible = false;
            //新增加一个工作簿，Workbook是直接保存，不会弹出保存对话框，加上Application会弹出保存对话框，值为false会报错    
            //excel.Application.Workbooks.Add(true);
            Microsoft.Office.Interop.Excel.Range range = worksheet.Range[worksheet.Cells[4, 1], worksheet.Cells[8, 1]];//获取Excel多个单元格区域
            //生成Excel中列头名称    

            worksheet.Cells[1, 1] = "报文名称";
            worksheet.Cells[1, 2] = "响应时间";
            worksheet.Cells[1, 3] = "报文时间";
            int i = 1;
            foreach (var item in filelist)
            {
                //响应时间
                string date = GetRepXmlFileCreateDate(item) == "" ? "190000000000" : GetRepXmlFileCreateDate(item);
                string year = GetRepXmlFileCreateDate(item) == "" ? "00" : date.Substring(0, 4);
                string month = GetRepXmlFileCreateDate(item) == "" ? "00" : date.Substring(4, 2);
                string day = GetRepXmlFileCreateDate(item) == "" ? "00" : date.Substring(6, 2);
                //报文时间
                string bwdate = GetRepXmlFileName(item);
                string bwyear = "20"+bwdate.Substring(6, 2);
                string bwmonth = bwdate.Substring(8, 2);
                string bwday = bwdate.Substring(10, 2);
                worksheet.Cells[i + 1, 1] = "Rep"+GetRepXmlFileName(item)+".xml";
                worksheet.Cells[i + 1, 2] = year + "-" + month + "-" + day;
                worksheet.Cells[i + 1, 3] = bwyear + "-" + bwmonth + "-" + bwday;
                i++;
            }

            range.EntireColumn.AutoFit();//自动调整列宽

            //设置禁止弹出保存和覆盖的询问提示框    
            excel.DisplayAlerts = true;
            excel.AlertBeforeOverwriting = true;
            //保存工作簿    
            //excel.Application.Workbooks.Add(true).Save();
            //保存excel文件    
            workbook.SaveAs(logPath+"ImportToExcel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
            //excel.Save("E:\\" + "Data.xls");
            //确保Excel进程关闭    
            excel.Quit();
            excel = null;
            GC.Collect();//如果不使用这条语句会导致excel进程无法正常退出，使用后正常退出  
            MessageBox.Show(this, "文件已经成功导出！", "信息提示");
        
            //filelist.ForEach(item=> {

            //    printLog(logPath+ "\\Log.txt", GetYWHFromXMLFile(item));
            //});

         */
            #endregion
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(skinTextBox1.Text.Trim()))
            {
                MessageBox.Show("请先选取文件");
                return;
            }
            else
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(StartOutPut)).Start(); //启动线程，导入数据
                if (pbar.IsDisposed)
                {
                    pbar = new ProgressBar();
                }

                pbar.ShowDialog(); //显示进度条 modify by cfl 2018年2月1日11:18:28
                pbar.Close();
            }
           
        }

        /// <summary>
        /// 开始导入 modify by cfl 2018年2月1日10:13:12
        /// </summary>
        public void StartUpload()
        {

            this.onUpLoadProgress += uploader_onUpLoadProgress;
            this.Start();
        }
        /// <summary>
        /// 开始导出原始报文信息
        /// </summary>
        public void StartOutPut()
        {
            this.onUpLoadProgress += uploader_onUpLoadProgress;
            this.StartOutPutDigitalSign();
        }
        /// <summary>
        /// 开始执行修改
        /// </summary>
        public void StartModify()
        {
            this.onUpLoadProgress += uploader_onUpLoadProgress;
            this.StartModifyXmlFile();
        }


        /// <summary>
        /// 同步更新UI modify by cfl 2018年2月1日10:13:12
        /// </summary>
        /// <param name="total"></param>
        /// <param name="current"></param>
        public void uploader_onUpLoadProgress(long total, long current)
        {


            if (this.InvokeRequired)
            {
                this.Invoke(new Form1.dUploadProgress(uploader_onUpLoadProgress), new Object[] { total, current });
            }
            else
            {
                pbar.pro_Bar1.Maximum = Convert.ToInt32(total); //modify by cfl 2018年2月1日11:17:36
                pbar.pro_Bar1.Value = Convert.ToInt32(current);
                //pbar.label4.Text = "处理个数：" + current;
                System.Windows.Forms.Application.DoEvents();
                //this.progressBar1.Maximum = (int)total;
                //this.progressBar1.Value = (int)current;
            }
        }

        /// <summary>
        /// 导入数据同时触发进度条事件 modify by cfl 2018年2月1日10:13:12
        /// </summary>
        Func<int, int, int> add = (a, b) => a + b;
        public void Start()
        {

            int dogs = 1;
            
            string filePath = new StringBuilder(skinTextBox1.Text.Trim()).Append("\\").ToString();
            List<string> filelist = Directory.GetFiles(filePath, "*.xml").ToList();
            string logPath = System.AppDomain.CurrentDomain.BaseDirectory;
            int zs = filelist.Count; //报文总数

            #region 废弃
            //实例化一个Excel.Application对象    
            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            //Microsoft.Office.Interop.Excel.Workbook workbook;

            //Microsoft.Office.Interop.Excel.Worksheet worksheet;



            //excel.Visible = false;                            //是Excel不可见，如果为true 导出数据的时候会弹出来excel显示数据插入的过程



            //workbook = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

            //worksheet = (Worksheet)workbook.Worksheets[1];
            //excel.Visible = false;
            //新增加一个工作簿，Workbook是直接保存，不会弹出保存对话框，加上Application会弹出保存对话框，值为false会报错    
            //excel.Application.Workbooks.Add(true);
            // Microsoft.Office.Interop.Excel.Range range = worksheet.Range[worksheet.Cells[4, 1], worksheet.Cells[8, 1]];//获取Excel多个单元格区域
            //生成Excel中列头名称    

            //worksheet.Cells[1, 1] = "报文名称";
            //worksheet.Cells[1, 2] = "响应时间";
            // worksheet.Cells[1, 3] = "报文时间";
            //string path = System.Windows.Forms.Application.StartupPath;

            //string connStr = path + @"\\testDB.db";
            //SQLiteConnection dbConnection = new SQLiteConnection("Data Source= " + connStr);
            #endregion

            int i = 1;

           

            foreach (var item in filelist)
            {
                //响应时间
                string date = GetRepXmlFileCreateDate(item) == "" ? "190000000000" : GetRepXmlFileCreateDate(item);
                string year = GetRepXmlFileCreateDate(item) == "" ? "00" : date.Substring(0, 4);
                string month = GetRepXmlFileCreateDate(item) == "" ? "00" : date.Substring(4, 2);
                string day = GetRepXmlFileCreateDate(item) == "" ? "00" : date.Substring(6, 2);
                //报文时间
                string bwdate = GetRepXmlFileName(item);
                string bwyear = "20" + bwdate.Substring(6, 2);
                string bwmonth = bwdate.Substring(8, 2);
                string bwday = bwdate.Substring(10, 2);

                string filename = "Rep" + GetRepXmlFileName(item) + ".xml";
                string uploaddate = year + "-" + month + "-" + day;
                string responsedate = bwyear + "-" + bwmonth + "-" + bwday;
                string responseinfo = GetRepXmlFileResponseInfo(item);

                #region 废弃
                //worksheet.Cells[i + 1, 1] = "Rep" + GetRepXmlFileName(item) + ".xml";
                //worksheet.Cells[i + 1, 2] = year + "-" + month + "-" + day;
                //worksheet.Cells[i + 1, 3] = bwyear + "-" + bwmonth + "-" + bwday;


                // dbConnection.Open();
                //SQLiteCommand cmd = new SQLiteCommand(dbConnection);
                // DbTransaction trans = dbConnection.BeginTransaction();
                //string sql = @"insert into repmessagefile(filename,uploaddate,responsedate,responseinfo) values('" + filename + "','" + uploaddate + "','" + responsedate + "','"+ responseinfo + "');";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                // trans.Commit();
                //dbConnection.Close();
                #endregion

                string sql = @"insert into repmessagefile(filename,uploaddate,responsedate,responseinfo) values(@filename,@uploaddate,@responsedate,@responseinfo);";
                SQLiteParameter[] parameters = new SQLiteParameter[] { new SQLiteParameter("@filename", filename), new SQLiteParameter("@uploaddate", uploaddate), new SQLiteParameter("@responsedate", responsedate), new SQLiteParameter("@responseinfo", responseinfo) };

                SQLiteHelper.ExecuteNonQuery(sql, parameters);


                i++;
                if (onUpLoadProgress != null)
                {
                    onUpLoadProgress(zs, dogs);
                    System.Threading.Thread.Sleep(10);
                }
                dogs++;
            }

            #region 废弃
            //range.EntireColumn.AutoFit();//自动调整列宽

            //设置禁止弹出保存和覆盖的询问提示框    
            //excel.DisplayAlerts = true;
            //excel.AlertBeforeOverwriting = true;
            //保存工作簿    
            //excel.Application.Workbooks.Add(true).Save();
            //保存excel文件    
            // workbook.SaveAs(logPath + "ImportToExcel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
            //excel.Save("E:\\" + "Data.xls");
            //确保Excel进程关闭    
            //excel.Quit();
            // excel = null;
            // GC.Collect();//如果不使用这条语句会导致excel进程无法正常退出，使用后正常退出  
            //MessageBox.Show(this, "文件已经成功导出！", "信息提示");
            //this.pbar.Close();


            // System.Environment.Exit(0); // 这是最彻底的退出方式，不管什么线程都被强制退出，把程序结束的很干净。
            #endregion

        }
        public void StartOutPutDigitalSign()
        {
           
               
            int dogs = 1;

            string filePath = new StringBuilder(skinTextBox1.Text.Trim()).Append("\\").ToString();
            List<string> filelist = Directory.GetFiles(filePath, "*.xml").ToList();
            string logPath = System.AppDomain.CurrentDomain.BaseDirectory;

            #region 废弃
            //实例化一个Excel.Application对象    
            ///Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            //Microsoft.Office.Interop.Excel.Workbook workbook;

            //Microsoft.Office.Interop.Excel.Worksheet worksheet;
            #endregion

            int zs = filelist.Count; //报文总数

            #region 废弃

            //excel.Visible = false;                            //是Excel不可见，如果为true 导出数据的时候会弹出来excel显示数据插入的过程



            // workbook = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

            // worksheet = (Worksheet)workbook.Worksheets[1];
            //excel.Visible = false;
            //新增加一个工作簿，Workbook是直接保存，不会弹出保存对话框，加上Application会弹出保存对话框，值为false会报错    
            //excel.Application.Workbooks.Add(true);
            //Microsoft.Office.Interop.Excel.Range range = worksheet.Range[worksheet.Cells[4, 1], worksheet.Cells[8, 1]];//获取Excel多个单元格区域
            //生成Excel中列头名称    

            // worksheet.Cells[1, 1] = "报文名称";
            // worksheet.Cells[1, 2] = "创建时间";
            // worksheet.Cells[1, 3] = "受理时间";
            // worksheet.Cells[1, 4] = "数字签名";
            //string path = System.Windows.Forms.Application.StartupPath;

            //string connStr = path + @"\\testDB.db";
            //SQLiteConnection dbConnection = new SQLiteConnection("Data Source= " + connStr);
            #endregion 

            int i = 1;

            foreach (var item in filelist)
            {
                //创建时间
                string createDate = GetBizXmlCreateDate(item) == "" ? "190000000000" : GetBizXmlCreateDate(item);
                //数字签名
                string digitalSign = GetBizXmlFileDigitalSign(item);
                //受理时间
                string slsj = GetBizXmlSLSJ(item);
                //受理人员
                string slry = GetBizXmlSLRY(item);

                string djsj = GetBizXmlDJSJ(item);

                //文件名称
                string filename = "Biz" + GetBizXmlFileName(item) + ".xml";
                //案件编号
                string casenum = GetBizXmlCasenum(item);
                //类型名称
                string casetypename = GetBizXmlYWLXMC(item);
                #region 废弃
                //dbConnection.Open();
                //SQLiteCommand cmd = new SQLiteCommand(dbConnection);
                //DbTransaction trans = dbConnection.BeginTransaction();
                //string sql = @"insert into messagefile(filename,casenum,casetypename,createdate,djsj,slsj,slry) values('" + filename+"','"+ casenum + "','"+ casetypename + "','"+ createDate + "','"+ djsj + "','"+ slsj + "','"+ slry + "');";
                //cmd.CommandText = sql;
                //cmd.ExecuteNonQuery();
                //trans.Commit();
                //dbConnection.Close();
                #endregion

                string sql = @"insert into messagefile(filename,casenum,casetypename,createdate,djsj,slsj,slry) values(@filename,@casenum,@casetypename,@createdate,@djsj,@slsj,@slry);";
                SQLiteParameter[] parameters = new SQLiteParameter[] { new SQLiteParameter("@filename", filename), new SQLiteParameter("@casenum", casenum), new SQLiteParameter("@casetypename", casetypename), new SQLiteParameter("@createdate", createDate), new SQLiteParameter("@djsj", djsj), new SQLiteParameter("@slsj", slsj), new SQLiteParameter("@slry", slry) };

                SQLiteHelper.ExecuteNonQuery(sql, parameters);

                #region 废弃
                //worksheet.Cells[i + 1, 1] = "Biz" + GetBizXmlFileName(item) + ".xml";
                //worksheet.Cells[i + 1, 2] = createDate;
                //worksheet.Cells[i + 1, 3] = slsj;
                //worksheet.Cells[i + 1, 4] = digitalSign;
                #endregion

                i++;
                if (onUpLoadProgress != null)
                {
                    onUpLoadProgress(zs, dogs);
                    System.Threading.Thread.Sleep(10);
                }
                dogs++;
            }

            #region 废弃
            /*
            range.EntireColumn.AutoFit();//自动调整列宽

            //设置禁止弹出保存和覆盖的询问提示框    
            excel.DisplayAlerts = true;
            excel.AlertBeforeOverwriting = true;
            //保存工作簿    
            //excel.Application.Workbooks.Add(true).Save();
            //保存excel文件    
            workbook.SaveAs(logPath + "OutPutDigitalSignToExcel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
            //excel.Save("E:\\" + "Data.xls");
            //确保Excel进程关闭    
            excel.Quit();
            excel = null;
            worksheet = null;
            workbook = null;
            GC.Collect();//如果不使用这条语句会导致excel进程无法正常退出，使用后正常退出  
            //MessageBox.Show(this, "文件已经成功导出！", "信息提示");
            //this.pbar.Close();
            */
            #endregion

        }

        public void StartModifyXmlFile()
        {
            int dogs = 1;

            if (string.IsNullOrEmpty(skinTextBox1.Text.Trim()))
            {
                MessageBox.Show("请选择文件夹！");
            }
            else
            {
                string filePath = new StringBuilder(skinTextBox1.Text.Trim()).Append("\\").ToString();
                List<string> filelist = Directory.GetFiles(filePath, "*.xml").ToList();
                int zs = filelist.Count; //报文总数
                string newfilePath = filePath + "NewFile\\";

                try
                {
                    filelist.ForEach(item => {
                        ModifyXmlFileName(item); //修改文件名移动到NewFile文件夹下      
                    });

                    Directory.GetFiles(newfilePath, "*.xml").ToList().ForEach(item => {
                        //修改报文内容(修改报文中BizMsgID字段值、CreateDate字段值、保存文件)
                        if (onUpLoadProgress != null)
                        {
                            onUpLoadProgress(zs, dogs);
                            System.Threading.Thread.Sleep(10);
                        }
                        dogs++;
                        ModifyXMLFile(item);

                    });

                    MessageBox.Show("修改完成！");
                }
                catch (Exception ex)
                {

                    MessageBox.Show("修改失败！原因：" + ex.ToString());
                }
            }
            
            
        }

        /// <summary>
        /// 日志打印   
        /// </summary>
        /// <param name="logpath"></param>
        /// <param name="errorinfo"></param>
        public static void printLog(string logpath,string fileinfo)
        {

            if (File.Exists(logpath)) // "C:/11111/2222/33333/11111.txt"
            {
                using (FileStream fs = new FileStream(logpath, FileMode.Append, FileAccess.Write))
                {
                    fs.Lock(0, fs.Length);
                    StreamWriter sw = new StreamWriter(fs);
                    try
                    {
                        sw.WriteLine(fileinfo);
                        sw.WriteLine("----------------------------------------------------------------------");
                        fs.Unlock(0, fs.Length);
                        sw.Flush();
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    finally
                    {
                        sw.Close();
                    }

                }
            }
            else
            {
                File.CreateText(logpath).Close(); //必须关闭，否则会报该进程已被占用的错误
                using (FileStream fs = new FileStream(logpath, FileMode.Append, FileAccess.Write))
                {
                    fs.Lock(0, fs.Length);
                    StreamWriter sw = new StreamWriter(fs);
                    try
                    {
                        sw.WriteLine(fileinfo);
                        sw.WriteLine("----------------------------------------------------------------------");
                        fs.Unlock(0, fs.Length);
                        sw.Flush();
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
            }
        }


        public void ExportExcel(DataTable dt)
        {
            try
            {
                //创建一个工作簿
                IWorkbook workbook = new HSSFWorkbook();

                //创建一个 sheet 表
                ISheet sheet = workbook.CreateSheet(dt.TableName);

                //创建一行
                IRow rowH = sheet.CreateRow(0);

                //创建一个单元格
                ICell cell = null;

                //创建单元格样式
                ICellStyle cellStyle = workbook.CreateCellStyle();

                //创建格式
                IDataFormat dataFormat = workbook.CreateDataFormat();

                //设置为文本格式，也可以为 text，即 dataFormat.GetFormat("text");
                cellStyle.DataFormat = dataFormat.GetFormat("@");

                //设置列名
                foreach (DataColumn col in dt.Columns)
                {
                    //创建单元格并设置单元格内容
                    rowH.CreateCell(col.Ordinal).SetCellValue(col.Caption);

                    //设置单元格格式
                    rowH.Cells[col.Ordinal].CellStyle = cellStyle;
                }

                //写入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //跳过第一行，第一行为列名
                    IRow row = sheet.CreateRow(i + 1);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                        cell.CellStyle = cellStyle;
                    }
                }

                //设置导出文件路径
                string path = System.Windows.Forms.Application.StartupPath;

                //设置新建文件路径及名称
                string savePath = path +"\\"+ DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xls";

                //创建文件
                FileStream file = new FileStream(savePath, FileMode.CreateNew, FileAccess.Write);

                //创建一个 IO 流
                MemoryStream ms = new MemoryStream();

                //写入到流
                workbook.Write(ms);

                //转换为字节数组
                byte[] bytes = ms.ToArray();

                file.Write(bytes, 0, bytes.Length);
                file.Flush();

                //还可以调用下面的方法，把流输出到浏览器下载
                //OutputClient(bytes);

                //释放资源
                //bytes = null;

                ms.Close();
                ms.Dispose();

                file.Close();
                file.Dispose();

                workbook.Close();
                sheet = null;
                workbook = null;
                MessageBox.Show("导出成功！");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 导出原始报文到Excel表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinButton3_Click(object sender, EventArgs e)
        {

            string sql = @"select * from messagefile";
            SQLiteParameter[] parameters = new SQLiteParameter[] { };
            DataTable dt = SQLiteHelper.GetDataTable(sql, parameters);

            ExportExcel(dt);

            #region 废弃
            //testOracleDate();
            //测试sqlite数据库读取

            //string path = System.Windows.Forms.Application.StartupPath;

            //string connStr = path + @"\\testDB.db";
            //SQLiteConnection dbConnection = new SQLiteConnection("Data Source= "+connStr);
            //dbConnection.Open();

            //DataSet ds = new DataSet();

            //SQLiteDataAdapter sQLite = new SQLiteDataAdapter("select * from messagefile", dbConnection);
            //sQLite.Fill(ds);

            //DataTable dt = ds.Tables[0];

            //ExportExcel(dt);



            //SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            //DbTransaction trans = dbConnection.BeginTransaction();
            //string sql = @"insert into messagefile(filename,casenum,createdate,djsj,slsj,slry) values('biz2019082301.xml','20190621001','2019-06-21','2019-06-21','2019-06-21','2019-06-21');";
            //cmd.CommandText = sql;
            //cmd.ExecuteNonQuery();
            //trans.Commit();
            //dbConnection.Close();
            #endregion



        }
        /// <summary>
        /// 导出响应报文到Excel表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButton4_Click(object sender, EventArgs e)
        {

            string sql = @"select * from repmessagefile";
            SQLiteParameter[] parameters = new SQLiteParameter[] { };
            DataTable dt = SQLiteHelper.GetDataTable(sql, parameters);

            ExportExcel(dt);

            #region 废弃
            //string path = System.Windows.Forms.Application.StartupPath;

            //string connStr = path + @"\\testDB.db";
            //SQLiteConnection dbConnection = new SQLiteConnection("Data Source= " + connStr);
            //dbConnection.Open();

            //DataSet ds = new DataSet();

            //SQLiteDataAdapter sQLite = new SQLiteDataAdapter("select * from repmessagefile", dbConnection);
            //sQLite.Fill(ds);

            //DataTable dt = ds.Tables[0];

            //ExportExcel(dt);
            #endregion

        }


    }

   
}
