using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fintech
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			OpenFileDialog file = new OpenFileDialog();
			//file.Filter = "Excel Dosyası |*.xlsx";
			file.ShowDialog();
			string filePath = file.FileName;
			variables.filePath = filePath;
			label1.Text = filePath;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex == 0)
			{
                Netsis netsis = new Netsis(dataGridView1);
                netsis.netsis(dataGridView1);
			}
			if (comboBox1.SelectedIndex == 1)
			{
                UtilityClass uObject = new UtilityClass();
                var connection = uObject.CreateOleDbConnection();
                var mainDataTable = uObject.ExcelToDataTable(connection);
                var lucaObject = uObject.ClassSwitcherRelatedToColumnNumber(mainDataTable, dataGridView1, connection);
                mainDataTable = lucaObject.DataTableForNColumn();
                uObject.SetMainDataTableVariable(mainDataTable);
            }
			if (comboBox1.SelectedIndex == 2)
			{
				LogoAccountingFront logoAccounting = new LogoAccountingFront();
				logoAccounting.logoAccountingFront(dataGridView1);
			}
			if (comboBox1.SelectedIndex == 3)
			{
				LogoGeneral logoGeneral = new LogoGeneral(dataGridView1);
			}
            GC.Collect();
            
            label3.Text = "Firma Adı : " + variables.FirmaAdi;
            
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Adat adat = new Adat();
			adat.Show();	
		}

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
