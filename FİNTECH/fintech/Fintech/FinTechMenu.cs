using Fintech.Denetim;
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
    public partial class FinTechMenu : Form
    {
        public FinTechMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Adat adat = new Adat();
            adat.Show();
        }


        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            //file.Filter = "Excel Dosyası |*.xlsx";
            file.ShowDialog();
            string filePath = file.FileName;
            variables.filePath = filePath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                DataGridView datagrid = new DataGridView();
                //Luca luca = new Luca(datagrid);


                UtilityClass uObject = new UtilityClass();
                var connection = uObject.CreateOleDbConnection();
                var mainDataTable = uObject.ExcelToDataTable(connection);
                var lucaObject = uObject.ClassSwitcherRelatedToColumnNumber(mainDataTable, datagrid, connection);
                mainDataTable = lucaObject.DataTableForNColumn();
                uObject.SetMainDataTableVariable(mainDataTable);




            }
            else
            {
            }
            YevmiyeFisi yevmiyeFisi = new YevmiyeFisi();
            yevmiyeFisi.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                DataGridView datagrid = new DataGridView();
                UtilityClass uObject = new UtilityClass();
                var connection = uObject.CreateOleDbConnection();
                var mainDataTable = uObject.ExcelToDataTable(connection);
                var lucaObject = uObject.ClassSwitcherRelatedToColumnNumber(mainDataTable, datagrid, connection);
                mainDataTable = lucaObject.DataTableForNColumn();
                uObject.SetMainDataTableVariable(mainDataTable);

            }
            else
            {
            }
            HareketsizHesaplar hareketsizhesaplar = new HareketsizHesaplar();
            hareketsizhesaplar.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                DataGridView datagrid = new DataGridView();
                UtilityClass uObject = new UtilityClass();
                var connection = uObject.CreateOleDbConnection();
                var mainDataTable = uObject.ExcelToDataTable(connection);
                var lucaObject = uObject.ClassSwitcherRelatedToColumnNumber(mainDataTable, datagrid, connection);
                mainDataTable = lucaObject.DataTableForNColumn();
                uObject.SetMainDataTableVariable(mainDataTable);

            }
            else
            {
            }
            Yaslandirma yaslandirma = new Yaslandirma();
            yaslandirma.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
            DataGridView datagrid = new DataGridView();
            Netsis netsis = new Netsis(datagrid);
            netsis.netsis(datagrid);
            
            }
            else
            {
            }
            SorunluFis sorunlufis = new SorunluFis();
            sorunlufis.Show();
        }
    }
}
