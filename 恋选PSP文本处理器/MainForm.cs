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
using System.Timers;
using System.Text.RegularExpressions;

namespace 恋选PSP文本处理器
{
    public partial class MainForm : Form
    {
        CodeTable oriCodeTable;
        GameText gameText;
        Boolean successLoad = false;
        System.Timers.Timer autoSaveTimer;
        Int32 textGCount = 0;
        Int32 textYCount = 0;
        Int32 textWCount = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            StartLogo startlogo = new StartLogo();
            startlogo.Show();
            panel_Waiting.Size = this.Size;

            if (!File.Exists("OriCodeTable.xml"))
            {
                MessageBox.Show(null, "找不到码表文件OriCodeTable.xml", "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Dispose();
                Application.Exit();
                return;
            }

            if (!File.Exists("sc.bin"))
            {
                MessageBox.Show(null, "找不到原始脚本文件sc.bin", "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Dispose();
                Application.Exit();
                return;
            }

            if (!File.Exists("lt.bin"))
            {
                MessageBox.Show(null, "找不到原始字库文件lt.bin", "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Dispose();
                Application.Exit();
                return;
            }
            this.oriCodeTable = CodeTable.OriCodeTableFactory();
            this.gameText = new GameText();

            //读取文本
            if (!Toolkit.deserializeTextListFromFile("Data.dat", ref gameText))
            {
                gameText = GameText.InitGameTextFactory();
            }

            Reload_DataGridView();

            //自动储存计时器
            autoSaveTimer = new System.Timers.Timer(1000 * 60);
            autoSaveTimer.Elapsed += new ElapsedEventHandler(autoSave);
            autoSaveTimer.AutoReset = true;
            autoSaveTimer.Enabled = true;

            startlogo.Close();
            successLoad = true;
        }

        private void autoSave(object source, System.Timers.ElapsedEventArgs e)
        {
            this.saveFile("AutoSaveData.dat", "[" + DateTime.Now.TimeOfDay.ToString() + "] 自动保存");
        }

        private void Reload_DataGridView()
        {
            panel_Waiting.Visible = true;
            panel_Waiting.Update();

            dataGridView.Rows.Clear();
            textGCount = 0;
            textYCount = 0;
            textWCount = 0;

            List<GameText.Line> thisGameTextList;
            if (隐藏已翻译文本ToolStripMenuItem.Checked == true)
            {
                thisGameTextList = this.gameText.getAllNot0AsList();
            }
            else
            {
                thisGameTextList = this.gameText.getAllAsList();
            }

            foreach (GameText.Line thisLine in thisGameTextList)
            {
                DataGridViewRow thisDataGridViewRow = new DataGridViewRow();
                thisDataGridViewRow.CreateCells(dataGridView, thisLine.id, thisLine.oriName, thisLine.chsName, thisLine.oriSentences, thisLine.chsSentences);

                if (状态着色ToolStripMenuItem.Checked == true)
                {
                    switch (gameText.getLinebyID(thisLine.id).status)
                    {
                        case 0: thisDataGridViewRow.DefaultCellStyle.BackColor = Color.LightGreen; textGCount++; break;
                        case 1: thisDataGridViewRow.DefaultCellStyle.BackColor = Color.Yellow; textYCount++; break;
                        case 2: textWCount++; break;
                        //default: thisRow.DefaultCellStyle.BackColor = Color.Black; break;
                    }
                }
                dataGridView.Rows.Add(thisDataGridViewRow);
            }
            dataGridView.Select();
            panel_Waiting.Visible = false;
            toolStripStatusLabel1.Text = "已翻：" + textGCount + " | 超长：" + textYCount + " | 未翻：" + textWCount;
        }

        private void 初始化文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show(null, "将会清空并重建所有文本！确认？", "初始化文本", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)) return;

            gameText = GameText.InitGameTextFactory();
            toolStripStatusLabel1.Text = "初始化完成";
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
                toolStripStatusLabel1.Text = "正在导入，请稍候……";
                Int32 lineCount = gameText.readFromTXT(thisFile.FileName);
                Reload_DataGridView();
                MessageBox.Show(null, "成功导入了 " + lineCount + " 行文本", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                toolStripStatusLabel1.Text = "导入完成";
            }
        }

