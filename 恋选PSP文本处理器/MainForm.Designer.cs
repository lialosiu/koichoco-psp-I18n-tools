namespace 恋选PSP文本处理器
{
    partial class MainForm
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入文本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出文本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.状态着色ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.隐藏已翻译文本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.初始化文本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看码表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.生成Scbin与相应码表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.说明ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lOSStudioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.L1_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TextBox_Ori_Text = new System.Windows.Forms.TextBox();
            this.PanelForTextBox_Ori = new System.Windows.Forms.Panel();
            this.TextBox_Ori_LineNum_Name = new System.Windows.Forms.TextBox();
            this.TextBox_Chs_Text = new System.Windows.Forms.TextBox();
            this.PanelForTextBox_Chs = new System.Windows.Forms.Panel();
            this.TextBox_Chs_LineNum_Name = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.MainMenuStrip.SuspendLayout();
            this.MainStatusStrip.SuspendLayout();
            this.PanelForTextBox_Ori.SuspendLayout();
            this.PanelForTextBox_Chs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MainMenuStrip.GripMargin = new System.Windows.Forms.Padding(2);
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.编辑ToolStripMenuItem,
            this.选项ToolStripMenuItem,
            this.工具ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(784, 24);
            this.MainMenuStrip.TabIndex = 0;
            this.MainMenuStrip.Text = "菜单";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入文本ToolStripMenuItem,
            this.导出文本ToolStripMenuItem,
            this.保存ToolStripMenuItem,
            this.toolStripSeparator1,
            this.退出ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 导入文本ToolStripMenuItem
            // 
            this.导入文本ToolStripMenuItem.Name = "导入文本ToolStripMenuItem";
            this.导入文本ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.导入文本ToolStripMenuItem.Text = "导入文本";
            this.导入文本ToolStripMenuItem.Click += new System.EventHandler(this.导入文本ToolStripMenuItem_Click);
            // 
            // 导出文本ToolStripMenuItem
            // 
            this.导出文本ToolStripMenuItem.Name = "导出文本ToolStripMenuItem";
            this.导出文本ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.导出文本ToolStripMenuItem.Text = "导出文本";
            this.导出文本ToolStripMenuItem.Click += new System.EventHandler(this.导出文本ToolStripMenuItem_Click);
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.保存ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem});
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.编辑ToolStripMenuItem.Text = "编辑";
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // 选项ToolStripMenuItem
            // 
            this.选项ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.状态着色ToolStripMenuItem,
            this.隐藏已翻译文本ToolStripMenuItem});
            this.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem";
            this.选项ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.选项ToolStripMenuItem.Text = "选项";
            // 
            // 状态着色ToolStripMenuItem
            // 
            this.状态着色ToolStripMenuItem.Checked = true;
            this.状态着色ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.状态着色ToolStripMenuItem.Name = "状态着色ToolStripMenuItem";
            this.状态着色ToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.状态着色ToolStripMenuItem.Text = "状态着色";
            this.状态着色ToolStripMenuItem.Click += new System.EventHandler(this.状态着色ToolStripMenuItem_Click);
            // 
            // 隐藏已翻译文本ToolStripMenuItem
            // 
            this.隐藏已翻译文本ToolStripMenuItem.Name = "隐藏已翻译文本ToolStripMenuItem";
            this.隐藏已翻译文本ToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.隐藏已翻译文本ToolStripMenuItem.Text = "隐藏已翻译文本";
            this.隐藏已翻译文本ToolStripMenuItem.Click += new System.EventHandler(this.隐藏已翻译文本ToolStripMenuItem_Click);
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.初始化文本ToolStripMenuItem,
            this.查看码表ToolStripMenuItem,
            this.生成Scbin与相应码表ToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // 初始化文本ToolStripMenuItem
            // 
            this.初始化文本ToolStripMenuItem.Name = "初始化文本ToolStripMenuItem";
            this.初始化文本ToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.初始化文本ToolStripMenuItem.Text = "初始化文本";
            this.初始化文本ToolStripMenuItem.Click += new System.EventHandler(this.初始化文本ToolStripMenuItem_Click);
            // 
            // 查看码表ToolStripMenuItem
            // 
            this.查看码表ToolStripMenuItem.Name = "查看码表ToolStripMenuItem";
            this.查看码表ToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.查看码表ToolStripMenuItem.Text = "查看码表";
            this.查看码表ToolStripMenuItem.Click += new System.EventHandler(this.查看码表ToolStripMenuItem_Click);
            // 
            // 生成Scbin与相应码表ToolStripMenuItem
            // 
            this.生成Scbin与相应码表ToolStripMenuItem.Name = "生成Scbin与相应码表ToolStripMenuItem";
            this.生成Scbin与相应码表ToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.生成Scbin与相应码表ToolStripMenuItem.Text = "生成 sc.bin 与相应码表";
            this.生成Scbin与相应码表ToolStripMenuItem.Click += new System.EventHandler(this.生成Scbin与相应码表ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.说明ToolStripMenuItem,
            this.toolStripSeparator2,
            this.lOSStudioToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 说明ToolStripMenuItem
            // 
            this.说明ToolStripMenuItem.Name = "说明ToolStripMenuItem";
            this.说明ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.说明ToolStripMenuItem.Text = "说明";
            this.说明ToolStripMenuItem.Click += new System.EventHandler(this.说明ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(129, 6);
            // 
            // lOSStudioToolStripMenuItem
            // 
            this.lOSStudioToolStripMenuItem.Name = "lOSStudioToolStripMenuItem";
            this.lOSStudioToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.lOSStudioToolStripMenuItem.Text = "LOS Studio";
            this.lOSStudioToolStripMenuItem.Click += new System.EventHandler(this.lOSStudioToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.About_ToolStripMenuItem_Click);
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.L1_ToolStripStatusLabel,
            this.MainToolStripStatusLabel});
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 440);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Size = new System.Drawing.Size(784, 22);
            this.MainStatusStrip.TabIndex = 1;
            this.MainStatusStrip.Text = "statusStrip1";
            // 
            // L1_ToolStripStatusLabel
            // 
            this.L1_ToolStripStatusLabel.Name = "L1_ToolStripStatusLabel";
            this.L1_ToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainToolStripStatusLabel
            // 
            this.MainToolStripStatusLabel.Name = "MainToolStripStatusLabel";
            this.MainToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // TextBox_Ori_Text
            // 
            this.TextBox_Ori_Text.BackColor = System.Drawing.Color.FloralWhite;
            this.TextBox_Ori_Text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Ori_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Ori_Text.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_Ori_Text.Location = new System.Drawing.Point(0, 14);
            this.TextBox_Ori_Text.Multiline = true;
            this.TextBox_Ori_Text.Name = "TextBox_Ori_Text";
            this.TextBox_Ori_Text.ReadOnly = true;
            this.TextBox_Ori_Text.Size = new System.Drawing.Size(782, 64);
            this.TextBox_Ori_Text.TabIndex = 4;
            // 
            // PanelForTextBox_Ori
            // 
            this.PanelForTextBox_Ori.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelForTextBox_Ori.Controls.Add(this.TextBox_Ori_Text);
            this.PanelForTextBox_Ori.Controls.Add(this.TextBox_Ori_LineNum_Name);
            this.PanelForTextBox_Ori.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelForTextBox_Ori.Location = new System.Drawing.Point(0, 280);
            this.PanelForTextBox_Ori.Margin = new System.Windows.Forms.Padding(0);
            this.PanelForTextBox_Ori.Name = "PanelForTextBox_Ori";
            this.PanelForTextBox_Ori.Size = new System.Drawing.Size(784, 80);
            this.PanelForTextBox_Ori.TabIndex = 5;
            // 
            // TextBox_Ori_LineNum_Name
            // 
            this.TextBox_Ori_LineNum_Name.BackColor = System.Drawing.Color.FloralWhite;
            this.TextBox_Ori_LineNum_Name.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Ori_LineNum_Name.Dock = System.Windows.Forms.DockStyle.Top;
            this.TextBox_Ori_LineNum_Name.Location = new System.Drawing.Point(0, 0);
            this.TextBox_Ori_LineNum_Name.Name = "TextBox_Ori_LineNum_Name";
            this.TextBox_Ori_LineNum_Name.ReadOnly = true;
            this.TextBox_Ori_LineNum_Name.Size = new System.Drawing.Size(782, 14);
            this.TextBox_Ori_LineNum_Name.TabIndex = 5;
            // 
            // TextBox_Chs_Text
            // 
            this.TextBox_Chs_Text.BackColor = System.Drawing.Color.LightYellow;
            this.TextBox_Chs_Text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Chs_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Chs_Text.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_Chs_Text.Location = new System.Drawing.Point(0, 14);
            this.TextBox_Chs_Text.Multiline = true;
            this.TextBox_Chs_Text.Name = "TextBox_Chs_Text";
            this.TextBox_Chs_Text.Size = new System.Drawing.Size(780, 62);
            this.TextBox_Chs_Text.TabIndex = 5;
            // 
            // PanelForTextBox_Chs
            // 
            this.PanelForTextBox_Chs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelForTextBox_Chs.Controls.Add(this.TextBox_Chs_Text);
            this.PanelForTextBox_Chs.Controls.Add(this.TextBox_Chs_LineNum_Name);
            this.PanelForTextBox_Chs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelForTextBox_Chs.Location = new System.Drawing.Point(0, 360);
            this.PanelForTextBox_Chs.Margin = new System.Windows.Forms.Padding(0);
            this.PanelForTextBox_Chs.Name = "PanelForTextBox_Chs";
            this.PanelForTextBox_Chs.Size = new System.Drawing.Size(784, 80);
            this.PanelForTextBox_Chs.TabIndex = 6;
            // 
            // TextBox_Chs_LineNum_Name
            // 
            this.TextBox_Chs_LineNum_Name.BackColor = System.Drawing.Color.LightYellow;
            this.TextBox_Chs_LineNum_Name.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Chs_LineNum_Name.Dock = System.Windows.Forms.DockStyle.Top;
            this.TextBox_Chs_LineNum_Name.Location = new System.Drawing.Point(0, 0);
            this.TextBox_Chs_LineNum_Name.Name = "TextBox_Chs_LineNum_Name";
            this.TextBox_Chs_LineNum_Name.ReadOnly = true;
            this.TextBox_Chs_LineNum_Name.Size = new System.Drawing.Size(780, 14);
            this.TextBox_Chs_LineNum_Name.TabIndex = 6;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(784, 256);
            this.dataGridView1.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.PanelForTextBox_Ori);
            this.Controls.Add(this.MainMenuStrip);
            this.Controls.Add(this.PanelForTextBox_Chs);
            this.Controls.Add(this.MainStatusStrip);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "恋选PSP文本处理器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            this.PanelForTextBox_Ori.ResumeLayout(false);
            this.PanelForTextBox_Ori.PerformLayout();
            this.PanelForTextBox_Chs.ResumeLayout(false);
            this.PanelForTextBox_Chs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 说明ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel L1_ToolStripStatusLabel;
        private System.Windows.Forms.TextBox TextBox_Ori_Text;
        private System.Windows.Forms.Panel PanelForTextBox_Ori;
        private System.Windows.Forms.TextBox TextBox_Chs_Text;
        private System.Windows.Forms.ToolStripMenuItem 查看码表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 初始化文本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel MainToolStripStatusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.TextBox TextBox_Ori_LineNum_Name;
        private System.Windows.Forms.Panel PanelForTextBox_Chs;
        private System.Windows.Forms.TextBox TextBox_Chs_LineNum_Name;
        private System.Windows.Forms.ToolStripMenuItem 导入文本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出文本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lOSStudioToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 选项ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 状态着色ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 隐藏已翻译文本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 生成Scbin与相应码表ToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

