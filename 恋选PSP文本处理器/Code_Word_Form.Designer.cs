namespace 恋选PSP文本处理器
{
    partial class Code_Word_Form
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
            this.Code_Word_ListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // Code_Word_ListBox
            // 
            this.Code_Word_ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Code_Word_ListBox.FormattingEnabled = true;
            this.Code_Word_ListBox.ItemHeight = 12;
            this.Code_Word_ListBox.Location = new System.Drawing.Point(0, 0);
            this.Code_Word_ListBox.Name = "Code_Word_ListBox";
            this.Code_Word_ListBox.Size = new System.Drawing.Size(84, 422);
            this.Code_Word_ListBox.TabIndex = 0;
            this.Code_Word_ListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // Code_Word_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(84, 422);
            this.Controls.Add(this.Code_Word_ListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Code_Word_Form";
            this.ShowIcon = false;
            this.Text = "码表";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Code_Word_ListBox;
    }
}