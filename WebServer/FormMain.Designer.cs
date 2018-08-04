namespace WebServer
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lab_Count = new System.Windows.Forms.Label();
            this.txt_MSG = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Count:";
            // 
            // lab_Count
            // 
            this.lab_Count.AutoSize = true;
            this.lab_Count.Location = new System.Drawing.Point(130, 58);
            this.lab_Count.Name = "lab_Count";
            this.lab_Count.Size = new System.Drawing.Size(11, 12);
            this.lab_Count.TabIndex = 1;
            this.lab_Count.Text = "0";
            // 
            // txt_MSG
            // 
            this.txt_MSG.Location = new System.Drawing.Point(12, 103);
            this.txt_MSG.Multiline = true;
            this.txt_MSG.Name = "txt_MSG";
            this.txt_MSG.Size = new System.Drawing.Size(685, 240);
            this.txt_MSG.TabIndex = 2;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 371);
            this.Controls.Add(this.txt_MSG);
            this.Controls.Add(this.lab_Count);
            this.Controls.Add(this.label1);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lab_Count;
        private System.Windows.Forms.TextBox txt_MSG;
    }
}

