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
    public partial class NameForm : Form
    {
        NameList namelist;
        public NameForm(ref NameList namelist)
        {
            this.namelist = namelist;
            InitializeComponent();
        }

        private void NameForm_Load(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            foreach (NameList.Name thisOne in this.namelist.getAll_NOTNULL())
            {
                DataGridViewRow thisRow = new DataGridViewRow();
                thisRow.CreateCells(dataGridView, thisOne.NameID, thisOne.OriName, thisOne.ChsName);
                dataGridView.Rows.Add(thisRow);
            }
        }
    }
}
