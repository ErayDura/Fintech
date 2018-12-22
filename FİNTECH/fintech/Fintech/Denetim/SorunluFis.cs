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
    public partial class SorunluFis : Form
    {

        public SorunluFis()
        {
            InitializeComponent();
        }

        private void SorunluFis_Load(object sender, EventArgs e)
        {
            DataTable datatable = new DataTable();
            datatable = variables.mainDataTable.Copy();
            Netsis netsis = new Netsis(dataGridView1);
            netsis.netsis(dataGridView1);
            datatable.Columns.Remove("FİŞ TÜRÜ");
            datatable.Columns.Remove("BELGE SERİ");
            datatable.Columns.Remove("BELGE NO");
            datatable.Columns.Remove("ÜNVAN");
            //datatable.Columns.Remove("TUTAR");
            datatable.Columns.Remove("İşlem Döviz Tutar");
            datatable.Columns.Remove("Döviz Kur");
            //datatable.Columns.Remove("HESAP KODU");
            //datatable.Columns.Remove("HESAP ADI");
            datatable.Columns.Remove("BAKİYE");
            datatable.Columns.Remove("Firma Döviz");
            datatable.AcceptChanges();
            GC.Collect();

            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                string a = datatable.Rows[i]["HESAP KODU"].ToString();
                string b = datatable.Rows[i]["BORÇ"].ToString();
                string c = datatable.Rows[i]["ALACAK"].ToString();
                if (a.StartsWith("331") && c == "0" && b != "0")
                {
                    
                  
                }
                else if (a.StartsWith("320") && c == "0" && b != "0")
                {

                }
                else if (a.StartsWith("431") && c == "0" && b != "0")
                {

                }
                else if (a.StartsWith("120") && c == "0" && b != "0")
                {

                }
                else if (a.StartsWith("100") && c == "0" && b != "0")
                {

                }
                else if (a.StartsWith("131") && c == "0" && b != "0")
                {

                }
                else if (a.StartsWith("100") && b == "0" && c != "0")
                {

                }
                else if (a.StartsWith("") && b == "0" && c != "0")
                {

                }
                else if (a.StartsWith("159") && b == "0" && c != "0")
                {

                }
                else if (a.StartsWith("131") && b == "0" && c != "0")
                {

                }
                else if (a.StartsWith("101") && b == "0" && c != "0")
                {

                }
                else if (a.StartsWith("195") && b == "0" && c != "0")
                {

                }
                else if (a.StartsWith("120") && b == "0" && c != "0")
                {

                }
                else if (a.StartsWith("320") && b == "0" && c != "0")
                {

                }
                else if (a.StartsWith("331") && b == "0" && c != "0")
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

