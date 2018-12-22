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
	public partial class Adat : Form
	{
		public Adat()
		{
			InitializeComponent();
		}
		public double interest=10;
		List<string> DifferentCode;
		AutoCompleteStringCollection Codes;
		int count = 1;		
		private void button1_Click(object sender, EventArgs e)
		{
			if (textBoxHesapKodu.Text=="")
			{
				MessageBox.Show("Lütfen Bir Hesap Kodu Giriniz.!!");
			}
			else
			{
				Interest ınterest = new Interest();
				ınterest.İnterest(dataGridView1, textBoxHesapKodu, textBoxFaizOrani, textBoxAdatHarici, dateTimePicker1, dateTimePicker2);
			}		
		}

		private void button2_Click(object sender, EventArgs e)
		{
			textBoxFaizOrani.Text =Convert.ToString(interest);
			//textBox3.Text=dateTimePicker1.Value.ToShortDateString();			
		}
		
		private void Adat_Load(object sender, EventArgs e)
		{
			//Bu Kısımda Auto Complete Özelliği Çalışır
			Interest ınterest = new Interest();
			var sonuclar = ınterest.GetDataTable(dataGridView1);		
			DifferentCode = new List<string>();
			//Farklı Hesap Kodlarını Alıyorum.
			for (int j = 0; j < sonuclar.Rows.Count; j++)
			{
				if (DifferentCode.Contains(sonuclar.Rows[j]["Hesap Kodu"].ToString()))
				{

				}
				else
				{
					DifferentCode.Add(sonuclar.Rows[j]["Hesap Kodu"].ToString());

				}
			}
			Codes = new AutoCompleteStringCollection();
			Codes.AddRange(DifferentCode.ToArray());
			textBoxHesapKodu.AutoCompleteCustomSource = Codes;
		}

		private void buttonEkle_Click(object sender, EventArgs e)
		{
			//count = panel1.Controls.Count;
			DateTimePicker dateTimePicker = new DateTimePicker();
			dateTimePicker.Left = 0;
			dateTimePicker.Top = count;
			//dateTimePicker.Location = new Point(0, panel1.Controls.Count*15);
			panel1.Controls.Add(dateTimePicker);
			
			DateTimePicker dateTimePicker3 = new DateTimePicker();
			dateTimePicker3.Left = 218;
			dateTimePicker3.Top = count;
			//dateTimePicker3.Location = new Point(220, panel1.Controls.Count * 15);
			dateTimePicker3.Value=dateTimePicker3.Value.AddDays(+1);
			panel1.Controls.Add(dateTimePicker3);

			TextBox textBox = new TextBox();
			textBox.Left = 428;
			textBox.Top = count;
			panel1.Controls.Add(textBox);

			Button button = new Button();
			button.Left = 542;
			button.Top = count;
			button.Text = "TCBM Faiz Oranı";
			button.Width = Convert.ToInt32(button2.Width);
			//button.Height = Convert.ToInt32(button2.Height);
			panel1.Controls.Add(button);
			count =count+30;
		}
	}
}
