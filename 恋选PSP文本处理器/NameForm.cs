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
        GameText gameText;
        Dictionary<String, List<Int32>> nameMap = new Dictionary<String, List<Int32>>();

        public NameForm(GameText gameText)
        {
            this.gameText = gameText;
            foreach (GameText.Line thisLine in gameText.getAllAsList())
            {
                if (thisLine.oriName == "") continue;
                if (nameMap.ContainsKey(thisLine.oriName))
                {
                    nameMap[thisLine.oriName].Add(thisLine.id);
                }
                else
                {
                    List<Int32> newList = new List<Int32>();
                    newList.Add(thisLine.id);
                    nameMap.Add(thisLine.oriName, newList);
                }
            }
            InitializeComponent();
        }

        private void NameForm_Load(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            foreach (var thisName in this.nameMap)
            {
                DataGridViewRow thisRow = new DataGridViewRow();
                thisRow.CreateCells(dataGridView, thisName.Key, gameText.getChsNamebyID(thisName.Value[0]));
                if (thisRow.Cells[0].Value.ToString().Length < thisRow.Cells[1].Value.ToString().Length)
                {
                    thisRow.DefaultCellStyle.BackColor = Color.Yellow;
                }
                else
                {
                    if (thisRow.Cells[0].Value.ToString() == thisRow.Cells[1].Value.ToString())
                    {
                        thisRow.DefaultCellStyle.BackColor = Color.White;
                    }
                    else
                    {
                        thisRow.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                }
                dataGridView.Rows.Add(thisRow);
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            List<Int32> thisNameIDList = nameMap[dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString()];

            foreach (Int32 thisID in thisNameIDList)
            {
                gameText.setChsNamebyID(thisID, dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString());
            }

            if (dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString().Length < dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString().Length)
            {
                dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
            }
            else
            {

                if (dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString() == dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString())
                {
                    dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    dataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
        }
    }
}
