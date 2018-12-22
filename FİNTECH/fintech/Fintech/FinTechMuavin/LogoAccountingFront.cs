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
	class LogoAccountingFront
	{
		public LogoAccountingFront()
		{
			
		}
		//Herşey Burda Çalışır.
		public void logoAccountingFront(DataGridView dataGrid)
		{
			ExcelWrite excel = new ExcelWrite();
			var sonuclar = GetTable("Information");

			BeginNumber(sonuclar, "191");
			CommentSplit(sonuclar);

			//excel.ExcelWritten(sonuclar, "Logo Ön Modül");
			dataGrid.DataSource = sonuclar;
		}
		//Tablo Olusturulur.
		public DataTable GetTable(String tableName)
		{
			OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + variables.filePath + "; Extended Properties='Excel 12.0 xml;HDR=YES;'");
			baglanti.Open();
			OleDbCommand sec = new OleDbCommand("SELECT * FROM [sayfa1$]", baglanti);
			OleDbDataAdapter adapter = new OleDbDataAdapter(sec);

			DataTable DTexcel = new DataTable();
			adapter.Fill(DTexcel);
			//Sütunlara isim verme
			DTexcel.Columns[0].ColumnName = "HESAP KODU";
			DTexcel.Columns[1].ColumnName = "HESAP ADI";
			DTexcel.Columns[5].ColumnName = "YEVMİYE TARİHİ";
			DTexcel.Columns[6].ColumnName = "FİŞ TÜRÜ";
			DTexcel.Columns[7].ColumnName = "FİŞ NO";
			DTexcel.Columns[8].ColumnName = "AÇIKLAMA";
			DTexcel.Columns[9].ColumnName = "BORÇ";
			DTexcel.Columns[10].ColumnName = "ALACAK";
			DTexcel.Columns[11].ColumnName = "BAKİYE";
			DTexcel.Columns[18].ColumnName = "İŞLEM DÖVİZ BAKİYE";
			DTexcel.Columns[16].ColumnName = "DÖVİZ KUR";
			DTexcel.Columns[15].ColumnName = "DÖVİZ ADI";
			DTexcel.Columns[17].ColumnName = "İŞLEM DÖVİZ TUTAR";
			//İstenilen Sıraya Koyma
			DTexcel.Columns["YEVMİYE TARİHİ"].SetOrdinal(2);
			DTexcel.Columns["FİŞ TÜRÜ"].SetOrdinal(3);
			DTexcel.Columns["FİŞ NO"].SetOrdinal(4);

			DTexcel.Columns["AÇIKLAMA"].SetOrdinal(5);//8-10 oldu
			DTexcel.Columns["BORÇ"].SetOrdinal(6);//9-11 oldu
			DTexcel.Columns["ALACAK"].SetOrdinal(7);//10-12 oldu
			DTexcel.Columns["BAKİYE"].SetOrdinal(8);
			DTexcel.Columns.Add("TUTAR", typeof(string)).SetOrdinal(9);

			DTexcel.Columns.Add("İŞLEM DÖVİZ BORÇ", typeof(string)).SetOrdinal(10);
			DTexcel.Columns.Add("İŞLEM DÖVİZ ALACAK", typeof(string)).SetOrdinal(11);

			DTexcel.Columns["İŞLEM DÖVİZ TUTAR"].SetOrdinal(12);
			DTexcel.Columns["İŞLEM DÖVİZ BAKİYE"].SetOrdinal(13);
			DTexcel.Columns.Add("FİRMA DÖVİZ", typeof(string)).SetOrdinal(14);
			DTexcel.Columns["DÖVİZ ADI"].SetOrdinal(15);//19-20 oldu
			DTexcel.Columns["DÖVİZ KUR"].SetOrdinal(16);//20-21oldu

			DTexcel.Columns.Add("BELGE SERİ NO", typeof(string)).SetOrdinal(17);//5 -6 oldu		
			DTexcel.Columns.Add("BELGE NO", typeof(string)).SetOrdinal(18);//6-8 oldu
			DTexcel.Columns.Add("ÜNVAN", typeof(string)).SetOrdinal(19);//7-9 oldu
			DTexcel.Columns.Add("BELGE TARİHİ", typeof(string)).SetOrdinal(20);
			DTexcel.Columns.Add("SR", typeof(string)).SetOrdinal(21);

			//Gereksiz Sütunların atılması
			DTexcel.Columns.Remove("F3");
			DTexcel.Columns.Remove("F4");
			DTexcel.Columns.Remove("F5");
			DTexcel.Columns.Remove("F13");
			DTexcel.Columns.Remove("F14");
			DTexcel.Columns.Remove("F15");
			DTexcel.Columns.Remove("F20");
			DTexcel.Columns.Remove("F21");
			DTexcel.Columns.Remove("F22");
			//DTexcel.Columns.Remove("F23");
			var reader = sec.ExecuteReader(CommandBehavior.SchemaOnly);
			var table = reader.GetSchemaTable();

			baglanti.Close();
			return DTexcel;
		}
		//Başlangıç Numarası Belirlenir.
		void BeginNumber(DataTable sonuclar, string number)
		{
			for (int i = 0; i < sonuclar.Rows.Count; i++)
			{
				if (sonuclar.Rows[i][0].ToString().StartsWith(number))
				{
					//191 ile başlıyorsa al başlamıyorsa sil
				}
				else
				{
					sonuclar.Rows[i].Delete();
				}
			}
			sonuclar.AcceptChanges();
		}
		//Açıklama belge no ve unvana ayrılır.
		void CommentSplit(DataTable sonuclar)
		{
			for (int i = 0; i < sonuclar.Rows.Count; i++)
			{
				//Bazen açıklama küçük harf içerdiği için hepsini büyütüyorum.
				sonuclar.Rows[i]["AÇIKLAMA"] = sonuclar.Rows[i]["AÇIKLAMA"].ToString().ToUpper();
				//Açıklama Split Etme
				string Rowsplit = sonuclar.Rows[i]["AÇIKLAMA"].ToString();
				var items = Rowsplit.Split(',');
				//a,b,c virgüllere göre parçalama yapıyorum.
				//Ünvan alma
				string swapForTitle = "";
				//virgül sayısı 6 ise
				if (items.Count() > 6)
				{
					swapForTitle = items[items.Count() - 4];// Bir nevi swap işlemi uyguluyorum. swapForTitle değişkeni ile
															//Bazı açıklamalarda belgeno, bla ,bla,Kasa/Fiş+belge no+ ünvan,bla ,bla
															//yazdığı için ordaki Kasa/Fiş+belge no ları atıp sadece ünvanlara erişmek istiyorum
					sonuclar.Rows[i]["ÜNVAN"] = swapForTitle;//Benim ünvanlarım sondan 4. virgülün içinde olduğu için onları
															 //direkt alıp ünvan bölmesine atıyorum.
				}
				//5 ise
				if (items.Count() == 6)
				{
					swapForTitle = items[items.Count() - 3];// Bir nevi swap işlemi uyguluyorum. swapForTitle değişkeni ile
															//Bazı açıklamalarda belgeno, bla ,bla,Kasa/Fiş+belge no+ ünvan,bla ,bla
															//yazdığı için ordaki Kasa/Fiş+belge no ları atıp sadece ünvanlara erişmek istiyorum
					sonuclar.Rows[i]["ÜNVAN"] = swapForTitle;//Benim ünvanlarım sondan 4. virgülün içinde olduğu için onları
															 //direkt alıp ünvan bölmesine atıyorum.
				}
				else if (items.Count() == 5)
				{
					//Bazı durumlarda sadece Belge No,Unvan gelir.
					sonuclar.Rows[i]["BELGE NO"] = items[0];
					for (int j = 1; j < items.Count(); j++)
					{
						sonuclar.Rows[i]["ÜNVAN"] += items[j] + " ";
					}
				}
				//3-4 ise
				else if (items.Count() > 2 && items.Count() < 5)
				{
					//Bazı durumlarda sadece Belge No,Unvan gelir.
					sonuclar.Rows[i]["BELGE NO"] = items[0];
					for (int j = 1; j < items.Count(); j++)
					{
						sonuclar.Rows[i]["ÜNVAN"] += items[j] + " ";
					}
				}
				//2 ise
				else if (items.Count() == 2)
				{
					//Bazı durumlarda sadece Belge No,Unvan gelir.					
					sonuclar.Rows[i]["ÜNVAN"] = items[0] + items[1];
				}
				//virgül yoksa veya 1 ise
				else if (items.Count() == 1)
				{
					sonuclar.Rows[i]["ÜNVAN"] = items[0];
				}
				//Belge No alma
				string documentNumber = items[0];//Bir nevi swap işlemi uyguluyorum. documentNumber değişkeni ile
												 //yukarıda yapmış olduğum virgüllere göre bölme işleminden ilk böldüğü yeri alıyorum 
												 //items[0] diyerek
												 //FT FİŞ gibi başladığı durumları almak için
				if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().StartsWith("FT:"))
				{
					BillType(sonuclar, i);
				}
				else if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().StartsWith("FT"))
				{
					BillType2(sonuclar, i);
				}
				else if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().StartsWith("FİŞ"))
				{
					voucherType(sonuclar, i);
				}
				else if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().Contains("Devreden Durum") || sonuclar.Rows[i]["AÇIKLAMA"].ToString().Contains("KDV TAHAKKUKU") || sonuclar.Rows[i]["AÇIKLAMA"].ToString().Contains("KDV VİRMAN") || (sonuclar.Rows[i]["AÇIKLAMA"].ToString().Contains("Mali Yılı")))
				{
					//Bunların silinmemesi için koşul yazıyoruz.
					sonuclar.Rows[i]["ÜNVAN"] = "";
					sonuclar.Rows[i]["BELGE NO"] = "";
				}
				else if (documentNumber.Contains("-"))
				{
					//Belge No alma
					//A-1515144 gibi terimler olduğu için items2[1]:belge no items2[0]: Belge Seri no lar bulunur.
					var items2 = documentNumber.Split('-');
					sonuclar.Rows[i]["BELGE NO"] = items2[1];
					//Belge seri no alma
					//Eğer bu 4 den küçükse bu bir belge seri no dur.
					if (items2[0].Count() < 4)
					{
						sonuclar.Rows[i]["BELGE SERİ NO"] = items2[0];
					}
				}
				//Bazı durumlarda birinci virgül ayırma kısmında belge no dan önce tarih yazabilir.
				//tarihi almamak için / split ediyorum. Belge no ya atıyorum.
				else if (documentNumber.Contains("/"))
				{
					var items2 = documentNumber.Split('/');
					sonuclar.Rows[i]["BELGE NO"] = items2[1];
				}
				else
				{
					sonuclar.Rows[i]["BELGE NO"] = documentNumber;
					//Bir belge no 8 den küçükse seri no su vardır o yüzden kontrol ediyorum.
					if (sonuclar.Rows[i]["BELGE NO"].ToString().Length <= 8)
					{
						string control = sonuclar.Rows[i]["BELGE NO"].ToString();
						int count = 0;
						foreach (char c in control)
						{
							if (char.IsLetter(c))
							{
								sonuclar.Rows[i]["BELGE SERİ NO"] += Convert.ToString(c);//Bulduğum seri noları seri no sütununa atıyorum.
								count++;//Belge nolardan seri noları atmak için
							}
							else if (char.IsDigit(c))
							{
								// İçerisinde sayılar
							}
						}
						//Belge no dan seri noları atıyorum.
						string remove = sonuclar.Rows[i]["BELGE NO"].ToString();
						sonuclar.Rows[i]["BELGE NO"] = remove.Substring(count, (sonuclar.Rows[i]["BELGE NO"].ToString().Length - count));
						if (sonuclar.Rows[i]["BELGE NO"].ToString().StartsWith("-"))
						{
							sonuclar.Rows[i]["BELGE NO"] = remove.Substring(count + 1, (sonuclar.Rows[i]["BELGE NO"].ToString().Length - count - 1));
						}
					}
				}
				//İ. DÖVİZ - ile başlıyorsa - kolonuna
				if (sonuclar.Rows[i]["İŞLEM DÖVİZ TUTAR"].ToString().StartsWith("-"))
				{
					sonuclar.Rows[i]["İŞLEM DÖVİZ ALACAK"] = sonuclar.Rows[i]["İŞLEM DÖVİZ TUTAR"].ToString().Substring(1, (sonuclar.Rows[i]["İŞLEM DÖVİZ TUTAR"].ToString().Count() - 1));
					sonuclar.Rows[i]["DÖVİZ KUR"] = Convert.ToDouble(sonuclar.Rows[i]["ALACAK"]) / Convert.ToDouble(sonuclar.Rows[i]["İŞLEM DÖVİZ ALACAK"]);//DÖVİZ kuru hesaplama
				}
				//+ ile başlıyorsa + kolonuna
				else
				{
					sonuclar.Rows[i]["İŞLEM DÖVİZ BORÇ"] = sonuclar.Rows[i]["İŞLEM DÖVİZ TUTAR"];//9 Borç
					sonuclar.Rows[i]["DÖVİZ KUR"] = Convert.ToDouble(sonuclar.Rows[i]["BORÇ"]) / Convert.ToDouble(sonuclar.Rows[i]["İŞLEM DÖVİZ BORÇ"]);//DÖVİZ kuru hesaplama
				}
				//Eğer Belge No 15 ise uyarı vercek.
				if (sonuclar.Rows[i]["BELGE NO"].ToString().Length == 15)
				{
					//uyarı vericek.
				}
				//Tutar sütununa borç- alacak
				sonuclar.Rows[i]["TUTAR"] = Convert.ToDouble(sonuclar.Rows[i]["BORÇ"]) - Convert.ToDouble(sonuclar.Rows[i]["ALACAK"]);
				//Bu fonksiyonu elimde kalan yanlış ünvanları düzeltmek için yazdım.
				TitleControl(sonuclar, i, items);
				//Ünvan İçerisinde NOLU yazıyorsa NOLU yazısından sonrasını alıyorum.
				var splitTitle = sonuclar.Rows[i]["ÜNVAN"].ToString().Split(' ');
				int index;
				for (int z = 0; z < splitTitle.Count(); z++)
				{
					if (splitTitle[z] == "NOLU")
					{
						index = z;
						sonuclar.Rows[i]["ÜNVAN"] = "";
						for (int f = index + 1; f < splitTitle.Count(); f++)
						{
							sonuclar.Rows[i]["ÜNVAN"] += splitTitle[f] + " ";
						}
						break;
					}
				}
				//Dogru sonuç için titleSplit2 diye bölüyorum. 700. diye başladığı için
				var splitTitle2 = sonuclar.Rows[i]["ÜNVAN"].ToString().Split(' ');

				if (IsNumeric(splitTitle2[0]) == true && sonuclar.Rows[i]["ÜNVAN"].ToString().StartsWith("7") && items.Count() > 5)
				{
					sonuclar.Rows[i]["ÜNVAN"] = items[items.Count() - 5];
				}

				//Dogru sonuç için titleSplit2 diye bölüyorum.
				var splitTitle4 = sonuclar.Rows[i]["ÜNVAN"].ToString().Split(' ');

				if (IsNumeric(splitTitle4[0]) == true)
				{
					sonuclar.Rows[i]["ÜNVAN"] = "";
					for (int z = 1; z < splitTitle4.Count(); z++)
					{
						sonuclar.Rows[i]["ÜNVAN"] += splitTitle4[z] + " ";
					}
				}
				string title2 = sonuclar.Rows[i]["ÜNVAN"].ToString();

				//Dogru sonuç için titleSplit3 diye bölüyorum.
				var splitTitle3 = sonuclar.Rows[i]["ÜNVAN"].ToString().Split(' ');
				if (splitTitle3[0] == "FİŞ" || splitTitle3[0] == "FŞ" || splitTitle3[0] == "FT")
				{
					sonuclar.Rows[i]["ÜNVAN"] = "";
					for (int z = 1; z < splitTitle3.Count(); z++)
					{
						sonuclar.Rows[i]["ÜNVAN"] += splitTitle3[z] + " ";
					}
				}
				sonuclar.Rows[i]["ÜNVAN"] = sonuclar.Rows[i]["ÜNVAN"].ToString().Trim();
				//ÜNVAN sayı ile başlıyorsa yanınlış yerde yazıyordur virgül kaydır.
				if (sonuclar.Rows[i]["ÜNVAN"].ToString().StartsWith("001") && items.Count() > 3)
				{
					sonuclar.Rows[i]["ÜNVAN"] = items[items.Count() - 3];
				}
				//Belge no sadece CBE içeriyorsa yanlıştır.
				if (items[0].ToString().Contains("CBE"))
				{
					sonuclar.Rows[i]["BELGE NO"] = items[0].ToString();
				}
				//sayı ile başlıyorsa at
				var splitForInt = sonuclar.Rows[i]["ÜNVAN"].ToString().Split(' ');
				if (IsNumeric(splitForInt[0]) == true)
				{
					sonuclar.Rows[i]["ÜNVAN"] = "";
					for (int z = 1; z < splitForInt.Count(); z++)
					{
						sonuclar.Rows[i]["ÜNVAN"] += splitForInt[z] + " ";
					}
				}
				//ACC ile başlıyorsa ÜNVAN at
				if (sonuclar.Rows[i]["ÜNVAN"].ToString().StartsWith("ACC"))
				{
					var ACCsplit = sonuclar.Rows[i]["ÜNVAN"].ToString().Split('/', '-');
					sonuclar.Rows[i]["ÜNVAN"] = ACCsplit[ACCsplit.Count() - 1];
				}
				//Ünvan yanlş yerde yazdığı virgülü bir kaydırıyorum.
				if (sonuclar.Rows[i]["ÜNVAN"].ToString().StartsWith("#") || sonuclar.Rows[i]["ÜNVAN"].ToString().StartsWith("UPA") || sonuclar.Rows[i]["ÜNVAN"].ToString().StartsWith("PLK") || sonuclar.Rows[i]["ÜNVAN"].ToString().StartsWith("UPR") && items.Count() > 6)
				{
					sonuclar.Rows[i]["ÜNVAN"] = items[items.Count() - 5];
				}
				//eğer kur 1 ise try yaz.
				if (Convert.ToDouble(sonuclar.Rows[i]["DÖVİZ KUR"])==1)
				{
					sonuclar.Rows[i]["DÖVİZ ADI"] = "TRY";
				}
			}
			sonuclar.AcceptChanges();
		}
		void BillType(DataTable sonuclar, int i)
		{
			//Ft: iki nokta ile başlıyorsa ilk 3 karektere ihtiyacım olmadığı için onları atıyorum.
			string FirstSplit = sonuclar.Rows[i]["AÇIKLAMA"].ToString().Substring(3, sonuclar.Rows[i]["AÇIKLAMA"].ToString().Length - 3);
			//Ft: attığımıza göre elimde belge no ve ÜNVAN kaldı.
			var items2 = FirstSplit.Split(' ');
			//0. parçada belge no lar olduğu için onları belge no adlı 6. sütunuma atıyorum.
			sonuclar.Rows[i]["BELGE NO"] = items2[0];
			//Geriye elimde ünvanlar kaldığı için onları sırayla ilgili kolana ekliyorum.
			for (int j = 1; j < items2.Count(); j++)
			{
				sonuclar.Rows[i]["ÜNVAN"] += items2[j] + " ";
			}
			//Belge no larının bazılarının önünde Belge seri noları bulunur.
			//Bu Koşul Belge seri no bulmak için kullanılır.
			if (sonuclar.Rows[i]["BELGE NO"].ToString().Contains("-"))
			{
				string Rowsplit = sonuclar.Rows[i]["BELGE NO"].ToString();
				var items = Rowsplit.Split('-');

				sonuclar.Rows[i]["BELGE SERİ NO"] = items[0];
				sonuclar.Rows[i]["BELGE NO"] = items[1];
			}
			if (sonuclar.Rows[i]["BELGE NO"].ToString().Length < 8)
			{
				string control = sonuclar.Rows[i]["BELGE NO"].ToString();
				int count = 0;
				foreach (char c in control)
				{
					if (char.IsLetter(c))
					{
						sonuclar.Rows[i]["BELGE SERİ NO"] += Convert.ToString(c);//Bulduğum seri noları seri no sütununa atıyorum.
						count++;//Belge nolardan seri noları atmak için
					}
					else if (char.IsDigit(c))
					{
						// İçerisinde sayılar
					}
				}

				string a = sonuclar.Rows[i]["BELGE NO"].ToString();
				sonuclar.Rows[i]["BELGE NO"] = a.Substring(count, (sonuclar.Rows[i]["BELGE NO"].ToString().Length - count));
			}
			AlertEnumber(sonuclar, i);
		}
		void BillType2(DataTable sonuclar, int i)
		{
			//Yukarıda yaptığım Ft: için yaptğımı burada Ft ile yapıyorum o yüzden ilk 2 karekteri atıyorum.
			string FirstSubstring = sonuclar.Rows[i]["AÇIKLAMA"].ToString().Substring(2, sonuclar.Rows[i]["AÇIKLAMA"].ToString().Length - 2);//Kelimenin başından FT yi atar
			string Rowsplit2 = FirstSubstring.TrimStart();

			string SecondSubstring = FirstSubstring.TrimStart();
			string TestSubject = "";

			TestSubject = SecondSubstring.Substring(4, SecondSubstring.Length - 4);//Sadece sayılardan başlasın diye
			int position = 0;
			for (int j = 0; j < TestSubject.Length; j++)
			{
				//char a=Convert.ToChar(TestSubject[j]);
				if (char.IsLetter(TestSubject[j]) || char.IsWhiteSpace(TestSubject[j]))
				{
					position = j;//ilk string olduğun yerin pozisyonunu tutar
					break;
				}
			}
			sonuclar.Rows[i]["BELGE NO"] = SecondSubstring.Substring(0, position + 4);//TestSubjecti olsutururken bastaki harfler gelmesin diye 3. konumdan başlatmıstım. o esiği +3 olarak ekliyoru.
			sonuclar.Rows[i]["ÜNVAN"] = SecondSubstring.Substring(position + 4, SecondSubstring.Length - (position + 4)).TrimStart();//Geri kalan kısım ünvan olduğu için onları da aldım.
																																	 //Belge no larının bazılarının önünde Belge seri noları bulunur.
																																	 //Bu Koşul Belge seri no bulmak için kullanılır.
			if (sonuclar.Rows[i]["BELGE NO"].ToString().Contains("-"))
			{
				string Rowsplit = sonuclar.Rows[i]["BELGE NO"].ToString();
				var items = Rowsplit.Split('-');

				sonuclar.Rows[i]["BELGE SERİ NO"] = items[0];
				sonuclar.Rows[i]["BELGE NO"] = items[1];
			}
			if (sonuclar.Rows[i]["BELGE NO"].ToString().Length < 8)
			{
				string control = sonuclar.Rows[i]["BELGE NO"].ToString();
				int count = 0;
				foreach (char c in control)
				{
					if (char.IsLetter(c))
					{
						sonuclar.Rows[i]["BELGE SERİ NO"] += Convert.ToString(c);//Bulduğum seri noları seri no sütununa atıyorum.
						count++;//Belge nolardan seri noları atmak için
					}
					else if (char.IsDigit(c))
					{
						// İçerisinde sayılar
					}
				}
				//Belge nodan seri noları atıyorum.
				string remove = sonuclar.Rows[i]["BELGE NO"].ToString();
				sonuclar.Rows[i]["BELGE NO"] = remove.Substring(count, (sonuclar.Rows[i]["BELGE NO"].ToString().Length - count));
			}
			AlertEnumber(sonuclar, i);
		}
		void voucherType(DataTable sonuclar, int i)
		{
			string Rowsplit = sonuclar.Rows[i]["AÇIKLAMA"].ToString();
			var items = Rowsplit.Split(':', ' ');
			//: veya boşluk görürsen böl 1. parçayı al bu sadece belge no ları elde etmemizi sağlar.
			string a = items[1];
			sonuclar.Rows[i]["BELGE NO"] = a;

			//Ünvanları almak için boşluklara göre split ediyorum.
			string Rowsplit2 = sonuclar.Rows[i]["AÇIKLAMA"].ToString();
			var items2 = Rowsplit2.Split(' ');
			//Burada j=1 den başlamasının sebebi ünvanların oradan başlaması
			//Bir firma birden fazla ünvana sahip olabilceği için for içerisinde 
			//kalan bütün parçaları ekliyorum.
			for (int j = 1; j < items2.Count(); j++)
			{
				sonuclar.Rows[i]["ÜNVAN"] += items2[j] + " ";
			}
		}
		void AlertEnumber(DataTable sonuclar, int i)
		{
			if (sonuclar.Rows[i]["BELGE NO"].ToString().Length == 15)
			{
				//uyarı vericek.
			}
		}
		void TitleControl(DataTable sonuclar, int i, string[] items)
		{
			//Bu Kısmı elimde kalan yanlış ünvanları düzeltmek için yazdım.
			string title = sonuclar.Rows[i]["ÜNVAN"].ToString();
			//ÜNVAN kısmında bazı durumlarda kasa/fiş belge no ünvan şeklinde yazıldığı için		
			if (title.StartsWith("KASA/FİŞ ") || title.StartsWith("FT") || title.StartsWith("FŞ") || title.StartsWith("KASA/FT") || title.StartsWith("KASA/FŞ"))
			{
				string Titlesplit = sonuclar.Rows[i]["ÜNVAN"].ToString();
				var Titleitems = Titlesplit.Split(' ');
				//Bu hücreyi ilk önce boşaltıyorum aşağıda += dediğim baştan tekrar dolduruyorum.
				sonuclar.Rows[i]["ÜNVAN"] = "";
				//kasa/fiş ve belge no atmak gerekli olduğu için döngüyü 2 den başlatıyorum.
				for (int z = 2; z <= Titleitems.Count() - 1; z++)
				{
					sonuclar.Rows[i]["ÜNVAN"] += Titleitems[z] + " ";
				}
			}
			if (title.StartsWith("KASA HESABI"))
			{
				string swapForTitle2 = items[items.Count() - 5];

				sonuclar.Rows[i]["ÜNVAN"] = swapForTitle2;
			}
			//95 ACC/Fiş ft gibi açıklamaları ÜNVAN ayırmak için yazıldı.
			if (title.Contains("FİŞ") || title.Contains("FŞ") || title.Contains("FT") || title.Contains("ACC"))
			{
				string Titlesplit = sonuclar.Rows[i]["ÜNVAN"].ToString();
				var Titleitems = Titlesplit.Split('/', '-');
				sonuclar.Rows[i]["ÜNVAN"] = Titleitems[Titleitems.Count() - 1].Trim();
			}
		}
		public bool IsNumeric(string s)
		{
			float output;
			return float.TryParse(s, out output);
		}
	}
}
