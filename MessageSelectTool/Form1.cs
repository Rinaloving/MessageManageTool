using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessageSelectTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选取(只能选取到文件夹)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog()==DialogResult.OK)
            {
              
                if (this.folderBrowserDialog1.SelectedPath.Trim() != "")
                {
                    textBox1.Text = this.folderBrowserDialog1.SelectedPath.Trim(); //D:\dzxml\UnUpload
                }
                


            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //获取文件夹下所有xml结尾的文件
            //ArrayList<string> list = F
            string filePath = textBox1.Text.Trim()+"\\";
            List<string> filelist  = Directory.GetFiles(filePath,"*.xml").ToList();
            filelist.ForEach(item=>{ 
                string filename = Path.GetFileName(item);
                textBox2.AppendText(filename + "\r\n");
                string newPath = filename.Substring(3, 6); //371402 区号
                CreateDirectory(filePath + newPath);
                string subPath = "20" + filename.Substring(9, 2); //2018 年份
                CreateDirectory(filePath + newPath + "\\" + subPath);
                string rsubPath = filename.Substring(11, 2); // 11 月份
                CreateDirectory(filePath + newPath + "\\" + subPath + "\\" + rsubPath);
                string destFileName = filePath + newPath + "\\" + subPath + "\\" + rsubPath;
                try
                {
                    //File.Move(item, destFileName + "\\" + filename); // 移动报文到对应的文件夹下
                    //直接移动遇到已存在文件容易挂掉
                    //改为先复制，再删除的方式移动文件
                    File.Copy(item, destFileName + "\\" + filename, true);
                    File.Delete(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("报文：" + filename + "移动失败！原因：" + ex.Message);
                    throw;
                }
            });
            
        }

        public void CreateDirectory(string name)
        {
            try
            {
                if (!Directory.Exists(name))
                    Directory.CreateDirectory(name);

            }
            catch (Exception ex)
            {
                MessageBox.Show("创建文件夹："+name+"异常！"+ex.Message);
                throw;
            }
            
            
        }
    }
}
