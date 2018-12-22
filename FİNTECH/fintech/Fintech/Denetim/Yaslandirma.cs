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
    public partial class Yaslandirma : Form
    {
        public Yaslandirma()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Value.ToShortDateString();
            DataTable dtable = new DataTable();
            dtable = variables.mainDataTable.Copy();
        }
    }
}
