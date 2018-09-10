namespace MESInterface.TJ
{
    partial class DownSO_UI
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.SOHeaderData = new System.Windows.Forms.DataGridView();
            this.SOHeader = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.SODetailData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.SOHeaderData)).BeginInit();
            this.SOHeader.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SODetailData)).BeginInit();
            this.SuspendLayout();
            // 
            // SOHeaderData
            // 
            this.SOHeaderData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SOHeaderData.Location = new System.Drawing.Point(-4, 0);
            this.SOHeaderData.Name = "SOHeaderData";
            this.SOHeaderData.RowTemplate.Height = 24;
            this.SOHeaderData.Size = new System.Drawing.Size(891, 189);
            this.SOHeaderData.TabIndex = 1;
            this.SOHeaderData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SOHeaderData_CellContentClick);
            // 
            // SOHeader
            // 
            this.SOHeader.Controls.Add(this.tabPage1);
            this.SOHeader.Controls.Add(this.tabPage2);
            this.SOHeader.Location = new System.Drawing.Point(3, 0);
            this.SOHeader.Name = "SOHeader";
            this.SOHeader.SelectedIndex = 0;
            this.SOHeader.Size = new System.Drawing.Size(895, 214);
            this.SOHeader.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SOHeaderData);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(887, 188);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SO Header";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.SODetailData);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(887, 185);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SO Detail";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SODetailData
            // 
            this.SODetailData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SODetailData.Location = new System.Drawing.Point(-4, 0);
            this.SODetailData.Name = "SODetailData";
            this.SODetailData.RowTemplate.Height = 24;
            this.SODetailData.Size = new System.Drawing.Size(891, 186);
            this.SODetailData.TabIndex = 0;
            // 
            // DownSO_UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SOHeader);
            this.Name = "DownSO_UI";
            this.Size = new System.Drawing.Size(898, 214);
            this.Load += new System.EventHandler(this.DownSO_UI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SOHeaderData)).EndInit();
            this.SOHeader.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SODetailData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView SOHeaderData;
        private System.Windows.Forms.TabControl SOHeader;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView SODetailData;
    }
}