        private void 导出文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog thisFile = new SaveFileDialog();
            thisFile.Filter = "文本文件|*.txt";
            if (thisFile.ShowDialog() == DialogResult.OK)
            {
                toolStripStatusLabel1.Text = "正在导出";
                if (!gameText.writeToTXT(thisFile.FileName))
                {
                    MessageBox.Show(null, "未知原因导致的导出失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripStatusLabel1.Text = "导出失败";
                }
                else
                {
                    MessageBox.Show(null, "成功导出文本", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    toolStripStatusLabel1.Text = "导出完成";
                }
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveFile("Data.dat", "已保存");
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

        private void BuildBinFile_old_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_Waiting.Visible = true;
            panel_Waiting.Refresh();

            SaveFileDialog output_ltbin = new SaveFileDialog();
            output_ltbin.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            output_ltbin.FileName = "lt";
            output_ltbin.DefaultExt = ".bin";
            output_ltbin.Filter = "恋选PSP字库文件|lt.bin";

            SaveFileDialog output_scbin = new SaveFileDialog();
            output_scbin.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            output_scbin.FileName = "sc";
            output_scbin.DefaultExt = ".bin";
            output_scbin.Filter = "恋选PSP脚本文件|sc.bin";

            if (output_ltbin.ShowDialog() != DialogResult.OK || output_scbin.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("操作已取消");
                panel_Waiting.Visible = false;
                return;
            }

            CodeTable chsCodeTable = CodeTable.CodeTableFactory(GameText.UsedChsCharListFactory((gameText.getAllNot2AsList())));
            //Toolkit.buildTheTxtCodeTableFile(chsCodeTable);
            Toolkit.buildTheGameFile_lt_bin(chsCodeTable, output_ltbin.FileName, false);
            Toolkit.buildTheGameFile_sc_bin_old(gameText, chsCodeTable, output_scbin.FileName);

            panel_Waiting.Visible = false;

            MessageBox.Show(null, "生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            toolStripStatusLabel1.Text = "生成成功";
        }

        private void 说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(null, "本程序仅供恋选PSP版汉化工作使用" + Environment.NewLine + "By Lialosiu", "说明");
        }

        private void textBox_ChsText_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((Int32)e.KeyChar)
            {
                case 13:
                    {
                        ChsTextBoxSubmit();
                        e.Handled = true;
                    }
                    break;
                default:
                    {
                        //mainToolStripStatusLabel.Text = ((int)(e.KeyChar)).ToString();
                    }
                    break;
            }
        }

        private void dataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null) return;
            textBox_OriName.Text = dataGridView.CurrentRow.Cells[1].Value.ToString();
            textBox_OriText.Text = dataGridView.CurrentRow.Cells[3].Value.ToString();
            textBox_ChsName.Text = dataGridView.CurrentRow.Cells[2].Value.ToString();
            textBox_ChsText.Text = dataGridView.CurrentRow.Cells[4].Value.ToString();
            textBox_ChsText.SelectAll();
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
                toolStripStatusLabel1.Text = "正在导入，请稍候……";

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
                toolStripStatusLabel1.Text = "导入完成";
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (successLoad && DialogResult.Yes == MessageBox.Show(null, "是否保存数据？", "退出", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                保存ToolStripMenuItem_Click(sender, null);
            }
        }

        private void 跳过已翻译文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            跳过已翻译文本ToolStripMenuItem.Checked = !跳过已翻译文本ToolStripMenuItem.Checked;
        }

        /// <summary>
        /// 序列化保存GameText
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="tips">提示</param>
        private void saveFile(String fileName, String tips)
        {
            Toolkit.serializeGameTextToFile(fileName, this.gameText);
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox_ChsText.SelectAll();
            textBox_ChsText.Focus();
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm();
            searchForm.SearchText += new SearchTextHandle(SearchTextAtDataGridView);
            searchForm.ReplaceText += new ReplaceTextHandle(ReplaceTextAtDataGridView);
            searchForm.Show();
        }

