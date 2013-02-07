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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
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
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mainToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBox_OriText = new System.Windows.Forms.TextBox();
            this.panel_ForOri = new System.Windows.Forms.Panel();
            this.textBox_OriName = new System.Windows.Forms.TextBox();
            this.textBox_ChsText = new System.Windows.Forms.TextBox();
            this.panel_ForChs = new System.Windows.Forms.Panel();
            this.textBox_ChsName = new System.Windows.Forms.TextBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridView_LineNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_OriName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_ChsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_OriText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_ChsText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_Waiting = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenuStrip.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.panel_ForOri.SuspendLayout();
            this.panel_ForChs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panel_Waiting.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.mainMenuStrip.GripMargin = new System.Windows.Forms.Padding(2);
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.编辑ToolStripMenuItem,
            this.选项ToolStripMenuItem,
            this.工具ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(784, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "菜单";
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
            this.隐藏已翻译文本ToolStripMenuItem.Checked = true;
            this.隐藏已翻译文本ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
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
            this.lOSStudioToolStripMenuItem});
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
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripStatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 440);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(784, 22);
            this.mainStatusStrip.TabIndex = 1;
            this.mainStatusStrip.Text = "statusStrip1";
            // 
            // mainToolStripStatusLabel
            // 
            this.mainToolStripStatusLabel.Name = "mainToolStripStatusLabel";
            this.mainToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // textBox_OriText
            // 
            this.textBox_OriText.BackColor = System.Drawing.Color.FloralWhite;
            this.textBox_OriText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_OriText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_OriText.Font = new System.Drawing.Font("极限新黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_OriText.Location = new System.Drawing.Point(0, 19);
            this.textBox_OriText.Multiline = true;
            this.textBox_OriText.Name = "textBox_OriText";
            this.textBox_OriText.ReadOnly = true;
            this.textBox_OriText.Size = new System.Drawing.Size(782, 59);
            this.textBox_OriText.TabIndex = 4;
            // 
            // panel_ForOri
            // 
            this.panel_ForOri.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_ForOri.Controls.Add(this.textBox_OriText);
            this.panel_ForOri.Controls.Add(this.textBox_OriName);
            this.panel_ForOri.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_ForOri.Location = new System.Drawing.Point(0, 280);
            this.panel_ForOri.Margin = new System.Windows.Forms.Padding(0);
            this.panel_ForOri.Name = "panel_ForOri";
            this.panel_ForOri.Size = new System.Drawing.Size(784, 80);
            this.panel_ForOri.TabIndex = 5;
            // 
            // textBox_OriName
            // 
            this.textBox_OriName.BackColor = System.Drawing.Color.FloralWhite;
            this.textBox_OriName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_OriName.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox_OriName.Font = new System.Drawing.Font("极限新黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_OriName.Location = new System.Drawing.Point(0, 0);
            this.textBox_OriName.Name = "textBox_OriName";
            this.textBox_OriName.ReadOnly = true;
            this.textBox_OriName.Size = new System.Drawing.Size(782, 19);
            this.textBox_OriName.TabIndex = 5;
            // 
            // textBox_ChsText
            // 
            this.textBox_ChsText.BackColor = System.Drawing.Color.LightYellow;
            this.textBox_ChsText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_ChsText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_ChsText.Font = new System.Drawing.Font("极限新黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_ChsText.Location = new System.Drawing.Point(0, 19);
            this.textBox_ChsText.Multiline = true;
            this.textBox_ChsText.Name = "textBox_ChsText";
            this.textBox_ChsText.Size = new System.Drawing.Size(780, 57);
            this.textBox_ChsText.TabIndex = 5;
            this.textBox_ChsText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_ChsText_KeyDown);
            this.textBox_ChsText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_ChsText_KeyPress);
            // 
            // panel_ForChs
            // 
            this.panel_ForChs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_ForChs.Controls.Add(this.textBox_ChsText);
            this.panel_ForChs.Controls.Add(this.textBox_ChsName);
            this.panel_ForChs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_ForChs.Location = new System.Drawing.Point(0, 360);
            this.panel_ForChs.Margin = new System.Windows.Forms.Padding(0);
            this.panel_ForChs.Name = "panel_ForChs";
            this.panel_ForChs.Size = new System.Drawing.Size(784, 80);
            this.panel_ForChs.TabIndex = 6;
            // 
            // textBox_ChsName
            // 
            this.textBox_ChsName.BackColor = System.Drawing.Color.LightYellow;
            this.textBox_ChsName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_ChsName.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox_ChsName.Font = new System.Drawing.Font("极限新黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_ChsName.Location = new System.Drawing.Point(0, 0);
            this.textBox_ChsName.Name = "textBox_ChsName";
            this.textBox_ChsName.ReadOnly = true;
            this.textBox_ChsName.Size = new System.Drawing.Size(780, 19);
            this.textBox_ChsName.TabIndex = 6;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridView_LineNum,
            this.dataGridView_OriName,
            this.dataGridView_ChsName,
            this.dataGridView_OriText,
            this.dataGridView_ChsText});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.GridColor = System.Drawing.Color.Silver;
            this.dataGridView.Location = new System.Drawing.Point(0, 24);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersWidth = 10;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(784, 256);
            this.dataGridView.TabIndex = 7;
            this.dataGridView.CurrentCellChanged += new System.EventHandler(this.dataGridView_CurrentCellChanged);
            // 
            // dataGridView_LineNum
            // 
            this.dataGridView_LineNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridView_LineNum.HeaderText = "行号";
            this.dataGridView_LineNum.MaxInputLength = 5;
            this.dataGridView_LineNum.MinimumWidth = 40;
            this.dataGridView_LineNum.Name = "dataGridView_LineNum";
            this.dataGridView_LineNum.ReadOnly = true;
            this.dataGridView_LineNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_LineNum.Width = 40;
            // 
            // dataGridView_OriName
            // 
            this.dataGridView_OriName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridView_OriName.HeaderText = "原名";
            this.dataGridView_OriName.MinimumWidth = 110;
            this.dataGridView_OriName.Name = "dataGridView_OriName";
            this.dataGridView_OriName.ReadOnly = true;
            this.dataGridView_OriName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_OriName.Width = 110;
            // 
            // dataGridView_ChsName
            // 
            this.dataGridView_ChsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridView_ChsName.HeaderText = "译名";
            this.dataGridView_ChsName.MinimumWidth = 110;
            this.dataGridView_ChsName.Name = "dataGridView_ChsName";
            this.dataGridView_ChsName.ReadOnly = true;
            this.dataGridView_ChsName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_ChsName.Width = 110;
            // 
            // dataGridView_OriText
            // 
            this.dataGridView_OriText.FillWeight = 99.61929F;
            this.dataGridView_OriText.HeaderText = "原文";
            this.dataGridView_OriText.Name = "dataGridView_OriText";
            this.dataGridView_OriText.ReadOnly = true;
            // 
            // dataGridView_ChsText
            // 
            this.dataGridView_ChsText.FillWeight = 99.61929F;
            this.dataGridView_ChsText.HeaderText = "译文";
            this.dataGridView_ChsText.Name = "dataGridView_ChsText";
            this.dataGridView_ChsText.ReadOnly = true;
            // 
            // panel_Waiting
            // 
            this.panel_Waiting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Waiting.BackColor = System.Drawing.Color.AliceBlue;
            this.panel_Waiting.Controls.Add(this.label1);
            this.panel_Waiting.Location = new System.Drawing.Point(0, 0);
            this.panel_Waiting.Name = "panel_Waiting";
            this.panel_Waiting.Size = new System.Drawing.Size(10, 10);
            this.panel_Waiting.TabIndex = 8;
            this.panel_Waiting.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("XHei SimSun.ShinYaLan", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 0);
            this.label1.TabIndex = 0;
            this.label1.Text = "处理中，请稍候 m(_ _)m";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.panel_Waiting);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panel_ForOri);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.panel_ForChs);
            this.Controls.Add(this.mainStatusStrip);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "恋选PSP文本处理器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.panel_ForOri.ResumeLayout(false);
            this.panel_ForOri.PerformLayout();
            this.panel_ForChs.ResumeLayout(false);
            this.panel_ForChs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panel_Waiting.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 说明ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.TextBox textBox_OriText;
        private System.Windows.Forms.Panel panel_ForOri;
        private System.Windows.Forms.TextBox textBox_ChsText;
        private System.Windows.Forms.ToolStripMenuItem 查看码表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 初始化文本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel mainToolStripStatusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox_OriName;
        private System.Windows.Forms.Panel panel_ForChs;
        private System.Windows.Forms.TextBox textBox_ChsName;
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
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridView_LineNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridView_OriName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridView_ChsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridView_OriText;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridView_ChsText;
        private System.Windows.Forms.Panel panel_Waiting;
        private System.Windows.Forms.Label label1;
    }
}

