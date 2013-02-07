using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
//using System.Runtime.InteropServices；

namespace 恋选PSP文本处理器
{
    public partial class MainForm : Form
    { 
        CodeTable ori_CodeTable = new CodeTable();
        Text text = new Text();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            StartLogo startlogo = new StartLogo();
            startlogo.Show();
            panel_Waiting.Size = this.Size;

            //读取码表
            if (!ori_CodeTable.loadCodeTable()) MessageBox.Show("初始化码表错误");

            //读取文本
            if (!text.readFromXML("Text.xml"))
            {
                MessageBox.Show("XML数据出现错误，将初始化文本");
                if (!text.resetText(ori_CodeTable))
                {
                    MessageBox.Show("初始化文本发生错误");
                    Application.Exit();
                }
                text.writeToXML("Text.xml");
            }
            reload_dataGridView();
            startlogo.Close();
        }

        private void reload_dataGridView()
        {
            panel_Waiting.Visible = true;
            panel_Waiting.Refresh();
            dataGridView.Rows.Clear();
            foreach (Text.oneLineText thisLine in text.getAll_NOTNULL())
            {
                if (隐藏已翻译文本ToolStripMenuItem.Checked && thisLine.Status == 0) continue;
                DataGridViewRow thisRow = new DataGridViewRow();
                thisRow.CreateCells(dataGridView, thisLine.LineNum, thisLine.OriName, thisLine.ChsName, thisLine.OriText, thisLine.ChsText);
                if (状态着色ToolStripMenuItem.Checked)
                {
                    switch (thisLine.Status)
                    {
                        case 0: thisRow.DefaultCellStyle.BackColor = Color.LightGreen; break;
                        case 1: thisRow.DefaultCellStyle.BackColor = Color.Yellow; break;
                        //case 2: thisRow.DefaultCellStyle.BackColor = Color.White; break;
                        //default: thisRow.DefaultCellStyle.BackColor = Color.Black; break;
                    }
                }
                dataGridView.Rows.Add(thisRow);
            }
            panel_Waiting.Visible = false;
        }

