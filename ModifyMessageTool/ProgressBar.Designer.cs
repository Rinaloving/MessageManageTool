namespace ModifyMessageTool
{
    partial class ProgressBar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressBar));
            this.pro_Bar1 = new CCWin.SkinControl.SkinProgressBar();
            this.SuspendLayout();
            // 
            // pro_Bar1
            // 
            this.pro_Bar1.Back = null;
            this.pro_Bar1.BackColor = System.Drawing.Color.Transparent;
            this.pro_Bar1.BarBack = null;
            this.pro_Bar1.BarRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.pro_Bar1.ForeColor = System.Drawing.Color.Red;
            this.pro_Bar1.Location = new System.Drawing.Point(29, 31);
            this.pro_Bar1.Name = "pro_Bar1";
            this.pro_Bar1.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.pro_Bar1.Size = new System.Drawing.Size(468, 23);
            this.pro_Bar1.TabIndex = 0;
            // 
            // ProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 69);
            this.Controls.Add(this.pro_Bar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProgressBar";
            this.Text = "正在导出数据......";
            this.TitleOffset = new System.Drawing.Point(0, 4);
            this.Load += new System.EventHandler(this.ProgressBar_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public CCWin.SkinControl.SkinProgressBar pro_Bar1;
    }
}