        private void QuickSearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((Int32)e.KeyChar)
            {
                case 13:
                    {
                        SearchTextAtDataGridView(1, true, QuickSearchTextBox.Text);
                    }
                    e.Handled = true;
                    break;

                default:
                    {
                        //mainToolStripStatusLabel.Text = ((int)(e.KeyChar)).ToString();
                    }
                    break;
            }
        }

        private Boolean SearchTextAtDataGridView(Int32 step, Boolean searchType, String searchText)
        {
            Int32 rowIndex = dataGridView.CurrentRow.Index + step;
            if (Regex.Match(searchText, @"\/，").Success)
            {
                while (true)
                {
                    if (rowIndex >= dataGridView.RowCount) break;
                    if (rowIndex <= 0) break;
                    Match searchText_Match = Regex.Match(dataGridView.Rows[rowIndex].Cells[searchType ? 4 : 3].Value.ToString(), @"(.*)(.)(，)(.)(.*)");
                    if (searchText_Match.Success && searchText_Match.Groups[2].ToString() == searchText_Match.Groups[4].ToString())
                    {
                        dataGridView.CurrentCell = dataGridView.Rows[rowIndex].Cells[0];
                        (searchType ? textBox_ChsText : textBox_OriText).Select(searchText_Match.Groups[3].Index, 1);
                        (searchType ? textBox_ChsText : textBox_OriText).Focus();
                        return true;
                    }
                    rowIndex += step;
                }
            }
            else if (Regex.Match(searchText, @"\/艹飞对话句末的句号").Success)
            {
                foreach (GameText.Line thisLine in gameText.getAllAsList())
                {
                    Match searchText_Match = Regex.Match(thisLine.chsSentences, @"(.*)。$");
                    if (searchText_Match.Success && thisLine.oriName != "")
                    {
                        thisLine.chsSentences = searchText_Match.Groups[1].Value.ToString();
                    }
                }
                Reload_DataGridView();
            }
            else
            {
                while (true)
                {
                    if (rowIndex >= dataGridView.RowCount) break;
                    if (rowIndex <= 0) break;
                    Int32 indexInText = dataGridView.Rows[rowIndex].Cells[searchType ? 4 : 3].Value.ToString().IndexOf(searchText);
                    if (indexInText != -1)
                    {
                        dataGridView.CurrentCell = dataGridView.Rows[rowIndex].Cells[0];
                        (searchType ? textBox_ChsText : textBox_OriText).Select(indexInText, searchText.Length);
                        (searchType ? textBox_ChsText : textBox_OriText).Focus();
                        return true;
                    }
                    rowIndex += step;
                }
            }
            return false;
        }

        private void ReplaceTextAtDataGridView(String searchText, String replaceText)
        {
            if (textBox_ChsText.SelectedText == searchText || Regex.Match(searchText, @"\/，").Success)
            {
                textBox_ChsText.SelectedText = replaceText;
                textBox_ChsText.Select(textBox_ChsText.Text.IndexOf(replaceText), replaceText.Length);
                ChsTextBoxSubmit();
            }
            SearchTextAtDataGridView(1, true, searchText);
        }

        private void ChsTextBoxSubmit()
        {
            if (dataGridView.CurrentRow.DefaultCellStyle.BackColor == Color.LightGreen) textGCount--;
            else if (dataGridView.CurrentRow.DefaultCellStyle.BackColor == Color.Yellow) textYCount--;
            else textWCount--;
            gameText.setChsSentencesbyID(Int32.Parse(dataGridView.CurrentRow.Cells[0].Value.ToString()), textBox_ChsText.Text);
            dataGridView.CurrentRow.Cells[4].Value = gameText.getChsSentencesbyID(Int32.Parse(dataGridView.CurrentRow.Cells[0].Value.ToString()));
            if (状态着色ToolStripMenuItem.Checked)
            {
                switch (gameText.getLinebyID(Int32.Parse(dataGridView.CurrentRow.Cells[0].Value.ToString())).status)
                {
                    case 0: dataGridView.CurrentRow.DefaultCellStyle.BackColor = Color.LightGreen; textGCount++; break;
                    case 1: dataGridView.CurrentRow.DefaultCellStyle.BackColor = Color.Yellow; textYCount++; break;
                    case 2: textWCount++; break;
                    //default: thisRow.DefaultCellStyle.BackColor = Color.Black; break;
                }
            }
            do
            {
                dataGridView.CurrentCell = dataGridView.Rows[dataGridView.CurrentRow.Index + 1].Cells[0];
            } while (跳过已翻译文本ToolStripMenuItem.Checked && dataGridView.CurrentCellAddress.Y < dataGridView.Rows.Count - 1 && gameText.getLinebyID(Int32.Parse(dataGridView.Rows[dataGridView.CurrentRow.Index].Cells[0].Value.ToString())).status == 0);
            toolStripStatusLabel1.Text = "已翻：" + textGCount + " | 超长：" + textYCount + " | 未翻：" + textWCount;
        }

        private void buildBinFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_Waiting.Visible = true;
            panel_Waiting.Refresh();

            SaveFileDialog output_ltbin = new SaveFileDialog();
            output_ltbin.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            output_ltbin.FileName = "lt";
            output_ltbin.DefaultExt = ".bin";
            output_ltbin.Filter = "恋选PSP字库文件|lt.bin";

            SaveFileDialog output_scbin = new SaveFileDialog();
            output_scbin.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            output_scbin.FileName = "sc";
            output_scbin.DefaultExt = ".bin";
            output_scbin.Filter = "恋选PSP脚本文件|sc.bin";

            if (output_ltbin.ShowDialog() != DialogResult.OK || output_scbin.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("操作已取消");
                panel_Waiting.Visible = false;
                return;
            }

            CodeTable chsCodeTable = CodeTable.CodeTableFactory(GameText.UsedChsCharListFactory((gameText.getAllNot2AsList())));
            //Toolkit.buildTheTxtCodeTableFile(chsCodeTable);
            Toolkit.buildTheGameFile_lt_bin(chsCodeTable, output_ltbin.FileName, false);
            Toolkit.buildTheGameFile_sc_bin(gameText, chsCodeTable, output_scbin.FileName);

            panel_Waiting.Visible = false;

            MessageBox.Show(null, "生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            toolStripStatusLabel1.Text = "生成成功";
        }

        private void buildBinFile_cht_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_Waiting.Visible = true;
            panel_Waiting.Refresh();

            SaveFileDialog output_ltbin = new SaveFileDialog();
            output_ltbin.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            output_ltbin.FileName = "lt";
            output_ltbin.DefaultExt = ".bin";
            output_ltbin.Filter = "恋选PSP字库文件|lt.bin";

            SaveFileDialog output_scbin = new SaveFileDialog();
            output_scbin.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            output_scbin.FileName = "sc";
            output_scbin.DefaultExt = ".bin";
            output_scbin.Filter = "恋选PSP脚本文件|sc.bin";

            if (output_ltbin.ShowDialog() != DialogResult.OK || output_scbin.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("操作已取消");
                panel_Waiting.Visible = false;
                return;
            }

            CodeTable chsCodeTable = CodeTable.CodeTableFactory(GameText.UsedChsCharListFactory((gameText.getAllNot2AsList())));
            //Toolkit.buildTheTxtCodeTableFile(chsCodeTable);
            Toolkit.buildTheGameFile_lt_bin(chsCodeTable, output_ltbin.FileName, true);
            Toolkit.buildTheGameFile_sc_bin(gameText, chsCodeTable, output_scbin.FileName);

            panel_Waiting.Visible = false;

            MessageBox.Show(null, "生成成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            toolStripStatusLabel1.Text = "生成成功";
        }

        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = textBox_ChsText.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                textBox_ChsText.Font = fontDialog.Font;
            }
        }

        private void 自动保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (自动保存ToolStripMenuItem.Checked)
            {
                autoSaveTimer.Enabled = false;
                自动保存ToolStripMenuItem.Checked = false;
            }
            else
            {
                autoSaveTimer.Enabled = true;
                自动保存ToolStripMenuItem.Checked = true;
            }
        }

    }
}