        private void 查看码表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Code_Word_Form(ori_CodeTable).Show();
        }

        private void 初始化文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show(null, "将会清空并重建所有文本！确认？", "初始化文本", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)) return;
            text = new Text();
            if (!text.resetText(ori_CodeTable))
            {
                MessageBox.Show("初始化文本发生错误");
                Application.Exit();
            }
            text.writeToXML("Text.xml");
            reload_dataGridView();
        }
        
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 导入文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog thisFile = new OpenFileDialog();
            thisFile.Filter = "文本文件|*.txt";
            if (thisFile.ShowDialog() == DialogResult.OK)
            {
                mainToolStripStatusLabel.Text = "正在导入，请稍候……";
                Int32 lineCount = text.readFromTXT(thisFile.FileName);
                reload_dataGridView();
                MessageBox.Show(null, "成功导入了 " + lineCount + " 行文本", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mainToolStripStatusLabel.Text = "导入完成";
            }
        }

        private void 导出文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog thisFile = new SaveFileDialog();
            thisFile.Filter = "文本文件|*.txt";
            if (thisFile.ShowDialog() == DialogResult.OK)
            {
                mainToolStripStatusLabel.Text = "正在导出";
                if (!text.writeToTXT(thisFile.FileName))
                {
                    MessageBox.Show(null, "未知原因导致的导出失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mainToolStripStatusLabel.Text = "导出失败";
                }
                else
                {
                    MessageBox.Show(null, "成功导出文本", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mainToolStripStatusLabel.Text = "导出完成";
                }
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text.writeToXML("Text.xml");
            mainToolStripStatusLabel.Text = "已保存";
        }

        private void lOSStudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://losacgn.com");
        }

        private void 状态着色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (状态着色ToolStripMenuItem.Checked == false)
            {
                状态着色ToolStripMenuItem.Checked = true;
            }
            else
            {
                状态着色ToolStripMenuItem.Checked = false;
            }
        }

        private void 隐藏已翻译文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (隐藏已翻译文本ToolStripMenuItem.Checked == false)
            {
                隐藏已翻译文本ToolStripMenuItem.Checked = true;
                reload_dataGridView();
            }
            else
            {
                隐藏已翻译文本ToolStripMenuItem.Checked = false;
                reload_dataGridView();
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reload_dataGridView();
        }

        private void 生成Scbin与相应码表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_Waiting.Visible = true;
            panel_Waiting.Refresh();
            ArrayList usedWords = new ArrayList();

            foreach (Text.oneLineText thisLine in text.getAll_NOTNULL())
            {
                foreach (Char thisWord_in_thisLine in thisLine.ChsName + thisLine.ChsText)
                {
                    Boolean unUsed = true;
                    foreach (Char thisWord_in_usedWords in usedWords)
                    {
                        if (thisWord_in_thisLine == thisWord_in_usedWords)
                        {
                            unUsed = false;
                            break;
                        }
                    }
                    if (unUsed) usedWords.Add(thisWord_in_thisLine);
                }
            }
            usedWords.Sort();
            
            CodeTable chs_CodeTable = new CodeTable();

            chs_CodeTable.addWord(ori_CodeTable.getWord(0));    //0000
            foreach (Char thisWord in usedWords)
            {
                chs_CodeTable.addWord(thisWord);
            }

            SaveFileDialog TheCodeWordFile = new SaveFileDialog();
            TheCodeWordFile.Filter = "码表文件|*.txt";
            if (TheCodeWordFile.ShowDialog() == DialogResult.OK)
            {
                StreamWriter output_TheCodeWordFile = new StreamWriter(new FileStream(TheCodeWordFile.FileName, FileMode.Create, FileAccess.Write),UnicodeEncoding.Unicode);
                for (Int32 i = 0; i < 0xffff; i++)
                {
                    Char c = chs_CodeTable.getWord(i);
                    if (c == '\0') break;
                    else output_TheCodeWordFile.WriteLine((i % 0x100).ToString("X2") + (i / 0x100).ToString("X2") + "=" + c);
                }
                output_TheCodeWordFile.Close();
            }
            else
            {
                MessageBox.Show("操作已取消");
                return;
            }

            Stream sc_file_for_point = Assembly.GetExecutingAssembly().GetManifestResourceStream("恋选PSP文本处理器.Files.sc.bin");
            Stream sc_file_for_text = Assembly.GetExecutingAssembly().GetManifestResourceStream("恋选PSP文本处理器.Files.sc.bin");
           
            SaveFileDialog TheFile = new SaveFileDialog();
            TheFile.Filter = "脚本文件|*.bin";
            if (TheFile.ShowDialog() == DialogResult.OK)
            {
                FileStream output_file = new FileStream(TheFile.FileName, FileMode.Create, FileAccess.ReadWrite);
                Assembly.GetExecutingAssembly().GetManifestResourceStream("恋选PSP文本处理器.Files.sc.bin").CopyTo(output_file);

                sc_file_for_point.Seek(0x90, SeekOrigin.Begin);         //跳至文本指针表
                while (true)
                {
                    Byte[] now_point_byte = new Byte[4];
                    sc_file_for_point.Read(now_point_byte, 0, 4);

                    //换算文本指针所指地址
                    Int32 now_point = 0x30000 + Convert.ToInt32(now_point_byte[3]) * 0x100 * 0x100 * 0x100 + Convert.ToInt32(now_point_byte[2]) * 0x100 * 0x100 + Convert.ToInt32(now_point_byte[1]) * 0x100 + Convert.ToInt32(now_point_byte[0]);

                    if (now_point <= 0x30000) break;       //文本指针结束标记

                    sc_file_for_text.Seek(now_point, SeekOrigin.Begin);     //根据当前文本指针跳至文本位置

                    Int32 line_num = 0;
                    String now_line_chs_name = "";
                    String now_line_chs_text = "";
                    Byte[] a_word_byte = new Byte[2];
                    Int32 Text_Length_Count = 0;
                    while (true)
                    {
                        sc_file_for_text.Read(a_word_byte, 0, 2);
                        if (a_word_byte[0] == 0xf0 && a_word_byte[1] == 0xff)       //入口标记
                        {
                            //读取行号
                            sc_file_for_text.Read(a_word_byte, 0, 2);
                            line_num = Convert.ToInt32(a_word_byte[1]) * 0x100 + Convert.ToInt32(a_word_byte[0]);

                            //获取当前行翻译后姓名与文本
                            now_line_chs_name = text.getChsName(line_num);
                            now_line_chs_text = text.getChsText(line_num);

                            //姓名处理
                            Int32 Name_Length_Count = 0;
                            while (true)
                            {
                                sc_file_for_text.Read(a_word_byte, 0, 2);
                                if (a_word_byte[0] == 0xff && a_word_byte[1] == 0xff) break;       //正文入口标记，结束姓名处理

                                output_file.Seek(sc_file_for_text.Position - 2, SeekOrigin.Begin);      //输出文件文本指针同步
                                if (Name_Length_Count < now_line_chs_name.Length)
                                    output_file.Write(chs_CodeTable.getCode(now_line_chs_name[Name_Length_Count++]), 0, 2);    //输出文本
                                else
                                    output_file.Write(new Byte[] { 0, 0 }, 0, 2);    //输出空白
                            }
                            continue;
                        }

                        if (a_word_byte[0] == 0xfe && a_word_byte[1] == 0xff) break;	//出口标记

                        output_file.Seek(sc_file_for_text.Position - 2, SeekOrigin.Begin);      //输出文件文本指针同步

                        if (Text_Length_Count < now_line_chs_text.Length)
                            output_file.Write(chs_CodeTable.getCode(now_line_chs_text[Text_Length_Count++]), 0, 2);    //输出文本
                        else
                            output_file.Write(new Byte[] { 0, 0 }, 0, 2);    //输出空白
                    }
                }
                sc_file_for_text.Close();
                sc_file_for_point.Close();

                //更新校验码
                {

                    Int32[] count = new Int32[16] { 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11 };
                    Byte[] thisLineByte = new Byte[16];
                    output_file.Seek(0,SeekOrigin.Begin);

                    while (true)
                    {
                        if (output_file.Position >= output_file.Length - 0x10) break;
                        output_file.Read(thisLineByte, 0, 16);
                        for (Int32 i = 0; i <= 15; i++)
                        {
                            count[i] += thisLineByte[i];
                            if (count[i] >= 0x100)
                            {
                                count[i] %= 0x100;
                                if (!(i == 7 || i == 15)) count[i + 1]++;
                            }
                        }
                    }
                    for (Int32 i = 0; i <= 15; i++)
                    {
                        thisLineByte[i] = (Byte)count[i];
                    }
                    output_file.Write(thisLineByte, 0, 16);
                }
                output_file.Close();
            }
            else
            {
                MessageBox.Show("操作已取消");
                return;
            }
            MessageBox.Show("成功生成sc.bin与相应码表");
            mainToolStripStatusLabel.Text = "成功生成sc.bin与相应码表";
            panel_Waiting.Visible = false;
        }

        private void 说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(null, "状态着色：" + Environment.NewLine + "绿色：已翻译文本" + Environment.NewLine + "黄色：已翻译，但长度越界文本" + Environment.NewLine + "白色：未翻译文本", "说明");
        }

        private void textBox_ChsText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                text.change(Int32.Parse(dataGridView.CurrentRow.Cells[0].Value.ToString()), textBox_ChsText.Text);
                dataGridView.CurrentRow.Cells[4].Value = text.getChsText(Int32.Parse(dataGridView.CurrentRow.Cells[0].Value.ToString()));
                if (状态着色ToolStripMenuItem.Checked)
                {
                    switch (text.getStatus(Int32.Parse(dataGridView.CurrentRow.Cells[0].Value.ToString())))
                    {
                        case 0: dataGridView.CurrentRow.DefaultCellStyle.BackColor = Color.LightGreen; break;
                        case 1: dataGridView.CurrentRow.DefaultCellStyle.BackColor = Color.Yellow; break;
                        //case 2: thisRow.DefaultCellStyle.BackColor = Color.White; break;
                        //default: thisRow.DefaultCellStyle.BackColor = Color.Black; break;
                    }
                }
                if (dataGridView.CurrentCellAddress.Y < dataGridView.Rows.Count - 1) dataGridView.CurrentCell = dataGridView.Rows[dataGridView.CurrentRow.Index + 1].Cells[0];
                e.Handled = true;
            }
        }

        private void dataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null) return;
            textBox_OriName.Text = dataGridView.CurrentRow.Cells[1].Value.ToString();
            textBox_OriText.Text = dataGridView.CurrentRow.Cells[3].Value.ToString();
            textBox_ChsName.Text = dataGridView.CurrentRow.Cells[2].Value.ToString();
            textBox_ChsText.Text = dataGridView.CurrentRow.Cells[4].Value.ToString();
        }

        private void textBox_ChsText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (dataGridView.CurrentCellAddress.Y > 0) dataGridView.CurrentCell = dataGridView.Rows[dataGridView.CurrentRow.Index - 1].Cells[0];
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (dataGridView.CurrentCellAddress.Y < dataGridView.Rows.Count - 1) dataGridView.CurrentCell = dataGridView.Rows[dataGridView.CurrentRow.Index + 1].Cells[0];
                e.Handled = true;
            }
        }
    }
}
