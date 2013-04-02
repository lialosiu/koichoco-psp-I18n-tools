using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 恋选PSP脚本处理器
{
    public partial class MainForm : Form
    {
        CodeTable oriCodeTable = CodeTable.getOriCodeTable();
        GameText gameText = new GameText();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            StartLogo startlogo = new StartLogo();
            startlogo.Show();
            panel_Waiting.Size = this.Size;

            //读取文本
            if (!Toolkit.deserializeTextListFromFile("Data.dat", ref gameText))
            {
                gameText = GameText.InitGameTextFactory();
                dataGridView.DataSource = gameText.getAllAsDataTable();
            }
            else Reload_DataGridView();

            startlogo.Close();
        }

        private void Reload_DataGridView()
        {
            panel_Waiting.Visible = true;
            panel_Waiting.Update();
            if (隐藏已翻译文本ToolStripMenuItem.Checked == true)
                dataGridView.DataSource = gameText.getStatusNotZeroAsDataTable();
            else
                dataGridView.DataSource = gameText.getAllAsDataTable();
            dataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView.Columns[0].Width = 40;
            dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView.Columns[1].Width = 150;
            dataGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView.Columns[2].Width = 150;
            panel_Waiting.Visible = false;
        }

        private void 初始化文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show(null, "将会清空并重建所有文本！确认？", "初始化文本", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)) return;

            gameText = GameText.InitGameTextFactory();
            mainToolStripStatusLabel.Text = "初始化完成";
            Reload_DataGridView();
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
                Int32 lineCount = gameText.readFromTXT(thisFile.FileName);
                Reload_DataGridView();
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
                if (!gameText.writeToTXT(thisFile.FileName))
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
            Toolkit.serializeGameTextToFile("Data.dat", gameText);
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
                Reload_DataGridView();
            }
            else
            {
                隐藏已翻译文本ToolStripMenuItem.Checked = false;
                Reload_DataGridView();
            }
        }

        private void 状态着色ToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Reload_DataGridView();
        }

        private void 生成scbin和ltbinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_Waiting.Visible = true;
            panel_Waiting.Refresh();

            List<Char> usedChsCharList = GameText.UsedChsCharListFactory(gameText.getAllNot2AsList());
            List<Char> lastCharList = new List<Char>();
            for (Int32 i = 0x0000; i <= 0x01EA; i++)
            {
                lastCharList.Add(oriCodeTable.getCharacter(i));         //加入原码表中部分字符
            }
            lastCharList.AddRange(usedChsCharList);                     //加入所使用的中文字符

            CodeTable chsCodeTable = new CodeTable();


            SaveFileDialog output_ltbin = new SaveFileDialog();
            output_ltbin.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            output_ltbin.FileName = "lt";
            output_ltbin.DefaultExt = ".bin";
            output_ltbin.Filter = "恋选PSP字库文件|lt.bin";
            if (output_ltbin.ShowDialog() == DialogResult.OK)
            {
                FileStream oriFileStream_ltbin = File.Open(@"lt.bin", FileMode.Open, FileAccess.Read);
                FileStream outputFileStream_ltbin = new FileStream(output_ltbin.FileName, FileMode.Create, FileAccess.ReadWrite);
                oriFileStream_ltbin.CopyTo(outputFileStream_ltbin);
                outputFileStream_ltbin.Seek(0, SeekOrigin.Begin);

                BinaryWriter outputFileBinaryWriter_ltbin = new BinaryWriter(outputFileStream_ltbin);

                for (Int32 i = 0; i < lastCharList.Count; i++)
                {
                    Char thisChar = lastCharList[i];
                    chsCodeTable.setCharacter(i, thisChar);
                    List<Byte> thisBytes = Toolkit.Character2Bytes(thisChar, new Font("微软雅黑", 10));

                    outputFileBinaryWriter_ltbin.Write(thisBytes.ToArray());

                    //  输出码表文件用
                    //output_TheCodeWordFile.WriteLine((i % 0x100).ToString("X2") + (i / 0x100).ToString("X2") + "=" + thisChar);
                }

                oriFileStream_ltbin.Close();

                Toolkit.updateTheVerifyCode(outputFileStream_ltbin);

                outputFileBinaryWriter_ltbin.Close();
                outputFileStream_ltbin.Close();
            }
            else
            {
                MessageBox.Show("操作已取消");
                panel_Waiting.Visible = false;
                return;
            }

            FileStream oriFileStream_scbin = File.Open(@"sc.bin", FileMode.Open, FileAccess.Read);
            Int64 offsetTextPoint = 0x90;
            Int64 offsetText = 0;

            Byte[] _Bytes;

            SaveFileDialog output_scbin = new SaveFileDialog();
            output_scbin.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            output_scbin.FileName = "sc";
            output_scbin.DefaultExt = ".bin";
            output_scbin.Filter = "恋选PSP脚本文件|sc.bin";
            if (output_scbin.ShowDialog() == DialogResult.OK)
            {
                FileStream outputFileStream_scbin = new FileStream(output_scbin.FileName, FileMode.Create, FileAccess.ReadWrite);
                oriFileStream_scbin.CopyTo(outputFileStream_scbin);

                while (true)
                {
                    _Bytes = new Byte[4];
                    oriFileStream_scbin.Seek(offsetTextPoint, SeekOrigin.Begin);
                    oriFileStream_scbin.Read(_Bytes, 0, 4);
                    offsetTextPoint = oriFileStream_scbin.Position;

                    //换算文本指针所指地址
                    offsetText = 0x30000 + Convert.ToInt32(_Bytes[3]) * 0x100 * 0x100 * 0x100 + Convert.ToInt32(_Bytes[2]) * 0x100 * 0x100 + Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);

                    if (offsetText <= 0x30000) break;   //文本指针结束

                    oriFileStream_scbin.Seek(offsetText, SeekOrigin.Begin);   //根据当前文本指针跳至文本位置

                    Int32 _ID = 0;
                    String _ChsName = null;
                    String _ChsSentences = null;

                    Int32 SentencesCharIndex = 0;

                    _Bytes = new Byte[2];

                    while (true)
                    {
                        oriFileStream_scbin.Read(_Bytes, 0, 2);

                        //入口标记
                        if (_Bytes[0] == 0xF0 && _Bytes[1] == 0xFF)
                        {
                            //读取ID
                            oriFileStream_scbin.Read(_Bytes, 0, 2);
                            _ID = Convert.ToInt32(_Bytes[1]) * 0x100 + Convert.ToInt32(_Bytes[0]);

                            //获取当前行翻译后姓名与文本
                            _ChsName = gameText.getChsNamebyID(_ID);
                            _ChsSentences = gameText.getChsSentencesbyID(_ID);

                            Int32 NameCharIndex = 0;

                            //姓名处理
                            while (true)
                            {
                                oriFileStream_scbin.Read(_Bytes, 0, 2);
                                if (_Bytes[0] == 0xff && _Bytes[1] == 0xff) break;      //正文入口标记，结束姓名处理
                                outputFileStream_scbin.Seek(oriFileStream_scbin.Position - 2, SeekOrigin.Begin);      //输出文件指针同步

                                _Bytes = new Byte[] { 0x00, 0x00 };

                                if (NameCharIndex < _ChsName.Length)
                                {
                                    Byte[] thisCodeByte = chsCodeTable.getCode(_ChsName[NameCharIndex++]);
                                    _Bytes = thisCodeByte == null ? new Byte[] { 0x60, 0x00 } : thisCodeByte;      //0060,'■'
                                }
                                outputFileStream_scbin.Write(_Bytes, 0, 2);
                            }
                            continue;
                        }

                        //强制换行标记？貌似在那个小游戏上面是换行的……
                        if (_Bytes[0] == 0xFE && _Bytes[1] == 0xFF)
                        {
                            continue;
                        }

                        //结束标记
                        if (_Bytes[0] == 0xFB && _Bytes[1] == 0xFF)
                        {
                            break;
                        }

                        //结束标记(不刷新)
                        if (_Bytes[0] == 0xFD && _Bytes[1] == 0xFF)
                        {
                            break;
                        }

                        //输出文件文本指针同步
                        outputFileStream_scbin.Seek(oriFileStream_scbin.Position - 2, SeekOrigin.Begin);

                        _Bytes = new Byte[] { 0x00, 0x00 };

                        if (SentencesCharIndex < _ChsSentences.Length)
                        {
                            Byte[] thisCodeByte = chsCodeTable.getCode(_ChsSentences[SentencesCharIndex++]);
                            _Bytes = thisCodeByte == null ? new Byte[] { 0x60, 0x00 } : thisCodeByte;      //0060,'■'
                        }
                        outputFileStream_scbin.Write(_Bytes, 0, 2);
                    }
                }

                oriFileStream_scbin.Close();

                Toolkit.updateTheVerifyCode(outputFileStream_scbin);

                outputFileStream_scbin.Close();
            }
            else
            {
                MessageBox.Show("操作已取消");
                panel_Waiting.Visible = false;
                return;
            }

            panel_Waiting.Visible = false;

            MessageBox.Show(null, "生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mainToolStripStatusLabel.Text = "生成成功";
        }

        private void 说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(null, "状态着色：" + Environment.NewLine + "绿色：已翻译文本" + Environment.NewLine + "黄色：已翻译，但长度越界文本" + Environment.NewLine + "白色：未翻译文本", "说明");
        }

        private void textBox_ChsText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                gameText.setChsSentencesbyID(Int32.Parse(dataGridView.CurrentRow.Cells[0].Value.ToString()), textBox_ChsText.Text);
                dataGridView.CurrentRow.Cells[4].Value = gameText.getChsSentencesbyID(Int32.Parse(dataGridView.CurrentRow.Cells[0].Value.ToString()));
                if (状态着色ToolStripMenuItem.Checked)
                {
                    switch (gameText.getLinebyID(Int32.Parse(dataGridView.CurrentRow.Cells[0].Value.ToString())).status)
                    {
                        case 0: dataGridView.CurrentRow.DefaultCellStyle.BackColor = Color.LightGreen; break;
                        case 1: dataGridView.CurrentRow.DefaultCellStyle.BackColor = Color.Yellow; break;
                        //case 2: thisRow.DefaultCellStyle.BackColor = Color.White; break;
                        //default: thisRow.DefaultCellStyle.BackColor = Color.Black; break;
                    }
                }
                do
                {
                    dataGridView.CurrentCell = dataGridView.Rows[dataGridView.CurrentRow.Index + 1].Cells[0];
                } while (dataGridView.CurrentCellAddress.Y < dataGridView.Rows.Count - 1 && gameText.getLinebyID(Int32.Parse(dataGridView.Rows[dataGridView.CurrentRow.Index].Cells[0].Value.ToString())).status == 0);
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

        private void 姓名处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NameForm nameForm = new NameForm(gameText);
            nameForm.ShowDialog();
            Reload_DataGridView();
        }

        private void 导入PC版TXT文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog Files = new OpenFileDialog();
            Files.Multiselect = true;
            Files.Filter = "文本文件|*.txt";
            if (Files.ShowDialog() == DialogResult.OK)
            {
                Int32 successCount1 = 0;
                Int32 successCount2 = 0;
                mainToolStripStatusLabel.Text = "正在导入，请稍候……";

                panel_Waiting.Visible = true;
                panel_Waiting.Refresh();


                //导入失败记录日志
                StreamWriter _LogFile1 = new StreamWriter(File.Open("NotImportText1.log", FileMode.Create, FileAccess.Write), Encoding.Unicode);
                StreamWriter _LogFile2 = new StreamWriter(File.Open("NotImportText2.log", FileMode.Create, FileAccess.Write), Encoding.Unicode);

                //重置已修改标记状态
                gameText.setAllLineChangeFlagToFalse();

                //首次匹配
                foreach (String thisFileName in Files.FileNames)
                {
                    successCount1 += gameText.readFromPCTXT(thisFileName, true, _LogFile1, _LogFile2);
                }

                //二次匹配
                foreach (String thisFileName in Files.FileNames)
                {
                    successCount2 += gameText.readFromPCTXT(thisFileName, false, _LogFile1, _LogFile2);
                }

                _LogFile1.Close();
                _LogFile2.Close();

                panel_Waiting.Visible = false;

                Reload_DataGridView();
                MessageBox.Show("首次匹配导入了 " + successCount1 + " 行文本" + Environment.NewLine + "二次匹配导入了 " + successCount2 + " 行文本", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mainToolStripStatusLabel.Text = "导入完成";
            }
        }

        private void dataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            if (状态着色ToolStripMenuItem.Checked == false) return;
            foreach (DataGridViewRow thisRow in dataGridView.Rows)
            {
                switch (gameText.getLinebyID(Int32.Parse(thisRow.Cells[0].Value.ToString())).status)
                {
                    case 0: this.dataGridView.Rows[thisRow.Index].DefaultCellStyle.BackColor = Color.LightGreen; break;
                    case 1: this.dataGridView.Rows[thisRow.Index].DefaultCellStyle.BackColor = Color.Yellow; break;
                    //case 2: thisRow.DefaultCellStyle.BackColor = Color.White; break;
                    //default: thisRow.DefaultCellStyle.BackColor = Color.Black; break;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(null, "是否保存数据？", "退出", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                保存ToolStripMenuItem_Click(sender,null);
            }
        }

    }
}
