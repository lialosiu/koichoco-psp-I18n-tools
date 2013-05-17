using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 恋选PSP文本处理器
{
    public delegate void ReplaceTextHandle(String searchText, String replaceText);
    public delegate Boolean SearchTextHandle(Int32 step, Boolean searchType, String searchText);

    public partial class SearchForm : Form
    {
        public event ReplaceTextHandle ReplaceText;
        public event SearchTextHandle SearchText;

        public SearchForm()
        {
            InitializeComponent();
        }

        private void buttonSearchPrevious_Click(object sender, EventArgs e)
        {
            SearchText(-1, radioButton2.Checked, textBox_SearchText.Text);
        }

        private void buttonSearchNext_Click(object sender, EventArgs e)
        {
            SearchText(1, radioButton2.Checked, textBox_SearchText.Text);
        }

        private void textBox_SearchText_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((Int32)e.KeyChar)
            {
                case 13:
                    {
                        SearchText(1, radioButton1.Checked, textBox_SearchText.Text);
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
        private void buttonReplaceText_Click(object sender, EventArgs e)
        {
            ReplaceText(textBox_SearchText.Text, textBox_ReplaceText.Text);
        }

    }
}
