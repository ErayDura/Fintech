using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fintech.Denetim
{
    public partial class YevmiyeFisi : Form
    {
        public YevmiyeFisi()
        {
            InitializeComponent();
        }

        private void YevmiyeFisi_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = variables.mainDataTable;
            dataGridView1.TopLeftHeaderCell.Value = "←";
            
            //dataGridView1.Rows[0].Cells[4].
            //MessageBox.Show(dataGridView1.Rows[].Cells[4].Value.ToString());
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.TopLeftHeaderCell.Selected)
            {
                dataGridView1.DataSource = variables.mainDataTable;
            }
            string faturaNo;
            faturaNo = dataGridView1.CurrentCell.Value.ToString();
            DataTable yevmiyeTable = new DataTable();
            DataTable deneme = new DataTable();
            deneme = variables.mainDataTable;
            yevmiyeTable = variables.mainDataTable.Copy();
            if (dataGridView1.CurrentCell.ColumnIndex.Equals(4))
            {
                for (int i = 0; i < yevmiyeTable.Rows.Count; i++)
                {
                    yevmiyeTable.AcceptChanges();
                    if (faturaNo == yevmiyeTable.Rows[i]["FİŞ NO"].ToString())
                    {

                    }
                    else
                    {
                        yevmiyeTable.Rows[i].Delete();
                        i = i - 1;
                        yevmiyeTable.AcceptChanges();
                    }
                    //progressBar1.Value = i * progressBar1.Maximum / yevmiyeTable.Rows.Count;
                }
            }
            yevmiyeTable.AcceptChanges();
            dataGridView1.DataSource = yevmiyeTable;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
