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
    public partial class HareketsizHesaplar : Form
    {
        public HareketsizHesaplar()
        {
            InitializeComponent();
        }

        private void HareketsizHesaplar_Load(object sender, EventArgs e)
        {
            DataTable datatable = new DataTable();
            datatable = variables.mainDataTable.Copy();

            datatable.Columns.Remove("YEMİYE TARİHİ");
            datatable.Columns.Remove("FİŞ TÜRÜ");
            datatable.Columns.Remove("FİŞ NO");
            datatable.Columns.Remove("AÇIKLAMA");
            datatable.Columns.Remove("BELGE SERİ");
            datatable.Columns.Remove("BELGE NO");
            datatable.Columns.Remove("ÜNVAN");
            datatable.Columns.Remove("TUTAR");
            datatable.Columns.Remove("İşlem Döviz Borç");
            datatable.Columns.Remove("İşlem Döviz Alacak");
            datatable.Columns.Remove("İşlem Döviz Tutar");
            datatable.Columns.Remove("İşlem Döviz Bakiye");
            datatable.Columns.Remove("BOŞ");
            datatable.Columns.Remove("BOŞŞ");
            datatable.Columns.Remove("Döviz Adı");
            datatable.Columns.Remove("Döviz Kur");
            datatable.Columns.Remove("BELGE TARİHİ");


            datatable.AcceptChanges();
            GC.Collect();

            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                string a = datatable.Rows[i]["ALACAK"].ToString();
                string b = datatable.Rows[i]["BORÇ"].ToString();
                if (a == "0,00" && b == "0,00")
                {

                }
                else
                {
                    datatable.Rows[i].Delete();
                    i = i - 1;
                }
                datatable.AcceptChanges();
            }
            dataGridView1.DataSource = datatable;

        }
    }
}
