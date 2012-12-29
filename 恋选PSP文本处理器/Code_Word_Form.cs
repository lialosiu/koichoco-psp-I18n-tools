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
    public partial class Code_Word_Form : Form
    {
        public Code_Word_Form(CodeTable _list)
        {
            InitializeComponent();
            for (Int32 i = 0; i < 0xffff; i++)
            {
                Int32[] temp = { i / 0x100, i % 0x100 };
                if (_list.getWord(i) != '\0') Code_Word_ListBox.Items.Add(temp[1].ToString("X2") + temp[0].ToString("X2") + " = " + _list.getWord(i));
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
