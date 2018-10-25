using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Excel;
using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp22
{
    class Netsis
    {
        public Netsis(DataGridView dataGrid)
        {

            var sonuclar = GetTable("Information");
            string b = "";
            for (int i = 0; i < sonuclar.Rows.Count; i++) //Tarihin Hesap Kodu kısmıno alıyorum
            {

                if (sonuclar.Rows[i]["YEMİYE TARİHİ"].ToString().StartsWith("Hesap Kodu"))
                {
                    sonuclar.Rows[i + 3][0] = sonuclar.Rows[i + 1][2].ToString();
                    b = sonuclar.Rows[i + 3][0].ToString();

                }
                else if (sonuclar.Rows[i]["YEMİYE TARİHİ"].ToString() == "100-01-001")
                {
                    sonuclar.Rows[i + 3][0] = sonuclar.Rows[i + 1][2].ToString();
                    b = "100-01-001";
                }


                sonuclar.Rows[i][0] = b;
            }
            sonuclar.Rows[2][0] = sonuclar.Rows[0][2].ToString();
            string c = "";
            for (int i = 0; i < sonuclar.Rows.Count; i++)//Açıklama kısmındaki Hesap adı kısmını çekme.
            {
                if (sonuclar.Rows[i]["Açıklama"].ToString().StartsWith("Hesap Adı"))
                {
                    sonuclar.Rows[i + 3][1] = sonuclar.Rows[i + 1]["Açıklama"].ToString();
                    c = sonuclar.Rows[i + 3][1].ToString();
                }
                else if (sonuclar.Rows[i]["Açıklama"].ToString() == "TL KASA HESABI")
                {
                    sonuclar.Rows[i + 3][1] = sonuclar.Rows[i + 1][2].ToString();
                    c = "TL KASA HESABI";
                }
                sonuclar.Rows[i][1] = c;

            }
            sonuclar.Rows[2][1] = sonuclar.Rows[0]["Açıklama"].ToString();


            sonuclar.AcceptChanges();


            for (int i = 0; i < sonuclar.Rows.Count; i++) //belge No
            {
                var a = sonuclar.Rows[i]["Açıklama"].ToString();
                var s = a.Split(' ');
                var t = s[s.Length - 1].ToString();
                Int64 serino;

                for (int N = 0; N < s.Length; N++)
                {
                    if (s[N].ToString().StartsWith("FN:"))
                    {


                        if (s[N].ToString().Length <= 5 && t.StartsWith("KDV"))
                        {
                            sonuclar.Rows[i]["Belge No"] += s[s.Length - 2].ToString();
                        }
                        else { sonuclar.Rows[i]["Belge No"] += s[N].Substring(3).ToString(); }
                    }

                    else if (s[N].ToString().StartsWith("NO:"))
                    {
                        //String str3 = s[N].ToString();
                        //Regex re = new Regex(@"([a-z A-Z]+)(\d+)");
                        //Match result = re.Match(str3);
                        if (s[N].ToString().Length == 3) {
                            string uy = "";
                            for (int r = N; r <= N + 1; r++) {
                                if (s[r].ToString().Equals("NO:"))
                                { }
                                else
                                {
                                    sonuclar.Rows[i]["Belge No"] += s[r].ToString();
                                }
                            }

                        }

                        else { sonuclar.Rows[i]["Belge No"] += s[N].Substring(3).ToString(); }

                    }

                }

            }
            for (int i = 0; i < sonuclar.Rows.Count; i++)
            {

            }

            for (int i = 0; i < sonuclar.Rows.Count; i++)//Belge Seri No
            {

                var a = sonuclar.Rows[i]["Açıklama"].ToString();
                var s = a.Split(' ');
                Int64 serino3;
                for (int N = 0; N < s.Length; N++)
                {
                    if (s[N].ToString().StartsWith("FN:"))
                    {
                        var e = s[N].ToString();
                        var d = e.Split(':');
                        if (s[N].ToString().Length == 4)
                        {

                            if (Int64.TryParse(d[1].ToString(), out serino3)) { }
                            else { sonuclar.Rows[i]["Belge Seri No"] += d[1].ToString(); }
                        }


                    }
                    String str4 = s[N].ToString();
                    Regex re = new Regex(@"([a-z A-Z]+)(\d+)");
                    Match result = re.Match(str4);

                    if (result.Groups[1].Value.Length == 1)
                    {
                        sonuclar.Rows[i]["Belge Seri No"] += result.Groups[1].Value.ToString();
                    }

                }
                //sonuclar.AcceptChanges();
                ////excel.ExcelWritten(sonuclar, "Netsis");
                //dataGrid.DataSource = sonuclar;
            }


            for (int i = 0; i < sonuclar.Rows.Count; i++)//Unvan
            {
                var a = sonuclar.Rows[i]["Açıklama"].ToString();
                var s = a.Split(' ');
                Int64 serino;
                for (int N = 0; N < s.Length; N++)
                {
                    var d = 0;

                    if (s[N].ToString().StartsWith("SN:"))
                    {

                        for (int f = 0; f < N; f++)
                        {

                            if (s[0].ToString().Length > 4 && Int64.TryParse(s[0].Substring(3).ToString(), out serino)) { for (int h = 1; h < N; h++) { sonuclar.Rows[i]["Unvan"] += s[h].ToString() + " "; } break; }
                            else if (Int64.TryParse(s[f + 1], out serino)) { for (int h = f + 2; h < N; h++) { sonuclar.Rows[i]["Unvan"] += s[h].ToString() + " "; } break; }
                            else if (s[f].ToString().StartsWith("MS/")) { for (int h = f + 1; h < N; h++) { sonuclar.Rows[i]["Unvan"] += s[h].ToString() + " "; } break; }
                            else { sonuclar.Rows[i]["Unvan"] += s[f].ToString() + " "; }
                        }

                    }
                    else if (s[N].ToString().Contains("FT.NIZ"))
                    {
                        d = N;
                        for (int f = 0; f <= N; f++)
                        {
                            sonuclar.Rows[i]["Unvan"] += s[f].ToString() + " ";
                        }

                    }
                    else if (s[N].ToString().Contains("KDVSI") || s[N].ToString().Contains("KDVsi"))
                    {

                        for (int f = 1; f < N; f++)
                        {
                            if (Int64.TryParse(s[f].ToString(), out serino))
                            {
                                for (int g = f; g <= N; g++) {
                                    sonuclar.Rows[i]["Unvan"] += s[g].ToString() + " "; }

                            }
                        }
                    }

                    else if (s[N].ToString().StartsWith("FT"))
                    {
                        for (int k = 0; k < N; k++)
                        {
                            sonuclar.Rows[i]["Unvan"] += s[k].ToString() + " ";
                        }
                    }
                    else if (s[0].ToString().StartsWith("MS/"))
                    {
                        for (int k = 1; k < s.Length; k++)
                        {
                            sonuclar.Rows[i]["Unvan"] += s[k].ToString() + " ";
                        }
                    }

                }
            }


            for (int i = 0; i < sonuclar.Rows.Count; i++)//Borç-Alacak
            {

                float e, j;
                if (float.TryParse(sonuclar.Rows[i]["Alacak"].ToString(), out j) && float.TryParse(sonuclar.Rows[i]["Borç"].ToString(), out e)) {
                    var d_alacak = Convert.ToSingle(sonuclar.Rows[i]["Alacak"].ToString());
                    var d_borç = Convert.ToSingle(sonuclar.Rows[i]["Borç"].ToString());
                    var sonuc = (d_alacak) - (d_borç);
                    sonuclar.Rows[i]["Tutar"] += sonuc.ToString(); }
                else { }

            }
            for (int i = 0; i < sonuclar.Rows.Count; i++)//İ.D.Tutarı İ.D.BORÇ-İ.D.Alacak
            {

                float e, j;
                if (float.TryParse(sonuclar.Rows[i]["İşlem Döviz Borç"].ToString(), out j) && float.TryParse(sonuclar.Rows[i]["İşlem Döviz Alacak"].ToString(), out e))
                {
                    var d_alacak = Convert.ToSingle(sonuclar.Rows[i]["İşlem Döviz Alacak"].ToString());
                    var d_borç = Convert.ToSingle(sonuclar.Rows[i]["İşlem Döviz Borç"].ToString());
                    var sonuc = (d_borç) - (d_alacak);
                    sonuclar.Rows[i]["İşlem Döviz Tutar"] += sonuc.ToString();
                }
                else { }

            }
            for (int i = 0; i < sonuclar.Rows.Count; i++)//Döviz Kuru Borç/İşlem Döviz
            {

                float e, j;
                if (float.TryParse(sonuclar.Rows[i]["İşlem Döviz Borç"].ToString(), out j) && float.TryParse(sonuclar.Rows[i]["İşlem Döviz Alacak"].ToString(), out e))
                {
                    float sonuc;
                    var d_alacak = Convert.ToSingle(sonuclar.Rows[i]["Alacak"].ToString());
                    var d_borç = Convert.ToSingle(sonuclar.Rows[i]["İşlem Döviz Alacak"].ToString());
                    sonuc = (d_alacak) / (d_borç);
                    if (d_borç == 0) { sonuc = 0; }
                    else { }

                    sonuclar.Rows[i]["Döviz Kuru"] += sonuc.ToString();
                }
                else { }

            }

            for (int i = 0; i < sonuclar.Rows.Count; i++)//Fiş Türü
            {
                if (sonuclar.Rows[i]["Açıklama"].ToString().Equals("Açılış Fişi")) { sonuclar.Rows[i]["Fiş Türü"] += "Açılış"; }
                else if (sonuclar.Rows[i]["Açıklama"].ToString().Equals("Açılıs Fişi")) { sonuclar.Rows[i]["Fiş Türü"] += "Açılış"; }
                else if (sonuclar.Rows[i]["Açıklama"].ToString().Equals("Açılıs Fisi")) { sonuclar.Rows[i]["Fiş Türü"] += "Açılış"; }
                else if (sonuclar.Rows[i]["Açıklama"].ToString().Equals("Acılıs Fisi")) { sonuclar.Rows[i]["Fiş Türü"] += "Açılış"; }
                else if (sonuclar.Rows[i]["Açıklama"].ToString().Equals("Acilis Fisi")) { sonuclar.Rows[i]["Fiş Türü"] += "Açılış"; }
                else if (sonuclar.Rows[i]["Açıklama"].ToString().Equals("Kapanış Fisi")) { sonuclar.Rows[i]["Fiş Türü"] += "Kapanış"; }
                else { sonuclar.Rows[i]["Fiş Türü"] += "Mahsup"; };
            }
            for (int i = 0; i < sonuclar.Rows.Count; i++)
            {
                if (sonuclar.Rows[i][2].ToString().Contains("."))
                {

                }
                else
                {
                    sonuclar.Rows[i].Delete();
                }

            }


            sonuclar.AcceptChanges();
            for (int i = 0; i < sonuclar.Rows.Count; i++)//191le başlayanlar
            {
                if (sonuclar.Rows[i][0].ToString().StartsWith("191"))
                {

                }
                else
                {
                    sonuclar.Rows[i].Delete();
                }

            }



            dataGrid.DataSource = sonuclar;

            

        }
        

        public System.Data.DataTable GetTable(String tableName)
        {
            
            OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + variables.filePath + "; Extended Properties='Excel 12.0 xml;HDR=YES;'");
            baglanti.Open();
            OleDbCommand sec = new OleDbCommand("SELECT * FROM [Orjinal$]", baglanti);
            OleDbDataAdapter adapter = new OleDbDataAdapter(sec);

            System.Data.DataTable DTexcel = new System.Data.DataTable();

            adapter.Fill(DTexcel);
            DTexcel.Columns[0].ColumnName = "YEMİYE TARİHİ";
            DTexcel.Columns.Add("Hesap Kodu", typeof(String)).SetOrdinal(0);
            DTexcel.Columns.Add("Hesap Adı ", typeof(String)).SetOrdinal(1);
            DTexcel.Columns["YEMİYE TARİHİ"].SetOrdinal(2);
            DTexcel.Columns.Add("Fiş Türü", typeof(String)).SetOrdinal(3);
            DTexcel.Columns.Add("Tutar", typeof(String)).SetOrdinal(10);
            DTexcel.Columns.Add("İşlem Döviz Tutar", typeof(String)).SetOrdinal(14);
            DTexcel.Columns.Add("Döviz Kuru", typeof(String)).SetOrdinal(17);
            DTexcel.Columns[4].ColumnName = "Fiş No";
            DTexcel.Columns[5].ColumnName = "Sr";
            DTexcel.Columns[6].ColumnName = "Açıklama";
            DTexcel.Columns[7].ColumnName = "Borç";
            DTexcel.Columns[8].ColumnName = "Alacak";
            DTexcel.Columns[9].ColumnName = "İşlem Döviz Borç";
            DTexcel.Columns[11].ColumnName = "İşlem Döviz Alacak";
            DTexcel.Columns[12].ColumnName = "Bakiye";
            DTexcel.Columns[13].ColumnName = "İşlem Döviz Bakiye";
            DTexcel.Columns[15].ColumnName = "Firma Döviz";
            DTexcel.Columns[16].ColumnName = "Döviz Adı";
            DTexcel.Columns.Add("Belge Seri No", typeof(String)).SetOrdinal(18);
            DTexcel.Columns.Add("Belge No", typeof(String)).SetOrdinal(19);
             DTexcel.Columns.Add("Unvan", typeof(String)).SetOrdinal(20);


            DTexcel.Columns["Hesap Kodu"].SetOrdinal(0);
            DTexcel.Columns["Hesap Adı "].SetOrdinal(1);
            DTexcel.Columns["YEMİYE TARİHİ"].SetOrdinal(2);
            DTexcel.Columns["Fiş Türü"].SetOrdinal(3);
            DTexcel.Columns["Fiş No"].SetOrdinal(4);
            DTexcel.Columns["Açıklama"].SetOrdinal(5);           
            DTexcel.Columns["Borç"].SetOrdinal(6);
            DTexcel.Columns["Alacak"].SetOrdinal(7);
            DTexcel.Columns["Bakiye"].SetOrdinal(8);
            DTexcel.Columns["Tutar"].SetOrdinal(9);
            DTexcel.Columns["İşlem Döviz Borç"].SetOrdinal(10);
            DTexcel.Columns["İşlem Döviz Alacak"].SetOrdinal(11);
            DTexcel.Columns["İşlem Döviz Bakiye"].SetOrdinal(12);
            DTexcel.Columns["İşlem Döviz Tutar"].SetOrdinal(13);
            DTexcel.Columns["Firma Döviz"].SetOrdinal(14);
            DTexcel.Columns["Döviz Adı"].SetOrdinal(15);
            DTexcel.Columns["Döviz Kuru"].SetOrdinal(16);
            DTexcel.Columns["Belge Seri No"].SetOrdinal(17);
            DTexcel.Columns["Belge No"].SetOrdinal(18);
            DTexcel.Columns["Unvan"].SetOrdinal(19);
            DTexcel.Columns["F13"].ColumnName = "  ";
            DTexcel.Columns["Sr"].SetOrdinal(21);



            var reader = sec.ExecuteReader(CommandBehavior.SchemaOnly);
            var table = reader.GetSchemaTable();
            
            
            baglanti.Close();
            
            return DTexcel;
        }
       

    }
}
