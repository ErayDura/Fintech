using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fintech
{
    

        class Netsis
        {
            public Netsis(DataGridView dataGrid)
            {
                 netsis(dataGrid);
            
        }
        public void netsis(DataGridView dataGrid)
        {
            var sonuclar = GetTable("Information");
            string b = "";
           
            for (int i = 0; i < sonuclar.Rows.Count; i++) //Tarihin HESAP KODU kısmıno alıyorum
            {

                if (sonuclar.Rows[i]["YEVMİYE TARİHİ"].ToString().StartsWith("Hesap Kodu"))
                {
                    sonuclar.Rows[i + 3][0] = sonuclar.Rows[i + 1][2].ToString();
                    b = sonuclar.Rows[i + 3][0].ToString();

                }
                else if (sonuclar.Rows[i]["YEVMİYE TARİHİ"].ToString() == "100-01-001")
                {
                    sonuclar.Rows[i + 3][0] = sonuclar.Rows[i + 1][2].ToString();
                    b = "100-01-001";
                }


                sonuclar.Rows[i][0] = b;
            }
            sonuclar.Rows[2][0] = sonuclar.Rows[0][2].ToString();
            string c = "";
            for (int i = 0; i < sonuclar.Rows.Count; i++)//AÇIKLAMA kısmındaki HESAP ADI kısmını çekme.
            {
                if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().StartsWith("Hesap Adı"))
                {
                    sonuclar.Rows[i + 3][1] = sonuclar.Rows[i + 1]["AÇIKLAMA"].ToString();
                    c = sonuclar.Rows[i + 3][1].ToString();
                }
                else if (sonuclar.Rows[i]["AÇIKLAMA"].ToString() == "TL KASA HESABI")
                {
                    sonuclar.Rows[i + 3][1] = sonuclar.Rows[i + 1][2].ToString();
                    c = "TL KASA HESABI";
                }
                sonuclar.Rows[i][1] = c;

            }
            sonuclar.Rows[2][1] = sonuclar.Rows[0]["AÇIKLAMA"].ToString();


            sonuclar.AcceptChanges();


            for (int i = 0; i < sonuclar.Rows.Count; i++) //BELGE NO
            {
                var a = sonuclar.Rows[i]["AÇIKLAMA"].ToString();
                var s = a.Split(' ');
                var t = s[s.Length - 1].ToString();
                Int64 serino;

                for (int N = 0; N < s.Length; N++)
                {
                    if (s[N].ToString().StartsWith("FN:"))
                    {


                        if (s[N].ToString().Length <= 5 && t.StartsWith("KDV"))
                        {
                            sonuclar.Rows[i]["BELGE NO"] += s[s.Length - 2].ToString();
                        }
                        else { sonuclar.Rows[i]["BELGE NO"] += s[N].Substring(3).ToString(); }
                    }

                    else if (s[N].ToString().StartsWith("NO:"))
                    {
                        //String str3 = s[N].ToString();
                        //Regex re = new Regex(@"([a-z A-Z]+)(\d+)");
                        //Match result = re.Match(str3);
                        if (s[N].ToString().Length == 3)
                        {
                            string uy = "";
                            for (int r = N; r <= N + 1; r++)
                            {
                                if (s[r].ToString().Equals("NO:"))
                                { }
                                else
                                {
                                    sonuclar.Rows[i]["BELGE NO"] += s[r].ToString();
                                }
                            }

                        }

                        else { sonuclar.Rows[i]["BELGE NO"] += s[N].Substring(3).ToString(); }

                    }

                }

            }
            for (int i = 0; i < sonuclar.Rows.Count; i++)
            {

            }

            for (int i = 0; i < sonuclar.Rows.Count; i++)//Belge Seri No
            {

                var a = sonuclar.Rows[i]["AÇIKLAMA"].ToString();
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
                            else { sonuclar.Rows[i]["BELGE SERİ"] += d[1].ToString(); }
                        }


                    }
                    String str4 = s[N].ToString();
                    Regex re = new Regex(@"([a-z A-Z]+)(\d+)");
                    Match result = re.Match(str4);

                    if (result.Groups[1].Value.Length == 1)
                    {
                        sonuclar.Rows[i]["BELGE SERİ"] += result.Groups[1].Value.ToString();
                    }

                }
                //sonuclar.AcceptChanges();
                ////excel.ExcelWritten(sonuclar, "Netsis");
                //dataGrid.DataSource = sonuclar;
            }


            for (int i = 0; i < sonuclar.Rows.Count; i++)//ÜNVAN
            {
                var a = sonuclar.Rows[i]["AÇIKLAMA"].ToString();
                var s = a.Split(' ');
                Int64 serino;
                for (int N = 0; N < s.Length; N++)
                {
                    var d = 0;

                    if (s[N].ToString().StartsWith("SN:"))
                    {

                        for (int f = 0; f < N; f++)
                        {

                            if (s[0].ToString().Length > 4 && Int64.TryParse(s[0].Substring(3).ToString(), out serino)) { for (int h = 1; h < N; h++) { sonuclar.Rows[i]["ÜNVAN"] += s[h].ToString() + " "; } break; }
                            else if (Int64.TryParse(s[f + 1], out serino)) { for (int h = f + 2; h < N; h++) { sonuclar.Rows[i]["ÜNVAN"] += s[h].ToString() + " "; } break; }
                            else if (s[f].ToString().StartsWith("MS/")) { for (int h = f + 1; h < N; h++) { sonuclar.Rows[i]["ÜNVAN"] += s[h].ToString() + " "; } break; }
                            else { sonuclar.Rows[i]["ÜNVAN"] += s[f].ToString() + " "; }
                        }

                    }
                    else if (s[N].ToString().Contains("FT.NIZ"))
                    {
                        d = N;
                        for (int f = 0; f <= N; f++)
                        {
                            sonuclar.Rows[i]["ÜNVAN"] += s[f].ToString() + " ";
                        }

                    }
                    else if (s[N].ToString().Contains("KDVSI") || s[N].ToString().Contains("KDVsi"))
                    {

                        for (int f = 1; f < N; f++)
                        {
                            if (Int64.TryParse(s[f].ToString(), out serino))
                            {
                                for (int g = f; g <= N; g++)
                                {
                                    sonuclar.Rows[i]["ÜNVAN"] += s[g].ToString() + " ";
                                }

                            }
                        }
                    }

                    else if (s[N].ToString().StartsWith("FT"))
                    {
                        for (int k = 0; k < N; k++)
                        {
                            sonuclar.Rows[i]["ÜNVAN"] += s[k].ToString() + " ";
                        }
                    }
                    else if (s[0].ToString().StartsWith("MS/"))
                    {
                        for (int k = 1; k < s.Length; k++)
                        {
                            sonuclar.Rows[i]["ÜNVAN"] += s[k].ToString() + " ";
                        }
                    }

                }
            }


            for (int i = 0; i < sonuclar.Rows.Count; i++)//BORÇ-ALACAK
            {

                double e, j;
                if (double.TryParse(sonuclar.Rows[i]["ALACAK"].ToString(), out j) && double.TryParse(sonuclar.Rows[i]["BORÇ"].ToString(), out e))
                {
                    double d_ALACAK = Convert.ToDouble(sonuclar.Rows[i]["ALACAK"]);
                    double d_BORÇ = Convert.ToDouble(sonuclar.Rows[i]["BORÇ"]);
                    double sonuc = (d_BORÇ) - (d_ALACAK);
                    sonuclar.Rows[i]["TUTAR"] = sonuc;
                }
                else { }

            }
            for (int i = 0; i < sonuclar.Rows.Count; i++)//İ.D.TUTARı İ.D.BORÇ-İ.D.ALACAK
            {

                double e, j;
                if (double.TryParse(sonuclar.Rows[i]["İŞLEM DÖVİZ BORÇ"].ToString(), out j) && double.TryParse(sonuclar.Rows[i]["İŞLEM DÖVİZ ALACAK"].ToString(), out e))
                {
                    double d_ALACAK = Convert.ToDouble(sonuclar.Rows[i]["İŞLEM DÖVİZ ALACAK"]);
                    double d_BORÇ = Convert.ToDouble(sonuclar.Rows[i]["İŞLEM DÖVİZ BORÇ"]);
                    double sonuc = (d_BORÇ) - (d_ALACAK);
                    sonuclar.Rows[i]["İŞLEM DÖVİZ TUTAR"] = sonuc;
                }
                else { }

            }
            for (int i = 0; i < sonuclar.Rows.Count; i++)//DÖVİZ KUR BORÇ/İŞLEM DÖVİZ
            {

                double e, j;
                if (double.TryParse(sonuclar.Rows[i]["İŞLEM DÖVİZ BORÇ"].ToString(), out j) && double.TryParse(sonuclar.Rows[i]["İŞLEM DÖVİZ ALACAK"].ToString(), out e))
                {
                    double sonuc;
                    double d_ALACAK = Convert.ToDouble(sonuclar.Rows[i]["ALACAK"]);
                    double d_BORÇ = Convert.ToDouble(sonuclar.Rows[i]["İŞLEM DÖVİZ ALACAK"]);
                    sonuc = (d_ALACAK) / (d_BORÇ);
                    if (d_BORÇ == 0) { sonuc = 0; }
                    else { }

                    sonuclar.Rows[i]["DÖVİZ KUR"] = sonuc;
                }
                else { }

            }

            for (int i = 0; i < sonuclar.Rows.Count; i++)//FİŞ TÜRÜ
            {
                if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().Equals("Açılış Fişi")) { sonuclar.Rows[i]["FİŞ TÜRÜ"] += "Açılış"; }
                else if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().Equals("Açılıs Fişi")) { sonuclar.Rows[i]["FİŞ TÜRÜ"] += "Açılış"; }
                else if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().Equals("Açılıs Fisi")) { sonuclar.Rows[i]["FİŞ TÜRÜ"] += "Açılış"; }
                else if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().Equals("Acılıs Fisi")) { sonuclar.Rows[i]["FİŞ TÜRÜ"] += "Açılış"; }
                else if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().Equals("Acilis Fisi")) { sonuclar.Rows[i]["FİŞ TÜRÜ"] += "Açılış"; }
                else if (sonuclar.Rows[i]["AÇIKLAMA"].ToString().Equals("Kapanış Fisi")) { sonuclar.Rows[i]["FİŞ TÜRÜ"] += "Kapanış"; }
                else { sonuclar.Rows[i]["FİŞ TÜRÜ"] += "Mahsup"; };
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
            //for (int i = 0; i < sonuclar.Rows.Count; i++)//191le başlayanlar
            //{
            //    if (sonuclar.Rows[i][0].ToString().StartsWith("191"))
            //    {

            //    }
            //    else
            //    {
            //        sonuclar.Rows[i].Delete();
            //    }

            //}
            for (int i = 0; i < sonuclar.Rows.Count; i++)
            {
                var a = sonuclar.Rows[i]["HESAP KODU"].ToString();
                a = a.Replace("-", ".");
                sonuclar.Rows[i]["HESAP KODU"] = a;
            }
            for (int i = 0; i < sonuclar.Rows.Count; i++)
            {
                sonuclar.Rows[i]["BORÇ"] = Math.Round((Convert.ToDouble(sonuclar.Rows[i]["BORÇ"])), 2);
                sonuclar.Rows[i]["ALACAK"] = Math.Round((Convert.ToDouble(sonuclar.Rows[i]["ALACAK"])), 2);
                sonuclar.Rows[i]["TUTAR"] = Math.Round((Convert.ToDouble(sonuclar.Rows[i]["TUTAR"])), 2);
                sonuclar.Rows[i]["DÖVİZ KUR"] = Math.Round((Convert.ToDouble(sonuclar.Rows[i]["DÖVİZ KUR"])), 2);
                sonuclar.Rows[i]["BAKİYE"] = Math.Round((Convert.ToDouble(sonuclar.Rows[i]["BAKİYE"])), 2);
                sonuclar.Rows[i]["İŞLEM DÖVİZ ALACAK"] = Math.Round((Convert.ToDouble(sonuclar.Rows[i]["İŞLEM DÖVİZ ALACAK"])), 2);
                sonuclar.Rows[i]["İŞLEM DÖVİZ BORÇ"] = Math.Round((Convert.ToDouble(sonuclar.Rows[i]["İŞLEM DÖVİZ BORÇ"])), 2);
                sonuclar.Rows[i]["İŞLEM DÖVİZ BAKİYE"] = Math.Round((Convert.ToDouble(sonuclar.Rows[i]["İŞLEM DÖVİZ BAKİYE"])), 2);
                sonuclar.Rows[i]["İŞLEM DÖVİZ TUTAR"] = Math.Round((Convert.ToDouble(sonuclar.Rows[i]["İŞLEM DÖVİZ TUTAR"])), 2);
            }
            for(int i=0; i<sonuclar.Rows.Count; i ++)
            {
              
            }
           
            sonuclar.AcceptChanges();
            dataGrid.DataSource = sonuclar;
            variables.mainDataTable = sonuclar;
        }



    public System.Data.DataTable GetTable(String tableName)
            {

                OleDbConnection baglanti = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + variables.filePath + "; Extended Properties='Excel 12.0 xml;HDR=YES;'");
                baglanti.Open();
                OleDbCommand sec = new OleDbCommand("SELECT * FROM [Orjinal$]", baglanti);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sec);

                System.Data.DataTable DTexcel = new System.Data.DataTable();
            DataGridView Datagrid = new DataGridView();

            adapter.Fill(DTexcel);
                DTexcel.Columns[0].ColumnName = "YEVMİYE TARİHİ";
                DTexcel.Columns.Add("HESAP KODU", typeof(String)).SetOrdinal(0);
                DTexcel.Columns.Add("HESAP ADI ", typeof(String)).SetOrdinal(1);
                DTexcel.Columns["YEVMİYE TARİHİ"].SetOrdinal(2);
                DTexcel.Columns.Add("FİŞ TÜRÜ", typeof(String)).SetOrdinal(3);
                DTexcel.Columns.Add("TUTAR", typeof(double)).SetOrdinal(10);
                DTexcel.Columns.Add("İŞLEM DÖVİZ TUTAR", typeof(double)).SetOrdinal(14);
                DTexcel.Columns.Add("DÖVİZ KUR", typeof(double)).SetOrdinal(17);
                DTexcel.Columns[4].ColumnName = "FİŞ NO";
                DTexcel.Columns[5].ColumnName = "SR";
                DTexcel.Columns[6].ColumnName = "AÇIKLAMA";
                DTexcel.Columns[7].ColumnName = "BORÇ";
                DTexcel.Columns[8].ColumnName = "ALACAK";
                DTexcel.Columns[9].ColumnName = "İŞLEM DÖVİZ BORÇ";
                DTexcel.Columns[11].ColumnName = "İŞLEM DÖVİZ ALACAK";
                DTexcel.Columns[12].ColumnName = "BAKİYE";
                DTexcel.Columns[13].ColumnName = "İŞLEM DÖVİZ BAKİYE";
                DTexcel.Columns[15].ColumnName = "FİRMA DÖVİZ";
                DTexcel.Columns[16].ColumnName = "DÖVİZ ADI";
                DTexcel.Columns.Add("BELGE SERİ", typeof(String)).SetOrdinal(18);
                DTexcel.Columns.Add("BELGE NO", typeof(String)).SetOrdinal(19);
                DTexcel.Columns.Add("ÜNVAN", typeof(String)).SetOrdinal(20);


                DTexcel.Columns["HESAP KODU"].SetOrdinal(0);
                DTexcel.Columns["HESAP ADI "].SetOrdinal(1);
                DTexcel.Columns["YEVMİYE TARİHİ"].SetOrdinal(2);
                DTexcel.Columns["FİŞ TÜRÜ"].SetOrdinal(3);
                DTexcel.Columns["FİŞ NO"].SetOrdinal(4);
                DTexcel.Columns["AÇIKLAMA"].SetOrdinal(5);
                DTexcel.Columns["BORÇ"].SetOrdinal(6);
                DTexcel.Columns["ALACAK"].SetOrdinal(7);
                DTexcel.Columns["BAKİYE"].SetOrdinal(8);
                DTexcel.Columns["TUTAR"].SetOrdinal(9);
                DTexcel.Columns["İŞLEM DÖVİZ BORÇ"].SetOrdinal(10);
                DTexcel.Columns["İŞLEM DÖVİZ ALACAK"].SetOrdinal(11);
                DTexcel.Columns["İŞLEM DÖVİZ BAKİYE"].SetOrdinal(12);
                DTexcel.Columns["İŞLEM DÖVİZ TUTAR"].SetOrdinal(13);
                DTexcel.Columns["FİRMA DÖVİZ"].SetOrdinal(14);
                DTexcel.Columns["DÖVİZ ADI"].SetOrdinal(15);
                DTexcel.Columns["DÖVİZ KUR"].SetOrdinal(16);
                DTexcel.Columns["BELGE SERİ"].SetOrdinal(17);
                DTexcel.Columns["BELGE NO"].SetOrdinal(18);
                DTexcel.Columns["ÜNVAN"].SetOrdinal(19);
                DTexcel.Columns["F13"].ColumnName = "  ";
                DTexcel.Columns["SR"].SetOrdinal(21);


            
            var reader = sec.ExecuteReader(CommandBehavior.SchemaOnly);
                var table = reader.GetSchemaTable();
            


                baglanti.Close();

                return DTexcel;
            }


        }
    }



