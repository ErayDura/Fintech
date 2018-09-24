using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using ClosedXML.Excel;
namespace LogoGenel
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e)
		{

		}
		private DataTable GetTable(String tableName)
		{
			OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + "LOGO_GENEL MUHASEBE.XLSX" + "; Extended Properties='Excel 12.0 xml;HDR=YES;'");
			baglanti.Open();
			OleDbCommand sec = new OleDbCommand("SELECT * FROM [sayfa1$]", baglanti);
			OleDbDataAdapter adapter = new OleDbDataAdapter(sec);

			DataTable DTexcel = new DataTable();
			adapter.Fill(DTexcel);
			/*DTexcel.Columns[0].ColumnName = "Hesap Kodu";
			DTexcel.Columns[1].ColumnName = "Hesap Adı";
			DTexcel.Columns[5].ColumnName = "Tarih";*/


			//DTexcel.Columns["Hesap Kodu"].SetOrdinal(1);
			//DTexcel.Columns["Hesap Adı"].SetOrdinal(0);
			//DTexcel.Columns.Add("Deneme1", typeof(string)).SetOrdinal(0);
			//dataGridView1.DataSource = DTexcel;
			DTexcel.Columns.Add("Belge No", typeof(string)).SetOrdinal(3);
			DTexcel.Columns.Add("Unvan", typeof(string)).SetOrdinal(4);
			var reader = sec.ExecuteReader(CommandBehavior.SchemaOnly);
			var table = reader.GetSchemaTable();

			baglanti.Close();
			return DTexcel;
		}

		public void ExcelYaz(DataTable a)
		{
			var wb = new XLWorkbook();
			var dataTable = a;
			wb.Worksheets.Add(dataTable, "sayfa1");
			wb.SaveAs("1.xlsx");
			MessageBox.Show("Excele Yazıldı.");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			ExcelYaz(GetTable("a"));
		}
	
		private void button2_Click(object sender, EventArgs e)
		{
			//https://mcansozeri.wordpress.com/2011/02/24/c-sharp-string-methodlari-i-endswith-startswith-toupper-tolower-indexof-padleft-padright-remove/

			var sonuclar = GetTable("Information");
			
			for (int i = 0; i < sonuclar.Rows.Count; i++)
			{
				if (sonuclar.Rows[i][0].ToString().StartsWith("191"))
				{

					//sonuclar.Rows[i][3] = "a";
				}
				else
				{
					sonuclar.Rows[i].Delete();

				}
			}
			sonuclar.AcceptChanges();
			  for (int i = 0; i < sonuclar.Rows.Count; i++)
            {//Belge Seri No
                string Rowsplit = sonuclar.Rows[i][10].ToString();
                var items = Rowsplit.Split(' ');

                string a = items[1];
                if (a.StartsWith("A-"))
                {
                    sonuclar.Rows[i][5] = "A";
                }
                else if (a.StartsWith("B-"))
                {
                    sonuclar.Rows[i][5] = "B";
                }
                else
                {

                }
            }
	    sonuclar.AcceptChanges();
			for (int i = 0; i < sonuclar.Rows.Count; i++)
			{
				if (sonuclar.Rows[i][10].ToString().StartsWith("FİŞ"))
				{
					string Rowsplit = sonuclar.Rows[i][10].ToString();
					var items = Rowsplit.Split(':',' ');

					string a = items[1];
					sonuclar.Rows[i][3] = a;

					string Rowsplit2 = sonuclar.Rows[i][10].ToString();
					var items2 = Rowsplit2.Split(' ');

					for (int j = 1; j < items2.Count(); j++)
					{
						sonuclar.Rows[i][4] += items2[j]+" ";
					}
				}
				else if (sonuclar.Rows[i][10].ToString().StartsWith("FT:"))
				{
					string FirstSplit = sonuclar.Rows[i][10].ToString().Substring(3, sonuclar.Rows[i][10].ToString().Length-3);

					string Rowsplit2 = FirstSplit;
					var items2 = Rowsplit2.Split(' '); 

					sonuclar.Rows[i][3] = items2[0];
					for (int j = 1; j < items2.Count(); j++)
					{
						sonuclar.Rows[i][4] += items2[j] + " ";
					}
				}
				else if (sonuclar.Rows[i][10].ToString().StartsWith("FT"))
				{
					string FirstSubstring = sonuclar.Rows[i][10].ToString().Substring(2, sonuclar.Rows[i][10].ToString().Length - 2);//Kelimenin başından FT yi atar
					string Rowsplit2 = FirstSubstring.TrimStart();

					string SecondSubstring=FirstSubstring.TrimStart();
					string TestSubject="";

					TestSubject=SecondSubstring.Substring(4, SecondSubstring.Length-4);//Sadece sayılardan başlasın diye
					int position=0;
					for (int j = 0; j < TestSubject.Length; j++)
					{
						//char a=Convert.ToChar(TestSubject[j]);
						if (char.IsLetter(TestSubject[j]) || char.IsWhiteSpace(TestSubject[j]))
						{
							position = j;//ilk string olduğun yerin pozisyonunu tutar
							break;
						}
					}
					sonuclar.Rows[i][3] = SecondSubstring.Substring(0, position+4);//TestSubjecti olsutururken bastaki harfler gelmesin diye 3. konumdan başlatmıstım. o esiği +3 olarak ekliyoru.
					sonuclar.Rows[i][4] = SecondSubstring.Substring(position + 4,SecondSubstring.Length - (position + 4)).TrimStart();//Geri kalan kısım ünvan olduğu için onları da aldım.
					//sonuclar.Rows[i][5] = SecondSubstring;
					//var items2 = Rowsplit2.Split(' ');
								//sonuclar.Rows[i][3] = items2[0];
					//for (int j = 1; j < items2.Count(); j++)
					//{
					//	sonuclar.Rows[i][4] += items2[j] + " ";
					//}
				}
				else
				{
					sonuclar.Rows[i].Delete();
				}
			}
			//FİLTRELEME
			//for (int i = 0; i < sonuclar.Rows.Count; i++)
			//{
			//	string Rowsplit = sonuclar.Rows[i][10].ToString();
			//	var items = Rowsplit.Split(' ');

			//	string a = items[1];
			//	if (a.StartsWith("A"))
			//	{

			//	}
			//	else
			//	{
			//		sonuclar.Rows[i].Delete();
			//	}
			//}
			//sonuclar.Columns["Tarih"].SetOrdinal(2);
			sonuclar.AcceptChanges();
			//ExcelYaz(sonuclar);
			dataGridView1.DataSource = sonuclar;
		}
	}
}
