namespace AutoCad
{
    partial class AutoCad
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
            this.location = new System.Windows.Forms.Label();
            this.locationT = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // location
            // 
            this.location.AutoSize = true;
            this.location.Dock = System.Windows.Forms.DockStyle.Left;
            this.location.Location = new System.Drawing.Point(0, 0);
            this.location.Margin = new System.Windows.Forms.Padding(0);
            this.location.MinimumSize = new System.Drawing.Size(29, 21);
            this.location.Name = "location";
            this.location.Size = new System.Drawing.Size(29, 21);
            this.location.TabIndex = 0;
            this.location.Text = "位置";
            this.location.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // locationT
            // 
            this.locationT.Dock = System.Windows.Forms.DockStyle.Left;
            this.locationT.Location = new System.Drawing.Point(29, 0);
            this.locationT.Margin = new System.Windows.Forms.Padding(0);
            this.locationT.Name = "locationT";
            this.locationT.Size = new System.Drawing.Size(100, 21);
            this.locationT.TabIndex = 1;
            // 
            // AutoCad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.locationT);
            this.Controls.Add(this.location);
            this.Name = "AutoCad";
            this.Size = new System.Drawing.Size(135, 505);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label location;
        private System.Windows.Forms.TextBox locationT;
    }
}
