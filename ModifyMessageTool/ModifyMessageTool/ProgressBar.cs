using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;

namespace ModifyMessageTool
{
    public partial class ProgressBar : CCSkinMain
    {
        public ProgressBar()
        {
            InitializeComponent();
        }

        private void ProgressBar_Load(object sender, EventArgs e)
        {
            //this.ControlBox = false; //隐藏窗口的最大化、最小化、关闭窗口
            Application.DoEvents();
        }
    }
}
