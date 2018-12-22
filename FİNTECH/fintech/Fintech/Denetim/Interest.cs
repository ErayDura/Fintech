using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fintech
{
	class Interest
	{
		public Interest()
		{			
			
		}
		public void İnterest(DataGridView dataGrid, TextBox textBoxHesapKodu, TextBox textBoxFaizOrani, TextBox textBoxAdatHarici, DateTimePicker dateTimePicker1, DateTimePicker dateTimePicker2)
		{
			var sonuclar = GetDataTable(dataGrid);
			BeginNumber(sonuclar, textBoxHesapKodu.Text, dateTimePicker1, dateTimePicker2);
			try
			{
				Calculate(sonuclar, textBoxFaizOrani.Text, textBoxAdatHarici.Text, dateTimePicker1, dateTimePicker2);
			}
			catch (Exception)
			{

			}
			//excel.ExcelWritten(sonuclar, "Adat");
			dataGrid.DataSource = sonuclar;
		}
		public DataTable GetDataTable(DataGridView dataGrid)
		{
			LogoAccountingFront logoAccountingFront=new LogoAccountingFront();
			
			variables.mainDataTable = logoAccountingFront.GetTable("");

			variables.mainDataTable.Columns["HESAP KODU"].SetOrdinal(0);
			variables.mainDataTable.Columns["YEVMİYE TARİHİ"].SetOrdinal(1);
			variables.mainDataTable.Columns["BORÇ"].SetOrdinal(2);
			variables.mainDataTable.Columns["ALACAK"].SetOrdinal(3);
			variables.mainDataTable.Columns.Add("BORÇ BAKİYE", typeof(string)).SetOrdinal(4);
			variables.mainDataTable.Columns.Add("ALACAK BAKİYE", typeof(string)).SetOrdinal(5);
			variables.mainDataTable.Columns.Add("ADAT HARİCİ TUTAR", typeof(string)).SetOrdinal(6);
			variables.mainDataTable.Columns["İŞLEM Döviz BAKİYE"].SetOrdinal(7);//İşlem Görecek Tutar
			variables.mainDataTable.Columns.Add("ADAT GÜN", typeof(string)).SetOrdinal(8);
			variables.mainDataTable.Columns.Add("BORÇ FAİZ", typeof(string)).SetOrdinal(9);
			variables.mainDataTable.Columns.Add("ALACAK FAİZ", typeof(string)).SetOrdinal(10);
			//variables.mainDataTable.Columns["Hesap Kodu"].ColumnMapping = MappingType.Hidden;//Hesap Kodu Gizleme
			//Gereksiz Sütunların Silinmesi
			variables.mainDataTable.Columns.Remove("HESAP ADI");
			variables.mainDataTable.Columns.Remove("FİŞ TÜRÜ");
			variables.mainDataTable.Columns.Remove("FİŞ NO");
			variables.mainDataTable.Columns.Remove("AÇIKLAMA");			
			variables.mainDataTable.Columns.Remove("BAKİYE");
			variables.mainDataTable.Columns.Remove("TUTAR");
			variables.mainDataTable.Columns.Remove("İŞLEM DÖVİZ BORÇ");
			variables.mainDataTable.Columns.Remove("İŞLEM DÖVİZ ALACAK");
			variables.mainDataTable.Columns.Remove("İŞLEM DÖVİZ TUTAR");		
			variables.mainDataTable.Columns.Remove("FİRMA DÖVİZ");
			variables.mainDataTable.Columns.Remove("DÖVİZ ADI");
			variables.mainDataTable.Columns.Remove("DÖVİZ KUR");
			variables.mainDataTable.Columns.Remove("BELGE SERİ NO");
			variables.mainDataTable.Columns.Remove("BELGE NO");
			variables.mainDataTable.Columns.Remove("ÜNVAN");
			variables.mainDataTable.Columns.Remove("BELGE TARİHİ");
			variables.mainDataTable.Columns.Remove("SR");

			
			return variables.mainDataTable;
		}		
		void BeginNumber(DataTable sonuclar, string textBoxHesapKodu, DateTimePicker dateTimePicker1, DateTimePicker dateTimePicker2)
		{
			for (int i = 0; i < sonuclar.Rows.Count; i++)
			{
				if (sonuclar.Rows[i]["HESAP KODU"].ToString().StartsWith(textBoxHesapKodu))
				{
					//number ile başlıyorsa al başlamıyorsa sil
					//Girilen Tarihler arasında ki değerleri alır.				
					DateFilter(sonuclar, i, dateTimePicker1, dateTimePicker2);						
				}
				else
				{
					sonuclar.Rows[i].Delete();
				}
			}
			sonuclar.AcceptChanges();
		}
		void Calculate(DataTable sonuclar, string textBoxFaizOrani, string textBoxAdatHarici,DateTimePicker dateTimePicker1,DateTimePicker dateTimePicker2)
		{
			sonuclar.Rows.Add("Sonuçlar", dateTimePicker2.Value.ToShortDateString(), 0, 0, 0, 0, 0, 0, 0, 0);
			for (int i = 0; i < sonuclar.Rows.Count; i++)
			{			
				//İşlem Döviz Bakiye Borç bakiyeye atma
				sonuclar.Rows[i]["BORÇ BAKİYE"] = sonuclar.Rows[i]["İŞLEM Döviz BAKİYE"];
				//Adat Harici Turarını Azami Bakiye Sutununa Aktarıyorum.
				sonuclar.Rows[i]["ADAT HARİCİ TUTAR"] = textBoxAdatHarici;
			
				//Adat Günü Hesaplamak için Kullanılır.
				CalculateDay(sonuclar,i,dateTimePicker2);				
				//Eğer Borç Bakiye - ile başlıyorsa alacaktır.
				if (sonuclar.Rows[i]["BORÇ BAKİYE"].ToString().StartsWith("-"))
				{
					sonuclar.Rows[i]["ALACAK BAKİYE"] = sonuclar.Rows[i]["BORÇ BAKİYE"].ToString().Substring(1, sonuclar.Rows[i]["Borç BAKİYE"].ToString().Length-1);
					sonuclar.Rows[i]["BORÇ BAKİYE"] = 0;
				}
				else
				{
					sonuclar.Rows[i]["ALACAK BAKİYE"] = 0;
				}
				//İşlem Döviz Bakiyelerden - leri atmak için
				if (sonuclar.Rows[i]["İŞLEM Döviz BAKİYE"].ToString().StartsWith("-"))
				{
					sonuclar.Rows[i]["İŞLEM Döviz BAKİYE"] = sonuclar.Rows[i]["İŞLEM Döviz BAKİYE"].ToString().Substring(1, sonuclar.Rows[i]["İŞLEM Döviz BAKİYE"].ToString().Length - 1);				
				}
				//Borç Faizi Hesaplamak için kullanılır.
				CalculateDebtInterest(sonuclar, i, textBoxFaizOrani);			
			}
		}
		void CalculateDay(DataTable sonuclar,int i,DateTimePicker dateTimePicker2)
		{
			//Hata var kontrol et Bütün Listeyi göremiyorsun !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			if (sonuclar.Rows.Count-1 != i)
			{
				DateTime beginDate = Convert.ToDateTime(sonuclar.Rows[i]["YEVMİYE TARİHİ"]);
				DateTime finishDate = Convert.ToDateTime(sonuclar.Rows[i + 1]["YEVMİYE TARİHİ"]);

				TimeSpan remainingDay = finishDate - beginDate;//Sonucu zaman olarak döndürür
				double totalDay = remainingDay.TotalDays;// kalanGun den TotalDays ile sadece toplam gun değerini çekiyoruz. 
				sonuclar.Rows[i]["ADAT GÜN"] = Convert.ToString(totalDay);
				sonuclar.Rows[sonuclar.Rows.Count-1]["ADAT GÜN"] = 0;
			}
		}
		void CalculateDebtInterest(DataTable sonuclar, int i,string textBoxFaizOrani)
		{
			double borcHesap = 0,alcakHesap=0;
			if (sonuclar.Rows[i]["BORÇ BAKİYE"].ToString() !="0")
			{
				//Adat harici borçtan büyükse faiz hesaplanmaz
				if (Convert.ToDouble(sonuclar.Rows[i]["ADAT HARİCİ TUTAR"])> Convert.ToDouble(sonuclar.Rows[i]["BORÇ BAKİYE"]))
				{
					sonuclar.Rows[i]["BORÇ FAİZ"] = 0;
				}
				else
				{
					borcHesap = Convert.ToDouble(sonuclar.Rows[i]["BORÇ BAKİYE"]) - Convert.ToDouble(sonuclar.Rows[i]["ADAT HARİCİ TUTAR"]);
					sonuclar.Rows[i]["İŞLEM Döviz BAKİYE"] = borcHesap;
					//İşlem Döviz Bakiye*Adat Gün*Faiz/36500
					sonuclar.Rows[i]["BORÇ FAİZ"] = Convert.ToDouble(sonuclar.Rows[i]["İŞLEM Döviz BAKİYE"]) * Convert.ToDouble(sonuclar.Rows[i]["ADAT GÜN"]) * Convert.ToDouble(textBoxFaizOrani) / 36500;
				}
			}
			else
			{
				//Adat harici alacaktan büyükse faiz hesaplanmaz
				if (Convert.ToDouble(sonuclar.Rows[i]["ADAT HARİCİ TUTAR"]) > Convert.ToDouble(sonuclar.Rows[i]["ALACAK BAKİYE"]))
				{
					sonuclar.Rows[i]["ALACAK FAİZ"] = 0;
				}
				else
				{
					alcakHesap = Convert.ToDouble(sonuclar.Rows[i]["ALACAK BAKİYE"]) - Convert.ToDouble(sonuclar.Rows[i]["ADAT HARİCİ TUTAR"]);
					sonuclar.Rows[i]["İŞLEM Döviz BAKİYE"] = alcakHesap;
					//İşlem Döviz Bakiye*Adat Gün*Faiz/36500					
					sonuclar.Rows[i]["ALACAK FAİZ"] = Convert.ToDouble(sonuclar.Rows[i]["İŞLEM Döviz BAKİYE"]) * Convert.ToDouble(sonuclar.Rows[i]["ADAT GÜN"]) * Convert.ToDouble(textBoxFaizOrani) / 36500;
				}					
			}				
		}
		void DateFilter(DataTable sonuclar, int i, DateTimePicker dateTimePicker1, DateTimePicker dateTimePicker2)
		{
			//dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);
			if (Convert.ToDateTime(sonuclar.Rows[i]["YEVMİYE TARİHİ"].ToString()) >= dateTimePicker1.Value.AddDays(-1) && Convert.ToDateTime(sonuclar.Rows[i]["YEVMİYE TARİHİ"].ToString()) <= dateTimePicker2.Value)
			{
				//Bu tarihler arasındaysa al değilse sil
			}
			else
			{
				sonuclar.Rows[i].Delete();
			}
		}
	}
}
